SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GerarMenuArvore]
@IdEmpresa int, @IdModulo int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
DECLARE @columns VARCHAR(8000)

select * into #temp_perfil 
FROM Perfil 
WHERE (idempresa is null or idempresa = @IdEmpresa) and IDModulo = @IdModulo ORDER BY IDPerfil 

while (select COUNT(*) from #temp_perfil) < 10 begin
  insert into #temp_perfil(Nome,Sigla,IDModulo,IDEmpresa) values ('indefinido','II',@IdModulo,@IdEmpresa)
end

SELECT top 10 @columns = COALESCE(@columns + ',[coluna_' + convert(varchar(10),idperfil) + ']','[coluna_' + convert(varchar(10),idperfil)+ ']') 
FROM #temp_perfil 
WHERE (idempresa is null or idempresa = @IdEmpresa) and IDModulo = @IdModulo ORDER BY IDPerfil 

DECLARE @query VARCHAR(8000)
SET @query = 'SELECT * FROM (SELECT pr.idrecurso as idopcao, pr.idpermissao,pr.idpermissaorecurso, 
    (case when pr.idpermissao <> 1 then 100000+pr.idpermissaorecurso else r.idrecurso end) as IDRecurso, 
    (case when pr.idpermissao <> 1 then r.idrecurso else r.idrecursopai end) as IDRecursoPai, 
    (case when pr.idpermissao <> 1 then f.nome else r.nome end) as Nome, 
    f.nome as Permissao, 
    ''coluna_''+convert(varchar(10),idperfil) as NomePerfil, 
    (select (case when isnull(idpermissaousuario,0) > 0 then 1 else 0 end) from permissaousuario where idrecurso = pr.idrecurso and idpermissao = pr.idpermissao and idperfil = x.idperfil and idempresa = '+convert(varchar(10),@IdEmpresa)+') 
      as existepermissao 
     FROM 
      PermissaoRecurso pr inner join Recurso r on pr.idrecurso = r.idrecurso
      inner join Permissao f on pr.idpermissao = f.idpermissao
      cross join perfil x
      where (r.visivel = 1 and x.idempresa is null or x.idempresa = '+convert(varchar(10),@IdEmpresa)+') and r.idmodulo = '+convert(varchar(10),@IdModulo)+') PIV
	  PIVOT
	  (count(existepermissao) FOR NomePerfil in (' + @columns + ')) AS chld'

EXECUTE (@query)


END
GO
