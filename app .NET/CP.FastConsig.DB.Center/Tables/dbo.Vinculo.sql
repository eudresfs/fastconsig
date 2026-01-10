CREATE TABLE [dbo].[Vinculo]
(
[IDVinculo] [int] NOT NULL IDENTITY(1, 1),
[IDConsignataria] [int] NOT NULL,
[IDConsignante] [int] NOT NULL,
[Ativo] [bit] NOT NULL CONSTRAINT [DF_Vinculo_Ativo] DEFAULT ((1))
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Vinculo] ADD CONSTRAINT [PK_Vinculo] PRIMARY KEY CLUSTERED  ([IDVinculo]) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Vinculo] ON [dbo].[Vinculo] ([IDConsignante], [IDConsignataria]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Vinculo] ADD CONSTRAINT [FK_Vinculo_Consignante] FOREIGN KEY ([IDConsignante]) REFERENCES [dbo].[Consignante] ([IDConsignante])
GO
ALTER TABLE [dbo].[Vinculo] ADD CONSTRAINT [FK_Vinculo_Consignataria] FOREIGN KEY ([IDConsignataria]) REFERENCES [dbo].[Consignataria] ([IDConsignataria])
GO
