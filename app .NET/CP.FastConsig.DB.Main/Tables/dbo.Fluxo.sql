CREATE TABLE [dbo].[Fluxo]
(
[IDFluxo] [int] NOT NULL IDENTITY(1, 1),
[IDFluxoTipo] [int] NULL,
[IDProdutoGrupo] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Fluxo] ADD CONSTRAINT [PK_FluxoCompra] PRIMARY KEY CLUSTERED  ([IDFluxo]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Fluxo] ADD CONSTRAINT [FK_Fluxo_FluxoTipo] FOREIGN KEY ([IDFluxoTipo]) REFERENCES [dbo].[FluxoTipo] ([IDFluxoTipo])
GO
ALTER TABLE [dbo].[Fluxo] ADD CONSTRAINT [FK_Fluxo_ProdutoGrupo] FOREIGN KEY ([IDProdutoGrupo]) REFERENCES [dbo].[ProdutoGrupo] ([IDProdutoGrupo])
GO
