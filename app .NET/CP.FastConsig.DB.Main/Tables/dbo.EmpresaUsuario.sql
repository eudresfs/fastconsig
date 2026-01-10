CREATE TABLE [dbo].[EmpresaUsuario]
(
[IDEmpresaUsuario] [int] NOT NULL IDENTITY(1, 1),
[IDEmpresa] [int] NOT NULL,
[IDUsuario] [int] NOT NULL,
[CreatedBy] [int] NULL,
[CreatedOn] [datetime] NULL,
[ModifiedBy] [int] NULL,
[ModifiedOn] [datetime] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmpresaUsuario] ADD CONSTRAINT [PK_EmpresaUsuario] PRIMARY KEY CLUSTERED  ([IDEmpresaUsuario]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_EmpresaUsuario] ON [dbo].[EmpresaUsuario] ([IDEmpresa]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_empresausuario2] ON [dbo].[EmpresaUsuario] ([IDUsuario]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmpresaUsuario] ADD CONSTRAINT [FK_EmpresaUsuario_Empresa] FOREIGN KEY ([IDEmpresa]) REFERENCES [dbo].[Empresa] ([IDEmpresa])
GO
ALTER TABLE [dbo].[EmpresaUsuario] ADD CONSTRAINT [FK_EmpresaUsuario_Usuario] FOREIGN KEY ([IDUsuario]) REFERENCES [dbo].[Usuario] ([IDUsuario])
GO
