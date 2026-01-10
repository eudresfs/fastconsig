CREATE TABLE [dbo].[Estado]
(
[SiglaEstado] [varchar] (2) COLLATE Latin1_General_CI_AI NOT NULL,
[Nome] [varchar] (50) COLLATE Latin1_General_CI_AI NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Estado] ADD CONSTRAINT [PK_Estado] PRIMARY KEY CLUSTERED  ([SiglaEstado]) ON [PRIMARY]
GO
