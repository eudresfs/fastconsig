CREATE TABLE [dbo].[TmpInadimplenciaGeralDetalhe]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[IDInadimplenciaGeral] [int] NULL,
[Descricao] [nchar] (50) COLLATE Latin1_General_CI_AI NULL,
[Percentual] [decimal] (18, 2) NULL,
[Competencia] [varchar] (7) COLLATE Latin1_General_CI_AI NULL,
[IDEmpresa] [int] NULL,
[Valor] [decimal] (18, 2) NULL
)
GO
ALTER TABLE [dbo].[TmpInadimplenciaGeralDetalhe] ADD CONSTRAINT [PK_TmpInadimplenciaGeralDetalhe] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
