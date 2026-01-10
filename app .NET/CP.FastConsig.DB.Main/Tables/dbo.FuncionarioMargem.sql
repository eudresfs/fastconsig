CREATE TABLE [dbo].[FuncionarioMargem]
(
[IDFuncionarioMargem] [int] NOT NULL IDENTITY(1, 1),
[IDFuncionario] [int] NOT NULL,
[IDProdutoGrupo] [int] NOT NULL,
[MargemFolha] [decimal] (18, 2) NOT NULL,
[MargemFolhaDisponivel] [decimal] (18, 2) NOT NULL,
[CreatedBy] [int] NULL,
[CreatedOn] [datetime] NULL,
[ModifiedBy] [int] NULL,
[ModifiedOn] [datetime] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FuncionarioMargem] ADD CONSTRAINT [PK_FuncionarioMargem] PRIMARY KEY CLUSTERED  ([IDFuncionarioMargem]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_FuncionarioMargem2] ON [dbo].[FuncionarioMargem] ([CreatedOn]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_FuncionarioMargem] ON [dbo].[FuncionarioMargem] ([IDFuncionario], [IDProdutoGrupo]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FuncionarioMargem] ADD CONSTRAINT [FK_FuncionarioMargem_Funcionario] FOREIGN KEY ([IDFuncionario]) REFERENCES [dbo].[Funcionario] ([IDFuncionario])
GO
ALTER TABLE [dbo].[FuncionarioMargem] ADD CONSTRAINT [FK_FuncionarioMargem_ServicoGrupo] FOREIGN KEY ([IDProdutoGrupo]) REFERENCES [dbo].[ProdutoGrupo] ([IDProdutoGrupo])
GO
