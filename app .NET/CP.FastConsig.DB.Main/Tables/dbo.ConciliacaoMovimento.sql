CREATE TABLE [dbo].[ConciliacaoMovimento]
(
[IDConciliacaoMovimento] [int] NOT NULL IDENTITY(1, 1),
[IDConciliacaoCorteFolha] [int] NULL,
[IDConciliacaoMovimentoTipo] [int] NULL,
[Competencia] [varchar] (7) COLLATE Latin1_General_CI_AI NULL,
[Matricula] [varchar] (15) COLLATE Latin1_General_CI_AI NULL,
[Verba] [varchar] (20) COLLATE Latin1_General_CI_AI NULL,
[Valor] [decimal] (18, 2) NOT NULL,
[ValorMovimento] [decimal] (18, 2) NOT NULL,
[ValorDescontado] [decimal] (18, 2) NOT NULL,
[IDConciliacaoRetorno] [int] NULL,
[IDConsignataria] [int] NULL,
[IDProduto] [int] NULL,
[IDAverbacaoParcela] [int] NULL,
[Prazo] [int] NULL,
[Parcela] [int] NULL
)
GO
ALTER TABLE [dbo].[ConciliacaoMovimento] ADD CONSTRAINT [PK_ContratoMovimento] PRIMARY KEY CLUSTERED  ([IDConciliacaoMovimento]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_contratomovimento2] ON [dbo].[ConciliacaoMovimento] ([Competencia], [Matricula], [Verba]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_contratomovimento3] ON [dbo].[ConciliacaoMovimento] ([Competencia], [Verba], [Matricula]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_contratomovimento] ON [dbo].[ConciliacaoMovimento] ([IDConciliacaoCorteFolha]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ConciliacaoMovimento] ADD CONSTRAINT [FK_ContratoMovimento_ContratoCorteFolha] FOREIGN KEY ([IDConciliacaoCorteFolha]) REFERENCES [dbo].[ConciliacaoCorteFolha] ([IDConciliacaoCorteFolha])
GO
ALTER TABLE [dbo].[ConciliacaoMovimento] ADD CONSTRAINT [FK_ContratoMovimento_ContratoMovimentoTipo] FOREIGN KEY ([IDConciliacaoMovimentoTipo]) REFERENCES [dbo].[ConciliacaoMovimentoTipo] ([IDConciliacaoMovimentoTipo])
GO
