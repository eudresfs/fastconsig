CREATE TABLE [dbo].[FuncionarioAutorizacaoTipo]
(
[IDFuncionarioAutorizacaoTipo] [int] NOT NULL,
[Nome] [varchar] (50) COLLATE Latin1_General_CI_AI NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FuncionarioAutorizacaoTipo] ADD CONSTRAINT [PK_FuncionarioAutorizacaoTipo] PRIMARY KEY CLUSTERED  ([IDFuncionarioAutorizacaoTipo]) ON [PRIMARY]
GO
