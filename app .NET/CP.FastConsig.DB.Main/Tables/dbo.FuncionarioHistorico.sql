CREATE TABLE [dbo].[FuncionarioHistorico]
(
[IDFuncionarioHistorico] [int] NOT NULL IDENTITY(1, 1),
[IDFuncionario] [int] NULL,
[IDFuncionarioSituacao] [int] NULL,
[IDFuncionarioRegime] [int] NULL,
[NomeSituacaoFolha] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[NomeRegimeFolha] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[NomeLocalFolha] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[NomeCargoFolha] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[NomeSetorFolha] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[MargemFolhaBruta] [decimal] (18, 2) NULL,
[MargemFolhaDisponivel] [decimal] (18, 2) NULL,
[CreatedBy] [int] NULL,
[CreatedOn] [datetime] NULL,
[Competencia] [varchar] (7) COLLATE Latin1_General_CI_AI NULL
)
GO
ALTER TABLE [dbo].[FuncionarioHistorico] ADD CONSTRAINT [PK_FuncionarioHistorico] PRIMARY KEY CLUSTERED  ([IDFuncionarioHistorico]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_FuncionarioHistorico] ON [dbo].[FuncionarioHistorico] ([IDFuncionario]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_FuncionarioHistorico2] ON [dbo].[FuncionarioHistorico] ([IDFuncionarioSituacao]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FuncionarioHistorico] ADD CONSTRAINT [FK_FuncionarioHistorico_Funcionario] FOREIGN KEY ([IDFuncionario]) REFERENCES [dbo].[Funcionario] ([IDFuncionario])
GO
ALTER TABLE [dbo].[FuncionarioHistorico] ADD CONSTRAINT [FK_FuncionarioHistorico_FuncionarioRegime] FOREIGN KEY ([IDFuncionarioRegime]) REFERENCES [dbo].[FuncionarioRegime] ([IDFuncionarioRegime])
GO
ALTER TABLE [dbo].[FuncionarioHistorico] ADD CONSTRAINT [FK_FuncionarioHistorico_FuncionarioSituacao] FOREIGN KEY ([IDFuncionarioSituacao]) REFERENCES [dbo].[FuncionarioSituacao] ([IDFuncionarioSituacao])
GO
