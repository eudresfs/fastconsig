CREATE TABLE [dbo].[FuncionarioMargemCargaDados]
(
[Competencia] [varchar] (7) COLLATE Latin1_General_CI_AI NOT NULL,
[IDFuncionarioMargem] [int] NOT NULL,
[IDFuncionario] [int] NOT NULL,
[IDProdutoGrupo] [int] NOT NULL,
[MargemFolha] [decimal] (18, 2) NOT NULL,
[MargemFolhaDisponivel] [decimal] (18, 2) NOT NULL,
[CreatedBy] [int] NULL,
[CreatedOn] [datetime] NULL,
[ModifiedBy] [int] NULL,
[ModifiedOn] [datetime] NULL
)
GO
ALTER TABLE [dbo].[FuncionarioMargemCargaDados] ADD CONSTRAINT [PK_FuncionarioMargemCargaDados] PRIMARY KEY CLUSTERED  ([Competencia], [IDFuncionarioMargem])
GO
ALTER TABLE [dbo].[FuncionarioMargemCargaDados] ADD CONSTRAINT [FK_FuncionarioMargemCargaDados_Funcionario] FOREIGN KEY ([IDFuncionario]) REFERENCES [dbo].[Funcionario] ([IDFuncionario])
GO
ALTER TABLE [dbo].[FuncionarioMargemCargaDados] ADD CONSTRAINT [FK_FuncionarioMargemCargaDados_ServicoGrupo] FOREIGN KEY ([IDProdutoGrupo]) REFERENCES [dbo].[ProdutoGrupo] ([IDProdutoGrupo])
GO
