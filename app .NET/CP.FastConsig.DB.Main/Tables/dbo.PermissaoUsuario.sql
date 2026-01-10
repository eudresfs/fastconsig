CREATE TABLE [dbo].[PermissaoUsuario]
(
[IDPermissaoUsuario] [int] NOT NULL IDENTITY(1, 1),
[IDPermissao] [int] NULL,
[IDRecurso] [int] NULL,
[IDPerfil] [int] NULL,
[IDEmpresa] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PermissaoUsuario] ADD CONSTRAINT [PK_PermissaoUsuario] PRIMARY KEY CLUSTERED  ([IDPermissaoUsuario]) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [idx_permissaousuario] ON [dbo].[PermissaoUsuario] ([IDEmpresa], [IDRecurso], [IDPerfil], [IDPermissao]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_permissaousuario3] ON [dbo].[PermissaoUsuario] ([IDPerfil], [IDRecurso], [IDEmpresa], [IDPermissao]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_permissaousuario2] ON [dbo].[PermissaoUsuario] ([IDRecurso], [IDPermissao], [IDPerfil], [IDEmpresa]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PermissaoUsuario] ADD CONSTRAINT [FK_PermissaoUsuario_Empresa] FOREIGN KEY ([IDEmpresa]) REFERENCES [dbo].[Empresa] ([IDEmpresa])
GO
ALTER TABLE [dbo].[PermissaoUsuario] ADD CONSTRAINT [FK_PermissaoUsuario_Perfil] FOREIGN KEY ([IDPerfil]) REFERENCES [dbo].[Perfil] ([IDPerfil])
GO
ALTER TABLE [dbo].[PermissaoUsuario] ADD CONSTRAINT [FK_PermissaoUsuario_Permissao] FOREIGN KEY ([IDPermissao]) REFERENCES [dbo].[Permissao] ([IDPermissao])
GO
ALTER TABLE [dbo].[PermissaoUsuario] ADD CONSTRAINT [FK_PermissaoUsuario_Recurso] FOREIGN KEY ([IDRecurso]) REFERENCES [dbo].[Recurso] ([IDRecurso])
GO
