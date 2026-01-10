CREATE TABLE [dbo].[EmpresaLogo]
(
[IDEmpresaLogo] [int] NOT NULL IDENTITY(1, 1),
[Nome] [varchar] (50) COLLATE Latin1_General_CI_AI NOT NULL,
[Arquivo] [varchar] (50) COLLATE Latin1_General_CI_AI NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmpresaLogo] ADD CONSTRAINT [PK_EmpresaLogo] PRIMARY KEY CLUSTERED  ([IDEmpresaLogo]) ON [PRIMARY]
GO
