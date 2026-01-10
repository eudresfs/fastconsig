CREATE TABLE [dbo].[ImportacaoLayout]
(
[IdImportacaoLayout] [int] NOT NULL IDENTITY(1, 1),
[NomeTabela] [varchar] (50) COLLATE Latin1_General_CI_AI NOT NULL,
[Filtros] [text] COLLATE Latin1_General_CI_AI NOT NULL,
[NomeLayout] [varchar] (300) COLLATE Latin1_General_CI_AI NOT NULL,
[IdBanco] [int] NOT NULL,
[IdUsuario] [int] NOT NULL,
[Ativo] [int] NOT NULL,
[Colunas] [text] COLLATE Latin1_General_CI_AI NOT NULL
)
GO
ALTER TABLE [dbo].[ImportacaoLayout] ADD CONSTRAINT [PK_ImportacaoLayout] PRIMARY KEY CLUSTERED  ([IdImportacaoLayout])
GO
