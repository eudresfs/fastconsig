using CP.FastConsig.BLL;
using System.Linq;
using CP.FastConsig.DAL;
using System.Collections.Generic;


namespace CP.FastConsig.Facade
{
    public static class FachadaConciliacao
    {
        public static IQueryable<ConciliacaoTipo> ListarConciliacaoTipo()
        {
            return Averbacoes.ListaConciliacaoTipo();
        }

        public static IQueryable<Empresa> ListarConsignatarias()
        {
            return Empresas.ListaConsignatarias();
        }

        public static IQueryable<Conciliacao> ListarConciliacao(string anomes, int idconsignataria, int idsituacao)
        {
            return Averbacoes.ListarConciliacao(anomes, idconsignataria, idsituacao);
        }

        public static Averbacao ObtemAverbacao(int id)
        {
            return Averbacoes.ObtemAverbacao(id);
        }

        public static List<ConciliacaoResumoFolha> ListarConciliacaoResumoFolha(string competencia, int idempresa)
        {
            return Averbacoes.ListarConciliacaoResumoFolha(competencia, idempresa);
        }

        public static List<ConciliacaoResumoConciliacao> ListarConciliacaoResumoConciliacao(string competencia, int idempresa)
        {
            return Averbacoes.ListarConciliacaoResumoConciliacao(competencia, idempresa);
        }

        public static IQueryable<ConciliacaoRepasse> ListarConciliacaoRepasses(string competencia, int idempresa)
        {
            return Averbacoes.ListarConciliacaoRepasses(competencia, idempresa);
        }

        public static IQueryable<ConciliacaoResultadoConciliacao> ListarConciliacaoAnalise(string competencia, int idempresa, int meses)
        {
            return Averbacoes.ListarConciliacaoAnalise(competencia, idempresa, meses);
        }

        public static IQueryable<AverbacaoParcela> ListarAnaliseAverbacaos(string anomesInicio, string anomesFim, int idempresa)
        {
            return Averbacoes.ListarAnaliseAverbacoes(anomesInicio, anomesFim, idempresa);
        }
    
        public static string ultimaCompetenciaConciliada(int idempresa)
        {
            return Consignatarias.ultimaCompetenciaConciliada(idempresa);
        }

    }

}