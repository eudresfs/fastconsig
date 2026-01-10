using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.ComponentModel;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;

namespace CP.FastConsig.BLL
{
    public class ODS_Usuario
    {

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IQueryable<Usuario> SelectGrid(int IdEmpresa, int idModulo, string sortExpression, string nameSearchString, int startRowIndex, int maximumRows)
        {

            if (nameSearchString != null && nameSearchString.Equals("Procurar por Nome, CPF, E-mail, Login, Perfil")) nameSearchString = string.Empty;

            Repositorio<Usuario> emp = new Repositorio<Usuario>();

            if (String.IsNullOrWhiteSpace(sortExpression))
            {
                sortExpression = "NomeCompleto";
            }
            if (String.IsNullOrWhiteSpace(nameSearchString))
            {
                nameSearchString = "";
            }

            var dados = emp.PesquisaTextual(nameSearchString,sortExpression);

            if (IdEmpresa > 0)
                dados = from d in dados
                        where d.UsuarioPerfil.Any(x => x.IDEmpresa == IdEmpresa)
                        select d;

            else if (idModulo > 0)
                dados = from d in dados
                        where d.UsuarioPerfil.Any(x => x.Perfil.IDModulo.Equals(idModulo))
                        select d;

            dados = dados.OrderBy(!string.IsNullOrEmpty(sortExpression) ? sortExpression : emp.ChavePrimaria());

            dados = dados.Where(x => !x.UsuarioPerfil.All(y => y.Perfil.IDModulo.Equals((int) Enums.Modulos.Funcionario)));

            List<Usuario> retorno = dados.ToList();

            if (!string.IsNullOrEmpty(nameSearchString)) retorno.AddRange(emp.Listar().Where(x => x.UsuarioPerfil.All(y => y.Perfil.IDModulo.Equals(idModulo)) && x.UsuarioPerfil.FirstOrDefault() != null && x.UsuarioPerfil.FirstOrDefault().Perfil.Nome.Contains(nameSearchString)));

            Quantidade = retorno.Count;

            return retorno.AsQueryable().ListarDaPagina(startRowIndex, maximumRows);

        }

        private int Quantidade;

        public int SelectGridCount(int IdEmpresa, int idModulo, string nameSearchString)
        {
            return Quantidade;
        }

    }

}