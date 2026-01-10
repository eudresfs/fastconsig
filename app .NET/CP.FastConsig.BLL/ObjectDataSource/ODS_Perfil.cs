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
    public class ODS_Perfil
    {

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IQueryable<Perfil> SelectGrid(int IdEmpresa, int IdModulo, string sortExpression, int startRowIndex, int maximumRows)
        {
            Repositorio<Perfil> serv = new Repositorio<Perfil>();

            if (String.IsNullOrWhiteSpace(sortExpression))
            {
                sortExpression = "";
            }

            var dados = serv.Listar();

            if (IdEmpresa == Convert.ToInt32(Geral.IdEmpresaConsignante()))
            {
                dados = dados.Where(x => x.IDEmpresa.Equals(null) && x.IDModulo == IdModulo);
            }
            else
            {
                dados = dados.Where(x => (x.IDEmpresa == IdEmpresa || x.IDEmpresa.Equals(null)) && x.IDModulo == IdModulo);
            }

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

        public int SelectGridCount(int IdEmpresa, int IdModulo)
        {
            return Quantidade;
        }
    }
}
