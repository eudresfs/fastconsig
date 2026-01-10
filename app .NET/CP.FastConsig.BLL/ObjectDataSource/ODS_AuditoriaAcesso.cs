using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.ComponentModel;
using CP.FastConsig.DAL;
using System.Linq.Expressions;

namespace CP.FastConsig.BLL
{
    public class ODS_AuditoriaAcesso
    {

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IQueryable<AuditoriaAcesso> SelectGrid(string sortExpression, string nameSearchString, int IdEmpresa, int startRowIndex, int maximumRows)
        {
            IQueryable<AuditoriaAcesso> auditorias = Geral.PesquisarAuditoriaAcesso(nameSearchString);

            if (String.IsNullOrWhiteSpace(sortExpression))
            {
                sortExpression = "";
            }
            if (String.IsNullOrWhiteSpace(nameSearchString))
            {
                nameSearchString = "";
            }

            auditorias = auditorias.Where(x => x.IDEmpresa == IdEmpresa).OrderByDescending(x => x.IDAuditoriaAcesso);
            Quantidade = auditorias.ToList().Count;

            return string.IsNullOrEmpty(sortExpression) ? auditorias.ListarDaPagina(startRowIndex, maximumRows).OrderByDescending(x => x.IDAuditoriaAcesso) : auditorias.ListarDaPagina(startRowIndex, maximumRows);
        }

        private int Quantidade;

        public int SelectGridCount(string nameSearchString, int IdEmpresa)
        {
            return Quantidade;
        }
    }
}
