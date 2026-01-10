CREATE TABLE [dbo].[AvisoLeitura]
(
[IDAvisoLeitura] [int] NOT NULL IDENTITY(1, 1),
[IDAviso] [int] NULL,
[IDUsuario] [int] NULL,
[Data] [datetime] NULL,
[IP] [varchar] (25) COLLATE Latin1_General_CI_AI NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AvisoLeitura] ADD CONSTRAINT [PK_AvisoLeitura] PRIMARY KEY CLUSTERED  ([IDAvisoLeitura]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_avisoleitura] ON [dbo].[AvisoLeitura] ([IDAviso]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_avisoleitura2] ON [dbo].[AvisoLeitura] ([IDUsuario]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AvisoLeitura] ADD CONSTRAINT [FK_AvisoLeitura_Aviso] FOREIGN KEY ([IDAviso]) REFERENCES [dbo].[Aviso] ([IDAviso])
GO
ALTER TABLE [dbo].[AvisoLeitura] ADD CONSTRAINT [FK_AvisoLeitura_Usuario] FOREIGN KEY ([IDUsuario]) REFERENCES [dbo].[Usuario] ([IDUsuario])
GO
