CREATE TABLE [dbo].[TmpBoasNoticias]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[IdGrupoBoaNoticia] [int] NULL,
[Tipo] [nchar] (50) COLLATE Latin1_General_CI_AI NULL,
[Qtde] [int] NULL,
[ValorMargem] [decimal] (18, 2) NULL,
[Possibilidade] [decimal] (18, 2) NULL,
[IDConsignataria] [int] NULL,
[Competencia] [varchar] (7) COLLATE Latin1_General_CI_AI NULL,
[createdon] [datetime] NULL
)
ALTER TABLE [dbo].[TmpBoasNoticias] ADD
CONSTRAINT [FK_TmpBoasNoticias_Empresa] FOREIGN KEY ([IDConsignataria]) REFERENCES [dbo].[Empresa] ([IDEmpresa])
GO
ALTER TABLE [dbo].[TmpBoasNoticias] ADD CONSTRAINT [PK_TmpBoasNoticias] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
