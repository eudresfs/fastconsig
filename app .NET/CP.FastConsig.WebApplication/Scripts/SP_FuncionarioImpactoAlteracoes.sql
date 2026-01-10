-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE SP_FuncionarioImpactoAlteracoes 
@Competencia varchar(7) AS
BEGIN
	SET NOCOUNT ON;
	
	declare @datatram varchar(30)
	declare @diacorte int
	declare @diacortehist int
	declare @PrazoMaximo int
	   
	-- limpar historico de alteracao do funcionario
	delete from FuncionarioHistorico where Competencia = @Competencia
	
    -- verificacao dos funcionarios que houveram alteracoes em seus dados principais
    select atual.idfuncionario, 
    (case when atual.IDFuncionarioSituacao <> antigo.IDFuncionarioSituacao then atual.IDFuncionarioSituacao else null end) as idsituacao,
    (case when atual.IDFuncionarioRegime <> antigo.IDFuncionarioRegime then atual.IDFuncionarioRegime else null end) as idregime,
    (case when atual.nomesituacaofolha <> antigo.NomeSituacaoFolha then atual.NomeSituacaoFolha else null end) as situacao,
    (case when atual.nomeregimefolha <> antigo.nomeregimefolha then atual.nomeregimefolha else null end) as regime,
    (case when atual.nomelocalfolha <> antigo.nomelocalfolha then atual.nomelocalfolha else null end) as local,
    (case when atual.nomecargofolha <> antigo.nomecargofolha then atual.nomecargofolha else null end) as cargo,
    (case when atual.nomesetorfolha <> antigo.nomesetorfolha then atual.nomesetorfolha else null end) as setor,
    (case when atual.MargemBruta <> antigo.MargemBruta then atual.MargemBruta else null end) as margembruta,
    (case when atual.MargemBase <> antigo.MargemBase then atual.MargemBase else null end) as margembase
    into #houvealteracao 
    from funcionariocargadados atual, funcionariocargadados antigo
    where atual.competencia = @Competencia and antigo.competencia = dbo.diminuicompetencia(@Competencia,1)
    and atual.idfuncionario = antigo.idfuncionario 

	-- inserção no historico do funcionario
	insert into FuncionarioHistorico(Competencia, IDFuncionario, IDFuncionarioSituacao, IDFuncionarioRegime, NomeSituacaoFolha, NomeRegimeFolha, NomeLocalFolha, NomeCargoFolha, NomeSetorFolha, MargemFolhaBruta, MargemFolhaDisponivel, CreatedBy, CreatedOn)
	select @Competencia, IDFuncionario, idsituacao, idregime, situacao, regime, local, cargo, setor, margembruta, margembase, 1, GETDATE() 
	from #houvealteracao
	where situacao is not null or regime is not null or local is not null or cargo is not null or setor is not null or margembruta is not null or margembase is not null    
	or idsituacao is not null or idregime is not null
	   
	-- limpar os dados da competencia
	delete from TmpMasNoticiasDetalhe 
	from TmpMasNoticias t, TmpMasNoticiasDetalhe td
	where t.competencia = @Competencia and t.Id = td.IDMasNoticia

	delete from TmpBoasNoticiasDetalhe 
	from TmpBoasNoticias t, TmpBoasNoticiasDetalhe td
	where t.competencia = @Competencia and t.Id = td.IDBoaNoticia

	delete from TmpMasNoticias where competencia = @Competencia   
	
	delete from TmpBoasNoticias where competencia = @Competencia   
	   
    -- verificar os funcionarios que sairam da folha
    select atual.idfuncionario, atual.nomesituacaofolha, p.cpf, p.Nome, atual.matricula 
    into #saiu 
    from funcionariocargadados atual, funcionariocargadados antigo, Pessoa p
    where atual.competencia = @Competencia and antigo.competencia = dbo.diminuicompetencia(@Competencia,1)
    and atual.idfuncionario = antigo.idfuncionario and atual.idfuncionariosituacao = 2 and antigo.idfuncionariosituacao = 1
    and atual.idpessoa = p.idpessoa

	-- obter dados de corte para seleção dos contrato elegiveis para desconto  
	select @diacorte = CONVERT(int,valor) from Parametro where Nome = 'DiaCorte'
	select @diacortehist=isnull(diacorte,0) from CorteHistorico where competencia=@Competencia

	if (@diacortehist > 0 and @diacortehist <> @diacorte)
		set @diacorte = @diacortehist	
	
    select @datatram = replace(@Competencia,'/','-')+'-'+dbo.PadString(CONVERT(varchar(2),@diacorte),'0',2)+' 23:59:59'

	-- select dos contratos elegiveis para desconto em folha para obtencao do saldo devedor, qtde. de contratos, etc.
	SELECT c.idconsignataria,c.idfuncionario,u.nomesituacaofolha,u.cpf, u.Nome, u.matricula,COUNT(distinct c.IDFuncionario) as QtdeFuncs,COUNT(distinct c.IDAverbacao) as QtdeContratos, SUM(cp.valor) as SaldoDevedor
	into #saldodevedor
	FROM Averbacao c, AverbacaoParcela cp, Funcionario f, Produto s, AverbacaoSituacao cs, ProdutoGrupo sg, ProdutoTipo st, #saiu u
	where c.IDAverbacao = cp.IDAverbacao and c.IDFuncionario = f.IDFuncionario and f.IDFuncionario = u.idfuncionario and s.IDProduto = c.IDProduto 
	and c.Ativo = 1 and f.Ativo = 1 and cp.Ativo = 1 and s.IDProdutoGrupo = sg.IDProdutoGrupo and sg.IDProdutoTipo = st.IDProdutoTipo
	and	cp.IDAverbacaoParcelaSituacao > 0 and cp.competencia >= @Competencia and (select top 1 idaverbacaosituacao from AverbacaoTramitacao where idaverbacao = c.idaverbacao and  createdon <= @datatram order by createdon desc) = cs.IDAverbacaoSituacao and
	(st.idProdutotipo = 3 or cs.ParaDescontoFolha = 1) 
	group by c.IDConsignataria,c.idfuncionario,u.nomesituacaofolha,u.cpf, u.Nome, u.matricula
    
    -- inserção na tabela de demonstração desses dados (TmpMasNoticias)
    insert into TmpMasNoticias(competencia, idconsignataria, DescricaoGrupo, Quantidade, SaldoDevedor, QuantidadeContratos,createdon)
    select @Competencia, idconsignataria, 'Saída de Funcionários de Minha Carteira', sum(QtdeFuncs), sum(SaldoDevedor), sum(QtdeContratos), GETDATE() 
    from #saldodevedor
    group by IDConsignataria
    
    -- detalhe
    insert into TmpMasNoticiasDetalhe(IDMasNoticia, Cpf, Matricula, Nome, Motivo, QtdeContratos, SaldoDevedor, IDFuncionario)
    SELECT t.ID, sd.Cpf, sd.Matricula, sd.Nome, sd.NomeSituacaoFolha, COUNT(*), sum(sd.saldoDevedor), sd.IDFuncionario
    FROM #saldodevedor sd, TmpMasNoticias t
    where t.Competencia = @Competencia and t.idconsignataria = sd.IDConsignataria 
    group by t.ID, sd.Cpf, sd.Matricula, sd.Nome, sd.NomeSituacaoFolha, sd.IDFuncionario
    
    -- verificar os funcionarios que entraram na folha
    select atual.idfuncionario, atual.nomesituacaofolha, p.cpf, p.Nome, atual.matricula, 0 as MargemAnterior, m_atual.margemfolha as MargemAtual
    into #entrou 
    from funcionariocargadados atual, Pessoa p, funcionariomargemcargadados m_atual
    where not exists(select 1 from funcionariocargadados antigo 
    where antigo.competencia = dbo.diminuicompetencia(@Competencia,1) and atual.idfuncionario = antigo.idfuncionario) 
    and atual.competencia = @Competencia and atual.idpessoa = p.IDPessoa
    and atual.idfuncionario = m_atual.idfuncionario and m_atual.competencia = @Competencia

    select @PrazoMaximo=CONVERT(int,valor) from Parametro where Nome = 'PrazoMaximo'
    
    -- inserção na tabela de demonstração desses dados (TmpBoasNoticias) - Novos Funcionários
    insert into TmpBoasNoticias(competencia, idconsignataria, IdGrupoBoaNoticia, Tipo, Qtde, ValorMargem, Possibilidade, createdon)
    select @Competencia, null, 1, 'Novos Funcionários', COUNT(*), SUM(margemfolha), SUM(margemfolha*@prazomaximo), GETDATE() 
    from #entrou e, funcionariomargemcargadados f
    where f.competencia = @Competencia and f.idprodutogrupo = 1 and e.idfuncionario = f.idfuncionario 

	-- detalhe
    insert into TmpBoasNoticiasDetalhe(IdBoaNoticia, Cpf, Matricula, Nome, MargemAnterior, MargemAtual, Possibilidade)
    SELECT t.ID, e.Cpf, e.Matricula, e.Nome, e.MargemAnterior, e.MargemAtual, @PrazoMaximo * e.MargemAtual
    FROM #entrou e, TmpBoasNoticias t
    where t.Competencia = @Competencia 
  
    -- verificar os funcionarios que tiveram aumento de margem na folha
    select atual.idfuncionario, p.Cpf, atual.Matricula, p.Nome, m_atual.margemfolha as MargemAnterior, m_antigo.margemfolha as MargemAtual, m_atual.margemfolha-m_antigo.margemfolha as aumentomargem
    into #aumentomargem
    from funcionariocargadados atual, funcionariocargadados antigo, funcionariomargemcargadados m_atual, funcionariomargemcargadados m_antigo, Pessoa p
    where antigo.competencia = dbo.diminuicompetencia(@Competencia,1) and atual.competencia = @Competencia 
    and m_antigo.competencia = dbo.diminuicompetencia(@Competencia,1) and m_atual.competencia = @Competencia
    and atual.idfuncionario = antigo.idfuncionario and m_atual.idfuncionario = m_antigo.idfuncionario 
    and atual.IDFuncionario = m_atual.IDFuncionario and atual.idpessoa = p.IDPessoa
    and m_atual.idprodutogrupo = 1 and m_antigo.idprodutogrupo = 1 and m_atual.margemfolha > m_antigo.margemfolha

    -- inserção na tabela de demonstração desses dados (TmpBoasNoticias) - Funcionários Antigos, ou seja, já existentes que tiveram aumento de margem
    insert into TmpBoasNoticias(competencia, idconsignataria, IdGrupoBoaNoticia, Tipo, Qtde, ValorMargem, Possibilidade, createdon)
    select @Competencia, null, 1, 'Funcionários Antigos com aumento de margem', COUNT(*), SUM(aumentomargem), SUM(margemfolha*@prazomaximo), GETDATE() 
    from #aumentomargem e, funcionariomargemcargadados f
    where f.competencia = @Competencia and f.idprodutogrupo = 1 and e.idfuncionario = f.idfuncionario 

	-- detalhe
    insert into TmpBoasNoticiasDetalhe(IdBoaNoticia, Cpf, Matricula, Nome, MargemAnterior, MargemAtual, Possibilidade)
    SELECT t.ID, e.Cpf, e.Matricula, e.Nome, e.MargemAnterior, e.MargemAtual, @PrazoMaximo * (MargemAtual-MargemAnterior)
    FROM #aumentomargem e, TmpBoasNoticias t
    where t.Competencia = @Competencia 

	-- verificacao dos clientes (funcionarios) de cada consignataria, ou seja, que possui averbação ativa
	select distinct a.idfuncionario, a.IDConsignataria, p.Cpf, f.Matricula, p.Nome
	into #meusclientes
	from Averbacao a, AverbacaoParcela ap, AverbacaoSituacao s, Funcionariocargadados f, Pessoa p
	where a.IDAverbacao = ap.IDAverbacao and ap.Competencia = @Competencia and f.competencia = @Competencia and a.IDAverbacaoSituacao = s.IDAverbacaoSituacao and f.IDPessoa = p.IDPessoa
	and s.ParaDescontoFolha = 1 and a.Ativo = 1 and ap.Ativo = 1 and a.IDFuncionario = f.IDFuncionario and f.IDFuncionarioSituacao = 1 and f.Ativo = 1
	
    -- inserção na tabela de demonstração desses dados (TmpBoasNoticias) - Meus Clientes, ou seja, já existentes que possui que tiveram aumento de margem
    insert into TmpBoasNoticias(competencia, idconsignataria, IdGrupoBoaNoticia, Tipo, Qtde, ValorMargem, Possibilidade, createdon)
    select @Competencia, m.idconsignataria, 1, 'Meus Clientes com aumento de margem', COUNT(*), SUM(aumentomargem), SUM(margemfolha*@prazomaximo), GETDATE() 
    from #aumentomargem e, funcionariomargemcargadados f, #meusclientes m
    where f.competencia = @Competencia and f.idprodutogrupo = 1 and e.idfuncionario = f.idfuncionario 
    and m.idfuncionario = f.idfuncionario
    group by m.IDConsignataria   
    
    -- detalhe
    insert into TmpBoasNoticiasDetalhe(IdBoaNoticia, Cpf, Matricula, Nome, MargemAnterior, MargemAtual, Possibilidade)
    SELECT t.ID, e.Cpf, e.Matricula, e.Nome, e.MargemAnterior, e.MargemAtual, @PrazoMaximo * (MargemAtual-MargemAnterior)
    FROM #aumentomargem e, TmpBoasNoticias t, #meusclientes m
    where t.Competencia = @Competencia and t.idconsignataria = m.IDConsignataria and m.idfuncionario = e.idfuncionario
END
GO
