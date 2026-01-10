CREATE TABLE [dbo].[AverbacaoAjusteDetalhe]
(
[IDAverbacaoAjusteDetalhe] [int] NOT NULL IDENTITY(1, 1),
[IDAverbacaoAjuste] [int] NOT NULL,
[IDFuncionario] [int] NOT NULL,
[IDAverbacao] [int] NOT NULL,
[ValorAtual] [decimal] (18, 2) NOT NULL,
[MargemDisponivel] [decimal] (18, 2) NOT NULL,
[ValorNovo] [decimal] (18, 2) NOT NULL,
[Aplicado] [bit] NOT NULL,
[Critica] [varchar] (300) COLLATE Latin1_General_CI_AI NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AverbacaoAjusteDetalhe] ADD CONSTRAINT [PK_ContratoAjusteDetalhe] PRIMARY KEY CLUSTERED  ([IDAverbacaoAjusteDetalhe]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_contratoajustedetalhe3] ON [dbo].[AverbacaoAjusteDetalhe] ([IDAverbacao]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_contratoajustedetalhe] ON [dbo].[AverbacaoAjusteDetalhe] ([IDAverbacaoAjuste]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_contratoajustedetalhe2] ON [dbo].[AverbacaoAjusteDetalhe] ([IDFuncionario]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AverbacaoAjusteDetalhe] ADD CONSTRAINT [FK_ContratoAjusteDetalhe_Contrato] FOREIGN KEY ([IDAverbacao]) REFERENCES [dbo].[Averbacao] ([IDAverbacao])
GO
ALTER TABLE [dbo].[AverbacaoAjusteDetalhe] ADD CONSTRAINT [FK_ContratoAjusteDetalhe_ContratoAjuste] FOREIGN KEY ([IDAverbacaoAjuste]) REFERENCES [dbo].[AverbacaoAjuste] ([IDAverbacaoAjuste])
GO
ALTER TABLE [dbo].[AverbacaoAjusteDetalhe] ADD CONSTRAINT [FK_ContratoAjusteDetalhe_Funcionario] FOREIGN KEY ([IDFuncionario]) REFERENCES [dbo].[Funcionario] ([IDFuncionario])
GO
