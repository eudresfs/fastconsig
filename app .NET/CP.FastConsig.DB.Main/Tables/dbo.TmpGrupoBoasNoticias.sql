CREATE TABLE [dbo].[TmpGrupoBoasNoticias]
(
[ID] [int] NOT NULL IDENTITY(1, 1),
[Grupo] [nchar] (30) COLLATE Latin1_General_CI_AI NULL,
[Total] [decimal] (18, 2) NULL,
[Possibilidade] [decimal] (18, 2) NULL,
[Competencia] [varchar] (7) COLLATE Latin1_General_CI_AI NULL
)
GO
ALTER TABLE [dbo].[TmpGrupoBoasNoticias] ADD CONSTRAINT [PK_TmpGrupoBoasNoticias] PRIMARY KEY CLUSTERED  ([ID]) ON [PRIMARY]
GO
