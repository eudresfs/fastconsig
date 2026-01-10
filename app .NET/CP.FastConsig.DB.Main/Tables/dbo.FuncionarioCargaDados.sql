CREATE TABLE [dbo].[FuncionarioCargaDados]
(
[Competencia] [varchar] (7) COLLATE Latin1_General_CI_AI NOT NULL,
[IDFuncionario] [int] NOT NULL,
[IDPessoa] [int] NOT NULL,
[Matricula] [varchar] (15) COLLATE Latin1_General_CI_AI NOT NULL,
[MatriculaAntiga] [varchar] (15) COLLATE Latin1_General_CI_AI NULL,
[DataAdmissao] [datetime] NULL,
[IDConsignante] [int] NULL,
[IDFuncionarioCategoria] [int] NULL,
[IDFuncionarioSituacao] [int] NULL,
[IDFuncionarioRegime] [int] NULL,
[NomeLocalFolha] [varchar] (80) COLLATE Latin1_General_CI_AI NULL,
[NomeSetorFolha] [varchar] (80) COLLATE Latin1_General_CI_AI NULL,
[NomeCargoFolha] [varchar] (80) COLLATE Latin1_General_CI_AI NULL,
[NomeRegimeFolha] [varchar] (80) COLLATE Latin1_General_CI_AI NULL,
[NomeSituacaoFolha] [varchar] (80) COLLATE Latin1_General_CI_AI NULL,
[MargemBruta] [decimal] (18, 2) NULL,
[MargemBase] [decimal] (18, 2) NULL,
[FonteRecurso] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[CreatedBy] [int] NULL,
[CreatedOn] [datetime] NULL,
[ModifiedBy] [int] NULL,
[ModifiedOn] [datetime] NULL,
[Ativo] [int] NOT NULL,
[MESEXONERAR] [varchar] (7) COLLATE Latin1_General_CI_AI NULL
)
GO
ALTER TABLE [dbo].[FuncionarioCargaDados] ADD CONSTRAINT [PK_FuncionarioCargaDados] PRIMARY KEY CLUSTERED  ([Competencia], [IDFuncionario])
GO
ALTER TABLE [dbo].[FuncionarioCargaDados] ADD CONSTRAINT [FK_FuncionarioCargaDados_FuncionarioCategoria] FOREIGN KEY ([IDFuncionarioCategoria]) REFERENCES [dbo].[FuncionarioCategoria] ([IDFuncionarioCategoria])
GO
ALTER TABLE [dbo].[FuncionarioCargaDados] ADD CONSTRAINT [FK_FuncionarioCargaDados_FuncionarioRegime] FOREIGN KEY ([IDFuncionarioRegime]) REFERENCES [dbo].[FuncionarioRegime] ([IDFuncionarioRegime])
GO
ALTER TABLE [dbo].[FuncionarioCargaDados] ADD CONSTRAINT [FK_FuncionarioCargaDados_FuncionarioSituacao] FOREIGN KEY ([IDFuncionarioSituacao]) REFERENCES [dbo].[FuncionarioSituacao] ([IDFuncionarioSituacao])
GO
ALTER TABLE [dbo].[FuncionarioCargaDados] ADD CONSTRAINT [FK_FuncionarioCargaDados_Pessoa] FOREIGN KEY ([IDPessoa]) REFERENCES [dbo].[Pessoa] ([IDPessoa])
GO
