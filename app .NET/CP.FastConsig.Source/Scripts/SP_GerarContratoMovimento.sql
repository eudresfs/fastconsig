ALTER PROCEDURE [dbo].[SP_GerarContratoMovimento]
	@AnoMes varchar(7)
AS

SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ COMMITTED

-- Gerar Movimento
insert into ContratoMovimento(competencia,IDContratoCorteFolha,IDContratoMovimentoTipo,Matricula,Verba,IDConsignataria,Valor, ValorMovimento)
select Competencia,IDContratoCorteFolha,1, Matricula, Verba, IDConsignataria, ValorParcela, ValorParcela
from ContratoCorteFolha
where Competencia = @AnoMes

-- Aplicar reduções