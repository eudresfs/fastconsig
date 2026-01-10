CREATE TABLE [dbo].[TmpBoasNoticiasDetalhe]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[IdBoaNoticia] [int] NULL,
[Cpf] [nchar] (15) COLLATE Latin1_General_CI_AI NULL,
[Matricula] [nchar] (10) COLLATE Latin1_General_CI_AI NULL,
[Nome] [nchar] (80) COLLATE Latin1_General_CI_AI NULL,
[MargemAnterior] [decimal] (18, 2) NULL,
[MargemAtual] [decimal] (18, 2) NULL,
[Possibilidade] [decimal] (18, 2) NULL,
[SaldoRestante] [decimal] (18, 2) NULL,
[Telefone] [nchar] (20) COLLATE Latin1_General_CI_AI NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TmpBoasNoticiasDetalhe] ADD CONSTRAINT [PK_TmpBoasNoticiasDetalhe] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
