using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CP.FastConsig.BLL;
using CP.FastConsig.DAL;

namespace CP.FastConsig.Facade
{

    public static class FachadaSubMenu
    {

        public static Recurso ObtemRecurso(int idrecurso)
        {
            return Geral.ObtemRecurso(idrecurso);
        }

        public static IQueryable<Recurso> ListaMenuOpcoes(int idpai, int idmodulo)
        {
            return Geral.ListaMenuOpcoes(idpai, idmodulo);
        }


        public static Permissao ObtemPermissao(int IdPermissao)
        {
            return Geral.ObtemPermissao(IdPermissao);
        }
    }

}