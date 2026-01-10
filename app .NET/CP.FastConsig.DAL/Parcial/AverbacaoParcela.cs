using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CP.FastConsig.DAL
{
    public partial class AverbacaoParcela
    {
        public int? UltimaSituacaoCorte
        {
            get
            {
                return null;
                //int dia = 10;
                //var consultadiafolha = new Repositorio<CorteHistorico>().Listar().Where(x => x.Competencia == this.Competencia).FirstOrDefault();
                //if (consultadiafolha != null)
                //    dia = Convert.ToInt32(consultadiafolha.DiaCorte);

                //int ano = Convert.ToInt32(this.Competencia.Substring(0, 4));
                //int mes = Convert.ToInt32(this.Competencia.Substring(5, 2));
                //DateTime datatram = new DateTime(ano, mes, dia, 23, 59, 59);
                
                //var tram = this.Averbacao.AverbacaoTramitacao.Where(x => x.CreatedOn <= datatram).OrderByDescending(x => x.IDAverbacaoTramitacao).FirstOrDefault();
                //if (tram != null) return tram.IDAverbacaoSituacao;
                //else return null;
            }
        }

        public string NomeSituacaoCorte
        {
            get
            {
                if (UltimaSituacaoCorte != null)
                    return new Repositorio<AverbacaoSituacao>().ObterPorId(this.UltimaSituacaoCorte.Value).Nome;
                else
                    return null;
            }
        }

        public string MesAno {
            get
            {
                return Funcoes.ConverteMesAno(this.Competencia);
            }
        }
    }
}
