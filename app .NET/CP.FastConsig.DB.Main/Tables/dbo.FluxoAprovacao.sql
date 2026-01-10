CREATE TABLE [dbo].[FluxoAprovacao]
(
[IDFluxoAprovacao] [int] NOT NULL IDENTITY(1, 1),
[IDProdutoGrupo] [int] NOT NULL,
[RequerAprovacaoConsignante] [bit] NOT NULL,
[RequerAprovacaoConsignataria] [bit] NOT NULL,
[RequerAprovacaoFuncionario] [bit] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FluxoAprovacao] ADD CONSTRAINT [PK_AprovacaoFluxo] PRIMARY KEY CLUSTERED  ([IDFluxoAprovacao]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_aprovacaofluxo] ON [dbo].[FluxoAprovacao] ([IDProdutoGrupo]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FluxoAprovacao] ADD CONSTRAINT [FK_AprovacaoFluxo_ServicoGrupo] FOREIGN KEY ([IDProdutoGrupo]) REFERENCES [dbo].[ProdutoGrupo] ([IDProdutoGrupo])
GO
