CREATE TABLE [dbo].[EmpresaSuspensao]
(
[IDEmpresaSuspensao] [int] NOT NULL IDENTITY(1, 1),
[IDEmpresa] [int] NOT NULL,
[IDEmpresaSituacaoSuspensao] [int] NOT NULL,
[IDEmpresaSituacaoAnterior] [int] NOT NULL,
[Data] [datetime] NOT NULL,
[Motivo] [varchar] (200) COLLATE Latin1_General_CI_AI NOT NULL,
[TipoPeriodo] [varchar] (1) COLLATE Latin1_General_CI_AI NOT NULL,
[DataInicial] [datetime] NULL,
[DataFinal] [datetime] NULL,
[CreatedBy] [int] NULL,
[CreatedOn] [datetime] NULL,
[ModifiedBy] [int] NULL,
[ModifiedOn] [datetime] NULL,
[Ativo] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmpresaSuspensao] ADD CONSTRAINT [PK_EmpresaSuspensao] PRIMARY KEY CLUSTERED  ([IDEmpresaSuspensao]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_EmpresaSuspensao] ON [dbo].[EmpresaSuspensao] ([IDEmpresa]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmpresaSuspensao] ADD CONSTRAINT [FK_EmpresaSuspensao_Empresa] FOREIGN KEY ([IDEmpresa]) REFERENCES [dbo].[Empresa] ([IDEmpresa])
GO
ALTER TABLE [dbo].[EmpresaSuspensao] ADD CONSTRAINT [FK_EmpresaSuspensao_EmpresaSituacao1] FOREIGN KEY ([IDEmpresaSituacaoAnterior]) REFERENCES [dbo].[EmpresaSituacao] ([IDEmpresaSituacao])
GO
ALTER TABLE [dbo].[EmpresaSuspensao] ADD CONSTRAINT [FK_EmpresaSuspensao_EmpresaSituacao] FOREIGN KEY ([IDEmpresaSituacaoSuspensao]) REFERENCES [dbo].[EmpresaSituacao] ([IDEmpresaSituacao])
GO
