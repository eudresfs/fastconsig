CREATE TABLE [dbo].[TmpEnviadosVDescontados]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Tipo] [nchar] (30) COLLATE Latin1_General_CI_AI NULL,
[Mes] [nchar] (10) COLLATE Latin1_General_CI_AI NULL,
[Valor] [decimal] (18, 2) NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TmpEnviadosVDescontados] ADD CONSTRAINT [PK_TmpEnviadosVDescontados] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
