SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_ExecutarGerarMenuArvore]
@IdEmpresa int, @IdModulo int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
SET NOCOUNT ON;

CREATE TABLE #tempx (idopcao int,idpermissao int,idpermissaorecurso int,IDRecurso int,IDRecursoPai int,Nome varchar(50),Permissao varchar(50),coluna_1 int,coluna_2 int,coluna_3 int, coluna_4 int, coluna_5 int, coluna_6 int, coluna_7 int, coluna_8 int, coluna_9 int, coluna_10 int)
insert #tempx
exec SP_GerarMenuArvore @IdEmpresa,@IdModulo

select * from #tempx order by IDRecurso

END
GO
