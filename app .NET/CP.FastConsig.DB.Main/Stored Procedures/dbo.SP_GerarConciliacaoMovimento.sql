
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].[SP_GerarConciliacaoMovimento]
	@AnoMes varchar(7)
AS

SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ COMMITTED

-- Gerar Movimento
insert into ConciliacaoMovimento(competencia,IDConciliacaoCorteFolha,IDConciliacaoMovimentoTipo,Matricula,Verba,IDConsignataria,Valor, ValorMovimento, ValorDescontado, IDProduto, IDAverbacaoParcela, Prazo, Parcela)
select Competencia,IDConciliacaoCorteFolha,1, Matricula, Verba, IDConsignataria, ValorParcela, ValorParcela, 0, IDProduto, IDAverbacaoParcela, Prazo, Parcela 
from ConciliacaoCorteFolha
where Competencia = @AnoMes

-- Aplicar reduções
GO
