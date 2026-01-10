CREATE TABLE [dbo].[TmpMasNoticias]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Competencia] [varchar] (7) COLLATE Latin1_General_CI_AI NULL,
[DescricaoGrupo] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[Quantidade] [int] NULL,
[SaldoDevedor] [decimal] (18, 2) NULL,
[QuantidadeContratos] [int] NULL,
[CreatedOn] [datetime] NULL,
[IDConsignataria] [int] NULL
)
ALTER TABLE [dbo].[TmpMasNoticias] ADD
CONSTRAINT [FK_TmpMasNoticias_Empresa] FOREIGN KEY ([IDConsignataria]) REFERENCES [dbo].[Empresa] ([IDEmpresa])
GO
ALTER TABLE [dbo].[TmpMasNoticias] ADD CONSTRAINT [PK_TmpMasNoticias] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
