CREATE TABLE [dbo].[FluxoTipo]
(
[IDFluxoTipo] [int] NOT NULL IDENTITY(1, 1),
[Nome] [varchar] (50) COLLATE Latin1_General_CI_AI NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FluxoTipo] ADD CONSTRAINT [PK_FluxoTipo] PRIMARY KEY CLUSTERED  ([IDFluxoTipo]) ON [PRIMARY]
GO
