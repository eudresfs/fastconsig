CREATE TABLE [dbo].[Importacao]
(
[IdImportacao] [int] NOT NULL IDENTITY(1, 1),
[IdUsuario] [int] NOT NULL,
[NomeArquivo] [varchar] (100) COLLATE Latin1_General_CI_AI NOT NULL,
[IncluirPrimeiraLinha] [bit] NOT NULL,
[Layout] [varchar] (100) COLLATE Latin1_General_CI_AI NOT NULL,
[NomeLayout] [varchar] (100) COLLATE Latin1_General_CI_AI NOT NULL,
[Observacao] [text] COLLATE Latin1_General_CI_AI NOT NULL,
[Telefone] [varchar] (20) COLLATE Latin1_General_CI_AI NOT NULL,
[IdBanco] [int] NOT NULL,
[Data] [datetime] NOT NULL,
[Ativo] [int] NOT NULL
)
GO
ALTER TABLE [dbo].[Importacao] ADD CONSTRAINT [PK_Importacao] PRIMARY KEY CLUSTERED  ([IdImportacao])
GO
