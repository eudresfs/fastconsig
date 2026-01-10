CREATE TABLE [dbo].[TmpMasNoticiasInadiplentesDetalhe]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[IDMasNoticiaInadiplente] [int] NOT NULL,
[Cpf] [nchar] (15) COLLATE Latin1_General_CI_AI NULL,
[Matricula] [nchar] (15) COLLATE Latin1_General_CI_AI NULL,
[Nome] [nchar] (80) COLLATE Latin1_General_CI_AI NULL,
[Valor] [decimal] (18, 2) NULL,
[QtdeParcelas] [int] NULL
)
ALTER TABLE [dbo].[TmpMasNoticiasInadiplentesDetalhe] ADD
CONSTRAINT [FK_TmpMasNoticiasInadiplentesDetalhe_TmpMasNoticiasInadiplentes] FOREIGN KEY ([IDMasNoticiaInadiplente]) REFERENCES [dbo].[TmpMasNoticiasInadiplentes] ([Id])
GO
ALTER TABLE [dbo].[TmpMasNoticiasInadiplentesDetalhe] ADD CONSTRAINT [PK_TmpMasNoticiasInadiplentesDetalhe] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
