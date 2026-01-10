CREATE TABLE [dbo].[MensagemSistema]
(
[IDMensagemSistema] [int] NOT NULL,
[Titulo] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[Texto] [varchar] (1000) COLLATE Latin1_General_CI_AI NULL,
[TextoHTML] [varchar] (1000) COLLATE Latin1_General_CI_AI NULL,
[IDModulo] [int] NULL,
[Perfis] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[DispararEmail] [bit] NOT NULL,
[DispararSMS] [bit] NOT NULL,
[ForcarLeitura] [bit] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[MensagemSistema] ADD CONSTRAINT [PK_MensagemSistema] PRIMARY KEY CLUSTERED  ([IDMensagemSistema]) ON [PRIMARY]
GO
