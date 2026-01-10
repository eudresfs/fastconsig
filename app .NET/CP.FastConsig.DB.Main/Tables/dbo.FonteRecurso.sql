CREATE TABLE [dbo].[FonteRecurso]
(
[IDFonteRecurso] [int] NOT NULL IDENTITY(1, 1),
[Nome] [varchar] (50) COLLATE Latin1_General_CI_AI NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FonteRecurso] ADD CONSTRAINT [PK_FonteRecurso] PRIMARY KEY CLUSTERED  ([IDFonteRecurso]) ON [PRIMARY]
GO
