CREATE TABLE [dbo].[FuncionarioAutorizacao]
(
[IDFuncionarioAutorizacao] [int] NOT NULL IDENTITY(1, 1),
[IDFuncionario] [int] NOT NULL,
[IDFuncionarioAutorizacaoTipo] [int] NOT NULL,
[AutorizacaoData] [datetime] NOT NULL,
[AutorizacaoValidade] [int] NOT NULL,
[Motivo] [varchar] (300) COLLATE Latin1_General_CI_AI NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FuncionarioAutorizacao] ADD CONSTRAINT [PK_FuncionarioAutorizacao] PRIMARY KEY CLUSTERED  ([IDFuncionarioAutorizacao]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_FuncionarioAutorizacao] ON [dbo].[FuncionarioAutorizacao] ([IDFuncionario]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FuncionarioAutorizacao] ADD CONSTRAINT [FK_FuncionarioAutorizacao_Funcionario] FOREIGN KEY ([IDFuncionario]) REFERENCES [dbo].[Funcionario] ([IDFuncionario])
GO
ALTER TABLE [dbo].[FuncionarioAutorizacao] ADD CONSTRAINT [FK_FuncionarioAutorizacao_FuncionarioAutorizacaoTipo] FOREIGN KEY ([IDFuncionarioAutorizacaoTipo]) REFERENCES [dbo].[FuncionarioAutorizacaoTipo] ([IDFuncionarioAutorizacaoTipo])
GO
