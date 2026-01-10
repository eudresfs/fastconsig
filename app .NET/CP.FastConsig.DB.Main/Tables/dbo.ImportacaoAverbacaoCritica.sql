CREATE TABLE [dbo].[ImportacaoAverbacaoCritica]
(
[ID] [int] NOT NULL IDENTITY(1, 1),
[Descricao] [varchar] (500) COLLATE Latin1_General_CI_AI NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ImportacaoAverbacaoCritica] ADD CONSTRAINT [PK_ImportacaoAverbacaoCritica] PRIMARY KEY CLUSTERED  ([ID]) ON [PRIMARY]
GO
