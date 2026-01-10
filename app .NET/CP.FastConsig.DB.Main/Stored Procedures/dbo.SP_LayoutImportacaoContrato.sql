SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
--exec SP_LayoutImportacaoContrato 0
--delete from averbacaoparcela
--delete from averbacaotramitacao
--delete from ImportacaoAverbacaoCritica
--delete from averbacao

--select MAX(competencia) from AverbacaoParcela
--select * from AverbacaoParcela
--select * from ImportacaoAverbacaoCritica

--select * from Funcionario where Matricula='4248'
--select * from Averbacao where IDFuncionario=2492895
--select * from Produto
--select distinct verba from AverbacaoImportacao
--select * from AverbacaoImportacao where Verba is null


CREATE PROCEDURE [dbo].[SP_LayoutImportacaoContrato](@ID_Historico int) 
AS
BEGIN
	SET NOCOUNT ON;	
	
    declare @idcontrato int
    declare @ImportacaoContratoNaoAlterar varchar(1)
    declare @ImportacaoTipoOperacao varchar(1) -- T-Geral, I-Somente Inclusao, A-Somente Alteracao
    declare @ImportacaoVerificaMargem varchar(1) -- N-Nao considera margem, S-Considera a margem para validação da inclusao/alteracao do contrato
    declare @CalcDivMargem decimal(15,2)
    declare @CalcMultMargem decimal(15,2)
    declare @AplicarMargemParcial varchar(1)
    declare @Competencia varchar(7)
    declare @ParcelaCorrente int
    declare @Contratos_Gerados table(idaverbacao int)
	
	select * into #temp_importacao from AverbacaoImportacao 
		
	-- ALTERACAO DOS CONTRATOS
/*	select t.id,a.IDAverbacao, t.valorparcela, t.prazo, t.MesAnoPrimeiraParcela as competencia, t.parcelacorrente, t.Obs, f.IDFuncionario, 
	p.idconsignataria, p.IDProduto, pg.IDProdutoTipo, a.IDAverbacaoSituacao
	into #existe 
	from #temp_importacao t, Averbacao a, Funcionario f, Produto p, ProdutoGrupo pg, ProdutoTipo pt
	where t.matricula = f.matricula and f.IDFuncionario = a.idfuncionario 
	and a.IDProduto = p.IDProduto and p.IDProdutoGrupo = pg.IDProdutoGrupo and pg.IDProdutoTipo = pt.IDProdutoTipo
	and p.verba = t.verba 
	and (t.numero is null or t.numero = a.numero)	 
*/
    -- PROCESSAMENTO DE CRITICAS --> matricula inexistente
	select t.id,t.matricula  
	into #critica_matricula
	from #temp_importacao t
	where not exists(select 1 from Funcionario where matricula = t.matricula and Ativo = 1)

    insert into ImportacaoAverbacaoCritica(descricao)
    select 'Matricula inexistente! --> Matricula:'+isnull(matricula,'') 
    from #critica_matricula where matricula is not null          
    -----------------------------------------------------------------------------------------------------------------------------------    

    -- PROCESSAMENTO DE CRITICAS --> verba inexistente
	select t.id,t.matricula, t.verba  
	into #critica_verba
	from #temp_importacao t
	where not exists(select 1 from Produto where verba = t.verba and Ativo = 1)

    insert into ImportacaoAverbacaoCritica(descricao)
    select 'Verba inexistente! --> Verba:'+isnull(verba,'')+'  Matricula:'+isnull(matricula,'')
    from #critica_verba where matricula is not null          
    -----------------------------------------------------------------------------------------------------------------------------------    

    -- INCLUSAO DE CONTRATOS   
    select t.numero, t.valorparcela, t.prazo, t.CompetenciaInicial as competencia, t.parcelacorrente, t.Obs, f.idfuncionario, 
    p.idconsignataria, p.IDProduto, pg.idprodutotipo
    into #naoexiste 
	from #temp_importacao t, Funcionario f, Produto p, ProdutoGrupo pg, produtotipo pt
	where t.matricula = f.matricula 
	and p.IDProdutoGrupo = pg.IDProdutoGrupo and pg.IDProdutoTipo = pt.IDProdutoTipo
	and p.verba = t.verba and f.Ativo = 1
    
    insert into Averbacao(numero,data,IDFuncionario,idconsignataria,IDProduto,valorparcela, ValorDeducaoMargem,IDAverbacaoSituacao,Ativo,obs,
	IDAverbacaoTipo,idusuario,ValorDevidoTotal, ValorContratado,prazo,competenciainicial,createdby,createdon)
	OUTPUT INSERTED.IDAverbacao into @Contratos_Gerados
	select t.numero, getdate(),IDFuncionario,idconsignataria,IDProduto,t.valorparcela, t.ValorParcela,2,1,t.obs,
	1,1,t.prazo * t.valorparcela,0,t.prazo,t.competencia,1,getdate()
	from #naoexiste t
	
    -- gerar tramitacao
    insert into AverbacaoTramitacao(IDAverbacao,IDAverbacaoSituacao,createdby,createdon,obs)
    select g.idaverbacao,1,1,getdate(),a.obs from @Contratos_Gerados g, Averbacao a
    where g.idaverbacao = a.IDAverbacao
    
    -- gerar parcelas
	DECLARE contratos CURSOR FOR select g.idaverbacao,competenciainicial,1
	from @Contratos_Gerados g, averbacao a, produto p, produtogrupo pg, produtotipo pt 
	where g.idaverbacao = a.idaverbacao and a.idproduto = p.idproduto and p.idprodutogrupo = pg.idprodutogrupo 
	and pg.idprodutotipo = pt.idprodutotipo and pt.idprodutotipo = 1 
	OPEN contratos
	FETCH NEXT FROM contratos INTO @idcontrato,@competencia,@parcelacorrente

	WHILE @@FETCH_STATUS = 0
	BEGIN        
      exec SP_ImportacaoAverbacao_Parcelas @idcontrato,@competencia,@parcelacorrente
      FETCH NEXT FROM contratos INTO @idcontrato,@competencia,@parcelacorrente
	END
	CLOSE contratos
	DEALLOCATE contratos		
		
END
GO
