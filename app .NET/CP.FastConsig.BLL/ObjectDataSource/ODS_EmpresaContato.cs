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
    public class ODS_EmpresaContato
    {

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IQueryable<EmpresaContato> SelectGrid(int IdEmpresa, string sortExpression, string nameSearchString, int startRowIndex, int maximumRows)
        {
            Repositorio<EmpresaContato> serv = new Repositorio<EmpresaContato>();

            if (String.IsNullOrWhiteSpace(sortExpression))
            {
                sortExpression = "";
            }
            if (String.IsNullOrWhiteSpace(nameSearchString))
            {
                nameSearchString = "";
            }

            var dados = serv.PesquisaTextual(nameSearchString, sortExpression);

            dados = dados.Where(x => x.IDEmpresa == IdEmpresa);

             if (!string.IsNullOrEmpty(sortExpression))
                dados = dados.OrderBy(sortExpression);
            else
            {
                dados = dados.OrderBy(serv.ChavePrimaria());
            }

            Quantidade = dados.ToList().Count;

            return dados.ListarDaPagina(startRowIndex, maximumRows);
        }

        private int Quantidade;

        public int SelectGridCount(int IdEmpresa, string nameSearchString)
        {
            return Quantidade;
        }
    }
}
