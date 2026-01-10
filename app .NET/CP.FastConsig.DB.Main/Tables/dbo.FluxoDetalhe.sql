CREATE TABLE [dbo].[FluxoDetalhe]
(
[IDFluxoDetalhe] [int] NOT NULL IDENTITY(1, 1),
[IDFluxo] [int] NOT NULL,
[Ordem] [int] NOT NULL,
[Ator] [varchar] (1) COLLATE Latin1_General_CI_AI NOT NULL,
[IDAverbacaoSituacao] [int] NULL,
[IDEmpresaSolicitacaoTipo] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FluxoDetalhe] ADD CONSTRAINT [PK_FluxoDetalhe] PRIMARY KEY CLUSTERED  ([IDFluxoDetalhe]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FluxoDetalhe] ADD CONSTRAINT [FK_FluxoDetalhe_AverbacaoSituacao] FOREIGN KEY ([IDAverbacaoSituacao]) REFERENCES [dbo].[AverbacaoSituacao] ([IDAverbacaoSituacao])
GO
ALTER TABLE [dbo].[FluxoDetalhe] ADD CONSTRAINT [FK_FluxoDetalhe_EmpresaSolicitacaoTipo] FOREIGN KEY ([IDEmpresaSolicitacaoTipo]) REFERENCES [dbo].[EmpresaSolicitacaoTipo] ([IDEmpresaSolicitacaoTipo])
GO
ALTER TABLE [dbo].[FluxoDetalhe] ADD CONSTRAINT [FK_FluxoDetalhe_Fluxo] FOREIGN KEY ([IDFluxo]) REFERENCES [dbo].[Fluxo] ([IDFluxo])
GO
