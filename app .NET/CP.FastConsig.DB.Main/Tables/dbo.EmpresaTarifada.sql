CREATE TABLE [dbo].[EmpresaTarifada]
(
[IDEmpresaTarifada] [int] NOT NULL,
[Nome] [varchar] (40) COLLATE Latin1_General_CI_AI NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmpresaTarifada] ADD CONSTRAINT [PK_EmpresaTarifada] PRIMARY KEY CLUSTERED  ([IDEmpresaTarifada]) ON [PRIMARY]
GO
