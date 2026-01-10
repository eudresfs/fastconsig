CREATE TABLE [dbo].[ConciliacaoRetorno]
(
[IDConciliacaoRetorno] [int] NOT NULL IDENTITY(1, 1),
[Competencia] [varchar] (7) COLLATE Latin1_General_CI_AI NULL,
[Matricula] [varchar] (15) COLLATE Latin1_General_CI_AI NULL,
[Verba] [varchar] (25) COLLATE Latin1_General_CI_AI NULL,
[ValorDescontado] [decimal] (18, 2) NOT NULL,
[IDConsignataria] [int] NULL,
[IDProduto] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ConciliacaoRetorno] ADD CONSTRAINT [PK_ContratoRetorno] PRIMARY KEY CLUSTERED  ([IDConciliacaoRetorno]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_ContratoRetorno] ON [dbo].[ConciliacaoRetorno] ([IDConsignataria]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_ContratoRetorno2] ON [dbo].[ConciliacaoRetorno] ([IDProduto]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_ContratoRetorno3] ON [dbo].[ConciliacaoRetorno] ([Matricula], [Verba], [Competencia]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ConciliacaoRetorno] ADD CONSTRAINT [FK_ContratoRetorno_Empresa] FOREIGN KEY ([IDConsignataria]) REFERENCES [dbo].[Empresa] ([IDEmpresa])
GO
ALTER TABLE [dbo].[ConciliacaoRetorno] ADD CONSTRAINT [FK_ContratoRetorno_Servico] FOREIGN KEY ([IDProduto]) REFERENCES [dbo].[Produto] ([IDProduto])
GO
