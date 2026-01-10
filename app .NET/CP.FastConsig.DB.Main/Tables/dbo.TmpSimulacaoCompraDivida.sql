CREATE TABLE [dbo].[TmpSimulacaoCompraDivida]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[ParcelasRestantes] [int] NOT NULL,
[ValorParcela] [decimal] (18, 2) NOT NULL,
[NomeBanco] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[Prazo] [int] NOT NULL CONSTRAINT [DF_TmpSimulacaoCompraDivida_Prazo] DEFAULT ((1)),
[ParcelasPagas] [int] NOT NULL CONSTRAINT [DF_TmpSimulacaoCompraDivida_ParcelasPagas] DEFAULT ((1)),
[Dia] [varchar] (10) COLLATE Latin1_General_CI_AI NULL
)
GO
ALTER TABLE [dbo].[TmpSimulacaoCompraDivida] ADD CONSTRAINT [PK_TmpSimulacaoCompraDivida] PRIMARY KEY CLUSTERED  ([Id])
GO
