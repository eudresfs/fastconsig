--exec SP_AnaliseInadimplencia '2012/02'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE SP_AnaliseInadimplencia 
@Competencia varchar(7)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	declare @IdConsignante int
	declare @total decimal(18,2)
	declare @CompetenciaInicial varchar(7)

	-- identificar a consignante	
	select @IdConsignante = idempresa 
	from Empresa 
	where IDEmpresaTipo = 2

	-- verificar volume de descontado e nao descontado por consinataria
    select c.IDConsignataria as IDEmpresa, ct.IDConciliacaoGrupo, c.IDFuncionario, SUM(c.valordescontado) as valordescontado, SUM(valor) as valor
    into #conciliacao
    from Conciliacao c, ConciliacaoTipo ct, ConciliacaoGrupo cg
    where Competencia = @Competencia and c.IDConciliacaoTipo = ct.IDConciliacaoTipo and ct.IDConciliacaoGrupo = cg.IDConciliacaoGrupo
    and ct.IDConciliacaoGrupo in (1,2)
    group by c.IDConsignataria, ct.IDConciliacaoGrupo, c.IDFuncionario

	-- obtem o total da conciliacao para calculo do percentual
	select @total=isnull(SUM(valor),1) from #conciliacao
	
	-- consignante: inserção da inadimplencia geral
	insert into TmpInadimplenciaGeral(competencia,idempresa,Descricao,Percentual, Valor)
	select @Competencia,@IdConsignante,(case when IDConciliacaoGrupo = 1 then 'Conciliado' else 'Inadimplente' end),
	(SUM((case when IDConciliacaoGrupo = 1 then valordescontado else valor end))/@total) * 100,sum(valordescontado)
	from #conciliacao
	group by IDConciliacaoGrupo

	-- consignataria: inserção da inadimplencia geral por consignataria
	insert into TmpInadimplenciaGeral(competencia,idempresa,Descricao,Percentual, Valor)
	select @Competencia,idempresa,(case when IDConciliacaoGrupo = 1 then 'Conciliado' else 'Inadimplente' end),
	(SUM((case when IDConciliacaoGrupo = 1 then valordescontado else valor end))/(select isnull(SUM(valor),1) from #conciliacao where idempresa = c.idempresa)) * 100,sum(valordescontado)
	from #conciliacao c
	group by idempresa, IDConciliacaoGrupo

	-- consignante: inadimplencia geral detalhe, o que pode ser recuperavel (idfuncionariosituacao = 1) ou solucao externa (idfuncionariosituacao = 2)	
	if (exists(select 1 from funcionariocargadados where Competencia = @Competencia)) begin
	  select f.*, fm.MargemFolha 
	  into #func  
	  from funcionariocargadados f, FuncionarioMargem fm
	  where f.Competencia=@Competencia and f.IDFuncionario = fm.IDFuncionario and fm.IDProdutoGrupo = 1 
	end
--	else begin
--	  select f.*, fm.MargemFolha 
--	  into #func 
--	  from Funcionario f, FuncionarioMargem fm
--	  where f.IDFuncionario = fm.IDFuncionario and fm.IDProdutoGrupo = 1
--	end
	  
	select c.idempresa,IDFuncionarioSituacao, SUM(valor) as valor
	into #recuperavel
	from #conciliacao c, #func f
	where c.IDConciliacaoGrupo = 2 and f.IDFuncionarioSituacao = 1
	group by c.IDEmpresa,IDFuncionarioSituacao

	-- consignataria: inserção da inadimplencia geral detalhe por consignataria
	insert into TmpInadimplenciaGeralDetalhe(competencia,idempresa,Descricao,Percentual, Valor)
	select @Competencia,@IdConsignante,(case when IDFuncionariosituacao = 1 then 'Recuperável' else 'Solução Externa' end),
	(SUM(valor)/(select isnull(SUM(valor),1) from #recuperavel)) * 100,sum(valor)
	from #recuperavel c
	group by IDFuncionarioSituacao
	    
	-- consignataria: inserção da inadimplencia geral detalhe por consignataria
	insert into TmpInadimplenciaGeralDetalhe(competencia,idempresa,Descricao,Percentual, Valor)
	select @Competencia,idempresa,(case when IDFuncionariosituacao = 1 then 'Recuperável' else 'Solução Externa' end),
	(SUM(valor)/(select isnull(SUM(valor),1) from #recuperavel where idempresa = c.idempresa)) * 100,sum(valor)
	from #recuperavel c
	group by idempresa, IDFuncionarioSituacao   

	-- resumo quantitativo de funcionarios por local
	select f.NomeLocalFolha, COUNT(*) as QtdeFuncLocal
	into #resumoqtdefuncporlocal
	from #func f
	where f.IDFuncionarioSituacao = 1
	group by f.NomeLocalFolha

	-- inadimplencia por local
	select c.IDConsignataria as idempresa,f.NomeLocalFolha as Descricao, COUNT(*) as QtdeFunc, 
	(COUNT(*) / r.QtdeFuncLocal) * 100 as PercLocal, (COUNT(*) / (select SUM(qtdefunclocal) from #resumoqtdefuncporlocal)) * 100 as PercGeral
	into #inadimplentelocal
	from #conciliacao c, #func f, #resumoqtdefuncporlocal r
	where c.IDConciliacaoGrupo = 2 and f.nomelocalfolha = r.nomelocalfolha
	group by c.IDConsignataria,f.NomeLocalFolha

	-- insercao da inadimplencia por local    
    insert into TmpInadimplenciaPadraoTrabalho(Competencia, IDEmpresa, Descricao, Percentual, PercentualGeral, QtdeFunc)
    select @Competencia, idempresa, Descricao, PercLocal, PercGeral, QtdeFunc
    from #inadimplentelocal
    
	-- inadimplencia por faixa de margem
	select c.IDConsignataria as idempresa,
	(case when f.margemfolha <= 500 then '0 a 500 reais'
	when f.margemfolha > 500 and f.margemfolha <= 1000 then '500,01 a 1.000 reais'
	when f.margemfolha > 1000 and f.margemfolha <= 1500 then '1000,01 a 1.500 reais'
	when f.margemfolha > 1500 and f.margemfolha <= 2000 then '1500,01 a 2.000 reais'
	when f.margemfolha > 2000 then 'Acima de 2000,01 reais' else '' end) as descricao,
	COUNT(*) as QtdeFunc, SUM(valor) as valor,
	(COUNT(*) / (select SUM(qtdefunclocal) from #resumoqtdefuncporlocal)) * 100 as Perc
	into #inadimplentemargem
	from #conciliacao c, #func f, #resumoqtdefuncporlocal r
	where c.IDConciliacaoGrupo = 2 and f.nomelocalfolha = r.nomelocalfolha
	group by c.IDConsignataria, descricao
	
	-- insercao da inadimplencia por faixa de margem    
    insert into TmpInadimplenciaPadraoMargem(Competencia, IDEmpresa, Descricao, Percentual, QtdeFunc)
    select @Competencia, idempresa, Descricao, Perc, QtdeFunc
    from #inadimplentemargem

	-- 
	select @CompetenciaInicial = MIN(competencia) from ConciliacaoCorteFolha
	select * from AverbacaoParcelaSituacao
	
    select c.IDConsignataria as IDEmpresa, c.IDFuncionario, f.nomesituacaofolha as situacao
    c.valordescontado, c.Valor, t.qtde as qtdeparcelasatrasada, t.valor as valoratrasado,
    (select COUNT(*) from AverbacaoParcela where IDAverbacao = a.idaverbacao and Competencia ) as ParcelasRestantes
    into #conciliacaoatrasado
    from Conciliacao c, ConciliacaoTipo ct, ConciliacaoMovimento cm, AverbacaoParcela ap, Averbacao a,
    (select a.IDAverbacao, COUNT(*) as qtde, SUM(ap.valor) as valor from AverbacaoParcela ap, Averbacao a, AverbacaoSituacao s 
    where ap.IDAverbacao = a.idaverbacao and a.IDAverbacaoSituacao = s.IDAverbacaoSituacao and s.DeduzMargem = 1 and 
    Competencia >= @CompetenciaInicial and IDAverbacaoParcelaSituacao in (1,4)
    group by a.IDAverbacao) as t
    where c.Competencia = @Competencia and c.IDConciliacaoTipo = ct.IDConciliacaoTipo and a.idaverbacao = t.idaverbacao
    and ct.IDConciliacaoGrupo = 2 and c.IDConciliacaoMovimento = cm.IDConciliacaoMovimento
    and cm.idaverbacaoparcela = ap.idaverbacaoparcela and ap.idaverbacao = a.idaverbacao

	insert into TmpInadimplenciaTempoDetalhe
	select c.IDConsignataria as idempresa,p.Nome, p.CPF, f.Matricula, f.NomeSituacaoFolha, c.valor
	into #inadimplente
	from #conciliacaoatrasado c, #func f, Pessoa p
	where c.IDFuncionario = f.IDFuncionario and f.idpessoa = p.idpessoa
END
GO
