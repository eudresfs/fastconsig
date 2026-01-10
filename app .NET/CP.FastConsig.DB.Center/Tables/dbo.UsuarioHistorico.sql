CREATE TABLE [dbo].[UsuarioHistorico]
(
[IDUsuarioHistorico] [int] NOT NULL IDENTITY(1, 1),
[IDUsuario] [int] NOT NULL,
[UsuarioSerializado] [text] COLLATE Latin1_General_CI_AS NULL,
[ModifiedOn] [datetime] NULL,
[ModifiedBy] [int] NULL,
[ModifiedByName] [varchar] (300) COLLATE Latin1_General_CI_AS NULL,
[ModifiedByType] [varchar] (1) COLLATE Latin1_General_CI_AS NULL,
[Ativo] [bit] NOT NULL CONSTRAINT [DF_UsuarioHistorico_Ativo] DEFAULT ((1)),
[Acao] [varchar] (300) COLLATE Latin1_General_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[UsuarioHistorico] ADD CONSTRAINT [PK_UsuarioHistorico] PRIMARY KEY CLUSTERED  ([IDUsuarioHistorico]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UsuarioHistorico] ADD CONSTRAINT [FK_UsuarioHistorico_Usuario] FOREIGN KEY ([IDUsuario]) REFERENCES [dbo].[Usuario] ([IDUsuario])
GO
