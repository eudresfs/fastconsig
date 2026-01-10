CREATE TABLE [dbo].[EmpresaSolicitacaoSaldoDevedor]
(
[IDEmpresaSolicitacaoSaldoDevedor] [int] NOT NULL IDENTITY(1, 1),
[IDEmpresaSolicitacao] [int] NULL,
[IDAverbacao] [int] NULL,
[Valor] [decimal] (18, 2) NULL,
[Validade] [date] NULL,
[IDTipoPagamento] [int] NULL,
[Identificador] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[Banco] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[Agencia] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[ContaCredito] [varchar] (50) COLLATE Latin1_General_CI_AI NULL,
[NomeFavorecido] [varchar] (80) COLLATE Latin1_General_CI_AI NULL,
[Observacao] [text] COLLATE Latin1_General_CI_AI NULL,
[Data] [date] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmpresaSolicitacaoSaldoDevedor] ADD CONSTRAINT [PK_EmpresaSolicitacaoSaldoDevedor] PRIMARY KEY CLUSTERED  ([IDEmpresaSolicitacaoSaldoDevedor]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_EmpresaSolicitacaoSaldoDevedor] ON [dbo].[EmpresaSolicitacaoSaldoDevedor] ([IDEmpresaSolicitacao]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmpresaSolicitacaoSaldoDevedor] ADD CONSTRAINT [FK_EmpresaSolicitacaoSaldoDevedor_EmpresaSolicitacaoSaldoDevedor] FOREIGN KEY ([IDEmpresaSolicitacao]) REFERENCES [dbo].[EmpresaSolicitacao] ([IDEmpresaSolicitacao])
GO
ALTER TABLE [dbo].[EmpresaSolicitacaoSaldoDevedor] ADD CONSTRAINT [FK_EmpresaSolicitacaoSaldoDevedor_TipoPagamento] FOREIGN KEY ([IDTipoPagamento]) REFERENCES [dbo].[TipoPagamento] ([IDTipoPagamento])
GO
