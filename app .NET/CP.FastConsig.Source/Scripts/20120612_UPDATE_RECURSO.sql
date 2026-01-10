update recurso set Imagem = 'IconeGerenciarPerfil.png' where nome = 'Gerenciar Perfis'
update recurso set Imagem = 'MenuCentralImportacaoBtn.png' where nome = 'Importar Aposentados'
update recurso set Imagem = 'MenuRelatorioAnaliseProducao.png' where nome = 'Relatório de Análise de Produção'

update recurso set Objetivo = 'Importe a lista de aposentados provenientes do órgão.' where nome = 'Importar Aposentados'
update recurso set Objetivo = 'Permite a Consignante analisar a produção das Consignatárias' where nome = 'Relatório de Análise de Produção' and idmodulo = 1
update recurso set Objetivo = 'Permite a consignatária analisar sua produção junto ao convênio.' where nome = 'Relatório de Análise de Produção' and idmodulo = 3