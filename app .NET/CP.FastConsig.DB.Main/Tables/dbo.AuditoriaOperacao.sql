CREATE TABLE [dbo].[AuditoriaOperacao]
(
[IDAuditoriaOperacao] [int] NOT NULL,
[Nome] [varchar] (25) COLLATE Latin1_General_CI_AI NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AuditoriaOperacao] ADD CONSTRAINT [PK_AuditoriaOperacao] PRIMARY KEY CLUSTERED  ([IDAuditoriaOperacao]) ON [PRIMARY]
GO
