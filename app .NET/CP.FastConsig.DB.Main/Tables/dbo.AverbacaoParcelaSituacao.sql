CREATE TABLE [dbo].[AverbacaoParcelaSituacao]
(
[IDAverbacaoParcelaSituacao] [int] NOT NULL,
[Nome] [varchar] (50) COLLATE Latin1_General_CI_AI NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AverbacaoParcelaSituacao] ADD CONSTRAINT [PK_ContratoParcelaSituacao] PRIMARY KEY CLUSTERED  ([IDAverbacaoParcelaSituacao]) ON [PRIMARY]
GO
