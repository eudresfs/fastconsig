using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CP.FastConsig.BLL;
using CP.FastConsig.DAL;

namespace CP.FastConsig.Facade
{

    public static class FachadaDashBoard
    {

        public static IEnumerable<GrupoMargem> ListarMargens(int idFuncionario)
        {
            return Funcionarios.ListarMargens(idFuncionario);
        }

    }

}