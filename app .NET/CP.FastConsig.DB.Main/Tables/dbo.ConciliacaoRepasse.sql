CREATE TABLE [dbo].[ConciliacaoRepasse]
(
[IDConciliacaoRepasse] [int] NOT NULL IDENTITY(1, 1),
[Competencia] [varchar] (7) COLLATE Latin1_General_CI_AI NOT NULL,
[Data] [datetime] NULL,
[IDFonteRecurso] [int] NOT NULL,
[Valor] [decimal] (18, 2) NOT NULL,
[IDEmpresa] [int] NOT NULL,
[Obs] [varchar] (150) COLLATE Latin1_General_CI_AI NULL,
[CreatedBy] [int] NULL,
[CreatedOn] [datetime] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ConciliacaoRepasse] ADD CONSTRAINT [PK_Repasse] PRIMARY KEY CLUSTERED  ([IDConciliacaoRepasse]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_contratorepasse1] ON [dbo].[ConciliacaoRepasse] ([Competencia], [IDEmpresa]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_ContratoRepasse] ON [dbo].[ConciliacaoRepasse] ([IDEmpresa], [Competencia]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ConciliacaoRepasse] ADD CONSTRAINT [FK_ContratoRepasse_FonteRecurso] FOREIGN KEY ([IDFonteRecurso]) REFERENCES [dbo].[FonteRecurso] ([IDFonteRecurso])
GO
