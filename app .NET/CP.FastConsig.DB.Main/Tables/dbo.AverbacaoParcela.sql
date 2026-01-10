CREATE TABLE [dbo].[AverbacaoParcela]
(
[IDAverbacaoParcela] [int] NOT NULL IDENTITY(1, 1),
[IDAverbacao] [int] NOT NULL,
[IDAverbacaoParcelaSituacao] [int] NOT NULL,
[Numero] [int] NOT NULL,
[Competencia] [varchar] (7) COLLATE Latin1_General_CI_AI NOT NULL,
[Valor] [decimal] (18, 2) NOT NULL,
[ValorDescontado] [decimal] (18, 2) NULL,
[Observacao] [varchar] (80) COLLATE Latin1_General_CI_AI NULL,
[CreatedBy] [int] NULL,
[CreatedOn] [datetime] NULL,
[ModifiedBy] [int] NULL,
[ModifiedOn] [datetime] NULL,
[Ativo] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AverbacaoParcela] ADD CONSTRAINT [PK_ContratoParcela] PRIMARY KEY CLUSTERED  ([IDAverbacaoParcela]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_competencia] ON [dbo].[AverbacaoParcela] ([Competencia], [Ativo]) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [idx_contratoparcela] ON [dbo].[AverbacaoParcela] ([IDAverbacao], [Competencia], [Ativo]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AverbacaoParcela] ADD CONSTRAINT [FK_ContratoParcela_Contrato] FOREIGN KEY ([IDAverbacao]) REFERENCES [dbo].[Averbacao] ([IDAverbacao])
GO
ALTER TABLE [dbo].[AverbacaoParcela] ADD CONSTRAINT [FK_ContratoParcela_ContratoParcelaSituacao] FOREIGN KEY ([IDAverbacaoParcelaSituacao]) REFERENCES [dbo].[AverbacaoParcelaSituacao] ([IDAverbacaoParcelaSituacao])
GO
