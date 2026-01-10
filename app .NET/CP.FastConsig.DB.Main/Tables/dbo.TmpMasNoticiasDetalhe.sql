CREATE TABLE [dbo].[TmpMasNoticiasDetalhe]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[IDMasNoticia] [int] NOT NULL,
[Cpf] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[Matricula] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[Nome] [varchar] (80) COLLATE Latin1_General_CI_AI NULL,
[Motivo] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[SaldoDevedor] [decimal] (18, 2) NULL,
[QtdeContratos] [int] NULL,
[IDFuncionario] [int] NULL
)
GO
ALTER TABLE [dbo].[TmpMasNoticiasDetalhe] ADD CONSTRAINT [PK_TmpMasNoticiasDetalhe] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
