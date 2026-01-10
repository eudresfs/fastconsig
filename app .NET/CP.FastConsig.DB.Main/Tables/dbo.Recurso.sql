CREATE TABLE [dbo].[Recurso]
(
[IDRecurso] [int] NOT NULL IDENTITY(1, 1),
[Nome] [varchar] (50) COLLATE Latin1_General_CI_AI NOT NULL,
[ArquivoHelp] [varchar] (100) COLLATE Latin1_General_CI_AI NULL,
[Arquivo] [varchar] (150) COLLATE Latin1_General_CI_AI NULL,
[IDModulo] [int] NULL,
[Visivel] [bit] NULL,
[Objetivo] [varchar] (500) COLLATE Latin1_General_CI_AI NULL,
[Bloqueado] [bit] NOT NULL,
[Ativo] [int] NULL,
[IDRecursoPai] [int] NULL,
[Imagem] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[ParametrosAoIniciar] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[Ordem] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Recurso] ADD CONSTRAINT [PK_Recurso] PRIMARY KEY CLUSTERED  ([IDRecurso]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_Recurso] ON [dbo].[Recurso] ([IDModulo], [Ativo]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_recurso2] ON [dbo].[Recurso] ([IDRecursoPai], [Ativo]) ON [PRIMARY]
GO
