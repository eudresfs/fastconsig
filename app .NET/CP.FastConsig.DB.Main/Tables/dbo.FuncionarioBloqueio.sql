CREATE TABLE [dbo].[FuncionarioBloqueio]
(
[IDFuncionarioBloqueio] [int] NOT NULL IDENTITY(1, 1),
[IDFuncionario] [int] NOT NULL,
[DataBloqueio] [datetime] NOT NULL,
[DataDesbloqueio] [datetime] NULL,
[TipoBloqueio] [varchar] (1) COLLATE Latin1_General_CI_AI NOT NULL,
[Chaves] [varchar] (2000) COLLATE Latin1_General_CI_AI NULL,
[Motivo] [varchar] (100) COLLATE Latin1_General_CI_AI NOT NULL,
[CreatedBy] [int] NULL,
[CreatedOn] [datetime] NULL,
[ModifiedBy] [int] NULL,
[ModifiedOn] [datetime] NULL,
[Ativo] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FuncionarioBloqueio] ADD CONSTRAINT [PK_FuncionarioBloqueio] PRIMARY KEY CLUSTERED  ([IDFuncionarioBloqueio]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_FuncionarioBloqueio] ON [dbo].[FuncionarioBloqueio] ([IDFuncionario]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FuncionarioBloqueio] ADD CONSTRAINT [FK_FuncionarioBloqueio_Funcionario] FOREIGN KEY ([IDFuncionario]) REFERENCES [dbo].[Funcionario] ([IDFuncionario])
GO
EXEC sp_addextendedproperty N'MS_Description', N'C/E/S (completo/empresa/serviço)', 'SCHEMA', N'dbo', 'TABLE', N'FuncionarioBloqueio', 'COLUMN', N'TipoBloqueio'
GO
