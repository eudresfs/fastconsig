using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CP.FastConsig.BLL;
using CP.FastConsig.DAL;


namespace CP.FastConsig.Facade
{
    public static class FachadaExportacao
    {
        public static IQueryable<Averbacao> ExportaAberbacao()
        {
            return Averbacoes.ExportaAverbacao();
        }

        public static void ArquivoParaDescontoFolha(string mesano)
        {
            Averbacoes.ArquivoParaDescontoFolha(mesano);
        }
    }
}
