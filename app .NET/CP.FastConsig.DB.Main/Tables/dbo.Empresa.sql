CREATE TABLE [dbo].[Empresa]
(
[IDEmpresa] [int] NOT NULL IDENTITY(1, 1),
[IDEmpresaTipo] [int] NOT NULL,
[IDEmpresaLogo] [int] NULL,
[ArquivoLogo] [nchar] (10) COLLATE Latin1_General_CI_AI NULL,
[CNPJ] [varchar] (20) COLLATE Latin1_General_CI_AI NOT NULL,
[RazaoSocial] [varchar] (80) COLLATE Latin1_General_CI_AI NOT NULL,
[Fantasia] [varchar] (80) COLLATE Latin1_General_CI_AI NULL,
[Sigla] [varchar] (15) COLLATE Latin1_General_CI_AI NULL,
[Endereco] [varchar] (100) COLLATE Latin1_General_CI_AI NOT NULL,
[Complemento] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[Bairro] [varchar] (30) COLLATE Latin1_General_CI_AI NOT NULL,
[Cidade] [varchar] (30) COLLATE Latin1_General_CI_AI NOT NULL,
[Estado] [varchar] (2) COLLATE Latin1_General_CI_AI NOT NULL,
[CEP] [varchar] (10) COLLATE Latin1_General_CI_AI NOT NULL,
[Fone1] [varchar] (15) COLLATE Latin1_General_CI_AI NULL,
[Fone2] [varchar] (15) COLLATE Latin1_General_CI_AI NULL,
[Fax] [varchar] (15) COLLATE Latin1_General_CI_AI NULL,
[Email] [varchar] (80) COLLATE Latin1_General_CI_AI NULL,
[Site] [varchar] (80) COLLATE Latin1_General_CI_AI NULL,
[IDEmpresaSituacao] [int] NOT NULL,
[IDEmpresaSituacaoAnterior] [int] NULL,
[IDUsuarioResponsavel] [int] NULL,
[RepasseDia] [int] NULL,
[RepasseBanco] [varchar] (3) COLLATE Latin1_General_CI_AI NULL,
[RepasseAgencia] [varchar] (6) COLLATE Latin1_General_CI_AI NULL,
[RepasseConta] [varchar] (20) COLLATE Latin1_General_CI_AI NULL,
[RepasseIDFederal] [varchar] (14) COLLATE Latin1_General_CI_AI NULL,
[RepasseFavorecido] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[IDEmpresaTarifada] [int] NOT NULL,
[ValorTarifa] [decimal] (10, 2) NULL,
[IDEmpresaAgenteTipo] [int] NULL,
[HomologacaoRequerida] [bit] NULL,
[HomologacaoData] [datetime] NULL,
[IDUsuarioHomologacao] [int] NULL,
[ContribuinteFastConsig] [bit] NULL,
[IDContribuinteFastConsig] [int] NULL,
[DiaCorte] [int] NULL,
[CreatedBy] [int] NULL,
[CreatedOn] [datetime] NULL,
[ModifiedBy] [int] NULL,
[ModifiedOn] [datetime] NULL,
[Ativo] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Empresa] ADD CONSTRAINT [PK_Empresa] PRIMARY KEY CLUSTERED  ([IDEmpresa]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_Empresa] ON [dbo].[Empresa] ([CNPJ]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_Empresa3] ON [dbo].[Empresa] ([Fantasia]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_Empresa5] ON [dbo].[Empresa] ([IDEmpresaSituacao]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_Empresa4] ON [dbo].[Empresa] ([IDEmpresaTipo]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_empresa6] ON [dbo].[Empresa] ([IDEmpresaTipo], [Ativo]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_Empresa2] ON [dbo].[Empresa] ([RazaoSocial]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Empresa] ADD CONSTRAINT [FK_Empresa_Estado] FOREIGN KEY ([Estado]) REFERENCES [dbo].[Estado] ([SiglaEstado])
GO
ALTER TABLE [dbo].[Empresa] ADD CONSTRAINT [FK_Empresa_EmpresaLogo] FOREIGN KEY ([IDEmpresaLogo]) REFERENCES [dbo].[EmpresaLogo] ([IDEmpresaLogo])
GO
ALTER TABLE [dbo].[Empresa] ADD CONSTRAINT [FK_Empresa_EmpresaSituacao] FOREIGN KEY ([IDEmpresaSituacao]) REFERENCES [dbo].[EmpresaSituacao] ([IDEmpresaSituacao])
GO
ALTER TABLE [dbo].[Empresa] ADD CONSTRAINT [FK_Empresa_EmpresaSituacao1] FOREIGN KEY ([IDEmpresaSituacaoAnterior]) REFERENCES [dbo].[EmpresaSituacao] ([IDEmpresaSituacao])
GO
ALTER TABLE [dbo].[Empresa] ADD CONSTRAINT [FK_Empresa_EmpresaTarifada] FOREIGN KEY ([IDEmpresaTarifada]) REFERENCES [dbo].[EmpresaTarifada] ([IDEmpresaTarifada])
GO
ALTER TABLE [dbo].[Empresa] ADD CONSTRAINT [FK_Empresa_EmpresaTipo] FOREIGN KEY ([IDEmpresaTipo]) REFERENCES [dbo].[EmpresaTipo] ([IDEmpresaTipo])
GO
ALTER TABLE [dbo].[Empresa] ADD CONSTRAINT [FK_Empresa_Usuario1] FOREIGN KEY ([IDUsuarioHomologacao]) REFERENCES [dbo].[Usuario] ([IDUsuario])
GO
ALTER TABLE [dbo].[Empresa] ADD CONSTRAINT [FK_Empresa_Usuario] FOREIGN KEY ([IDUsuarioResponsavel]) REFERENCES [dbo].[Usuario] ([IDUsuario])
GO
CREATE FULLTEXT INDEX ON [dbo].[Empresa] KEY INDEX [PK_Empresa] ON [INDEXACAO]
GO
ALTER FULLTEXT INDEX ON [dbo].[Empresa] ADD ([CNPJ] LANGUAGE 1033)
GO
ALTER FULLTEXT INDEX ON [dbo].[Empresa] ADD ([RazaoSocial] LANGUAGE 1033)
GO
ALTER FULLTEXT INDEX ON [dbo].[Empresa] ADD ([Fantasia] LANGUAGE 1033)
GO
