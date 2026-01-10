
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_AnaliseProducaoConsignados] 
	-- Add the parameters for the stored procedure here
	@IdEmpresa int,
	@MesInicio varchar(7), 
	@MesFim varchar(7)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.  
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 'Carteira' as Tipo, e.fantasia as NomeConsignataria,  pg.nome as TipoProduto, a.Competencia, 
	count(*) as Quantidade, sum(a.prazo * a.ValorMovimento) as ValorConsignado, sum(a.ValorMovimento) as ValorMensal
	into #temp
	from ConciliacaoMovimento a, Produto p, ProdutoGrupo pg, empresa e
	where a.Verba = p.VerbaFolha and p.idprodutogrupo = pg.idprodutogrupo
	and a.idconsignataria = e.idempresa
	and (a.IDConsignataria = @IdEmpresa or @IdEmpresa = 0) and a.Competencia >= @MesInicio and a.Competencia <= @MesFim
	group by e.fantasia,  pg.nome, a.competencia

	SELECT 'Produzido' as Tipo, e.fantasia as NomeConsignataria,  pg.nome as TipoProduto, av.CompetenciaInicial as Competencia, 
	count(*) as Quantidade, sum(a.prazo * a.ValorMovimento) as ValorConsignado, sum(a.ValorMovimento) as ValorMensal
	into #tempx
	from ConciliacaoMovimento a, Produto p, ProdutoGrupo pg, empresa e, AverbacaoParcela ap, Averbacao av
	where a.Verba = p.VerbaFolha and p.idprodutogrupo = pg.idprodutogrupo and a.idaverbacaoparcela = ap.IDAverbacaoParcela and ap.IDAverbacao = av.idaverbacao 
	and av.CompetenciaInicial in (select Competencia from #temp)
	and a.idconsignataria = e.idempresa
	and (a.IDConsignataria = @IdEmpresa or @IdEmpresa = 0) and a.Competencia >= @MesInicio and a.Competencia <= @MesFim
	group by e.fantasia,  pg.nome, av.CompetenciaInicial

	update #tempx set Quantidade = 0, ValorConsignado = 0, ValorMensal = 0
	where Competencia = (select MIN(Competencia) from ConciliacaoMovimento)
	
	select * into #carteira from #temp
	union
	select * from #tempx

	select *, 
	(select ValorMensal from #carteira 
	where NomeConsignataria = c.NomeConsignataria and TipoProduto = c.TipoProduto and Tipo = 'Carteira' and c.Tipo <> 'Produzido'
	and dbo.DiminuiCompetencia(c.Competencia,1) = Competencia) as ValorAnterior
	into #final
	from #carteira c
	
	select *, ((valormensal-ValorAnterior)/ValorAnterior)*100 as Percentual 
	from #final
END
GO
