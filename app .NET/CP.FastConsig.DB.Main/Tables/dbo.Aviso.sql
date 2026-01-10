CREATE TABLE [dbo].[Aviso]
(
[IDAviso] [int] NOT NULL IDENTITY(1, 1),
[IDModulo] [int] NULL,
[Data] [datetime] NOT NULL,
[Titulo] [varchar] (50) COLLATE Latin1_General_CI_AI NOT NULL,
[SolicitarLeitura] [bit] NOT NULL,
[SempreVisivel] [bit] NOT NULL,
[Texto] [text] COLLATE Latin1_General_CI_AI NOT NULL,
[FiltroConsignatarias] [varchar] (500) COLLATE Latin1_General_CI_AI NULL,
[FiltroConsignantes] [varchar] (500) COLLATE Latin1_General_CI_AI NULL,
[FiltroAgentes] [varchar] (500) COLLATE Latin1_General_CI_AI NULL,
[FiltroPerfis] [varchar] (500) COLLATE Latin1_General_CI_AI NULL,
[Ativo] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Aviso] ADD CONSTRAINT [PK_Aviso] PRIMARY KEY CLUSTERED  ([IDAviso]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_aviso] ON [dbo].[Aviso] ([IDModulo], [Data], [Ativo]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Aviso] ADD CONSTRAINT [FK_Aviso_Modulo] FOREIGN KEY ([IDModulo]) REFERENCES [dbo].[Modulo] ([IDModulo])
GO
