CREATE TABLE [dbo].[AverbacaoAjusteTipo]
(
[IDAverbacaoAjusteTipo] [int] NOT NULL,
[Nome] [varchar] (25) COLLATE Latin1_General_CI_AI NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AverbacaoAjusteTipo] ADD CONSTRAINT [PK_ContratoAjusteTipo] PRIMARY KEY CLUSTERED  ([IDAverbacaoAjusteTipo]) ON [PRIMARY]
GO
