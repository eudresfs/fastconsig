CREATE TABLE [dbo].[TmpVolumeInadimplencia]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Mes] [varchar] (20) COLLATE Latin1_General_CI_AI NULL,
[SomaParcelas] [decimal] (18, 2) NULL,
[IDEmpresa] [int] NULL
)
GO
ALTER TABLE [dbo].[TmpVolumeInadimplencia] ADD CONSTRAINT [PK_TmpVolumeInadimplencia] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
