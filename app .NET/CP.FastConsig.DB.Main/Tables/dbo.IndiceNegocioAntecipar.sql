CREATE TABLE [dbo].[IndiceNegocioAntecipar]
(
[IDIndiceNegAntecipar] [int] NOT NULL IDENTITY(1, 1),
[Competencia] [varchar] (7) COLLATE Latin1_General_CI_AI NULL,
[Data] [datetime] NULL,
[Descricao] [varchar] (30) COLLATE Latin1_General_CI_AI NULL,
[IDEmpresa] [int] NULL,
[Quantidade] [int] NULL,
[Valor] [decimal] (18, 2) NULL,
[NaoLucrou] [decimal] (18, 2) NULL
)
GO
ALTER TABLE [dbo].[IndiceNegocioAntecipar] ADD CONSTRAINT [PK_IndiceNegocioAntecipar] PRIMARY KEY CLUSTERED  ([IDIndiceNegAntecipar])
GO
ALTER TABLE [dbo].[IndiceNegocioAntecipar] ADD CONSTRAINT [FK_IndiceNegocioAntecipar_Empresa] FOREIGN KEY ([IDEmpresa]) REFERENCES [dbo].[Empresa] ([IDEmpresa])
GO
