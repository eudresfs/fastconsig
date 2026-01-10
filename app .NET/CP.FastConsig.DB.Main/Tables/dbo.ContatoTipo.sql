CREATE TABLE [dbo].[ContatoTipo]
(
[IDContatoTipo] [int] NOT NULL,
[Nome] [varchar] (20) COLLATE Latin1_General_CI_AI NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ContatoTipo] ADD CONSTRAINT [PK_ContatoTipo] PRIMARY KEY CLUSTERED  ([IDContatoTipo]) ON [PRIMARY]
GO
