CREATE TABLE [dbo].[FuncionarioRegime]
(
[IDFuncionarioRegime] [int] NOT NULL,
[Nome] [varchar] (50) COLLATE Latin1_General_CI_AI NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FuncionarioRegime] ADD CONSTRAINT [PK_FuncionarioRegime] PRIMARY KEY CLUSTERED  ([IDFuncionarioRegime]) ON [PRIMARY]
GO
