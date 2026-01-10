SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].[SP_RestauraPermissoes]
	-- Add the parameters for the stored procedure here
	@IDEmpresa int
AS
BEGIN
	SET NOCOUNT ON;

	delete from PermissaoUsuario where IDEmpresa = @IDEmpresa
	
    -- Insert statements for procedure here
    insert into PermissaoUsuario(IDEmpresa, IDPerfil, IDPermissao, IDRecurso)
    select @IDEmpresa, IDPerfil, IDPermissao, IDRecurso from PermissaoSistema
END
GO
