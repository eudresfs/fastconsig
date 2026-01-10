CREATE TABLE [dbo].[EmpresaContato]
(
[IDEmpresaContato] [int] NOT NULL IDENTITY(1, 1),
[IDEmpresa] [int] NOT NULL,
[IDEmpresaContatoPerfil] [int] NULL,
[IDContatoTipo] [int] NOT NULL,
[Nome] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[Titulo] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[Conteudo] [varchar] (50) COLLATE Latin1_General_CI_AI NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmpresaContato] ADD CONSTRAINT [PK_EmpresaContato] PRIMARY KEY CLUSTERED  ([IDEmpresaContato]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_EmpresaContato] ON [dbo].[EmpresaContato] ([IDEmpresa]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmpresaContato] ADD CONSTRAINT [FK_EmpresaContato_ContatoTipo] FOREIGN KEY ([IDContatoTipo]) REFERENCES [dbo].[ContatoTipo] ([IDContatoTipo])
GO
ALTER TABLE [dbo].[EmpresaContato] ADD CONSTRAINT [FK_EmpresaContato_Empresa] FOREIGN KEY ([IDEmpresa]) REFERENCES [dbo].[Empresa] ([IDEmpresa])
GO
ALTER TABLE [dbo].[EmpresaContato] ADD CONSTRAINT [FK_EmpresaContato_EmpresaContatoPerfil] FOREIGN KEY ([IDEmpresaContatoPerfil]) REFERENCES [dbo].[EmpresaContatoPerfil] ([IDEmpresaContatoPerfil])
GO
