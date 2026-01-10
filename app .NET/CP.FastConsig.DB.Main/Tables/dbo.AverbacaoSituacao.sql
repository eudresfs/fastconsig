CREATE TABLE [dbo].[AverbacaoSituacao]
(
[IDAverbacaoSituacao] [int] NOT NULL,
[Nome] [varchar] (50) COLLATE Latin1_General_CI_AI NOT NULL,
[DeduzMargem] [bit] NOT NULL,
[Compra] [bit] NULL,
[ParaDescontoFolha] [bit] NULL,
[Liquidavel] [bit] NULL,
[Cancelavel] [bit] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AverbacaoSituacao] ADD CONSTRAINT [PK_ContratoSituacao] PRIMARY KEY CLUSTERED  ([IDAverbacaoSituacao]) ON [PRIMARY]
GO
