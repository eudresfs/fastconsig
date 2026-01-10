CREATE TABLE [dbo].[TmpInadimplenciaGeral]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Descricao] [nchar] (50) COLLATE Latin1_General_CI_AI NULL,
[Percentual] [decimal] (18, 2) NULL,
[Competencia] [varchar] (7) COLLATE Latin1_General_CI_AI NULL,
[IDEmpresa] [int] NULL,
[Valor] [decimal] (18, 2) NULL
)
GO
ALTER TABLE [dbo].[TmpInadimplenciaGeral] ADD CONSTRAINT [PK_TmpInadimplenciaGeral] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
