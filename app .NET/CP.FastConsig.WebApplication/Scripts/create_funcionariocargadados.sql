GO

/****** Object:  Table [dbo].[Funcionario]    Script Date: 02/29/2012 11:18:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FuncionarioCargaDados](
	[Competencia] [varchar] (7) NOT NULL,
	[IDFuncionario] [int] NOT NULL,
	[IDPessoa] [int] NOT NULL,
	[Matricula] [varchar](15) NOT NULL,
	[MatriculaAntiga] [varchar](15) NULL,
	[DataAdmissao] [datetime] NULL,
	[IDConsignante] [int] NULL,
	[IDFuncionarioCategoria] [int] NULL,
	[IDFuncionarioSituacao] [int] NULL,
	[IDFuncionarioRegime] [int] NULL,
	[NomeLocalFolha] [varchar](80) NULL,
	[NomeSetorFolha] [varchar](80) NULL,
	[NomeCargoFolha] [varchar](80) NULL,
	[NomeRegimeFolha] [varchar](80) NULL,
	[NomeSituacaoFolha] [varchar](80) NULL,
	[MargemBruta] [decimal](18, 2) NULL,
	[MargemBase] [decimal](18, 2) NULL,
	[FonteRecurso] [varchar](50) NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[Ativo] [int] NOT NULL,
	[MESEXONERAR] [varchar](7) NULL,
 CONSTRAINT [PK_FuncionarioCargaDados] PRIMARY KEY CLUSTERED 
(
	[Competencia], [IDFuncionario] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[FuncionarioCargaDados]  WITH CHECK ADD  CONSTRAINT [FK_FuncionarioCargaDados_FuncionarioCategoria] FOREIGN KEY([IDFuncionarioCategoria])
REFERENCES [dbo].[FuncionarioCategoria] ([IDFuncionarioCategoria])
GO

ALTER TABLE [dbo].[FuncionarioCargaDados] CHECK CONSTRAINT [FK_FuncionarioCargaDados_FuncionarioCategoria]
GO

ALTER TABLE [dbo].[FuncionarioCargaDados]  WITH CHECK ADD  CONSTRAINT [FK_FuncionarioCargaDados_FuncionarioRegime] FOREIGN KEY([IDFuncionarioRegime])
REFERENCES [dbo].[FuncionarioRegime] ([IDFuncionarioRegime])
GO

ALTER TABLE [dbo].[FuncionarioCargaDados] CHECK CONSTRAINT [FK_FuncionarioCargaDados_FuncionarioRegime]
GO

ALTER TABLE [dbo].[FuncionarioCargaDados]  WITH CHECK ADD  CONSTRAINT [FK_FuncionarioCargaDados_FuncionarioSituacao] FOREIGN KEY([IDFuncionarioSituacao])
REFERENCES [dbo].[FuncionarioSituacao] ([IDFuncionarioSituacao])
GO

ALTER TABLE [dbo].[FuncionarioCargaDados] CHECK CONSTRAINT [FK_FuncionarioCargaDados_FuncionarioSituacao]
GO

ALTER TABLE [dbo].[FuncionarioCargaDados]  WITH CHECK ADD  CONSTRAINT [FK_FuncionarioCargaDados_Pessoa] FOREIGN KEY([IDPessoa])
REFERENCES [dbo].[Pessoa] ([IDPessoa])
GO

ALTER TABLE [dbo].[FuncionarioCargaDados] CHECK CONSTRAINT [FK_FuncionarioCargaDados_Pessoa]
GO


