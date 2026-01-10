CREATE TABLE [dbo].[EmpresaCoeficiente]
(
[IDEmpresaCoeficiente] [int] NOT NULL IDENTITY(1, 1),
[IDEmpresa] [int] NOT NULL,
[IDProdutoGrupo] [int] NOT NULL,
[Data] [datetime] NOT NULL,
[InicioVigencia] [datetime] NOT NULL,
[Carencia] [int] NULL,
[ValidadeDias] [int] NULL,
[Ativo] [int] NOT NULL CONSTRAINT [DF__EmpresaCo__Ativo__757D1FCD] DEFAULT ((1))
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmpresaCoeficiente] ADD CONSTRAINT [PK_EmpresaCoeficiente] PRIMARY KEY CLUSTERED  ([IDEmpresaCoeficiente]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_EmpresaCoeficiente] ON [dbo].[EmpresaCoeficiente] ([IDEmpresa]) ON [PRIMARY]
GO
