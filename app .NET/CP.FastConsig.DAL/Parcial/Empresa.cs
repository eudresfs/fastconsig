using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CP.FastConsig.DAL
{
    public partial class Empresa
    {
        public string Nome { 
            get
            {
                if (this.Fantasia != null)
                {
                    return this.Fantasia;
                }
                else
                {
                    return this.RazaoSocial;
                }
            }
        }
    }
}
