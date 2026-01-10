CREATE TABLE [dbo].[ConciliacaoCorteFolha]
(
[IDConciliacaoCorteFolha] [int] NOT NULL IDENTITY(1, 1),
[Competencia] [varchar] (7) COLLATE Latin1_General_CI_AI NULL,
[IDAverbacaoParcela] [int] NOT NULL,
[IDAverbacaoSituacao] [int] NOT NULL,
[Data] [datetime] NULL,
[IDCorteHistorico] [int] NOT NULL,
[ValorParcela] [decimal] (18, 2) NOT NULL,
[Verba] [varchar] (25) COLLATE Latin1_General_CI_AI NULL,
[Matricula] [varchar] (25) COLLATE Latin1_General_CI_AI NULL,
[IDProdutoGrupo] [int] NULL,
[IDProdutoTipo] [int] NULL,
[IDConsignataria] [int] NULL,
[IDProduto] [int] NULL,
[IDFuncionario] [int] NULL,
[Prazo] [int] NULL,
[Parcela] [int] NULL
)
GO
ALTER TABLE [dbo].[ConciliacaoCorteFolha] ADD CONSTRAINT [PK_ContratoCorteFolha] PRIMARY KEY CLUSTERED  ([IDConciliacaoCorteFolha]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_contratocortefolha] ON [dbo].[ConciliacaoCorteFolha] ([Competencia]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_contratocortefolha2] ON [dbo].[ConciliacaoCorteFolha] ([IDAverbacaoParcela]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_contratocortefolha3] ON [dbo].[ConciliacaoCorteFolha] ([IDAverbacaoSituacao]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_contratocortefolha6] ON [dbo].[ConciliacaoCorteFolha] ([IDConsignataria]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_contratocortefolha5] ON [dbo].[ConciliacaoCorteFolha] ([IDFuncionario]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_contratocortefolha4] ON [dbo].[ConciliacaoCorteFolha] ([Matricula]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ConciliacaoCorteFolha] ADD CONSTRAINT [FK_ContratoCorteFolha_ContratoParcela] FOREIGN KEY ([IDAverbacaoParcela]) REFERENCES [dbo].[AverbacaoParcela] ([IDAverbacaoParcela])
GO
ALTER TABLE [dbo].[ConciliacaoCorteFolha] ADD CONSTRAINT [FK_ContratoCorteFolha_ContratoSituacao] FOREIGN KEY ([IDAverbacaoSituacao]) REFERENCES [dbo].[AverbacaoSituacao] ([IDAverbacaoSituacao])
GO
ALTER TABLE [dbo].[ConciliacaoCorteFolha] ADD CONSTRAINT [FK_ContratoCorteFolha_CorteHistorico] FOREIGN KEY ([IDCorteHistorico]) REFERENCES [dbo].[CorteHistorico] ([IDCorteHistorico])
GO
