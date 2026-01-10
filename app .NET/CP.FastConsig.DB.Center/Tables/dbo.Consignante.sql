CREATE TABLE [dbo].[Consignante]
(
[IDConsignante] [int] NOT NULL IDENTITY(1, 1),
[Nome] [varchar] (50) COLLATE Latin1_General_CI_AS NULL,
[Logo] [varchar] (50) COLLATE Latin1_General_CI_AS NULL,
[BancoDados] [varchar] (50) COLLATE Latin1_General_CI_AS NULL,
[URL] [varchar] (150) COLLATE Latin1_General_CI_AS NULL,
[Tipo] [varchar] (1) COLLATE Latin1_General_CI_AS NULL,
[Ativo] [bit] NOT NULL CONSTRAINT [DF_Consignante_Ativo] DEFAULT ((1))
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Consignante] ADD CONSTRAINT [PK_Consignante] PRIMARY KEY CLUSTERED  ([IDConsignante]) ON [PRIMARY]
GO
