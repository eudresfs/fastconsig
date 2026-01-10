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
    public class ODS_Averbacao
    {

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IQueryable<Averbacao> SelectAverbacaoDoFunc(int IDFunc, string sortExpression, int startRowIndex, int maximumRows)
        {
            Repositorio<Averbacao> emp = new Repositorio<Averbacao>();

            if (String.IsNullOrWhiteSpace(sortExpression))
            {
                sortExpression = "";
            }

            var dados = emp.Listar().Where(x => x.IDFuncionario == IDFunc);
            if (!string.IsNullOrEmpty(sortExpression))
                dados = dados.OrderBy(sortExpression);
            else
            {
                dados = dados.OrderBy(emp.ChavePrimaria());
            }

            Quantidade = dados.ToList().Count;

            return dados.ListarDaPagina(startRowIndex, maximumRows);
        }

        private int Quantidade;

        public int SelectAverbacaoDoFuncCount(int IDFunc)
        {
            return Quantidade;
        }
    }
}
