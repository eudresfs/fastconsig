CREATE TABLE [dbo].[TmpInadimplenciaTempoDetalhe]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[IDInadimplenciaTempo] [int] NULL,
[Competencia] [varchar] (7) COLLATE Latin1_General_CI_AI NULL,
[Descricao] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[Nome] [varchar] (80) COLLATE Latin1_General_CI_AI NULL,
[CPF] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[Matricula] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[Situacao] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[ValorParcela] [decimal] (18, 2) NULL,
[ParcelasRestante] [int] NULL,
[MargemNegativa] [decimal] (18, 2) NULL,
[ValorAtrasado] [decimal] (18, 2) NULL,
[Proposta] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[IDEmpresa] [int] NULL
)
GO
ALTER TABLE [dbo].[TmpInadimplenciaTempoDetalhe] ADD CONSTRAINT [PK_TmpInadimplenciaTempoDetalhe] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TmpInadimplenciaTempoDetalhe] ADD CONSTRAINT [FK_TmpInadimplenciaTempoDetalhe_TmpInadimplenciaTempo] FOREIGN KEY ([IDInadimplenciaTempo]) REFERENCES [dbo].[TmpInadimplenciaTempo] ([Id])
GO
