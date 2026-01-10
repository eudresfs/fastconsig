CREATE TABLE [dbo].[ConciliacaoMovimentoTipo]
(
[IDConciliacaoMovimentoTipo] [int] NOT NULL,
[Nome] [varchar] (30) COLLATE Latin1_General_CI_AI NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ConciliacaoMovimentoTipo] ADD CONSTRAINT [PK_ContratoMovimentoTipo] PRIMARY KEY CLUSTERED  ([IDConciliacaoMovimentoTipo]) ON [PRIMARY]
GO
