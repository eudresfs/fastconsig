CREATE TABLE [dbo].[AverbacaoTramitacao]
(
[IDAverbacaoTramitacao] [int] NOT NULL IDENTITY(1, 1),
[IDAverbacao] [int] NOT NULL,
[IDAverbacaoSituacao] [int] NOT NULL,
[IDEmpresa] [int] NULL,
[OBS] [varchar] (1000) COLLATE Latin1_General_CI_AI NULL,
[CreatedBy] [int] NOT NULL,
[CreatedOn] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE TRIGGER [dbo].[T_ContratoTramitacao_Insert] 
   ON  [dbo].[AverbacaoTramitacao] 
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	update Averbacao set IDAverbacaoSituacao = i.IDAverbacaoSituacao 
	from Averbacao a, inserted i
	where a.IDAverbacao = i.IDAverbacao
END
GO
ALTER TABLE [dbo].[AverbacaoTramitacao] ADD CONSTRAINT [PK_ContratoTramitacao] PRIMARY KEY CLUSTERED  ([IDAverbacaoTramitacao]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_ContratoTramitacao2] ON [dbo].[AverbacaoTramitacao] ([CreatedOn], [IDAverbacao]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_ContratoTramitacao] ON [dbo].[AverbacaoTramitacao] ([IDAverbacao], [CreatedOn]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AverbacaoTramitacao] ADD CONSTRAINT [FK_ContratoTramitacao_Usuario] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Usuario] ([IDUsuario])
GO
ALTER TABLE [dbo].[AverbacaoTramitacao] ADD CONSTRAINT [FK_ContratoTramitacao_Contrato] FOREIGN KEY ([IDAverbacao]) REFERENCES [dbo].[Averbacao] ([IDAverbacao])
GO
ALTER TABLE [dbo].[AverbacaoTramitacao] ADD CONSTRAINT [FK_ContratoTramitacao_ContratoSituacao] FOREIGN KEY ([IDAverbacaoSituacao]) REFERENCES [dbo].[AverbacaoSituacao] ([IDAverbacaoSituacao])
GO
ALTER TABLE [dbo].[AverbacaoTramitacao] ADD CONSTRAINT [FK_ContratoTramitacao_Empresa] FOREIGN KEY ([IDEmpresa]) REFERENCES [dbo].[Empresa] ([IDEmpresa])
GO
