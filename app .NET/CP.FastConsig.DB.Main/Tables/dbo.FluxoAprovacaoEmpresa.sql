CREATE TABLE [dbo].[FluxoAprovacaoEmpresa]
(
[IDFluxoAprovacaoEmpresa] [int] NOT NULL IDENTITY(1, 1),
[IDProdutoGrupo] [int] NOT NULL,
[IDEmpresa] [int] NOT NULL,
[RequerAprovacao] [bit] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FluxoAprovacaoEmpresa] ADD CONSTRAINT [PK_FluxoAprovacaoEmpresa] PRIMARY KEY CLUSTERED  ([IDFluxoAprovacaoEmpresa]) ON [PRIMARY]
GO
