CREATE TABLE [dbo].[TmpRecuperavelPorFolha]
(
[ID] [int] NOT NULL IDENTITY(1, 1),
[Nome] [varchar] (80) COLLATE Latin1_General_CI_AI NULL,
[Matricula] [varchar] (15) COLLATE Latin1_General_CI_AI NULL,
[CPF] [varchar] (14) COLLATE Latin1_General_CI_AI NULL,
[Parcela] [int] NULL,
[Pagas_Prazo] [varchar] (15) COLLATE Latin1_General_CI_AI NULL,
[Margem] [decimal] (18, 2) NULL,
[EmAtraso] [decimal] (18, 2) NULL,
[PropostaRepactuacao] [varchar] (100) COLLATE Latin1_General_CI_AI NULL,
[Competencia] [varchar] (7) COLLATE Latin1_General_CI_AI NULL,
[IDEmpresa] [int] NULL
)
GO
