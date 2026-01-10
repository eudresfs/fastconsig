GO

/****** Object:  Table [dbo].[FuncionarioMargem]    Script Date: 02/29/2012 14:38:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FuncionarioMargemCargaDados](
	[Competencia] [varchar] (7) NOT NULL,
	[IDFuncionarioMargem] [int] NOT NULL,
	[IDFuncionario] [int] NOT NULL,
	[IDProdutoGrupo] [int] NOT NULL,
	[MargemFolha] [decimal](18, 2) NOT NULL,
	[MargemFolhaDisponivel] [decimal](18, 2) NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
 CONSTRAINT [PK_FuncionarioMargemCargaDados] PRIMARY KEY CLUSTERED 
(
	[Competencia],[IDFuncionarioMargem] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[FuncionarioMargemCargaDados]  WITH CHECK ADD  CONSTRAINT [FK_FuncionarioMargemCargaDados_Funcionario] FOREIGN KEY([IDFuncionario])
REFERENCES [dbo].[Funcionario] ([IDFuncionario])
GO

ALTER TABLE [dbo].[FuncionarioMargemCargaDados] CHECK CONSTRAINT [FK_FuncionarioMargemCargaDados_Funcionario]
GO

ALTER TABLE [dbo].[FuncionarioMargemCargaDados]  WITH CHECK ADD  CONSTRAINT [FK_FuncionarioMargemCargaDados_ServicoGrupo] FOREIGN KEY([IDProdutoGrupo])
REFERENCES [dbo].[ProdutoGrupo] ([IDProdutoGrupo])
GO

ALTER TABLE [dbo].[FuncionarioMargemCargaDados] CHECK CONSTRAINT [FK_FuncionarioMargemCargaDados_ServicoGrupo]
GO


