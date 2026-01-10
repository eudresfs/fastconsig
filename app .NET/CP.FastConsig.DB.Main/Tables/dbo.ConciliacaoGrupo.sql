CREATE TABLE [dbo].[ConciliacaoGrupo]
(
[IDConciliacaoGrupo] [int] NOT NULL,
[Nome] [varchar] (20) COLLATE Latin1_General_CI_AI NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ConciliacaoGrupo] ADD CONSTRAINT [PK_ConciliacaoGrupo] PRIMARY KEY CLUSTERED  ([IDConciliacaoGrupo]) ON [PRIMARY]
GO
