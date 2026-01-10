CREATE TABLE [dbo].[UsuarioVinculo]
(
[IDUsuarioVinculo] [int] NOT NULL IDENTITY(1, 1),
[IDUsuario] [int] NOT NULL,
[IDVinculo] [int] NOT NULL,
[Ativo] [bit] NOT NULL CONSTRAINT [DF_UsuarioVinculo_Ativo] DEFAULT ((1))
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UsuarioVinculo] ADD CONSTRAINT [PK_UsuarioVinculo] PRIMARY KEY CLUSTERED  ([IDUsuarioVinculo]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UsuarioVinculo] ADD CONSTRAINT [FK_UsuarioVinculo_Usuario] FOREIGN KEY ([IDUsuario]) REFERENCES [dbo].[Usuario] ([IDUsuario])
GO
ALTER TABLE [dbo].[UsuarioVinculo] ADD CONSTRAINT [FK_UsuarioVinculo_Vinculo] FOREIGN KEY ([IDVinculo]) REFERENCES [dbo].[Vinculo] ([IDVinculo])
GO
