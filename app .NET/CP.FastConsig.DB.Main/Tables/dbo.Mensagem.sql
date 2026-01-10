CREATE TABLE [dbo].[Mensagem]
(
[IDMensagem] [int] NOT NULL IDENTITY(1, 1),
[IDRemetente] [int] NULL,
[IDDestinatario] [int] NULL,
[IDAssunto] [int] NOT NULL,
[IDEmpresa] [int] NULL,
[DataHora] [datetime] NOT NULL,
[Mensagem] [varchar] (500) COLLATE Latin1_General_CI_AI NOT NULL,
[IDMensagemOrigem] [int] NULL,
[Pendente] [bit] NOT NULL,
[Status] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Mensagem] ADD CONSTRAINT [PK_Mensagem] PRIMARY KEY CLUSTERED  ([IDMensagem]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_mensagem3] ON [dbo].[Mensagem] ([IDDestinatario]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_mensagem2] ON [dbo].[Mensagem] ([IDEmpresa]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_mensagem] ON [dbo].[Mensagem] ([IDRemetente]) ON [PRIMARY]
GO
