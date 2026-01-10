CREATE TABLE [dbo].[Funcionario]
(
[IDFuncionario] [int] NOT NULL IDENTITY(1, 1),
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
ALTER TABLE [dbo].[Funcionario] ADD CONSTRAINT [PK_Favorecido] PRIMARY KEY CLUSTERED  ([IDFuncionario]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_Funcionario3] ON [dbo].[Funcionario] ([IDConsignante]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_Funcionario4] ON [dbo].[Funcionario] ([IDFuncionarioCategoria]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_Funcionario6] ON [dbo].[Funcionario] ([IDFuncionarioRegime]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_Funcionario5] ON [dbo].[Funcionario] ([IDFuncionarioSituacao]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_Funcionario] ON [dbo].[Funcionario] ([IDPessoa]) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [idx_funcionario_matricula] ON [dbo].[Funcionario] ([Matricula]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Funcionario] ADD CONSTRAINT [FK_Funcionario_FuncionarioCategoria] FOREIGN KEY ([IDFuncionarioCategoria]) REFERENCES [dbo].[FuncionarioCategoria] ([IDFuncionarioCategoria])
GO
ALTER TABLE [dbo].[Funcionario] ADD CONSTRAINT [FK_Funcionario_FuncionarioRegime] FOREIGN KEY ([IDFuncionarioRegime]) REFERENCES [dbo].[FuncionarioRegime] ([IDFuncionarioRegime])
GO
ALTER TABLE [dbo].[Funcionario] ADD CONSTRAINT [FK_Funcionario_FuncionarioSituacao] FOREIGN KEY ([IDFuncionarioSituacao]) REFERENCES [dbo].[FuncionarioSituacao] ([IDFuncionarioSituacao])
GO
ALTER TABLE [dbo].[Funcionario] ADD CONSTRAINT [FK_Funcionario_Pessoa] FOREIGN KEY ([IDPessoa]) REFERENCES [dbo].[Pessoa] ([IDPessoa])
GO
CREATE FULLTEXT INDEX ON [dbo].[Funcionario] KEY INDEX [PK_Favorecido] ON [INDEXACAO]
GO
ALTER FULLTEXT INDEX ON [dbo].[Funcionario] ADD ([Matricula] LANGUAGE 1033)
GO
