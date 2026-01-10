CREATE TABLE [dbo].[AverbacaoAjuste]
(
[IDAverbacaoAjuste] [int] NOT NULL IDENTITY(1, 1),
[IDConsignataria] [int] NOT NULL,
[IDProduto] [int] NOT NULL,
[IDContratoAjusteTipo] [int] NOT NULL,
[Data] [datetime] NOT NULL,
[Descricao] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[NovoValor] [decimal] (18, 2) NOT NULL,
[NomeArquivo] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[Situacao] [varchar] (1) COLLATE Latin1_General_CI_AI NOT NULL,
[Ativo] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AverbacaoAjuste] ADD CONSTRAINT [PK_ContratoAjuste] PRIMARY KEY CLUSTERED  ([IDAverbacaoAjuste]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_contratoajuste2] ON [dbo].[AverbacaoAjuste] ([IDConsignataria]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_contratoajuste] ON [dbo].[AverbacaoAjuste] ([IDConsignataria], [Ativo]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_contratoajuste3] ON [dbo].[AverbacaoAjuste] ([IDProduto], [Ativo]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AverbacaoAjuste] ADD CONSTRAINT [FK_ContratoAjuste_Empresa] FOREIGN KEY ([IDConsignataria]) REFERENCES [dbo].[Empresa] ([IDEmpresa])
GO
ALTER TABLE [dbo].[AverbacaoAjuste] ADD CONSTRAINT [FK_ContratoAjuste_ContratoAjusteTipo] FOREIGN KEY ([IDContratoAjusteTipo]) REFERENCES [dbo].[AverbacaoAjusteTipo] ([IDAverbacaoAjusteTipo])
GO
ALTER TABLE [dbo].[AverbacaoAjuste] ADD CONSTRAINT [FK_ContratoAjuste_Servico] FOREIGN KEY ([IDProduto]) REFERENCES [dbo].[Produto] ([IDProduto])
GO
