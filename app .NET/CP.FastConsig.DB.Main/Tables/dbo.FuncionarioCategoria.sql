CREATE TABLE [dbo].[FuncionarioCategoria]
(
[IDFuncionarioCategoria] [int] NOT NULL,
[Nome] [varchar] (50) COLLATE Latin1_General_CI_AI NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FuncionarioCategoria] ADD CONSTRAINT [PK_FuncionarioCategoria] PRIMARY KEY CLUSTERED  ([IDFuncionarioCategoria]) ON [PRIMARY]
GO
