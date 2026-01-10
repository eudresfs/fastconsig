CREATE TABLE [dbo].[Usuario]
(
[IDUsuario] [int] NOT NULL IDENTITY(1, 1),
[CPF] [varchar] (14) COLLATE Latin1_General_CI_AI NOT NULL,
[NomeCompleto] [varchar] (60) COLLATE Latin1_General_CI_AI NOT NULL,
[Celular] [varchar] (15) COLLATE Latin1_General_CI_AI NULL,
[OBS] [varchar] (100) COLLATE Latin1_General_CI_AI NULL,
[Email] [varchar] (100) COLLATE Latin1_General_CI_AI NULL,
[ApelidoLogin] [varchar] (100) COLLATE Latin1_General_CI_AI NOT NULL,
[Senha] [varchar] (100) COLLATE Latin1_General_CI_AI NOT NULL,
[SenhaProvisoria] [varchar] (100) COLLATE Latin1_General_CI_AI NULL,
[CreatedBy] [int] NULL,
[CreatedOn] [datetime] NULL,
[ModifiedBy] [int] NULL,
[ModifiedOn] [datetime] NULL,
[Ativo] [int] NULL,
[QtdAcessos] [int] NOT NULL CONSTRAINT [DF_Usuario_QtdAcessos] DEFAULT ((0)),
[UltimoAcesso] [datetime] NULL,
[Situacao] [varchar] (1) COLLATE Latin1_General_CI_AI NOT NULL CONSTRAINT [DF_Usuario_Situacao] DEFAULT ('A')
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Usuario] ADD CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED  ([IDUsuario]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_Usuario] ON [dbo].[Usuario] ([ApelidoLogin]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_usuario2] ON [dbo].[Usuario] ([CPF]) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [idxcpf] ON [dbo].[Usuario] ([CPF]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_usuario3] ON [dbo].[Usuario] ([NomeCompleto]) ON [PRIMARY]
GO
CREATE FULLTEXT INDEX ON [dbo].[Usuario] KEY INDEX [PK_Usuario] ON [INDEXACAO]
GO
ALTER FULLTEXT INDEX ON [dbo].[Usuario] ADD ([CPF] LANGUAGE 1046)
GO
ALTER FULLTEXT INDEX ON [dbo].[Usuario] ADD ([NomeCompleto] LANGUAGE 1046)
GO
ALTER FULLTEXT INDEX ON [dbo].[Usuario] ADD ([Email] LANGUAGE 1046)
GO
ALTER FULLTEXT INDEX ON [dbo].[Usuario] ADD ([ApelidoLogin] LANGUAGE 1046)
GO
