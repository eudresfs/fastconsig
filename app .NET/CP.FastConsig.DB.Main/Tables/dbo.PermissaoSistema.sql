CREATE TABLE [dbo].[PermissaoSistema]
(
[IDPermissaoSistema] [int] NOT NULL IDENTITY(1, 1),
[IDPermissao] [int] NOT NULL,
[IDRecurso] [int] NOT NULL,
[IDPerfil] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PermissaoSistema] ADD CONSTRAINT [PK_PermissaoSistema] PRIMARY KEY CLUSTERED  ([IDPermissaoSistema]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_PermissaoSistema2] ON [dbo].[PermissaoSistema] ([IDPerfil], [IDPermissao], [IDRecurso]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_PermissaoSistema] ON [dbo].[PermissaoSistema] ([IDPermissao], [IDRecurso], [IDPerfil]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_PermissaoSistema3] ON [dbo].[PermissaoSistema] ([IDRecurso], [IDPermissao], [IDPerfil]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PermissaoSistema] ADD CONSTRAINT [FK_PermissaoSistema_Permissao] FOREIGN KEY ([IDPermissao]) REFERENCES [dbo].[Permissao] ([IDPermissao])
GO
ALTER TABLE [dbo].[PermissaoSistema] ADD CONSTRAINT [FK_PermissaoSistema_Recurso] FOREIGN KEY ([IDRecurso]) REFERENCES [dbo].[Recurso] ([IDRecurso])
GO
