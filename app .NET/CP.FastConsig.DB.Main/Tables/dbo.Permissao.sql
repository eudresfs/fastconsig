CREATE TABLE [dbo].[Permissao]
(
[IDPermissao] [int] NOT NULL,
[Nome] [varchar] (50) COLLATE Latin1_General_CI_AI NOT NULL,
[Descricao] [varchar] (500) COLLATE Latin1_General_CI_AI NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Permissao] ADD CONSTRAINT [PK_Permissao] PRIMARY KEY CLUSTERED  ([IDPermissao]) ON [PRIMARY]
GO
