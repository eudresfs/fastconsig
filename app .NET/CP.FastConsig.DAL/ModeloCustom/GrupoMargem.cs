using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CP.FastConsig.DAL
{
    public class GrupoMargem
    {
        public int IDProdutoGrupo { get; set; }
        public string Nome { get; set; }
        public decimal MargemFolha { get; set; }
        public decimal? MargemUtilizada { get; set; }
        public decimal? MargemDisponivel { get { return MargemFolha - MargemUtilizada.Value; } }
    }
}
