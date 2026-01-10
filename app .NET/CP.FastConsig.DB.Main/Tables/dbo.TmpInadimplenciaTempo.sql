CREATE TABLE [dbo].[TmpInadimplenciaTempo]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Descricao] [nchar] (50) COLLATE Latin1_General_CI_AI NULL,
[Valor] [decimal] (18, 2) NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TmpInadimplenciaTempo] ADD CONSTRAINT [PK_TmpInadimplenciaTempo] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
