using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CP.FastConsig.DAL
{
    public partial class Pessoa
    {
        public string CPFMascara { 
            get
            {
                if (this.CPF.Length == 11)
                    return string.Format("{0}.{1}.{2}-{3}", this.CPF.Substring(0, 3), this.CPF.Substring(3, 3), this.CPF.Substring(6, 3), this.CPF.Substring(9, 2));
                else
                    return this.CPF;
            }
        }
    }
}
