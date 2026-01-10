CREATE TABLE [dbo].[Assunto]
(
[IDAssunto] [int] NOT NULL IDENTITY(1, 1),
[Nome] [varchar] (50) COLLATE Latin1_General_CI_AI NOT NULL,
[Interno] [bit] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Assunto] ADD CONSTRAINT [PK_Assunto] PRIMARY KEY CLUSTERED  ([IDAssunto]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_assunto] ON [dbo].[Assunto] ([Interno]) ON [PRIMARY]
GO
