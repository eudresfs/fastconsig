CREATE TABLE [dbo].[EmpresaTipo]
(
[IDEmpresaTipo] [int] NOT NULL,
[Nome] [varchar] (80) COLLATE Latin1_General_CI_AI NOT NULL,
[Consignataria] [bit] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmpresaTipo] ADD CONSTRAINT [PK_EmpresaTipo] PRIMARY KEY CLUSTERED  ([IDEmpresaTipo]) ON [PRIMARY]
GO
