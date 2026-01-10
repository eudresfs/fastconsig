CREATE TABLE [dbo].[Conciliacao]
(
[IDConciliacao] [int] NOT NULL IDENTITY(1, 1),
[Competencia] [varchar] (7) COLLATE Latin1_General_CI_AI NULL,
[IDConciliacaoMovimento] [int] NULL,
[IDConciliacaoRetorno] [int] NULL,
[IDFuncionario] [int] NULL,
[IDProduto] [int] NULL,
[IDConsignataria] [int] NULL,
[Matricula] [varchar] (15) COLLATE Latin1_General_CI_AI NULL,
[Verba] [varchar] (25) COLLATE Latin1_General_CI_AI NULL,
[Valor] [decimal] (18, 2) NOT NULL,
[ValorDescontado] [decimal] (18, 2) NOT NULL,
[IDConciliacaoTipo] [int] NULL,
[Motivo] [varchar] (150) COLLATE Latin1_General_CI_AI NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Conciliacao] ADD CONSTRAINT [PK_ContratoConciliacao] PRIMARY KEY CLUSTERED  ([IDConciliacao]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_contratoconciliacao] ON [dbo].[Conciliacao] ([Competencia]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_contratoconciliacao5] ON [dbo].[Conciliacao] ([IDConciliacaoTipo]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_contratoconciliacao2] ON [dbo].[Conciliacao] ([IDConsignataria]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_contratoconciliacao3] ON [dbo].[Conciliacao] ([IDFuncionario]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_contratoconciliacao4] ON [dbo].[Conciliacao] ([Matricula]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Conciliacao] ADD CONSTRAINT [FK_ContratoConciliacao_ContratoMovimento] FOREIGN KEY ([IDConciliacaoMovimento]) REFERENCES [dbo].[ConciliacaoMovimento] ([IDConciliacaoMovimento])
GO
ALTER TABLE [dbo].[Conciliacao] ADD CONSTRAINT [FK_ContratoConciliacao_ContratoRetorno] FOREIGN KEY ([IDConciliacaoRetorno]) REFERENCES [dbo].[ConciliacaoRetorno] ([IDConciliacaoRetorno])
GO
ALTER TABLE [dbo].[Conciliacao] ADD CONSTRAINT [FK_ContratoConciliacao_ContratoConciliacaoTipo] FOREIGN KEY ([IDConciliacaoTipo]) REFERENCES [dbo].[ConciliacaoTipo] ([IDConciliacaoTipo])
GO
ALTER TABLE [dbo].[Conciliacao] ADD CONSTRAINT [FK_ContratoConciliacao_Empresa] FOREIGN KEY ([IDConsignataria]) REFERENCES [dbo].[Empresa] ([IDEmpresa])
GO
ALTER TABLE [dbo].[Conciliacao] ADD CONSTRAINT [FK_ContratoConciliacao_Funcionario] FOREIGN KEY ([IDFuncionario]) REFERENCES [dbo].[Funcionario] ([IDFuncionario])
GO
ALTER TABLE [dbo].[Conciliacao] ADD CONSTRAINT [FK_ContratoConciliacao_Servico] FOREIGN KEY ([IDProduto]) REFERENCES [dbo].[Produto] ([IDProduto])
GO
