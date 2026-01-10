CREATE TABLE [dbo].[EmpresaAgenteTipo]
(
[IDEmpresaAgenteTipo] [int] NOT NULL,
[Nome] [varchar] (80) COLLATE Latin1_General_CI_AI NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmpresaAgenteTipo] ADD CONSTRAINT [PK_EmpresaAgenteTipo] PRIMARY KEY CLUSTERED  ([IDEmpresaAgenteTipo]) ON [PRIMARY]
GO
