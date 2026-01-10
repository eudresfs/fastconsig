--exec SP_IndiceNegocio '2012/02', '2012-03-12'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE SP_IndiceNegocio
	@Competencia varchar(7),
	@Data datetime
AS
BEGIN
	SET NOCOUNT ON;
	
	declare @PrazoMaximo int
	
    select @PrazoMaximo=CONVERT(int,valor) from Parametro where Nome = 'PrazoMaximo'
	
	-- excluindo dados ja processados
	delete from indicenegociorealizado where Competencia = @Competencia and data = @Data
	delete from IndiceNegocioRealizar where Competencia = @Competencia and data = @Data
	delete from IndiceNegocioAntecipar where Competencia = @Competencia and data = @Data
	
	-- conquistas realizadas
	select idaverbacaotipo, a.idconsignataria,(prazo * valorparcela) as valorbruto,
	((prazo * valorparcela)-ValorContratado) as lucrobruto,
	(select SUM(ap.Valor) from AverbacaoVinculo av, Averbacao ava, AverbacaoParcela ap 
	where av.IDAverbacaoPai = a.IDAverbacao and av.IDAverbacao = ava.IDAverbacao and ava.IDAverbacao = ap.IDAverbacao and ap.Competencia > @Competencia) as ValorNegociado
	into #resumo
	from Averbacao a
	where a.CompetenciaInicial = @Competencia
	
	insert into indicenegociorealizado(competencia,idempresa, data,idaverbacaotipo, quantidade, valorbruto, lucrobruto, valoradicionado)
	select @Competencia, idconsignataria, @Data, idaverbacaotipo, COUNT(*), SUM(valorbruto), SUM(lucrobruto), 
	(case when IDAverbacaoTipo = 1 then SUM(valorbruto)
	else sum(valorbruto - valornegociado) end) as adicionado
	from #resumo
	group by IDAverbacaoTipo, IDConsignataria
	
	-- correr para conquistar
	select idfuncionario, IDConsignataria, SUM(ap.Valor) as saldo
	into #funcsporempresa
	from Averbacao a, AverbacaoSituacao s, AverbacaoParcela ap
	where a.IDAverbacaoSituacao = s.IDAverbacaoSituacao
	and s.DeduzMargem = 1 and a.IDAverbacao = ap.IDAverbacao and ap.Competencia > @Competencia
	group by a.idfuncionario, IDConsignataria
	
	select distinct idfuncionario
	into #funcsclientes
	from #funcsporempresa f
	
	select f.IDFuncionario, fm.MargemFolha,
	(select SUM(valorparcela) from Averbacao a, AverbacaoSituacao s, Produto p
	where a.IDFuncionario = f.idfuncionario and a.IDAverbacaoSituacao = s.IDAverbacaoSituacao 
	and s.DeduzMargem = 1 and a.Ativo = 1 and a.IDProduto = p.IDProduto and p.IDProdutoGrupo = 1) as MargemUtilizada
	into #funcsmargens
	from #funcsclientes f, FuncionarioMargem fm
	where f.IDFuncionario = fm.IDFuncionario and fm.IDProdutoGrupo = 1
	
	insert into IndiceNegocioRealizar(competencia,idempresa, data, quantidade, margemdisponivel, volumenegocio, volumeadicionado)
	select @Competencia, e.IDConsignataria, @Data, COUNT(*), sum(MargemFolha - MargemUtilizada), sum((MargemFolha - MargemUtilizada) + saldo)*@prazomaximo,
	(sum((MargemFolha - MargemUtilizada) + saldo)*@prazomaximo)-SUM(saldo)
	from #funcsmargens m, #funcsporempresa e
	where m.idfuncionario = e.IDFuncionario and (MargemFolha - MargemUtilizada) > 0
	group by e.IDConsignataria
	
	-- trabalhar para antecipar: perdeu por compra de divida
	select a.IDConsignataria, COUNT(distinct a.IDConsignataria) as qtde, SUM(pai.ValorContratado) as valorquitado	
	into #comprados
	from AverbacaoVinculo av, Averbacao a, Averbacao pai 
	where av.IDAverbacao = a.IDAverbacao and pai.IDAverbacao = av.IDAverbacaoPai
	and pai.CompetenciaInicial = @Competencia and pai.IDAverbacaoTipo in (2,4) and pai.IDAverbacaoSituacao > 0
	group by a.idconsignataria 

	select a.IDConsignataria, SUM(ap.Valor) as Saldo
	into #saldorestante
	from AverbacaoVinculo av, Averbacao a, Averbacao pai, AverbacaoParcela ap 
	where av.IDAverbacao = a.IDAverbacao and pai.IDAverbacao = av.IDAverbacaoPai and pai.IDAverbacaoSituacao > 0
	and pai.CompetenciaInicial = @Competencia and pai.IDAverbacaoTipo in (2,4) and a.IDAverbacao = ap.IDAverbacao
	and ap.Competencia >= @Competencia
	group by a.idconsignataria 
	
	insert into IndiceNegocioAntecipar(competencia,idempresa, data, descricao, quantidade, valor, naolucrou)
	select @Competencia, c.idconsignataria, @Data, 'Perdeu por Compra de Dívida', qtde, isnull(saldo,0), isnull(saldo,0) - valorquitado
	from #comprados c, #saldorestante s 
	where c.idconsignataria = s.idconsignataria
	
	-- trabalhar para antecipar: perdeu por não acompanhar
	select a.IDConsignataria, COUNT(distinct a.IDConsignataria) as qtde, SUM(pai.ValorParcela * @PrazoMaximo) as valorutilizado, SUM(pai.ValorContratado) as valorcontrato	
	into #renegociados
	from AverbacaoVinculo av, Averbacao a, Averbacao pai, #funcsporempresa f
	where av.IDAverbacao = a.IDAverbacao and pai.IDAverbacao = av.IDAverbacaoPai
	and pai.CompetenciaInicial = @Competencia and pai.IDAverbacaoTipo in (3,4) and pai.IDAverbacaoSituacao > 0
	and a.IDFuncionario = f.IDFuncionario and a.IDConsignataria = f.IDConsignataria and pai.IDConsignataria <> a.IDConsignataria
	group by a.idconsignataria 
	
	select a.IDConsignataria, COUNT(distinct a.IDConsignataria) as qtde, SUM(a.ValorParcela * @PrazoMaximo) as valorutilizado, SUM(a.ValorContratado) as valorcontrato
	into #averbsimples
	from Averbacao a, #funcsporempresa f
	where a.CompetenciaInicial = @Competencia and a.IDAverbacaoTipo = 1 and a.IDAverbacaoSituacao > 0
	and a.IDFuncionario = f.IDFuncionario and a.IDConsignataria <> f.IDConsignataria 
	group by a.idconsignataria 
	
	insert into IndiceNegocioAntecipar(competencia,idempresa, data, descricao, quantidade, valor, naolucrou)
	select @Competencia, isnull(s.idconsignataria,r.idconsignataria), @Data, 'Perdeu por não acompanhar', 
	isnull(r.qtde,0) + isnull(s.qtde,0), isnull(r.valorutilizado,0) + isnull(s.valorutilizado,0), isnull(r.valorcontrato,0) + isnull(s.valorcontrato,0)
	from #averbsimples s full outer join #renegociados r on r.idconsignataria = s.idconsignataria
	
END
GO

