CREATE TABLE [dbo].[TmpInadimplenciaPadraoTrabalho]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Descricao] [nchar] (50) COLLATE Latin1_General_CI_AI NULL,
[Percentual] [decimal] (18, 2) NULL,
[PercentualGeral] [decimal] (18, 2) NULL,
[Competencia] [varchar] (7) COLLATE Latin1_General_CI_AI NULL,
[IDEmpresa] [int] NULL,
[QtdeFunc] [int] NULL
)
GO
ALTER TABLE [dbo].[TmpInadimplenciaPadraoTrabalho] ADD CONSTRAINT [PK_TmpInadimplenciaPadraoTrabalho] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
