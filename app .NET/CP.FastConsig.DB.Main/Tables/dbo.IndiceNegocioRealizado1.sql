CREATE TABLE [dbo].[IndiceNegocioRealizado]
(
[IDIndiceNegRealizado] [int] NOT NULL IDENTITY(1, 1),
[Competencia] [varchar] (7) COLLATE Latin1_General_CI_AI NULL,
[Data] [datetime] NULL,
[IDAverbacaoTipo] [int] NULL,
[IDEmpresa] [int] NULL,
[Quantidade] [int] NULL,
[ValorBruto] [decimal] (18, 2) NULL,
[ValorAdicionado] [decimal] (18, 2) NULL,
[LucroBruto] [decimal] (18, 2) NULL
)
GO
ALTER TABLE [dbo].[IndiceNegocioRealizado] ADD CONSTRAINT [PK_IndiceNegocioRealizado] PRIMARY KEY CLUSTERED  ([IDIndiceNegRealizado])
GO
ALTER TABLE [dbo].[IndiceNegocioRealizado] ADD CONSTRAINT [FK_IndiceNegocioRealizado_AverbacaoTipo] FOREIGN KEY ([IDAverbacaoTipo]) REFERENCES [dbo].[AverbacaoTipo] ([IDAverbacaoTipo])
GO
ALTER TABLE [dbo].[IndiceNegocioRealizado] ADD CONSTRAINT [FK_IndiceNegocioRealizado_Empresa] FOREIGN KEY ([IDEmpresa]) REFERENCES [dbo].[Empresa] ([IDEmpresa])
GO
