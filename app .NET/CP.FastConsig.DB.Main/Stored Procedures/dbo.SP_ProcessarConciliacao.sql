SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
--exec SP_ProcessarConciliacaoConciliacao '2011/10'

--select * from ConciliacaoConciliacao

CREATE PROCEDURE [dbo].[SP_ProcessarConciliacao]
	@AnoMes varchar(7)
AS

SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ COMMITTED

-- Excluir movimento
delete from Conciliacao where Competencia = @AnoMes

-- tratar tabela Conciliacao retorno
update ConciliacaoRetorno set IDConsignataria = (select MAX(idconsignataria) from Produto where VerbaFolha = ConciliacaoRetorno.Verba and Ativo = 1)

update ConciliacaoRetorno set IDProduto = s.IDProduto
from ConciliacaoRetorno r, Produto s
where r.Verba = s.VerbaFolha
and (select COUNT(*) from Produto where VerbaFolha = r.Verba and Ativo = 1) = 1

-- associar movimento a tabela de retorno quando valor conincide
select r.ValorDescontado, m.Valor, r.IDConciliacaoRetorno, m.Matricula, m.Verba, m.IDConciliacaoMovimento 
into #temp_mov 
from ConciliacaoMovimento m, ConciliacaoRetorno r
where m.Matricula = r.Matricula and m.Verba = r.Verba and m.Competencia = r.Competencia
and m.Competencia = @AnoMes

select IDConciliacaoRetorno, IDConciliacaoMovimento into #temp_valorsoma 
from #temp_mov t 
where ValorDescontado = (select sum(valor) from #temp_mov where Matricula = t.Matricula and verba = t.verba)

update ConciliacaoMovimento set IDConciliacaoRetorno = t.IDConciliacaoRetorno, valordescontado = ValorMovimento
from ConciliacaoMovimento m, #temp_valorsoma t
where m.IDConciliacaoMovimento = t.IDConciliacaoMovimento

-- tratamento dos Conciliacaos que não coincidiram seus valores com os Conciliacaos enviados para folha
select IDConciliacaoRetorno, IDConciliacaoMovimento, ValorDescontado, Valor, 
ROW_NUMBER() over (order by IDConciliacaoRetorno, ValorDescontado, Valor desc) as idordem 
into #temp_valorparcial 
from #temp_mov t
where not exists(select 1 from #temp_valorsoma where IDConciliacaoRetorno = t.IDConciliacaoRetorno)
order by IDConciliacaoRetorno, ValorDescontado, Valor desc

select *, SUM(valor) over (partition by idConciliacaoretorno) as valortotalmovimento, 
(select SUM(valor) from #temp_valorparcial where IDConciliacaoRetorno = t.IDConciliacaoRetorno and idordem <= t.idordem) as valoracum
into #valorcalcadescontar
from #temp_valorparcial t

select IDConciliacaoRetorno, IDConciliacaoMovimento, (case when valoracum <= ValorDescontado and valoracum <> valortotalmovimento then valor 
when valoracum > ValorDescontado and valor-abs(ValorDescontado - valoracum) > 0 then valor-abs(ValorDescontado - valoracum) 
when ValorDescontado > valoracum  then valor+abs(ValorDescontado-valoracum) 
else 0 end) as valoradescontar 
into #valoradescontar
from #valorcalcadescontar

update ConciliacaoMovimento set IDConciliacaoRetorno = t.IDConciliacaoRetorno, valordescontado = t.valoradescontar
from ConciliacaoMovimento m, #valoradescontar t
where m.IDConciliacaoMovimento = t.IDConciliacaoMovimento

-- Gerar Conciliacao
select * into #temp_cm from ConciliacaoMovimento where Competencia = @AnoMes

select * into #temp_cr from ConciliacaoRetorno where Competencia = @AnoMes

select @AnoMes as Competencia,
m.IDConciliacaoMovimento, r.IDConciliacaoRetorno, isnull(r.Matricula,m.Matricula) as Matricula , r.Matricula as rmatricula, r.Verba as rverba,
isnull(r.Verba,m.Verba) as Verba, ISNULL(m.Valor,0) as Valor, isnull(m.ValorDescontado,r.ValorDescontado) as ValorDescontado, m.ValorMovimento, m.IDConsignataria
into #temp_conciliacao
from #temp_cm m full outer join #temp_cr r on m.IDConciliacaoRetorno = r.IDConciliacaoRetorno

insert into Conciliacao(competencia,IDConciliacaoMovimento,IDConciliacaoRetorno,IDFuncionario,IDProduto,Matricula,Verba,Valor, ValorDescontado, IDConciliacaoTipo, IDConsignataria)
select competencia,IDConciliacaoMovimento,IDConciliacaoRetorno,f.IDFuncionario,s.IDProduto,t.Matricula,t.Verba,Valor, ValorDescontado, 
(case when ValorMovimento > 0 and isnull(valordescontado,0) > 0 and ValorMovimento = valordescontado then 1 -- Descontado
when ValorMovimento > 0 and isnull(valordescontado,0) = 0 then 2 -- Não Descontado
when ValorMovimento > 0 and isnull(valordescontado,0) > 0 and isnull(valordescontado,0) < ValorMovimento  then 3 -- Descontado a menor
when ValorMovimento > 0 and isnull(valordescontado,0) > 0 and isnull(valordescontado,0) > ValorMovimento  then 4 -- Descontado a maior
when s.IDProduto is null then 5 -- Não Conciliado - Verba Inexistente
when f.IDFuncionario is null then 6 -- Não Conciliado - Funcionario Inexistente
when IDConciliacaoRetorno > 0 and IDConciliacaoMovimento is null then 7 -- Não Conciliado - Conciliacao Inexistente
else null end), t.IDConsignataria
from #temp_conciliacao t
left join Funcionario f on t.rmatricula = f.matricula and f.Ativo = 1
left join Produto s on t.rverba = s.verbafolha and s.Ativo = 1
where t.Competencia = @AnoMes




GO
