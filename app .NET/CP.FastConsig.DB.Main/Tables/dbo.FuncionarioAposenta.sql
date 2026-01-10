CREATE TABLE [dbo].[FuncionarioAposenta]
(
[IDFuncionarioAposenta] [int] NOT NULL IDENTITY(1, 1),
[Data] [datetime] NULL,
[IDFuncionario] [int] NULL,
[Matricula] [varchar] (20) COLLATE Latin1_General_CI_AI NULL,
[Nome] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[CPF] [varchar] (14) COLLATE Latin1_General_CI_AI NULL,
[Numero] [varchar] (20) COLLATE Latin1_General_CI_AI NULL,
[Identificador] [varchar] (20) COLLATE Latin1_General_CI_AI NULL,
[CompetenciaInicial] [varchar] (7) COLLATE Latin1_General_CI_AI NULL,
[CompetenciaFinal] [varchar] (7) COLLATE Latin1_General_CI_AI NULL,
[Prazo] [int] NULL,
[ValorParcela] [decimal] (18, 2) NULL,
[NomeConsignataria] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[Verba] [varchar] (10) COLLATE Latin1_General_CI_AI NULL,
[ValorContrato] [decimal] (18, 2) NULL,
[Importado] [bit] NULL
)
GO
ALTER TABLE [dbo].[FuncionarioAposenta] ADD CONSTRAINT [PK_FuncionarioAposenta] PRIMARY KEY CLUSTERED  ([IDFuncionarioAposenta])
GO
