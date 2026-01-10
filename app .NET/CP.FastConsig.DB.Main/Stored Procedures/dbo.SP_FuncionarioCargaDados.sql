SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_FuncionarioCargaDados] 
@Competencia varchar(7) AS
BEGIN
	SET NOCOUNT ON;
	
	-- limpar carga da competencia, caso novo arquivo seja importado
	delete from funcionariocargadados where competencia = @Competencia
	delete from funcionariomargemcargadados where competencia = @Competencia
	
	-- inclusao da carga dos dados de funcionarios após importação dos dados
	insert into funcionariocargadados
	(Competencia, IDFuncionario, IDPessoa, Matricula, MatriculaAntiga, DataAdmissao, IDConsignante, IDFuncionarioCategoria, IDFuncionarioSituacao, IDFuncionarioRegime, NomeLocalFolha, NomeSetorFolha, NomeCargoFolha, NomeRegimeFolha, NomeSituacaoFolha, MargemBruta, MargemBase, FonteRecurso, CreatedBy,  CreatedOn, ModifiedBy,  ModifiedOn, Ativo, MESEXONERAR)
    select @Competencia, IDFuncionario, IDPessoa, Matricula, MatriculaAntiga, DataAdmissao, IDConsignante, IDFuncionarioCategoria, IDFuncionarioSituacao, IDFuncionarioRegime, NomeLocalFolha, NomeSetorFolha, NomeCargoFolha, NomeRegimeFolha, NomeSituacaoFolha, MargemBruta, MargemBase, FonteRecurso, CreatedBy,  CreatedOn, ModifiedBy,  ModifiedOn, Ativo, MESEXONERAR 
    from funcionario
    
	-- inclusao da carga dos dados de margem dos funcionarios após importação dos dados
    insert into FuncionarioMargemCargaDados(Competencia, IDFuncionarioMargem, IDFuncionario, IDProdutoGrupo, MargemFolha, MargemFolhaDisponivel, CreatedBy, CreatedOn, ModifiedBy,  ModifiedOn)
	select @Competencia, IDFuncionarioMargem, IDFuncionario, IDProdutoGrupo, MargemFolha, MargemFolhaDisponivel, CreatedBy, CreatedOn, ModifiedBy,  ModifiedOn 
	from FuncionarioMargem

	exec SP_FuncionarioImpactoAlteracoes @Competencia   
END
GO
