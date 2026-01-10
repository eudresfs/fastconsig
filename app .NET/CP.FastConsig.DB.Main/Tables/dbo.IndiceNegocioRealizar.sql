CREATE TABLE [dbo].[IndiceNegocioRealizar]
(
[IDIndiceNegRealizar] [int] NOT NULL IDENTITY(1, 1),
[Competencia] [varchar] (7) COLLATE Latin1_General_CI_AI NULL,
[Data] [datetime] NULL,
[IDEmpresa] [int] NULL,
[Quantidade] [int] NULL,
[MargemDisponivel] [decimal] (18, 2) NULL,
[VolumeNegocio] [decimal] (18, 2) NULL,
[VolumeAdicionado] [decimal] (18, 2) NULL
)
GO
ALTER TABLE [dbo].[IndiceNegocioRealizar] ADD CONSTRAINT [PK_IndiceNegocioRealizar] PRIMARY KEY CLUSTERED  ([IDIndiceNegRealizar])
GO
ALTER TABLE [dbo].[IndiceNegocioRealizar] ADD CONSTRAINT [FK_IndiceNegocioRealizar_Empresa] FOREIGN KEY ([IDEmpresa]) REFERENCES [dbo].[Empresa] ([IDEmpresa])
GO
