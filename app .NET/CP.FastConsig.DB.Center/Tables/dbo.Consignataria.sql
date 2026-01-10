CREATE TABLE [dbo].[Consignataria]
(
[IDConsignataria] [int] NOT NULL IDENTITY(1, 1),
[Nome] [varchar] (50) COLLATE Latin1_General_CI_AS NULL,
[Logo] [varchar] (50) COLLATE Latin1_General_CI_AS NULL,
[Ativo] [bit] NOT NULL CONSTRAINT [DF_Consignataria_Ativo] DEFAULT ((1))
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Consignataria] ADD CONSTRAINT [PK_Consignataria] PRIMARY KEY CLUSTERED  ([IDConsignataria]) ON [PRIMARY]
GO
