--exec SP_ProcessarContratoConciliacao '2011/10'

--select * from ContratoConciliacao

ALTER PROCEDURE [dbo].[SP_ProcessarContratoConciliacao]
	@AnoMes varchar(7)
AS

SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ COMMITTED

-- Excluir movimento
delete from ContratoConciliacao where Competencia = @AnoMes

-- associar movimento a tabela de retorno quando valor conincide
select r.ValorDescontado, m.Valor, r.IDContratoRetorno, m.Matricula, m.Verba, m.IDContratoMovimento into #temp_mov 
from ContratoMovimento m, ContratoRetorno r
where m.Matricula = r.Matricula and m.Verba = r.Verba and m.Competencia = r.Competencia
and m.Competencia = @AnoMes

select IDContratoRetorno, IDContratoMovimento into #temp_valorsoma 
from #temp_mov t 
where ValorDescontado = (select sum(valor) from #temp_mov where Matricula = t.Matricula and verba = t.verba)

update ContratoMovimento set IDContratoRetorno = t.IDContratoRetorno, valordescontado = ValorMovimento
from ContratoMovimento m, #temp_valorsoma t
where m.IDContratoMovimento = t.IDContratoMovimento

-- tratamento dos contratos que não coincidiram seus valores com os contratos enviados para folha
select IDContratoRetorno, IDContratoMovimento, ValorDescontado, Valor, 
ROW_NUMBER() over (order by IDContratoRetorno, ValorDescontado, Valor desc) as idordem into #temp_valorparcial 
from #temp_mov t
where not exists(select 1 from #temp_valorsoma where IDContratoRetorno = t.IDContratoRetorno)
order by IDContratoRetorno, ValorDescontado, Valor desc

select *, SUM(valor) over (partition by idcontratoretorno) as valortotalmovimento, 
(select SUM(valor) from #temp_valorparcial where IDContratoRetorno = t.IDContratoRetorno and idordem <= t.idordem) as valoracum
into #valorcalcadescontar
from #temp_valorparcial t

select IDContratoRetorno, IDContratoMovimento, (case when valoracum <= ValorDescontado and valoracum <> valortotalmovimento then valor 
when valoracum > ValorDescontado and valor-abs(ValorDescontado - valoracum) > 0 then valor-abs(ValorDescontado - valoracum) 
when ValorDescontado > valoracum  then valor+abs(ValorDescontado-valoracum) 
else 0 end) as valoradescontar 
into #valoradescontar
from #valorcalcadescontar

update ContratoMovimento set IDContratoRetorno = t.IDContratoRetorno, valordescontado = t.valoradescontar
from ContratoMovimento m, #valoradescontar t
where m.IDContratoMovimento = t.IDContratoMovimento

-- Gerar Conciliacao
select * into #temp_cm from ContratoMovimento where Competencia = @AnoMes

select * into #temp_cr from ContratoRetorno where Competencia = @AnoMes

select @AnoMes as Competencia,
m.IDContratoMovimento, r.IDContratoRetorno, isnull(r.Matricula,m.Matricula) as Matricula , r.Matricula as rmatricula, r.Verba as rverba,
isnull(r.Verba,m.Verba) as Verba, ISNULL(m.Valor,0) as Valor, isnull(m.ValorDescontado,r.ValorDescontado) as ValorDescontado, m.ValorMovimento
into #temp_conciliacao
from #temp_cm m full outer join #temp_cr r on m.IDContratoRetorno = r.IDContratoRetorno

insert into ContratoConciliacao(competencia,IDContratoMovimento,IDContratoRetorno,IDFuncionario,IDServico,Matricula,Verba,Valor, ValorDescontado, IDContratoConciliacaoTipo)
select competencia,IDContratoMovimento,IDContratoRetorno,f.IDFuncionario,s.IDServico,t.Matricula,t.Verba,Valor, ValorDescontado, 
(case when ValorMovimento > 0 and isnull(valordescontado,0) > 0 and ValorMovimento = valordescontado then 1 -- Descontado
when ValorMovimento > 0 and isnull(valordescontado,0) = 0 then 2 -- Não Descontado
when ValorMovimento > 0 and isnull(valordescontado,0) > 0 and isnull(valordescontado,0) < ValorMovimento  then 3 -- Descontado a menor
when ValorMovimento > 0 and isnull(valordescontado,0) > 0 and isnull(valordescontado,0) > ValorMovimento  then 4 -- Descontado a maior
when s.IDServico is null then 5 -- Não Conciliado - Verba Inexistente
when f.IDFuncionario is null then 6 -- Não Conciliado - Funcionario Inexistente
when IDContratoRetorno > 0 and IDContratoMovimento is null then 7 -- Não Conciliado - Contrato Inexistente
else null end)
from #temp_conciliacao t
left join Funcionario f on t.rmatricula = f.matricula and f.Ativo = 1
left join Servico s on t.rverba = s.verbafolha and s.Ativo = 1
where t.Competencia = @AnoMes




