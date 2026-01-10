CREATE TABLE [dbo].[AverbacaoImportacao]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Matricula] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[CPF] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[ParcelaCorrente] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[MesAnoPrimeiraParcela] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[MesAnoParcelaCorrente] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[Data] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[Identificador] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[Numero] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[ValorParcela] [decimal] (18, 2) NULL,
[ValorDeducaoMargem] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[Prazo] [int] NULL,
[CET] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[ValorContratado] [decimal] (18, 2) NULL,
[ValorDevidoTotal] [decimal] (18, 2) NULL,
[CompetenciaInicial] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[CompetenciaFinal] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[Coeficiente] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[Obs] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[PrazoAprovacao] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[Verba] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[IdImportacao] [int] NULL
)
GO
