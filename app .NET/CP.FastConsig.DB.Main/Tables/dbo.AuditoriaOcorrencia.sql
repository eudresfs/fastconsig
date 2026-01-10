CREATE TABLE [dbo].[AuditoriaOcorrencia]
(
[IDAuditoriaOcorrencia] [int] NOT NULL IDENTITY(1, 1),
[Data] [datetime] NULL,
[IDModulo] [int] NULL,
[IDEmpresa] [int] NULL,
[IDPerfil] [int] NULL,
[IDUsuario] [int] NULL,
[IDRecurso] [int] NULL,
[NomeRecurso] [varchar] (150) COLLATE Latin1_General_CI_AI NULL,
[Descricao] [text] COLLATE Latin1_General_CI_AI NULL,
[Navegador] [text] COLLATE Latin1_General_CI_AI NULL,
[IP] [varchar] (50) COLLATE Latin1_General_CI_AI NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[AuditoriaOcorrencia] ADD CONSTRAINT [PK_AuditoriaOcorrencia] PRIMARY KEY CLUSTERED  ([IDAuditoriaOcorrencia]) ON [PRIMARY]
GO
