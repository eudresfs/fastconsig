CREATE TABLE [dbo].[AverbacaoTipo]
(
[IDAverbacaoTipo] [int] NOT NULL,
[Nome] [varchar] (30) COLLATE Latin1_General_CI_AI NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AverbacaoTipo] ADD CONSTRAINT [PK_ContratoOrigem] PRIMARY KEY CLUSTERED  ([IDAverbacaoTipo]) ON [PRIMARY]
GO
