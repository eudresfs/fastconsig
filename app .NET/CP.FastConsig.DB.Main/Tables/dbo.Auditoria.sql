CREATE TABLE [dbo].[Auditoria]
(
[IDAuditoria] [int] NOT NULL IDENTITY(1, 1),
[IDModulo] [int] NULL,
[IDUsuario] [int] NULL,
[IDRecurso] [int] NULL,
[IDPerfil] [int] NULL,
[NomeRecurso] [varchar] (150) COLLATE Latin1_General_CI_AI NULL,
[Data] [datetime] NOT NULL,
[Tabela] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[Chave] [int] NULL,
[TipoOperacao] [varchar] (1) COLLATE Latin1_General_CI_AI NULL,
[Registro] [text] COLLATE Latin1_General_CI_AI NULL,
[IP] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[Browser] [text] COLLATE Latin1_General_CI_AI NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Auditoria] ADD CONSTRAINT [PK_Auditoria] PRIMARY KEY CLUSTERED  ([IDAuditoria]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_auditoria] ON [dbo].[Auditoria] ([Data], [IDRecurso]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_auditoria2] ON [dbo].[Auditoria] ([Data], [IDUsuario]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Auditoria] ADD CONSTRAINT [FK_Auditoria_Modulo] FOREIGN KEY ([IDModulo]) REFERENCES [dbo].[Modulo] ([IDModulo])
GO
ALTER TABLE [dbo].[Auditoria] ADD CONSTRAINT [FK_Auditoria_Recurso] FOREIGN KEY ([IDRecurso]) REFERENCES [dbo].[Recurso] ([IDRecurso])
GO
ALTER TABLE [dbo].[Auditoria] ADD CONSTRAINT [FK_Auditoria_Usuario] FOREIGN KEY ([IDUsuario]) REFERENCES [dbo].[Usuario] ([IDUsuario])
GO
