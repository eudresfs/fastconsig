CREATE TABLE [dbo].[PermissaoRecurso]
(
[IDPermissaoRecurso] [int] NOT NULL IDENTITY(1, 1),
[IDPermissao] [int] NOT NULL,
[IDRecurso] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PermissaoRecurso] ADD CONSTRAINT [PK_PermissaoPagina] PRIMARY KEY CLUSTERED  ([IDPermissaoRecurso]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_PermissaoRecurso] ON [dbo].[PermissaoRecurso] ([IDPermissao], [IDRecurso]) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [idx_permissaorecurso_unique] ON [dbo].[PermissaoRecurso] ([IDPermissao], [IDRecurso]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_PermissaoRecurso2] ON [dbo].[PermissaoRecurso] ([IDRecurso], [IDPermissao]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PermissaoRecurso] ADD CONSTRAINT [FK_PermissaoRecurso_Permissao] FOREIGN KEY ([IDPermissao]) REFERENCES [dbo].[Permissao] ([IDPermissao])
GO
ALTER TABLE [dbo].[PermissaoRecurso] ADD CONSTRAINT [FK_PermissaoRecurso_Recurso] FOREIGN KEY ([IDRecurso]) REFERENCES [dbo].[Recurso] ([IDRecurso])
GO
