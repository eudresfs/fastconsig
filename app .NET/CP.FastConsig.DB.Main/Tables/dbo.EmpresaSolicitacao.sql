CREATE TABLE [dbo].[EmpresaSolicitacao]
(
[IDEmpresaSolicitacao] [int] NOT NULL IDENTITY(1, 1),
[IDEmpresa] [int] NULL,
[IDFuncionario] [int] NULL,
[IDEmpresaSolicitacaoTipo] [int] NOT NULL,
[IDEmpresaSolicitacaoSituacao] [int] NOT NULL,
[IDAverbacao] [int] NULL,
[IDEmpresaSolicitacaoOriginal] [int] NULL,
[IDEmpresaSolicitante] [int] NULL,
[IDSolicitante] [int] NULL,
[IDResponsavel] [int] NULL,
[DataSolicitacao] [datetime] NOT NULL,
[Descricao] [varchar] (1000) COLLATE Latin1_General_CI_AI NULL,
[DataAtendimento] [datetime] NULL,
[Motivo] [varchar] (1000) COLLATE Latin1_General_CI_AI NULL,
[Ativo] [int] NULL
)
ALTER TABLE [dbo].[EmpresaSolicitacao] WITH NOCHECK ADD
CONSTRAINT [FK_EmpresaSolicitacao_Averbacao] FOREIGN KEY ([IDAverbacao]) REFERENCES [dbo].[Averbacao] ([IDAverbacao]) NOT FOR REPLICATION
ALTER TABLE [dbo].[EmpresaSolicitacao] NOCHECK CONSTRAINT [FK_EmpresaSolicitacao_Averbacao]
ALTER TABLE [dbo].[EmpresaSolicitacao] WITH NOCHECK ADD
CONSTRAINT [FK_EmpresaSolicitacao_Empresa1] FOREIGN KEY ([IDEmpresaSolicitante]) REFERENCES [dbo].[Empresa] ([IDEmpresa]) NOT FOR REPLICATION
ALTER TABLE [dbo].[EmpresaSolicitacao] NOCHECK CONSTRAINT [FK_EmpresaSolicitacao_Empresa1]
GO
ALTER TABLE [dbo].[EmpresaSolicitacao] ADD CONSTRAINT [PK_EmpresaSolicitacao] PRIMARY KEY CLUSTERED  ([IDEmpresaSolicitacao]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_EmpresaSolicitacao2] ON [dbo].[EmpresaSolicitacao] ([IDAverbacao], [Ativo]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_EmpresaSolicitacao] ON [dbo].[EmpresaSolicitacao] ([IDEmpresa], [Ativo]) ON [PRIMARY]
GO

ALTER TABLE [dbo].[EmpresaSolicitacao] ADD CONSTRAINT [FK_EmpresaSolicitacao_Empresa] FOREIGN KEY ([IDEmpresa]) REFERENCES [dbo].[Empresa] ([IDEmpresa])
GO
ALTER TABLE [dbo].[EmpresaSolicitacao] ADD CONSTRAINT [FK_EmpresaSolicitacao_EmpresaSolicitacaoSituacao] FOREIGN KEY ([IDEmpresaSolicitacaoSituacao]) REFERENCES [dbo].[EmpresaSolicitacaoSituacao] ([IDEmpresaSolicitacaoSituacao])
GO
ALTER TABLE [dbo].[EmpresaSolicitacao] ADD CONSTRAINT [FK_EmpresaSolicitacao_EmpresaSolicitacaoTipo] FOREIGN KEY ([IDEmpresaSolicitacaoTipo]) REFERENCES [dbo].[EmpresaSolicitacaoTipo] ([IDEmpresaSolicitacaoTipo])
GO

ALTER TABLE [dbo].[EmpresaSolicitacao] ADD CONSTRAINT [FK_EmpresaSolicitacao_Usuario1] FOREIGN KEY ([IDResponsavel]) REFERENCES [dbo].[Usuario] ([IDUsuario])
GO
ALTER TABLE [dbo].[EmpresaSolicitacao] ADD CONSTRAINT [FK_EmpresaSolicitacao_Usuario] FOREIGN KEY ([IDSolicitante]) REFERENCES [dbo].[Usuario] ([IDUsuario])
GO
EXEC sp_addextendedproperty N'MS_Description', N'Pendente; Atendido; Rejeitado; Em análise', 'SCHEMA', N'dbo', 'TABLE', N'EmpresaSolicitacao', 'COLUMN', N'IDEmpresaSolicitacaoSituacao'
GO
