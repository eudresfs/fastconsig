CREATE TABLE [dbo].[AverbacaoHistorico]
(
[IDAverbacaoHistorico] [int] NOT NULL IDENTITY(1, 1),
[IDAverbacao] [int] NULL,
[ValorParcela] [decimal] (18, 2) NULL,
[Prazo] [int] NULL,
[CompetenciaInicial] [varchar] (7) COLLATE Latin1_General_CI_AI NULL,
[Obs] [varchar] (250) COLLATE Latin1_General_CI_AI NULL,
[CreatedBy] [int] NULL,
[CreatedOn] [datetime] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AverbacaoHistorico] ADD CONSTRAINT [PK_ContratoHistorico] PRIMARY KEY CLUSTERED  ([IDAverbacaoHistorico]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_contratohistorico] ON [dbo].[AverbacaoHistorico] ([IDAverbacao]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AverbacaoHistorico] ADD CONSTRAINT [FK_ContratoHistorico_Contrato] FOREIGN KEY ([IDAverbacao]) REFERENCES [dbo].[Averbacao] ([IDAverbacao])
GO
