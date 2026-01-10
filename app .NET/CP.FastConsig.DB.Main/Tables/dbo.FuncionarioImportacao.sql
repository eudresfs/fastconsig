CREATE TABLE [dbo].[FuncionarioImportacao]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Matricula] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[MatriculaAntiga] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[DataAdmissao] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[NomeLocalFolha] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[NomeSetorFolha] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[NomeCargoFolha] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[NomeRegimeFolha] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[NomeSituacaoFolha] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[MargemBruta] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[MargemBase] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[FonteRecurso] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[Nome] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[CPF] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[RG] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[DataNascimento] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[Endereco] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[Complemento] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[Bairro] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[Cidade] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[Estado] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[CEP] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[Celular] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[Email] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[NomePai] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[NomeMae] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[Sexo] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[Fone] [varchar] (255) COLLATE Latin1_General_CI_AI NULL,
[IdImportacao] [int] NULL
)
GO
ALTER TABLE [dbo].[FuncionarioImportacao] ADD CONSTRAINT [PK_FuncionarioImportacao] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
