CREATE TABLE [dbo].[Parametro]
(
[IDParametro] [int] NOT NULL IDENTITY(1, 1),
[Nome] [varchar] (30) COLLATE Latin1_General_CI_AI NOT NULL,
[Valor] [varchar] (100) COLLATE Latin1_General_CI_AI NOT NULL,
[Tipo] [varchar] (3) COLLATE Latin1_General_CI_AI NULL,
[Dominio] [varchar] (150) COLLATE Latin1_General_CI_AI NULL,
[Descricao] [nvarchar] (70) COLLATE Latin1_General_CI_AI NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Parametro] ADD CONSTRAINT [PK_Parametro] PRIMARY KEY CLUSTERED  ([IDParametro]) ON [PRIMARY]
GO
