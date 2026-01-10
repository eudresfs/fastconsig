CREATE TABLE [dbo].[EmpresaIP]
(
[IDEmpresaIP] [int] NOT NULL IDENTITY(1, 1),
[IDEmpresa] [int] NULL,
[IPInicio] [varchar] (25) COLLATE Latin1_General_CI_AI NULL,
[IPFim] [varchar] (25) COLLATE Latin1_General_CI_AI NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmpresaIP] ADD CONSTRAINT [PK_EmpresaIP] PRIMARY KEY CLUSTERED  ([IDEmpresaIP]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_EmpresaIP] ON [dbo].[EmpresaIP] ([IDEmpresa]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmpresaIP] ADD CONSTRAINT [FK_EmpresaIP_Empresa] FOREIGN KEY ([IDEmpresa]) REFERENCES [dbo].[Empresa] ([IDEmpresa])
GO
