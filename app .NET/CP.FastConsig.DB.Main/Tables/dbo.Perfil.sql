CREATE TABLE [dbo].[Perfil]
(
[IDPerfil] [int] NOT NULL IDENTITY(1, 1),
[Nome] [varchar] (50) COLLATE Latin1_General_CI_AI NOT NULL,
[Sigla] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[IDModulo] [int] NOT NULL,
[IDEmpresa] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Perfil] ADD CONSTRAINT [PK_Perfil] PRIMARY KEY CLUSTERED  ([IDPerfil]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_Perfil] ON [dbo].[Perfil] ([IDEmpresa]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Perfil] ADD CONSTRAINT [FK_Perfil_Empresa] FOREIGN KEY ([IDEmpresa]) REFERENCES [dbo].[Empresa] ([IDEmpresa])
GO
