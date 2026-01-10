CREATE TABLE [dbo].[Modulo]
(
[IDModulo] [int] NOT NULL,
[Nome] [varchar] (30) COLLATE Latin1_General_CI_AI NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Modulo] ADD CONSTRAINT [PK_Modulo] PRIMARY KEY CLUSTERED  ([IDModulo]) ON [PRIMARY]
GO
