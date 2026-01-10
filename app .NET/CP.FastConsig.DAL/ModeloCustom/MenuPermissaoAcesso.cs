using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CP.FastConsig.DAL
{
    public class MenuPermissaoAcesso
    {           
        public int idopcao { get; set; }
        public int idpermissao { get; set; }     
        public int idpermissaorecurso { get; set; }
        public int IDRecurso { get; set; }
        public int? IDRecursoPai { get; set; }
        public string Nome { get; set; }
        public string Permissao { get; set; }             
        public int coluna_1 { get; set; }
        public int coluna_2 { get; set; }
        public int coluna_3 { get; set; }
        public int coluna_4 { get; set; }
        public int coluna_5 { get; set; }
        public int coluna_6 { get; set; }
        public int coluna_7 { get; set; }
        public int coluna_8 { get; set; }
        public int coluna_9 { get; set; }
        public int coluna_10 { get; set; }
    }
}
