CREATE TABLE [dbo].[EmpresaSolicitacaoSituacao]
(
[IDEmpresaSolicitacaoSituacao] [int] NOT NULL,
[Nome] [varchar] (25) COLLATE Latin1_General_CI_AI NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmpresaSolicitacaoSituacao] ADD CONSTRAINT [PK_EmpresaSolicitacaoSituacao] PRIMARY KEY CLUSTERED  ([IDEmpresaSolicitacaoSituacao]) ON [PRIMARY]
GO
