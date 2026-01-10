CREATE TABLE [dbo].[EmpresaSolicitacaoTipo]
(
[IDEmpresaSolicitacaoTipo] [int] NOT NULL,
[Nome] [varchar] (60) COLLATE Latin1_General_CI_AI NOT NULL,
[Descricao] [varchar] (250) COLLATE Latin1_General_CI_AI NULL,
[Prazo] [int] NOT NULL,
[IDEmpresaSituacaoSuspensaoAntes] [int] NULL,
[IDEmpresaSituacaoSuspensaoApos] [int] NULL,
[Modulo] [varchar] (10) COLLATE Latin1_General_CI_AI NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmpresaSolicitacaoTipo] ADD CONSTRAINT [PK_EmpresaSolicitacaoTipo] PRIMARY KEY CLUSTERED  ([IDEmpresaSolicitacaoTipo]) ON [PRIMARY]
GO
