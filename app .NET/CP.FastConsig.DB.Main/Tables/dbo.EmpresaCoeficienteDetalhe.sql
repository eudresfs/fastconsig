CREATE TABLE [dbo].[EmpresaCoeficienteDetalhe]
(
[IDEmpresaCoeficienteDetalhe] [int] NOT NULL IDENTITY(1, 1),
[IDEmpresaCoeficiente] [int] NOT NULL,
[Prazo] [int] NOT NULL,
[Coeficiente] [decimal] (18, 8) NOT NULL,
[CET] [decimal] (18, 3) NULL,
[Dia] [int] NULL,
[Ativo] [int] NOT NULL CONSTRAINT [DF__EmpresaCo__Ativo__76714406] DEFAULT ((1))
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmpresaCoeficienteDetalhe] ADD CONSTRAINT [PK_EmpresaCoeficienteDetalhe] PRIMARY KEY CLUSTERED  ([IDEmpresaCoeficienteDetalhe]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_EmpresaCoeficienteDetalhe] ON [dbo].[EmpresaCoeficienteDetalhe] ([IDEmpresaCoeficiente]) ON [PRIMARY]
GO
