CREATE TABLE [dbo].[Usuario]
(
[IDUsuario] [int] NOT NULL IDENTITY(1, 1),
[CPF] [varchar] (14) COLLATE Latin1_General_CI_AS NULL,
[Nome] [varchar] (50) COLLATE Latin1_General_CI_AS NULL,
[Email] [varchar] (50) COLLATE Latin1_General_CI_AS NULL,
[Telefone] [varchar] (20) COLLATE Latin1_General_CI_AS NULL,
[Login] [varchar] (50) COLLATE Latin1_General_CI_AS NULL,
[Senha] [varchar] (50) COLLATE Latin1_General_CI_AS NULL,
[Ativo] [int] NOT NULL CONSTRAINT [DF_Usuario_Ativo] DEFAULT ((1)),
[CreatedBy] [int] NULL,
[CreatedByType] [varchar] (1) COLLATE Latin1_General_CI_AS NULL,
[CreatedByName] [varchar] (100) COLLATE Latin1_General_CI_AS NULL,
[CreatedOn] [datetime] NULL,
[IDConsignataria] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Usuario] ADD CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED  ([IDUsuario]) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Usuario] ON [dbo].[Usuario] ([CPF]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Usuario] ADD CONSTRAINT [FK_Usuario_Consignataria] FOREIGN KEY ([IDConsignataria]) REFERENCES [dbo].[Consignataria] ([IDConsignataria])
GO
