CREATE TABLE [dbo].[ProdutoTipo]
(
[IDProdutoTipo] [int] NOT NULL,
[Nome] [varchar] (40) COLLATE Latin1_General_CI_AI NOT NULL,
[Descricao] [varchar] (200) COLLATE Latin1_General_CI_AI NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ProdutoTipo] ADD CONSTRAINT [PK_ServicoTipo] PRIMARY KEY CLUSTERED  ([IDProdutoTipo]) ON [PRIMARY]
GO
