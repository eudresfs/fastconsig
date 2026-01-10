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
    public class ODS_Funcionario
    {

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IQueryable<Funcionario> SelectGrid(string sortExpression, string nameSearchString, int IdEmpresa, int startRowIndex, int maximumRows)
        {
            Repositorio<Funcionario> func = new Repositorio<Funcionario>();

            //IQueryable<Pessoa> dadospessoas = Funcionarios.PesquisarPessoas(nameSearchString);

            IQueryable<Funcionario> funcDePessoas;

            if (nameSearchString.ToLower() == "clientes")
            {
                funcDePessoas = new Repositorio<Averbacao>().Listar().Where(w => w.IDConsignataria == IdEmpresa && w.AverbacaoSituacao.DeduzMargem).Select(x => x.Funcionario).Distinct(); //PesquisaTextual(texto,"");
            }
            else if (nameSearchString.ToLower() == "clientes com margem")
            {
                funcDePessoas = new Repositorio<Averbacao>().Listar().Where(w => w.IDConsignataria == IdEmpresa && w.AverbacaoSituacao.DeduzMargem).Select(x => x.Funcionario).Distinct(); //PesquisaTextual(texto,"");
                funcDePessoas = (from f in funcDePessoas.ToList()
                                where f.MargemDisponivel1 > 0
                                select f).Distinct().AsQueryable();
            }
            else
            {
                funcDePessoas = Funcionarios.PesquisarFuncionarios(nameSearchString);
            }
                //funcDePessoas = from p in dadospessoas
                //  from f in p.Funcionario
                //  select f;

            if (String.IsNullOrWhiteSpace(sortExpression))
            {
                sortExpression = "";
            }
            if (String.IsNullOrWhiteSpace(nameSearchString))
            {
                nameSearchString = "";
            }

            //var dados = func.PesquisaTextual(nameSearchString, sortExpression);

            var dados = funcDePessoas; //dados.Concat(funcDePessoas);

            //if (!string.IsNullOrEmpty(sortExpression))
            //    dados = dados.OrderBy(sortExpression);
            //else
            //{
            //    dados = dados.OrderBy(func.ChavePrimaria());
            //}
            dados = dados.OrderBy(x => x.Pessoa.Nome);

            Quantidade = dados.ToList().Count;

            return string.IsNullOrEmpty(sortExpression) ? dados.ListarDaPagina(startRowIndex, maximumRows).OrderBy(x => x.Pessoa.Nome) : dados.ListarDaPagina(startRowIndex, maximumRows);
        }

        private int Quantidade;

        public int SelectGridCount(string nameSearchString, int IdEmpresa)
        {
            if (Quantidade == 0)
                SelectGrid("", nameSearchString, IdEmpresa, 0, 10);
            return Quantidade;
        }
    }
}
