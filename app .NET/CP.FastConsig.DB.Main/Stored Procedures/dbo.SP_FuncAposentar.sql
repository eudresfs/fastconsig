SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].[SP_FuncAposentar]
	-- Add the parameters for the stored procedure here
	@IDFunc int
AS
BEGIN
	SET NOCOUNT ON;
	declare @BDPrev varchar(50)
	declare @sql varchar(3250)
	declare @idconsignante int

	select @idconsignante=idempresa from empresa where idempresatipo = 2

	update funcionario set idfuncionariocategoria = 3, idfuncionariosituacao = 5, nomesituacaofolha='Aposentado'
	where idfuncionario = @IDFunc

	insert into averbacaotramitacao(idaverbacao,idaverbacaosituacao,obs,createdby,createdon)
	select idaverbacao, 7, 'Suspensão da Averbação por Aposentadoria do Funcionário', 1, getdate()
	from averbacao a, averbacaosituacao s
	where idfuncionario = @IDFunc and ativo = 1	
	and a.idaverbacaosituacao = s.idaverbacaosituacao and s.deduzmargem = 1
	
	select @BDPrev = valor from parametro where nome='BDPrevidencia' and isnull(valor,'') <> ''

	set @sql = 'insert into funcionarioaposenta(data,idfuncionario,matricula,nome,cpf,numero,identificador,competenciainicial,
	competenciafinal,prazo,valorparcela,nomeconsignataria,verba,valorcontrato)
	select getdate(), a.idfuncionario, f.matricula, p.nome, p.cpf, a.numero, a.identificador, a.competenciainicial, 
	a.competenciafinal,a.prazo,a.valorparcela,e.fantasia,p.verbafolha,a.valorcontratado
	from averbacao a, averbacaosituacao s, funcionario f, empresa e, produto p, pessoa o
	where idfuncionario = '+convert(varchar(10),@IDFunc)+' and ativo = 1 and a.idfuncionario = f.idfuncionario
	and a.idaverbacaosituacao = s.idaverbacaosituacao and s.deduzmargem = 1 
	and a.idconsignataria = e.idempresa and a.idproduto = p.idproduto and f.idpessoa = p.idpessoa '
	
	EXECUTE(@sql)
	
	if @BDPrev <> '' begin
		set @sql = 'insert into '+@BDPrev+'..funcionarioaposenta(data,idfuncionario,matricula,nome,cpf,numero,identificador,competenciainicial,
		competenciafinal,prazo,valorparcela,nomeconsignataria,verba,valorcontrato)
		select getdate(), a.idfuncionario, f.matricula, p.nome, p.cpf, a.numero, a.identificador, a.competenciainicial, 
		a.competenciafinal,a.prazo,a.valorparcela,e.fantasia,p.verbafolha,a.valorcontratado
		from averbacao a, averbacaosituacao s, funcionario f, empresa e, produto p, pessoa o
		where idfuncionario = '+convert(varchar(10),@IDFunc)+' and ativo = 1 and a.idfuncionario = f.idfuncionario
		and a.idaverbacaosituacao = s.idaverbacaosituacao and s.deduzmargem = 1 
		and a.idconsignataria = e.idempresa and a.idproduto = p.idproduto and f.idpessoa = o.idpessoa '
		
		EXECUTE(@sql)

		set @sql = 'insert into empresasolicitacao(idempresa,idempresasolicitacaotipo,
		idempresasolicitacaosituacao,datasolicitacao,descricao,motivo) 
		values ('+convert(varchar(10),@idconsignante)+',19,1,getdate(),"Importar Funcionários Aposentados","Importar Funcionários Aposentados"'
		
		EXECUTE(@sql)	
	end
END
GO
