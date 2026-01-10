CREATE TABLE [dbo].[FuncionarioSituacao]
(
[IDFuncionarioSituacao] [int] NOT NULL,
[Nome] [varchar] (50) COLLATE Latin1_General_CI_AI NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FuncionarioSituacao] ADD CONSTRAINT [PK_FuncionarioSituacao] PRIMARY KEY CLUSTERED  ([IDFuncionarioSituacao]) ON [PRIMARY]
GO
