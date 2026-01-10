CREATE TABLE [dbo].[Averbacao]
(
[IDAverbacao] [int] NOT NULL IDENTITY(1, 1),
[IDProduto] [int] NOT NULL,
[IDConsignataria] [int] NOT NULL,
[IDAgente] [int] NULL,
[IDFuncionario] [int] NOT NULL,
[IDAverbacaoSituacao] [int] NOT NULL,
[IDAverbacaoTipo] [int] NOT NULL,
[IDUsuario] [int] NOT NULL,
[Data] [datetime] NOT NULL,
[Identificador] [varchar] (20) COLLATE Latin1_General_CI_AI NULL,
[Numero] [varchar] (20) COLLATE Latin1_General_CI_AI NULL,
[ValorParcela] [decimal] (18, 2) NOT NULL,
[ValorDeducaoMargem] [decimal] (18, 2) NOT NULL,
[Prazo] [int] NULL,
[CET] [decimal] (18, 2) NULL,
[ValorContratado] [decimal] (18, 2) NOT NULL,
[ValorDevidoTotal] [decimal] (18, 2) NULL CONSTRAINT [DF_Averbacao_ValorDevidoTotal] DEFAULT ((0)),
[ValorRefinanciado] [decimal] (18, 2) NULL CONSTRAINT [DF_Averbacao_ValorRefinanciamento] DEFAULT ((0)),
[CompetenciaInicial] [varchar] (7) COLLATE Latin1_General_CI_AI NULL,
[CompetenciaFinal] [varchar] (7) COLLATE Latin1_General_CI_AI NULL,
[Coeficiente] [decimal] (18, 6) NULL,
[Obs] [varchar] (500) COLLATE Latin1_General_CI_AI NULL,
[PrazoAprovacao] [datetime] NULL,
[IDAverbacaoPai] [int] NULL,
[CreatedBy] [int] NULL,
[CreatedOn] [datetime] NULL,
[ModifiedBy] [int] NULL,
[ModifiedOn] [datetime] NULL,
[Ativo] [int] NOT NULL,
[ValorTroco] [decimal] (18, 2) NOT NULL CONSTRAINT [DF__Averbacao__Valor__6551A4FC] DEFAULT ((0))
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Averbacao] ADD CONSTRAINT [PK_Contrato] PRIMARY KEY CLUSTERED  ([IDAverbacao]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_contrato8] ON [dbo].[Averbacao] ([CompetenciaInicial], [CompetenciaFinal]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_contrato6] ON [dbo].[Averbacao] ([Data]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_contrato2] ON [dbo].[Averbacao] ([IDAgente]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_Contrato9] ON [dbo].[Averbacao] ([IDAverbacaoPai]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_contrato4] ON [dbo].[Averbacao] ([IDAverbacaoSituacao]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_contrato5] ON [dbo].[Averbacao] ([IDAverbacaoTipo]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_contrato1] ON [dbo].[Averbacao] ([IDConsignataria]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_contrato3] ON [dbo].[Averbacao] ([IDFuncionario]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_servico] ON [dbo].[Averbacao] ([IDProduto]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_contrato7] ON [dbo].[Averbacao] ([Numero]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Averbacao] ADD CONSTRAINT [FK_Contrato_Empresa] FOREIGN KEY ([IDAgente]) REFERENCES [dbo].[Empresa] ([IDEmpresa])
GO
ALTER TABLE [dbo].[Averbacao] ADD CONSTRAINT [FK_Contrato_Empresa2] FOREIGN KEY ([IDAgente]) REFERENCES [dbo].[Empresa] ([IDEmpresa])
GO
ALTER TABLE [dbo].[Averbacao] ADD CONSTRAINT [FK_Contrato_ContratoSituacao] FOREIGN KEY ([IDAverbacaoSituacao]) REFERENCES [dbo].[AverbacaoSituacao] ([IDAverbacaoSituacao])
GO
ALTER TABLE [dbo].[Averbacao] ADD CONSTRAINT [FK_Contrato_ContratoTipo] FOREIGN KEY ([IDAverbacaoTipo]) REFERENCES [dbo].[AverbacaoTipo] ([IDAverbacaoTipo])
GO
ALTER TABLE [dbo].[Averbacao] ADD CONSTRAINT [FK_Contrato_Empresa1] FOREIGN KEY ([IDConsignataria]) REFERENCES [dbo].[Empresa] ([IDEmpresa])
GO
ALTER TABLE [dbo].[Averbacao] ADD CONSTRAINT [FK_Contrato_Funcionario] FOREIGN KEY ([IDFuncionario]) REFERENCES [dbo].[Funcionario] ([IDFuncionario])
GO
ALTER TABLE [dbo].[Averbacao] ADD CONSTRAINT [FK_Contrato_Servico] FOREIGN KEY ([IDProduto]) REFERENCES [dbo].[Produto] ([IDProduto])
GO
ALTER TABLE [dbo].[Averbacao] ADD CONSTRAINT [FK_Contrato_Usuario] FOREIGN KEY ([IDUsuario]) REFERENCES [dbo].[Usuario] ([IDUsuario])
GO
CREATE FULLTEXT INDEX ON [dbo].[Averbacao] KEY INDEX [PK_Contrato] ON [INDEXACAO]
GO
ALTER FULLTEXT INDEX ON [dbo].[Averbacao] ADD ([Identificador] LANGUAGE 1033)
GO
ALTER FULLTEXT INDEX ON [dbo].[Averbacao] ADD ([Numero] LANGUAGE 1033)
GO
