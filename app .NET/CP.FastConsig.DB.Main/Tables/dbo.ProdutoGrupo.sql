CREATE TABLE [dbo].[ProdutoGrupo]
(
[IDProdutoGrupo] [int] NOT NULL,
[IDProdutoTipo] [int] NOT NULL,
[IDConsignante] [int] NULL,
[Nome] [varchar] (50) COLLATE Latin1_General_CI_AI NOT NULL,
[PercentualMargemBruta] [smallmoney] NULL,
[PrioridadeDesconto] [smallint] NULL,
[IDProdutoGrupoCompartilha] [int] NULL CONSTRAINT [DF_ServicoGrupo_IDServicoGrupoCompartilha] DEFAULT ((0)),
[CarenciaMaximaConsignante] [int] NULL,
[PrazoMaximoConsignante] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ProdutoGrupo] ADD CONSTRAINT [PK_ServicoGrupo] PRIMARY KEY CLUSTERED  ([IDProdutoGrupo]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_servicogrupo] ON [dbo].[ProdutoGrupo] ([IDProdutoTipo]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ProdutoGrupo] ADD CONSTRAINT [FK_ServicoGrupo_Empresa] FOREIGN KEY ([IDConsignante]) REFERENCES [dbo].[Empresa] ([IDEmpresa])
GO
ALTER TABLE [dbo].[ProdutoGrupo] ADD CONSTRAINT [FK_ProdutoGrupo_ProdutoGrupo] FOREIGN KEY ([IDProdutoGrupo]) REFERENCES [dbo].[ProdutoGrupo] ([IDProdutoGrupo])
GO
ALTER TABLE [dbo].[ProdutoGrupo] ADD CONSTRAINT [FK_ServicoGrupo_ServicoTipo] FOREIGN KEY ([IDProdutoTipo]) REFERENCES [dbo].[ProdutoTipo] ([IDProdutoTipo])
GO
