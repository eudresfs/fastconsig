CREATE TABLE [dbo].[RetornoFolhaImportacao]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Matricula] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[Nome] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[CPF] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[Competencia] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[ValorBruto] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[ValorDescontado] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[IdImportacao] [int] NULL
)
GO
ALTER TABLE [dbo].[RetornoFolhaImportacao] ADD CONSTRAINT [PK_RetornoFolhaImportacao] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
