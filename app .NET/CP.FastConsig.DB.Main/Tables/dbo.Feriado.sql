CREATE TABLE [dbo].[Feriado]
(
[IDFeriado] [int] NOT NULL IDENTITY(1, 1),
[Nome] [varchar] (30) COLLATE Latin1_General_CI_AI NOT NULL,
[Data] [datetime] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Feriado] ADD CONSTRAINT [PK_Feriado] PRIMARY KEY CLUSTERED  ([IDFeriado]) ON [PRIMARY]
GO
