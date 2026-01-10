CREATE TABLE [dbo].[TipoPagamento]
(
[IDTipoPagamento] [int] NOT NULL,
[Nome] [varchar] (30) COLLATE Latin1_General_CI_AI NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TipoPagamento] ADD CONSTRAINT [PK_TipoPagamento] PRIMARY KEY CLUSTERED  ([IDTipoPagamento]) ON [PRIMARY]
GO
