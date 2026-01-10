CREATE TABLE [dbo].[UsuarioPerfil]
(
[IDUsuarioPerfil] [int] NOT NULL IDENTITY(1, 1),
[IDUsuario] [int] NOT NULL,
[IDPerfil] [int] NOT NULL,
[IDEmpresa] [int] NOT NULL,
[CreatedBy] [int] NULL,
[CreatedOn] [datetime] NULL,
[ModifiedBy] [int] NULL,
[ModifiedOn] [datetime] NULL,
[Bloqueado] [bit] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UsuarioPerfil] ADD CONSTRAINT [PK_UsuarioPerfil] PRIMARY KEY CLUSTERED  ([IDUsuarioPerfil]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_UsuarioPerfil2] ON [dbo].[UsuarioPerfil] ([IDEmpresa], [IDPerfil]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_usuarioperifil3] ON [dbo].[UsuarioPerfil] ([IDPerfil], [IDEmpresa]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_UsuarioPerfil] ON [dbo].[UsuarioPerfil] ([IDUsuario]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UsuarioPerfil] ADD CONSTRAINT [FK_UsuarioPerfil_Empresa] FOREIGN KEY ([IDEmpresa]) REFERENCES [dbo].[Empresa] ([IDEmpresa])
GO
ALTER TABLE [dbo].[UsuarioPerfil] ADD CONSTRAINT [FK_UsuarioPerfil_Perfil] FOREIGN KEY ([IDPerfil]) REFERENCES [dbo].[Perfil] ([IDPerfil])
GO
ALTER TABLE [dbo].[UsuarioPerfil] ADD CONSTRAINT [FK_UsuarioPerfil_Usuario] FOREIGN KEY ([IDUsuario]) REFERENCES [dbo].[Usuario] ([IDUsuario])
GO
