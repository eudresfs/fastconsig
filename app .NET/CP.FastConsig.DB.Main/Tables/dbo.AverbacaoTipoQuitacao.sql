CREATE TABLE [dbo].[AverbacaoTipoQuitacao]
(
[IDAverbacaoTipoQuitacao] [int] NOT NULL,
[Nome] [varchar] (30) COLLATE Latin1_General_CI_AI NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AverbacaoTipoQuitacao] ADD CONSTRAINT [PK_ContratoTipoQuitacao] PRIMARY KEY CLUSTERED  ([IDAverbacaoTipoQuitacao]) ON [PRIMARY]
GO
