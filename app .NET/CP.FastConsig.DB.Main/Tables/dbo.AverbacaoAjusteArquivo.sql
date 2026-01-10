CREATE TABLE [dbo].[AverbacaoAjusteArquivo]
(
[IDAverbacaoAjusteArquivo] [int] NOT NULL IDENTITY(1, 1),
[IDAverbacaoAjuste] [int] NOT NULL,
[NumeroLinha] [int] NOT NULL,
[Matricula] [varchar] (15) COLLATE Latin1_General_CI_AI NULL,
[ValorNovo] [decimal] (18, 2) NOT NULL,
[ValorAntigo] [decimal] (18, 2) NULL,
[Critica] [varchar] (300) COLLATE Latin1_General_CI_AI NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AverbacaoAjusteArquivo] ADD CONSTRAINT [PK_ContratoAjusteArquivo] PRIMARY KEY CLUSTERED  ([IDAverbacaoAjusteArquivo]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_contratoajustearquivo] ON [dbo].[AverbacaoAjusteArquivo] ([IDAverbacaoAjuste]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AverbacaoAjusteArquivo] ADD CONSTRAINT [FK_ContratoAjusteArquivo_ContratoAjuste] FOREIGN KEY ([IDAverbacaoAjuste]) REFERENCES [dbo].[AverbacaoAjuste] ([IDAverbacaoAjuste])
GO
