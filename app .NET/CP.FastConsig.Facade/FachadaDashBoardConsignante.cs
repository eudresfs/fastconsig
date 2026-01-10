using System;
using System.Collections.Generic;
using System.Linq;
using CP.FastConsig.BLL;
using CP.FastConsig.DAL;
using CP.FastConsig.Common;
using CP.FastConsig.Util;

namespace CP.FastConsig.Facade
{

    public static class FachadaDashBoardConsignante
    {

        public static Parametro obterParametro(string nome)
        {
            return Geral.ObtemParametro( nome );
        }

        public static List<TmpEnviadosVDescontados> listaEnviadosVDescontados()
        {
            return Consignantes.listaEnviadosVDescontados().ToList();
        }

        public static List<ADescontar_E_Descontados> listaADescontarVDescontados(int nMeses)
        {
            List<ADescontar_E_Descontados> dados = new List<ADescontar_E_Descontados>();
            
            DateTime dtDataAtual = DateTime.Now.Date;

            for (int i = nMeses; i >= 1; i--)
            {
                DateTime dtData = dtDataAtual.AddMonths(i * (-1));

                string competencia = dtData.Year.ToString() + "/" + dtData.Month.ToString().PadLeft(2, '0' );

                decimal? valorADescontar = Consignantes.ADescontar(competencia);

                decimal? valorDescontados = Consignantes.Descontados(competencia);

                ADescontar_E_Descontados dadoNaoDescontado = new ADescontar_E_Descontados();
                dadoNaoDescontado.Mes = Utilidades.ConverteMesAno(competencia);
                dadoNaoDescontado.Tipo = "A Descontar";
                dadoNaoDescontado.Valor = (decimal)valorADescontar;

                dados.Add(dadoNaoDescontado);

                ADescontar_E_Descontados dadoDescontado = new ADescontar_E_Descontados();
                dadoDescontado.Mes = Utilidades.ConverteMesAno(competencia);
                dadoDescontado.Tipo = "Descontados";
                dadoDescontado.Valor = (decimal)valorDescontados;

                dados.Add(dadoDescontado); 

            }
            
            return dados;
        }


        public static List<TmpOcorrencias> obtemOcorrencias(int tipo)
        {
            return Consignantes.obtemOcorrencias(tipo).ToList();
        }

        public static List<EmpresaSolicitacaoTipo> listaSolicitacoesTipo()
        {
            return Consignantes.listaSolicitacoesTipo().ToList();
        }

        public static List<EmpresaSolicitacao> listaSolicitacoes()
        {
            return Consignantes.listaSolicitacoes().ToList();
        }

        public static List<Empresa> listaConsignatarias()
        {
            return Consignatarias.ObtemConsignatarias().ToList();
        }

        public static void AdicionaSolicitacao_Alerta(int idEmpresaSolicitante, int IDEmpresa, int IDSolicitacaoTipo, int Usuario)
        {
            string sOcorrencia = Solicitacoes.ObtemDescricao( IDSolicitacaoTipo );
            
            Solicitacoes.AdicionaSolicitacao(idEmpresaSolicitante, (int)Enums.SolicitacaoTipo.Ouvidoria , (int)Enums.SolicitacaoSituacao.Pendente, IDEmpresa, null, null, Usuario, null, "Alerta banco fora do prazo - " + sOcorrencia, "");
        }

        public static List<EmpresaSolicitacaoTipo> listaPrazosExecucao(string idmodulo)
        {
            return Consignantes.listaPrazosExecucao(idmodulo).ToList();
        }

        public static Empresa obtemEmpresa(int idempresa)
        {
            return Consignatarias.ObtemConsignataria(idempresa);
        }

        public static string IDEmpresaConsignante()
        {
            return Geral.IdEmpresaConsignante();
        }

        public static EmpresaSolicitacaoTipo obtemSolicitacaoTipo(int tipo)
        {
            return new Repositorio<EmpresaSolicitacaoTipo>().Listar().Where(x => x.IDEmpresaSolicitacaoTipo == tipo).FirstOrDefault();
        }

        public static EmpresaSuspensao ObtemEmpresaSuspensao(int idEmpresa)
        {
            return Consignatarias.ObtemSuspensoes(idEmpresa).ToList().FirstOrDefault();
        }

    }

}