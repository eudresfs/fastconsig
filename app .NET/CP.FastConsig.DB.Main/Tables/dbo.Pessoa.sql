CREATE TABLE [dbo].[Pessoa]
(
[IDPessoa] [int] NOT NULL IDENTITY(1, 1),
[Nome] [varchar] (80) COLLATE Latin1_General_CI_AI NOT NULL,
[CPF] [varchar] (14) COLLATE Latin1_General_CI_AI NOT NULL,
[RG] [varchar] (15) COLLATE Latin1_General_CI_AI NULL,
[DataNascimento] [datetime] NULL,
[IDUsuario] [int] NULL,
[Endereco] [varchar] (80) COLLATE Latin1_General_CI_AI NULL,
[Complemento] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[Bairro] [varchar] (30) COLLATE Latin1_General_CI_AI NULL,
[Cidade] [varchar] (30) COLLATE Latin1_General_CI_AI NULL,
[Estado] [varchar] (2) COLLATE Latin1_General_CI_AI NULL,
[CEP] [varchar] (10) COLLATE Latin1_General_CI_AI NULL,
[Celular] [varchar] (15) COLLATE Latin1_General_CI_AI NULL,
[Email] [varchar] (60) COLLATE Latin1_General_CI_AI NULL,
[NomeMae] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[NomePai] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[Sexo] [varchar] (1) COLLATE Latin1_General_CI_AI NULL,
[CreatedBy] [int] NULL,
[CreatedOn] [datetime] NULL,
[ModifiedBy] [int] NULL,
[ModifiedOn] [datetime] NULL,
[Ativo] [int] NULL,
[Fone] [varchar] (20) COLLATE Latin1_General_CI_AI NULL
)
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE TRIGGER [dbo].[T_Pessoa] 
   ON  [dbo].[Pessoa] 
   AFTER INSERT,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

update Pessoa set endereco = (case when endereco is null then '' else endereco end),
bairro = (case when bairro is null then '' else bairro end),
estado = (case when estado is null then '' else estado end),
cidade = (case when cidade is null then '' else cidade end),
cep = (case when cep is null then '' else cep end),
fone = (case when fone is null then '' else fone end),
celular = (case when celular is null then '' else celular end),
email = (case when email is null then '' else email end),
rg = (case when rg is null then '' else rg end),
complemento = (case when complemento is null then '' else complemento end)


END
GO

ALTER TABLE [dbo].[Pessoa] ADD CONSTRAINT [PK_Pessoa] PRIMARY KEY CLUSTERED  ([IDPessoa]) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [idx_pessoa_cpf] ON [dbo].[Pessoa] ([CPF]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_Pessoa2] ON [dbo].[Pessoa] ([CPF]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_Pessoa4] ON [dbo].[Pessoa] ([IDUsuario]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_Pessoa] ON [dbo].[Pessoa] ([Nome]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Pessoa] ADD CONSTRAINT [FK_Pessoa_Usuario] FOREIGN KEY ([IDUsuario]) REFERENCES [dbo].[Usuario] ([IDUsuario])
GO
CREATE FULLTEXT INDEX ON [dbo].[Pessoa] KEY INDEX [PK_Pessoa] ON [INDEXACAO]
GO
ALTER FULLTEXT INDEX ON [dbo].[Pessoa] ADD ([Nome] LANGUAGE 1033)
GO
ALTER FULLTEXT INDEX ON [dbo].[Pessoa] ADD ([CPF] LANGUAGE 1033)
GO
