CREATE TABLE [dbo].[AcessoVideo]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Nome] [varchar] (300) COLLATE Latin1_General_CI_AI NOT NULL,
[Email] [varchar] (300) COLLATE Latin1_General_CI_AI NOT NULL,
[Empresa] [varchar] (300) COLLATE Latin1_General_CI_AI NOT NULL,
[DataAcesso] [datetime] NOT NULL,
[Estado] [varchar] (2) COLLATE Latin1_General_CI_AI NULL,
[Telefone] [varchar] (20) COLLATE Latin1_General_CI_AI NULL
)
GO
ALTER TABLE [dbo].[AcessoVideo] ADD CONSTRAINT [PK_AcessoVideo] PRIMARY KEY CLUSTERED  ([Id])
GO
