CREATE TABLE [dbo].[CorteHistorico]
(
[IDCorteHistorico] [int] NOT NULL IDENTITY(1, 1),
[Data] [datetime] NOT NULL,
[Competencia] [varchar] (7) COLLATE Latin1_General_CI_AI NOT NULL,
[DiaCorte] [varchar] (2) COLLATE Latin1_General_CI_AI NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CorteHistorico] ADD CONSTRAINT [PK_CorteHistorico] PRIMARY KEY CLUSTERED  ([IDCorteHistorico]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_cortehistorico] ON [dbo].[CorteHistorico] ([Competencia]) ON [PRIMARY]
GO
