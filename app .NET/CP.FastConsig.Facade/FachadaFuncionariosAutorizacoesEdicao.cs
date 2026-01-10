using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CP.FastConsig.BLL;
using CP.FastConsig.DAL;

namespace CP.FastConsig.Facade
{

    public static class FachadaFuncionariosAutorizacoesEdicao
    {

        public static void SalvarAutorizacao(FuncionarioAutorizacao dado)
        {
            Funcionarios.SalvarAutorizacao(dado);
        }

        public static IQueryable<FuncionarioAutorizacaoTipo> ListaAutorizacoesTipo()
        {
            return Funcionarios.ListaAutorizacoesTipo();
        }

        public static FuncionarioAutorizacao ObtemAutorizacao(int IdAutorizacaoEdicao)
        {
            return Funcionarios.ObtemAutorizacao(IdAutorizacaoEdicao);
        }

    }

}