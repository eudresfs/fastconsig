CREATE TABLE [dbo].[TmpOcorrencias]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Banco] [nchar] (50) COLLATE Latin1_General_CI_AI NULL,
[Contratos] [int] NULL,
[Tipo] [int] NULL,
[Logo] [nchar] (20) COLLATE Latin1_General_CI_AI NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TmpOcorrencias] ADD CONSTRAINT [PK_TmpOcorrencias] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
