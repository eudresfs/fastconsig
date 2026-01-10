
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].[SP_GerarConciliacaoCorteFolha]
	@AnoMes varchar(7)
AS

SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ COMMITTED

declare @idcorte int
declare @datatram varchar(30)
declare @dia varchar(2)

if not exists(select 1 from CorteHistorico where Competencia = @AnoMes) begin
    select @dia = valor from Parametro where Nome = 'DiaCorte'
	insert into CorteHistorico(Competencia,DiaCorte,Data)
	values (@AnoMes,CONVERT(int,@dia),GETDATE());
end

select @idcorte = idcortehistorico, @dia = convert(varchar(2),DiaCorte) from CorteHistorico where Competencia = @AnoMes
select @datatram = replace(@AnoMes,'/','-')+'-'+@dia+' 23:59:59'

-- zerar tabela para nova geração
delete from conciliacaomovimento where idConciliacaoCorteFolha in (select idConciliacaoCorteFolha from ConciliacaoCorteFolha where competencia = @AnoMes)
delete from ConciliacaoCorteFolha where competencia = @AnoMes 

update Funcionario set mesexonerar = null, IDFuncionarioSituacao = 2, NomeSituacaoFolha = 'Exonerado' where mesexonerar is not null and mesexonerar = @AnoMes

-- gerar parcelas das mensalidades
insert into AverbacaoParcela(IDAverbacao,numero,competencia,valor,IDAverbacaoParcelaSituacao,ativo)
select c.IDAverbacao, 1, @AnoMes, c.valorparcela, 1, 1
FROM Averbacao c, Funcionario f, Produto s, AverbacaoSituacao cs, ProdutoGrupo sg, ProdutoTipo st
where c.IDFuncionario = f.IDFuncionario and s.IDProduto = c.IDProduto and c.IDAverbacaoSituacao = cs.IDAverbacaoSituacao
and c.Ativo = 1 and f.Ativo = 1 and f.IDFuncionarioSituacao = 1 and s.IDProdutoGrupo = sg.IDProdutoGrupo and sg.IDProdutoTipo = st.IDProdutoTipo
and st.IDProdutoTipo = 2 and cs.ParaDescontoFolha = 1
and c.competenciainicial <= @AnoMes 
and not exists(select 1 from Averbacaoparcela cp where cp.idAverbacao = c.idAverbacao and cp.competencia = @AnoMes and cp.Ativo = 1)

-- inclusão na tabela de corte
insert into ConciliacaoCorteFolha(competencia,idAverbacaoparcela,IDAverbacaoSituacao,Data,IDCorteHistorico,ValorParcela, Verba, Matricula, IDProdutoGrupo, IDProdutoTipo, IDConsignataria, IDFuncionario, IDProduto, Prazo, Parcela)
SELECT @AnoMes, cp.IDAverbacaoParcela, cs.IDAverbacaoSituacao, GETDATE(), @idcorte, cp.Valor, s.verbafolha, f.Matricula, s.IDProdutoGrupo, st.IDProdutoTipo, c.IDConsignataria, c.IDFuncionario, c.idproduto, c.Prazo, cp.Numero
FROM Averbacao c, AverbacaoParcela cp, Funcionario f, Produto s, AverbacaoSituacao cs, ProdutoGrupo sg, ProdutoTipo st
where c.IDAverbacao = cp.IDAverbacao and c.IDFuncionario = f.IDFuncionario and s.IDProduto = c.IDProduto 
and c.Ativo = 1 and f.Ativo = 1 and f.IDFuncionarioSituacao = 1 and cp.Ativo = 1 and s.IDProdutoGrupo = sg.IDProdutoGrupo and sg.IDProdutoTipo = st.IDProdutoTipo
and	cp.IDAverbacaoParcelaSituacao > 0 and cp.competencia = @AnoMes and (select top 1 idaverbacaosituacao from AverbacaoTramitacao where idaverbacao = c.idaverbacao and  createdon <= @datatram order by createdon desc) = cs.IDAverbacaoSituacao and
(st.idProdutotipo = 3 or cs.ParaDescontoFolha = 1) 


GO
