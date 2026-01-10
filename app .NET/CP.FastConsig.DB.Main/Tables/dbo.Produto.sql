CREATE TABLE [dbo].[Produto]
(
[IDProduto] [int] NOT NULL IDENTITY(1, 1),
[IDProdutoGrupo] [int] NOT NULL,
[IDConsignataria] [int] NOT NULL,
[IDConsignante] [int] NULL,
[Nome] [varchar] (50) COLLATE Latin1_General_CI_AI NOT NULL,
[Verba] [varchar] (25) COLLATE Latin1_General_CI_AI NOT NULL,
[VerbaFolha] [varchar] (25) COLLATE Latin1_General_CI_AI NULL,
[PrazoMaximo] [int] NULL,
[CarenciaMaxima] [int] NULL,
[DesativadoConsignante] [bit] NOT NULL,
[Ativo] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Produto] ADD CONSTRAINT [PK_Servico] PRIMARY KEY CLUSTERED  ([IDProduto]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_Servico3] ON [dbo].[Produto] ([IDConsignante], [Ativo]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_Servico2] ON [dbo].[Produto] ([IDConsignataria], [Ativo]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_Servico] ON [dbo].[Produto] ([IDProdutoGrupo], [Ativo]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_servico4] ON [dbo].[Produto] ([Verba], [Ativo]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_servico5] ON [dbo].[Produto] ([VerbaFolha], [Ativo]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Produto] ADD CONSTRAINT [FK_Servico_Empresa3] FOREIGN KEY ([IDConsignante]) REFERENCES [dbo].[Empresa] ([IDEmpresa])
GO
ALTER TABLE [dbo].[Produto] ADD CONSTRAINT [FK_Servico_Empresa2] FOREIGN KEY ([IDConsignataria]) REFERENCES [dbo].[Empresa] ([IDEmpresa])
GO
ALTER TABLE [dbo].[Produto] ADD CONSTRAINT [FK_Servico_ServicoGrupo] FOREIGN KEY ([IDProdutoGrupo]) REFERENCES [dbo].[ProdutoGrupo] ([IDProdutoGrupo])
GO
