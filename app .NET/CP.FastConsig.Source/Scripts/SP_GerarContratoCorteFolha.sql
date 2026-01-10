ALTER PROCEDURE [dbo].[SP_GerarContratoCorteFolha]
	@AnoMes varchar(7)
AS

SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ COMMITTED

declare @idcorte int
declare @datatram varchar(30)
declare @dia varchar(2)

select @idcorte = idcortehistorico, @dia = convert(varchar(2),DiaCorte) from CorteHistorico where Competencia = @AnoMes
select @datatram = replace(@AnoMes,'/','-')+'-'+@dia+' 23:59:59'

-- zerar tabela para nova geração
delete from ContratoCorteFolha where competencia = @AnoMes 

-- gerar parcelas das mensalidades
insert into contratoparcela(IDContrato,numero,competencia,valor,IDContratoParcelaSituacao,ativo)
select c.IDContrato, 1, @AnoMes, c.valorparcela, 1, 1
FROM Contrato c, Funcionario f, Servico s, ContratoSituacao cs, ServicoGrupo sg, ServicoTipo st
where c.IDFuncionario = f.IDFuncionario and s.IDServico = c.IDServico and c.IDContratoSituacao = cs.IDContratoSituacao
and c.Ativo = 1 and f.Ativo = 1 and f.IDFuncionarioSituacao = 1 and s.IDServicoGrupo = sg.IDServicoGrupo and sg.IDServicoTipo = st.IDServicoTipo
and st.IDServicoTipo = 2 and cs.ParaDescontoFolha = 1
and c.competenciainicial <= @AnoMes 
and not exists(select 1 from contratoparcela cp where cp.idcontrato = c.idcontrato and cp.competencia = @AnoMes and cp.Ativo = 1)

-- inclusão na tabela de corte
insert into ContratoCorteFolha(competencia,idcontratoparcela,IDContratoSituacao,Data,IDCorteHistorico,ValorParcela, Verba, Matricula, IDServicoGrupo, IDServicoTipo, IDConsignataria, IDFuncionario)
SELECT @AnoMes, cp.IDContratoParcela, ct.IDContratoSituacao, GETDATE(), @idcorte, cp.Valor, s.verbafolha, f.Matricula, s.IDServicoGrupo, st.IDServicoTipo, c.IDConsignataria, c.IDFuncionario	
FROM Contrato c, ContratoParcela cp, Funcionario f, Servico s, ContratoSituacao cs, ServicoGrupo sg, ServicoTipo st, (select top 1 IDContrato, IDContratoSituacao from contratotramitacao where createdon <= @datatram order by idcontrato asc,idcontratotramitacao desc) ct
where c.IDContrato = cp.IDContrato and c.IDFuncionario = f.IDFuncionario and s.IDServico = c.IDServico and  c.IDContrato = ct.IDContrato
and c.Ativo = 1 and f.Ativo = 1 and f.IDFuncionarioSituacao = 1 and cp.Ativo = 1 and s.IDServicoGrupo = sg.IDServicoGrupo and sg.IDServicoTipo = st.IDServicoTipo
and	cp.IDContratoParcelaSituacao > 0 and cp.competencia = @AnoMes and ct.IDContratoSituacao = cs.IDContratoSituacao and
(st.idservicotipo = 3 or cs.ParaDescontoFolha = 1) 

