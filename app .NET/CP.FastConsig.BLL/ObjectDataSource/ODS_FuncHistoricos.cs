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
    public class ODS_FuncHistoricos
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IQueryable<FuncionarioHistorico> SelectGrid(int IdFuncionario, string nameSearchString, string sortExpression, int startRowIndex, int maximumRows)
        {
            Repositorio<FuncionarioHistorico> fa = new Repositorio<FuncionarioHistorico>();

            if (String.IsNullOrWhiteSpace(sortExpression))
            {
                sortExpression = "";
            }
            if (String.IsNullOrWhiteSpace(nameSearchString))
            {
                nameSearchString = "";
            }

            var dados = fa.Listar().Where(x => x.IDFuncionario == IdFuncionario && (x.NomeCargoFolha.Contains(nameSearchString) || x.NomeRegimeFolha.Contains(nameSearchString) || x.NomeLocalFolha.Contains(nameSearchString) || x.NomeSetorFolha.Contains(nameSearchString) || x.NomeSituacaoFolha.Contains(nameSearchString)));

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
