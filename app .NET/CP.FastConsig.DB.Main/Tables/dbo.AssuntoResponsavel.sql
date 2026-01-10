CREATE TABLE [dbo].[AssuntoResponsavel]
(
[IDAssuntoResponsavel] [int] NOT NULL IDENTITY(1, 1),
[IDAssunto] [int] NOT NULL,
[IDUsuario] [int] NOT NULL,
[IDEmpresa] [int] NULL,
[CreatedBy] [int] NULL,
[CreatedOn] [datetime] NULL,
[ModifiedBy] [int] NULL,
[ModifiedOn] [datetime] NULL,
[Ativo] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AssuntoResponsavel] ADD CONSTRAINT [PK_Responsavel] PRIMARY KEY CLUSTERED  ([IDAssuntoResponsavel]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_assuntoresponsavel] ON [dbo].[AssuntoResponsavel] ([IDAssunto], [IDEmpresa], [Ativo]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_assuntoresponsavel2] ON [dbo].[AssuntoResponsavel] ([IDEmpresa], [IDAssunto], [IDUsuario]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AssuntoResponsavel] ADD CONSTRAINT [FK_AssuntoResponsavel_Assunto] FOREIGN KEY ([IDAssunto]) REFERENCES [dbo].[Assunto] ([IDAssunto])
GO
ALTER TABLE [dbo].[AssuntoResponsavel] ADD CONSTRAINT [FK_AssuntoResponsavel_Empresa] FOREIGN KEY ([IDEmpresa]) REFERENCES [dbo].[Empresa] ([IDEmpresa])
GO
ALTER TABLE [dbo].[AssuntoResponsavel] ADD CONSTRAINT [FK_AssuntoResponsavel_Usuario] FOREIGN KEY ([IDUsuario]) REFERENCES [dbo].[Usuario] ([IDUsuario])
GO
