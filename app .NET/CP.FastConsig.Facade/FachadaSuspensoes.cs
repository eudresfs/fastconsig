using System.Collections.Generic;
using CP.FastConsig.BLL;
using System.Linq;
using CP.FastConsig.DAL;

namespace CP.FastConsig.Facade
{

    public static class FachadaSuspensoes
    {

        public static void RemoveSuspensao(int IdEmpresaSuspensao)
        {
            Empresas.RemoveSuspensao(IdEmpresaSuspensao);
        }


        public static IQueryable<EmpresaSituacao> ListaSituacao()
        {
            return new Repositorio<EmpresaSituacao>().Listar();
        }

        public static EmpresaSuspensao ObtemSuspensao(int IdSuspensaoEdicao)
        {
            return Empresas.ObtemSuspensao(IdSuspensaoEdicao);
        }

        public static void SalvarSuspensao(EmpresaSuspensao dado)
        {
            Empresas.SalvarSuspensao(dado);
        }

        public static Empresa ObtemEmpresa(int idEmpresa)
        {
            return Empresas.ObtemEmpresa(idEmpresa);
        }

        public static EmpresaSuspensao ObtemUltimaSuspensao(int idEmpresa)
        {
            return Empresas.ObtemUltimaSuspensao(idEmpresa);
        }

        public static List<EmpresaSuspensao> ObtemSuspensoes(int idEmpresa)
        {
            return Empresas.ObtemSuspensoes(idEmpresa);
        }

    }

}