CREATE TABLE [dbo].[AverbacaoVinculo]
(
[IDAverbacaoVinculo] [int] NOT NULL IDENTITY(1, 1),
[IDAverbacao] [int] NOT NULL,
[IDAverbacaoPai] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AverbacaoVinculo] ADD CONSTRAINT [PK_ContratoVinculo] PRIMARY KEY CLUSTERED  ([IDAverbacaoVinculo]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_ContratoVinculo] ON [dbo].[AverbacaoVinculo] ([IDAverbacao]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_ContratoVinculo2] ON [dbo].[AverbacaoVinculo] ([IDAverbacaoPai]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AverbacaoVinculo] ADD CONSTRAINT [FK_ContratoVinculo_Contrato] FOREIGN KEY ([IDAverbacao]) REFERENCES [dbo].[Averbacao] ([IDAverbacao])
GO
ALTER TABLE [dbo].[AverbacaoVinculo] ADD CONSTRAINT [FK_ContratoVinculo_Contrato1] FOREIGN KEY ([IDAverbacaoPai]) REFERENCES [dbo].[Averbacao] ([IDAverbacao])
GO
