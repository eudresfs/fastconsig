CREATE TABLE [dbo].[MensagemSistemaGerada]
(
[IDMensagemSistemaGerada] [int] NOT NULL IDENTITY(1, 1),
[IDMensagemSistema] [int] NULL,
[IDUsuario] [int] NULL,
[Data] [datetime] NULL,
[MensagemLida] [bit] NULL,
[Texto] [varchar] (1000) COLLATE Latin1_General_CI_AI NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[MensagemSistemaGerada] ADD CONSTRAINT [PK_MensagemSistemaGerada] PRIMARY KEY CLUSTERED  ([IDMensagemSistemaGerada]) ON [PRIMARY]
GO
