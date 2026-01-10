CREATE TABLE [dbo].[NextNumero]
(
[Numero] [int] NOT NULL IDENTITY(1, 1),
[CreatedOn] [datetime] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[NextNumero] ADD CONSTRAINT [PK_NextNumeroContrato] PRIMARY KEY CLUSTERED  ([Numero]) ON [PRIMARY]
GO
