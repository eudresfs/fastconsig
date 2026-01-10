using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CP.FastConsig.DAL;
using CP.FastConsig.BLL;

namespace CP.FastConsig.Facade
{
    public static class FachadaIndicesNegocios
    {
        public static IQueryable<IndiceNegocioRealizar> indicesAConquistar(string competencia, int IDEmpresa)
        {
            return new Repositorio<IndiceNegocioRealizar>().Listar().Where(x => x.IDEmpresa == IDEmpresa && x.Competencia == competencia);
        }

        public static IQueryable<IndiceNegocioRealizado> indicesConquistado(string competencia, int IDEmpresa)
        {
            return new Repositorio<IndiceNegocioRealizado>().Listar().Where(x => x.IDEmpresa == IDEmpresa && x.Competencia == competencia);
        }

        public static IQueryable<IndiceNegocioAntecipar> indicesAntecipar(string competencia, int IDEmpresa)
        {
            return new Repositorio<IndiceNegocioAntecipar>().Listar().Where(x => x.IDEmpresa == IDEmpresa && x.Competencia == competencia);
        }

    }
}
