CREATE TABLE [dbo].[ConciliacaoTipo]
(
[IDConciliacaoTipo] [int] NOT NULL,
[Nome] [varchar] (40) COLLATE Latin1_General_CI_AI NULL,
[IDConciliacaoGrupo] [int] NOT NULL CONSTRAINT [DF_ContratoConciliacaoTipo_Conciliado] DEFAULT ((0))
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ConciliacaoTipo] ADD CONSTRAINT [PK_ContratoConciliacaoTipo] PRIMARY KEY CLUSTERED  ([IDConciliacaoTipo]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_contratoconciliacaotipo] ON [dbo].[ConciliacaoTipo] ([IDConciliacaoGrupo]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ConciliacaoTipo] ADD CONSTRAINT [FK_ContratoConciliacaoTipo_ConciliacaoGrupo] FOREIGN KEY ([IDConciliacaoGrupo]) REFERENCES [dbo].[ConciliacaoGrupo] ([IDConciliacaoGrupo])
GO
