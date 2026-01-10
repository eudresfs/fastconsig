CREATE TABLE [dbo].[EmpresaSolicitacaoQuitacao]
(
[IDEmpresaSolicitacaoQuitacao] [int] NOT NULL IDENTITY(1, 1),
[IDEmpresaSolicitacao] [int] NOT NULL,
[DataQuitacao] [date] NOT NULL,
[Valor] [decimal] (18, 2) NOT NULL,
[IDAverbacaoTipoQuitacao] [int] NOT NULL,
[IDAverbacao] [int] NOT NULL,
[Observacao] [text] COLLATE Latin1_General_CI_AI NULL,
[Comprovante] [varchar] (50) COLLATE Latin1_General_CI_AI NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmpresaSolicitacaoQuitacao] ADD CONSTRAINT [PK_EmpresaSolicitacaoQuitacao] PRIMARY KEY CLUSTERED  ([IDEmpresaSolicitacaoQuitacao]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmpresaSolicitacaoQuitacao] ADD CONSTRAINT [FK_EmpresaSolicitacaoQuitacao_EmpresaSolicitacao] FOREIGN KEY ([IDEmpresaSolicitacao]) REFERENCES [dbo].[EmpresaSolicitacao] ([IDEmpresaSolicitacao])
GO
