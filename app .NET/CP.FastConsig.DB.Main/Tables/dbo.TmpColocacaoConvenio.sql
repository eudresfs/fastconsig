CREATE TABLE [dbo].[TmpColocacaoConvenio]
(
[id] [int] NOT NULL IDENTITY(1, 1),
[IDServicoGrupo] [int] NULL,
[Qtde] [int] NULL,
[Total] [numeric] (18, 2) NULL,
[Ano] [int] NULL,
[Mes] [int] NULL,
[IDEmpresa] [int] NULL,
[Competencia] [varchar] (7) COLLATE Latin1_General_CI_AI NULL,
[Tipo] [varchar] (20) COLLATE Latin1_General_CI_AI NULL
)
GO
ALTER TABLE [dbo].[TmpColocacaoConvenio] ADD CONSTRAINT [PK_TmpColocacaoConvenio] PRIMARY KEY CLUSTERED  ([id]) ON [PRIMARY]
GO
