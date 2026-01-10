using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CP.FastConsig.DAL;
using CP.FastConsig.BLL;

namespace CP.FastConsig.Facade
{
    public static class FachadaFluxo
    {

        public static IQueryable<FluxoTipo> ListaFluxoTipo()
        {
            return Geral.ListaFluxoTipo();
        }

        public static IQueryable<EmpresaSolicitacaoTipo> ListaSolitacaoTipo()
        {
            return Geral.ListaSolicitacaoTipo();
        }
    }
}
