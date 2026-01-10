CREATE TABLE [dbo].[EmpresaVinculo]
(
[IDEmpresaVinculo] [int] NOT NULL IDENTITY(1, 1),
[IDEmpresa] [int] NOT NULL,
[IDEmpresaVinculada] [int] NOT NULL,
[CreatedBy] [int] NULL,
[CreatedOn] [datetime] NULL,
[ModifiedBy] [int] NULL,
[ModifiedOn] [datetime] NULL,
[Ativo] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmpresaVinculo] ADD CONSTRAINT [PK_EmpresaVinculo] PRIMARY KEY CLUSTERED  ([IDEmpresaVinculo]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_EmpresaVinculo] ON [dbo].[EmpresaVinculo] ([IDEmpresa], [Ativo]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_empresavinculo2] ON [dbo].[EmpresaVinculo] ([IDEmpresaVinculada], [Ativo]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmpresaVinculo] ADD CONSTRAINT [FK_EmpresaVinculo_Empresa] FOREIGN KEY ([IDEmpresa]) REFERENCES [dbo].[Empresa] ([IDEmpresa])
GO
ALTER TABLE [dbo].[EmpresaVinculo] ADD CONSTRAINT [FK_EmpresaVinculo_Empresa1] FOREIGN KEY ([IDEmpresaVinculada]) REFERENCES [dbo].[Empresa] ([IDEmpresa])
GO
