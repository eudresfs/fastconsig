using System;
using CP.FastConsig.DAL;
using CP.FastConsig.BLL;
using System.Linq;

namespace CP.FastConsig.Facade
{

    public static class FachadaConsignatarias
    {

        public static void RemoveEmpresa(int idEmpresa)
        {
            Empresas.RemoveEmpresa(idEmpresa);
        }

        public static Empresa ObtemEmpresa(int idEmpresa)
        {
            return Empresas.ObtemEmpresa(idEmpresa);
        }

        public static IQueryable<Empresa> ListaConsignatarias()
        {
            var dados = new Repositorio<Empresa>().Listar();
            var consulta = from d in dados
                           where d.EmpresaTipo.Consignataria
                           select d;
            return consulta;
        }

        public static IQueryable<TipoPagamento> ListaTiposPagamentos()
        {
            return Consignatarias.ListaTiposPagamentos();
        }

        public static string obtemPrazoMaximo(int consignataria)
        {
            return new Repositorio<Produto>().Listar().Where( x => x.IDConsignataria == consignataria && x.Ativo == 1).Max(x => x.PrazoMaximo).ToString();
        }

        public static IQueryable<Produto> Produtos(int consignataria)
        {
            return new Repositorio<Produto>().Listar().Where(x => x.IDConsignataria == consignataria && x.Ativo == 1);
        }

        public static int diaCorteConsignataria(int idempresa)
        {

            Empresa e = ObtemEmpresa( idempresa );

            int diaCorte;

            if ((e.DiaCorte.Equals(null)) || (e.DiaCorte.Equals(0)))
                diaCorte = Convert.ToInt32(FachadaGeral.obtemParametro("DiaCorte").Valor);
            else
                diaCorte = (int)e.DiaCorte.Value;

            return diaCorte;

        }

        public static bool existeVerbaEmpresa(int produtogrupo, int empresa)
        {
            return new Repositorio<Produto>().Listar().Where( x => x.IDConsignataria == empresa && x.IDProdutoGrupo == produtogrupo && x.Ativo == 1).Count() > 0;
        }

    }

}