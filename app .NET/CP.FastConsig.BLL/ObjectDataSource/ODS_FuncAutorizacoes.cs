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
    public class ODS_FuncAutorizacoes
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IQueryable<FuncionarioAutorizacao> SelectGrid(int IdFuncionario, string nameSearchString, string sortExpression, int startRowIndex, int maximumRows)
        {
            Repositorio<FuncionarioAutorizacao> fa = new Repositorio<FuncionarioAutorizacao>();

            if (String.IsNullOrWhiteSpace(sortExpression))
            {
                sortExpression = "";
            }
            if (String.IsNullOrWhiteSpace(nameSearchString))
            {
                nameSearchString = "";
            }

            var dados = fa.PesquisaTextual(nameSearchString, sortExpression);

            dados = dados.Where(x => x.IDFuncionario == IdFuncionario);

             if (!string.IsNullOrEmpty(sortExpression))
                dados = dados.OrderBy(sortExpression);
            else
            {
                dados = dados.OrderBy(fa.ChavePrimaria());
            }
           //if (!string.IsNullOrEmpty(sortExpression))
            //    dados = dados.OrderBy(sortExpression);
            //else
            //{
            //    dados = dados.OrderBy(func.ChavePrimaria());
            //}

            Quantidade = dados.ToList().Count;

            return dados.ListarDaPagina(startRowIndex, maximumRows);
        }

        private int Quantidade;

        public int SelectGridCount(int IdFuncionario, string nameSearchString)
        {
            return Quantidade;
        }
    }
}
