CREATE TABLE [dbo].[EmpresaSituacao]
(
[IDEmpresaSituacao] [int] NOT NULL,
[Nome] [varchar] (40) COLLATE Latin1_General_CI_AI NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmpresaSituacao] ADD CONSTRAINT [PK_EmpresaSituacao] PRIMARY KEY CLUSTERED  ([IDEmpresaSituacao]) ON [PRIMARY]
GO
