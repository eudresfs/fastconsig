CREATE TABLE [dbo].[TmpInadimplenciaPadraoMargem]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Descricao] [nchar] (50) COLLATE Latin1_General_CI_AI NULL,
[Percentual] [decimal] (18, 2) NULL,
[Competencia] [varchar] (7) COLLATE Latin1_General_CI_AI NULL,
[IDEmpresa] [int] NULL,
[QtdeFunc] [int] NULL
)
GO
ALTER TABLE [dbo].[TmpInadimplenciaPadraoMargem] ADD CONSTRAINT [PK_TmpInadimplenciaPadraoMargem] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
