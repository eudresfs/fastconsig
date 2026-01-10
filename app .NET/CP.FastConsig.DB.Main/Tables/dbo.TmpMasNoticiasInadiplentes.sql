CREATE TABLE [dbo].[TmpMasNoticiasInadiplentes]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[DescricaoGrupo] [nchar] (50) COLLATE Latin1_General_CI_AI NULL,
[Quantidade] [int] NULL,
[NaoConciliado] [decimal] (18, 2) NULL,
[QuantidadeContrato] [int] NULL,
[Parcelas] [int] NULL,
[Competencia] [varchar] (7) COLLATE Latin1_General_CI_AI NULL,
[IDEmpresa] [int] NULL
)
GO
ALTER TABLE [dbo].[TmpMasNoticiasInadiplentes] ADD CONSTRAINT [PK_TmpMasNoticiasInadiplentes] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
