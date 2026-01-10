SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].[SP_AlterarParcelaSituacao]
	@AnoMes varchar(7), @idaverbacao int, @idsituacao int
AS

SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ COMMITTED

update averbacaoparcela set idaverbacaoparcelasituacao = @idsituacao where idaverbacao = @idaverbacao and Competencia >= @AnoMes
GO
