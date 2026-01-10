using System.Collections.Generic;
using CP.FastConsig.BLL;
using CP.FastConsig.DAL;
using System.Linq;

namespace CP.FastConsig.Facade
{

    public static class FachadaConsignatariasEdicao
    {

        public static List<Estado> ObtemEstados()
        {
            return Geral.ObtemEstados();
        }

        public static List<EmpresaTipo> ObtemEmpresaTipos()
        {
            return Empresas.ObtemEmpresaTipos();
        }

        public static List<EmpresaTipo> ObtemTodasEmpresaTipos()
        {
            return Empresas.ObtemTodasEmpresaTipos();
        }

        public static void SalvarEmpresa(Empresa empresa)
        {
            Empresas.SalvarEmpresa(empresa);
        }

        public static object ObtemEmpresaPorCnpj(string cnpj)
        {
            return Empresas.ObtemEmpresaPorCnpj(cnpj);
        }

        public static Empresa ObtemEmpresa(int idEmpresa)
        {
            return Empresas.ObtemEmpresa(idEmpresa);
        }


        public static IQueryable<EmpresaTarifada> ObtemTipoTarifas()
        {
            return Empresas.ListaTipoTarifas();
        }

        public static void RestaurarPermissoes(int IdEmpresaEdicao)
        {
            Empresas.RestaurarPermissoes(IdEmpresaEdicao);
        }

        public static void SalvarVinculo(int idConsignataria, int idAgente)
        {
            Empresas.SalvarVinculo(idConsignataria, idAgente);
        }

    }

}