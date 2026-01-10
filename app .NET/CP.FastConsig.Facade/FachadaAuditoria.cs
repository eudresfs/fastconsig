using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CP.FastConsig.BLL;
using CP.FastConsig.DAL;

namespace CP.FastConsig.Facade
{
    public static class FachadaAuditoria
    {
        public static IQueryable<Auditoria> localizaAuditoria(string busca)
        {
            return Geral.localizaAuditoria( busca );
        }
    }
}
