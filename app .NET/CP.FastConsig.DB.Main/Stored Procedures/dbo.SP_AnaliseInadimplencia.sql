SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_AnaliseInadimplencia] 
@Competencia varchar(7)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	declare @IdConsignante int
	declare @total decimal(18,2)
	declare @CompetenciaInicial varchar(7)

	-- excluindo dados das tabelas caso ja tinham sido processadas
	delete from TmpInadimplenciaGeralDetalhe where competencia = @Competencia

	delete from TmpInadimplenciaGeral where competencia = @Competencia
	
	delete from TmpInadimplenciaPadraoTrabalho where competencia = @Competencia 
	
	delete from TmpInadimplenciaPadraoMargem where competencia = @Competencia 
	
	delete from TmpMasNoticiasInadiplentesDetalhe 
	from TmpMasNoticiasInadiplentesDetalhe g, TmpMasNoticiasInadiplentes t
	where g.IDMasNoticiaInadiplente = t.Id and competencia = @Competencia 

	delete from TmpMasNoticiasInadiplentes where competencia = @Competencia 

	delete from TmpVolumeInadimplencia where Mes = @Competencia
	
	delete from TmpRecuperavelPorFolha where competencia = @Competencia

	delete from TmpNaoRecuperavel where competencia = @Competencia
	-- identificar a consignante	
	select @IdConsignante = idempresa 
	from Empresa 
	where IDEmpresaTipo = 2

	-- verificar volume de descontado e nao descontado por consinataria
    select c.IDConsignataria as IDEmpresa, ct.IDConciliacaoGrupo, c.IDFuncionario, c.valordescontado, c.valor, cm.parcela, cm.prazo
    into #conciliacao
    from Conciliacao c, ConciliacaoTipo ct, ConciliacaoGrupo cg, ConciliacaoMovimento cm
    where c.Competencia = @Competencia and c.IDConciliacaoTipo = ct.IDConciliacaoTipo and ct.IDConciliacaoGrupo = cg.IDConciliacaoGrupo
    and ct.IDConciliacaoGrupo in (1,2) and c.IDConciliacaoMovimento = cm.IDConciliacaoMovimento
    --group by c.IDConsignataria, ct.IDConciliacaoGrupo, c.IDFuncionario

	-- obtem o total da conciliacao para calculo do percentual
	select @total=isnull(SUM(valor),1) from #conciliacao
	
	-- consignante: inserção da inadimplencia geral
	insert into TmpInadimplenciaGeral(competencia,idempresa,Descricao,Percentual, Valor)
	select @Competencia,@IdConsignante,(case when IDConciliacaoGrupo = 1 then 'Conciliado' else 'Inadimplente' end),
	(SUM((case when IDConciliacaoGrupo = 1 then valordescontado else valor end))/@total) * 100,sum(valordescontado)
	from #conciliacao
	group by IDConciliacaoGrupo

	-- volume de inadimplencia por consignataria
	insert into TmpVolumeInadimplencia(mes,idempresa,SomaParcelas)
	select @Competencia,idempresa,
	SUM(valor)
	from #conciliacao c
	where c.IDConciliacaoGrupo = 2
	group by idempresa

	-- consignataria: inserção da inadimplencia geral por consignataria
	insert into TmpInadimplenciaGeral(competencia,idempresa,Descricao,Percentual, Valor)
	select @Competencia,idempresa,(case when IDConciliacaoGrupo = 1 then 'Conciliado' else 'Inadimplente' end),
	(SUM((case when IDConciliacaoGrupo = 1 then valordescontado else valor end))/(select isnull(SUM(valor),1) from #conciliacao where idempresa = c.idempresa)) * 100,sum(valordescontado)
	from #conciliacao c
	group by idempresa, IDConciliacaoGrupo

    select f.IDFuncionario, f.Matricula, f.IDFuncionarioSituacao, f.NomeLocalFolha, f.NomeSituacaoFolha, fm.MargemFolha, f.IDPessoa 
    into #func
    from funcionariocargadados f, FuncionarioMargem fm
    where f.Competencia='2011/12' and f.IDFuncionario = fm.IDFuncionario and fm.IDProdutoGrupo = 1 
    and 0 = 1
	
	-- consignante: inadimplencia geral detalhe, o que pode ser recuperavel (idfuncionariosituacao = 1) ou solucao externa (idfuncionariosituacao = 2)	
	if (exists(select 1 from funcionariocargadados where Competencia = @Competencia)) begin
	  insert into #func(IDFuncionario, Matricula, NomeLocalFolha, NomeSituacaoFolha, IDFuncionarioSituacao, MargemFolha, IDPessoa)
	  select f.IDFuncionario, f.Matricula, f.NomeLocalFolha, f.NomeSituacaoFolha, f.IDFuncionarioSituacao, fm.MargemFolha, f.IDPessoa 
	  from funcionariocargadados f, FuncionarioMargem fm
	  where f.Competencia=@Competencia and f.IDFuncionario = fm.IDFuncionario and fm.IDProdutoGrupo = 1 
	end
	else begin
	  insert into #func(IDFuncionario, Matricula, NomeLocalFolha, NomeSituacaoFolha, IDFuncionarioSituacao, MargemFolha, IDPessoa)
	  select f.IDFuncionario, f.Matricula, f.NomeLocalFolha, f.NomeSituacaoFolha, f.IDFuncionarioSituacao, fm.MargemFolha, f.IDPessoa 
	  from Funcionario f, FuncionarioMargem fm
	  where f.IDFuncionario = fm.IDFuncionario and fm.IDProdutoGrupo = 1
	end
	  
	select c.idempresa,c.IDFuncionario, c.valor, c.parcela, f.idfuncionariosituacao, c.idconciliacaogrupo
	into #inadimplente
	from #conciliacao c, #func f
	where c.IDConciliacaoGrupo = 2 and c.IDFuncionario = f.IDFuncionario
	--group by c.IDEmpresa,c.IDFuncionario

	select c.idempresa,c.IDFuncionario, c.valor, c.parcela, c.idfuncionariosituacao
	into #recuperavel
	from #inadimplente c
	where c.IDFuncionarioSituacao = 1

	--select p.Nome, f.Matricula, p.CPF, r.parcela, 
	--f.margemfolha - (select SUM(valorparcela) from Averbacao a, AverbacaoSituacao s, Produto pr
	--where a.IDFuncionario = f.idfuncionario and a.IDAverbacaoSituacao = s.IDAverbacaoSituacao 
	--and s.DeduzMargem = 1 and a.Ativo = 1 and a.IDProduto = pr.IDProduto and pr.IDProdutoGrupo = 1) as Margem,
	--(select COUNT(*) from Conciliacao)
	
	--from #recuperavel r, #func f, Pessoa p
	--where r.IDFuncionario = f.IDFuncionario and f.IDPessoa = p.IDPessoa

	--TmpRecuperavelPorFolha 
	
	--TmpNaoRecuperavel 

	-- consignataria: inserção da inadimplencia geral detalhe por consignataria
	insert into TmpInadimplenciaGeralDetalhe(competencia,idempresa,IDInadimplenciaGeral, Descricao,Percentual, Valor)
	select @Competencia,@IdConsignante, t.Id ,(case when IDFuncionariosituacao = 1 then 'Recuperável' else 'Solução Externa' end),
	(SUM(c.valor)/(select SUM(valor) from #inadimplente where IDFuncionarioSituacao = c.IDFuncionarioSituacao)) * 100,sum(c.valor)
	from #inadimplente c, TmpInadimplenciaGeral t
	where t.Competencia = @Competencia and t.IDEmpresa = @IdConsignante and t.Descricao = (case when IDConciliacaoGrupo = 1 then 'Conciliado' else 'Inadimplente' end)
	group by IDFuncionarioSituacao, t.Id
	    
	-- consignataria: inserção da inadimplencia geral detalhe por consignataria
	insert into TmpInadimplenciaGeralDetalhe(competencia,idempresa,IDInadimplenciaGeral,Descricao,Percentual, Valor)
	select @Competencia,c.idempresa,t.id, (case when IDFuncionariosituacao = 1 then 'Recuperável' else 'Solução Externa' end),
	(SUM(c.valor)/(select isnull(SUM(valor),1) from #inadimplente where IDFuncionarioSituacao = c.IDFuncionarioSituacao and idempresa = c.idempresa)) * 100,sum(c.valor)
	from #inadimplente c, TmpInadimplenciaGeral t
	where t.Competencia = @Competencia and c.IDEmpresa = t.IDEmpresa and t.Descricao = (case when IDConciliacaoGrupo = 1 then 'Conciliado' else 'Inadimplente' end)
	group by c.idempresa, IDFuncionarioSituacao, t.Id   

	-- resumo quantitativo de funcionarios por local
	select f.NomeLocalFolha, COUNT(*) as QtdeFuncLocal
	into #resumoqtdefuncporlocal
	from #func f
	where f.IDFuncionarioSituacao = 1
	group by f.NomeLocalFolha

	-- inadimplencia por local
	select c.idempresa,f.NomeLocalFolha as Descricao, COUNT(*) as QtdeFunc, 
	convert(decimal(18,6),(COUNT(*) / convert(decimal(18,2),(r.QtdeFuncLocal)))) * 100 as PercLocal, 
	(COUNT(*) / convert(decimal(18,2),(select SUM(qtdefunclocal) from #resumoqtdefuncporlocal))) * 100 as PercGeral
	into #inadimplentelocal
	from #conciliacao c, #func f, #resumoqtdefuncporlocal r
	where c.IDConciliacaoGrupo = 2 and c.IDFuncionario = f.IDFuncionario and f.nomelocalfolha = r.nomelocalfolha
	group by c.IDEmpresa,f.NomeLocalFolha, r.QtdeFuncLocal

	-- insercao da inadimplencia por local    
    insert into TmpInadimplenciaPadraoTrabalho(Competencia, IDEmpresa, Descricao, Percentual, PercentualGeral, QtdeFunc)
    select @Competencia, idempresa, Descricao, PercLocal, PercGeral, QtdeFunc
    from #inadimplentelocal
    
    -- resumo por margem
    select c.idempresa,
	(case when f.margemfolha <= 500 then '0 a 500 reais'
	when f.margemfolha > 500 and f.margemfolha <= 1000 then '500,01 a 1.000 reais'
	when f.margemfolha > 1000 and f.margemfolha <= 1500 then '1000,01 a 1.500 reais'
	when f.margemfolha > 1500 and f.margemfolha <= 2000 then '1500,01 a 2.000 reais'
	when f.margemfolha > 2000 then 'Acima de 2000,01 reais' else '' end) as descricao,
	COUNT(*) as QtdeFunc
	into #resumomargem
	from #conciliacao c, #func f
	where c.IDConciliacaoGrupo = 2 and c.IDFuncionario = f.IDFuncionario 
	group by c.IDEmpresa, (case when f.margemfolha <= 500 then '0 a 500 reais'
	when f.margemfolha > 500 and f.margemfolha <= 1000 then '500,01 a 1.000 reais'
	when f.margemfolha > 1000 and f.margemfolha <= 1500 then '1000,01 a 1.500 reais'
	when f.margemfolha > 1500 and f.margemfolha <= 2000 then '1500,01 a 2.000 reais'
	when f.margemfolha > 2000 then 'Acima de 2000,01 reais' else '' end)
	
	-- inadimplencia por faixa de margem
	select c.idempresa,
	(case when f.margemfolha <= 500 then '0 a 500 reais'
	when f.margemfolha > 500 and f.margemfolha <= 1000 then '500,01 a 1.000 reais'
	when f.margemfolha > 1000 and f.margemfolha <= 1500 then '1000,01 a 1.500 reais'
	when f.margemfolha > 1500 and f.margemfolha <= 2000 then '1500,01 a 2.000 reais'
	when f.margemfolha > 2000 then 'Acima de 2000,01 reais' else '' end) as descricao,
	COUNT(*) as QtdeFunc, SUM(valor) as valor,
	convert(decimal(18,2),(COUNT(*) / convert(decimal(18,6),(select SUM(qtdefunc) from #resumomargem where IDEmpresa = c.IDEmpresa))) * 100) as Perc
	into #inadimplentemargem
	from #conciliacao c, #func f
	where c.IDConciliacaoGrupo = 2 and c.IDFuncionario = f.IDFuncionario
	group by c.IDEmpresa, (case when f.margemfolha <= 500 then '0 a 500 reais'
	when f.margemfolha > 500 and f.margemfolha <= 1000 then '500,01 a 1.000 reais'
	when f.margemfolha > 1000 and f.margemfolha <= 1500 then '1000,01 a 1.500 reais'
	when f.margemfolha > 1500 and f.margemfolha <= 2000 then '1500,01 a 2.000 reais'
	when f.margemfolha > 2000 then 'Acima de 2000,01 reais' else '' end)
		
	-- insercao da inadimplencia por faixa de margem    
    insert into TmpInadimplenciaPadraoMargem(Competencia, IDEmpresa, Descricao, Percentual, QtdeFunc)
    select @Competencia, idempresa, Descricao, Perc, QtdeFunc
    from #inadimplentemargem

	select @CompetenciaInicial = MIN(competencia) from ConciliacaoCorteFolha
	
    select c.IDConsignataria as IDEmpresa, c.IDFuncionario, 
    c.valordescontado, c.Valor, t.qtde as qtdeparcelasatrasada, t.valor as valoratrasado
    into #conciliacaoatrasado
    from Conciliacao c, ConciliacaoTipo ct, ConciliacaoMovimento cm, AverbacaoParcela ap, Averbacao a,
    (select a.IDAverbacao, COUNT(*) as qtde, SUM(ap.valor) as valor from AverbacaoParcela ap, Averbacao a, AverbacaoSituacao s 
    where ap.IDAverbacao = a.idaverbacao and a.IDAverbacaoSituacao = s.IDAverbacaoSituacao and s.DeduzMargem = 1 and 
    Competencia >= @CompetenciaInicial and IDAverbacaoParcelaSituacao in (1,4)
    group by a.IDAverbacao) as t
    where c.Competencia = @Competencia and c.IDConciliacaoTipo = ct.IDConciliacaoTipo and a.idaverbacao = t.idaverbacao
    and ct.IDConciliacaoGrupo = 2 and c.IDConciliacaoMovimento = cm.IDConciliacaoMovimento
    and cm.idaverbacaoparcela = ap.idaverbacaoparcela and ap.idaverbacao = a.idaverbacao

	select c.idempresa,p.Nome, p.CPF, f.Matricula, f.NomeSituacaoFolha, c.valor
	into #inadimplentes
	from #conciliacaoatrasado c, #func f, Pessoa p
	where c.IDFuncionario = f.IDFuncionario and f.idpessoa = p.idpessoa
	
	insert into TmpMasNoticiasInadiplentes(Competencia, idempresa, DescricaoGrupo, Quantidade, NaoConciliado, QuantidadeContrato, Parcelas)
    select @Competencia as competencia, idempresa,'Inadimplentes' as descricao, COUNT(distinct c.IDFuncionario) as qtde,
    SUM(valoratrasado) as valor, COUNT(*) as qtdecontrato, SUM(qtdeparcelasatrasada)
	from #conciliacaoatrasado c, #func f, Pessoa p
	where c.IDFuncionario = f.IDFuncionario and f.idpessoa = p.idpessoa
	group by c.IDEmpresa
	
	insert into TmpMasNoticiasInadiplentesDetalhe(IDMasNoticiaInadiplente, Cpf, Matricula, Nome, Valor, QtdeParcelas)
    select t.id, p.CPF, f.Matricula, p.Nome, valoratrasado, qtdeparcelasatrasada
	from #conciliacaoatrasado c, #func f, Pessoa p, TmpMasNoticiasInadiplentes t
	where c.IDFuncionario = f.IDFuncionario and f.idpessoa = p.idpessoa
	and t.Competencia = @Competencia and c.IDEmpresa = t.IDEmpresa 
	
END
GO
