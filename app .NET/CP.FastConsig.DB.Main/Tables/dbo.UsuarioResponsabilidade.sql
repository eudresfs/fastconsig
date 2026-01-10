CREATE TABLE [dbo].[UsuarioResponsabilidade]
(
[IDUsuarioResponsabilidade] [int] NOT NULL IDENTITY(1, 1),
[IDUsuario] [int] NOT NULL,
[IDEmpresa] [int] NOT NULL,
[IDEmpresaSolicitacaoTipo] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UsuarioResponsabilidade] ADD CONSTRAINT [PK_UsuarioResponsabilidade] PRIMARY KEY CLUSTERED  ([IDUsuarioResponsabilidade]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [URUsuario] ON [dbo].[UsuarioResponsabilidade] ([IDEmpresa], [IDUsuario]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UsuarioResponsabilidade] ADD CONSTRAINT [FK_UsuarioResponsabilidade_Empresa] FOREIGN KEY ([IDEmpresa]) REFERENCES [dbo].[Empresa] ([IDEmpresa])
GO
ALTER TABLE [dbo].[UsuarioResponsabilidade] ADD CONSTRAINT [FK_UsuarioResponsabilidade_EmpresaSolicitacaoTipo] FOREIGN KEY ([IDEmpresaSolicitacaoTipo]) REFERENCES [dbo].[EmpresaSolicitacaoTipo] ([IDEmpresaSolicitacaoTipo])
GO
ALTER TABLE [dbo].[UsuarioResponsabilidade] ADD CONSTRAINT [FK_UsuarioResponsabilidade_Usuario] FOREIGN KEY ([IDUsuario]) REFERENCES [dbo].[Usuario] ([IDUsuario])
GO
