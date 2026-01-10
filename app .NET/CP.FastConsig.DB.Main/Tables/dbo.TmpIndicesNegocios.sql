CREATE TABLE [dbo].[TmpIndicesNegocios]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Tipo] [int] NULL,
[Qtde] [int] NULL,
[Margem] [decimal] (18, 2) NULL,
[Valor] [decimal] (18, 2) NULL,
[Lucro] [decimal] (18, 2) NULL,
[Descricao] [nchar] (50) COLLATE Latin1_General_CI_AI NULL,
[DicaValor] [nchar] (100) COLLATE Latin1_General_CI_AI NULL,
[DicaLucro] [nchar] (100) COLLATE Latin1_General_CI_AI NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TmpIndicesNegocios] ADD CONSTRAINT [PK_TmpIndicesNegocios] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
