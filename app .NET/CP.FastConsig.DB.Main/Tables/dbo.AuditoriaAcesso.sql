CREATE TABLE [dbo].[AuditoriaAcesso]
(
[IDAuditoriaAcesso] [int] NOT NULL IDENTITY(1, 1),
[Data] [datetime] NULL,
[IDModulo] [int] NULL,
[IDEmpresa] [int] NULL,
[IDPerfil] [int] NULL,
[IDUsuario] [int] NULL,
[IDRecurso] [int] NULL,
[NomeRecurso] [varchar] (150) COLLATE Latin1_General_CI_AI NULL,
[Descricao] [varchar] (100) COLLATE Latin1_General_CI_AI NULL,
[Navegador] [text] COLLATE Latin1_General_CI_AI NULL,
[IP] [varchar] (50) COLLATE Latin1_General_CI_AI NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[AuditoriaAcesso] ADD CONSTRAINT [PK_AuditoriaAcesso] PRIMARY KEY CLUSTERED  ([IDAuditoriaAcesso]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AuditoriaAcesso] WITH NOCHECK ADD CONSTRAINT [FK_AuditoriaAcesso_Empresa1] FOREIGN KEY ([IDEmpresa]) REFERENCES [dbo].[Empresa] ([IDEmpresa])
GO
ALTER TABLE [dbo].[AuditoriaAcesso] WITH NOCHECK ADD CONSTRAINT [FK_AuditoriaAcesso_Modulo1] FOREIGN KEY ([IDModulo]) REFERENCES [dbo].[Modulo] ([IDModulo])
GO
ALTER TABLE [dbo].[AuditoriaAcesso] WITH NOCHECK ADD CONSTRAINT [FK_AuditoriaAcesso_Perfil1] FOREIGN KEY ([IDPerfil]) REFERENCES [dbo].[Perfil] ([IDPerfil])
GO
ALTER TABLE [dbo].[AuditoriaAcesso] WITH NOCHECK ADD CONSTRAINT [FK_AuditoriaAcesso_Recurso1] FOREIGN KEY ([IDRecurso]) REFERENCES [dbo].[Recurso] ([IDRecurso])
GO
ALTER TABLE [dbo].[AuditoriaAcesso] WITH NOCHECK ADD CONSTRAINT [FK_AuditoriaAcesso_Usuario1] FOREIGN KEY ([IDUsuario]) REFERENCES [dbo].[Usuario] ([IDUsuario])
GO
ALTER TABLE [dbo].[AuditoriaAcesso] NOCHECK CONSTRAINT [FK_AuditoriaAcesso_Empresa1]
GO
ALTER TABLE [dbo].[AuditoriaAcesso] NOCHECK CONSTRAINT [FK_AuditoriaAcesso_Modulo1]
GO
ALTER TABLE [dbo].[AuditoriaAcesso] NOCHECK CONSTRAINT [FK_AuditoriaAcesso_Perfil1]
GO
ALTER TABLE [dbo].[AuditoriaAcesso] NOCHECK CONSTRAINT [FK_AuditoriaAcesso_Recurso1]
GO
ALTER TABLE [dbo].[AuditoriaAcesso] NOCHECK CONSTRAINT [FK_AuditoriaAcesso_Usuario1]
GO
