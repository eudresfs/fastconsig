CREATE TABLE [dbo].[EmpresaContatoPerfil]
(
[IDEmpresaContatoPerfil] [int] NOT NULL,
[Nome] [varchar] (25) COLLATE Latin1_General_CI_AI NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmpresaContatoPerfil] ADD CONSTRAINT [PK_ContatoPerfil] PRIMARY KEY CLUSTERED  ([IDEmpresaContatoPerfil]) ON [PRIMARY]
GO
