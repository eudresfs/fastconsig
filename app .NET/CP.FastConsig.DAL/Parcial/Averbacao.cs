using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CP.FastConsig.Common;

namespace CP.FastConsig.DAL
{
    public partial class Averbacao
    {
        public DateTime? UltimaTramitacao
        {
            get
            {
                var tram = this.AverbacaoTramitacao.OrderByDescending(x => x.IDAverbacaoTramitacao).FirstOrDefault();
                if (tram != null) return tram.CreatedOn;
                else return null;
            }
        }

        public string UltimaSolicitacao
        {
            get
            {
                var solic = this.EmpresaSolicitacao.OrderByDescending(x => x.IDEmpresaSolicitacao).FirstOrDefault();
                if (solic != null)
                    return solic.EmpresaSolicitacaoTipo.Nome;
                else
                    return "";
            }
        }

        public decimal SaldoRestante
        {
            get
            {
                string anomes = Funcoes.ObtemAnoMesCorte(1, this.IDConsignataria);

                int prazorestante = 0;
                if (AverbacaoParcela.Count() > 0)
                    prazorestante = this.AverbacaoParcela.Count(x => x.Competencia.CompareTo(anomes) >= 0);
                else
                    prazorestante = this.Prazo.HasValue ? this.Prazo.Value : 0;

                return prazorestante * this.ValorParcela;
            }
        }

        public int PrazoRestante
        {
            get
            {
                string anomes = Funcoes.ObtemAnoMesCorte(1, this.IDConsignataria);

                int prazorestante = 0;
                if (this.AverbacaoParcela.Count() > 0)
                    prazorestante = this.AverbacaoParcela.Count(x => x.Competencia.CompareTo(anomes) >= 0);
                else
                    prazorestante = this.Prazo.HasValue ? this.Prazo.Value : 0;

                return prazorestante;
            }
        }

        public int ParcelaAtual
        {
            get
            {
                string anomes = Funcoes.ObtemAnoMesCorte(1, this.IDConsignataria);
                AverbacaoParcela ap = this.AverbacaoParcela.FirstOrDefault(x => x.Competencia == anomes);
                if (ap != null)
                    return ap.Numero;
                else
                    return 0;
            }
        }

        public decimal SaldoDevedorValor
        {
            get
            {
                var es_saldodevedor = this.EmpresaSolicitacao.Where(x => x.IDEmpresaSolicitacaoTipo == (int)Enums.SolicitacaoTipo.InformarSaldoDevedordeContratos).OrderByDescending(y => y.IDEmpresaSolicitacao).FirstOrDefault();
                if (es_saldodevedor != null)
                {
                    var saldodevedor = es_saldodevedor.EmpresaSolicitacaoSaldoDevedor.OrderByDescending(x => x.IDEmpresaSolicitacaoSaldoDevedor).FirstOrDefault();

                    if (saldodevedor != null && DateTime.Today.CompareTo(saldodevedor.Data.Value.AddDays(30)) <= 0)
                        return (saldodevedor.Valor.HasValue ? saldodevedor.Valor.Value : 0);
                    else
                        return SaldoRestante;
                }
                else
                    return SaldoRestante;
            }
        }

        public DateTime? SaldoDevedorData
        {
            get
            {
                var es_saldodevedor = this.EmpresaSolicitacao.Where(x => x.IDEmpresaSolicitacaoTipo == (int)Enums.SolicitacaoTipo.InformarSaldoDevedordeContratos).OrderByDescending(y => y.IDEmpresaSolicitacao).FirstOrDefault();
                if (es_saldodevedor != null)
                {
                    var saldodevedor = es_saldodevedor.EmpresaSolicitacaoSaldoDevedor.OrderByDescending(x => x.IDEmpresaSolicitacaoSaldoDevedor).FirstOrDefault();

                    if (saldodevedor != null && DateTime.Today.CompareTo(saldodevedor.Data.Value.AddDays(30)) <= 0)
                        return (saldodevedor.Data);
                    else
                        return null;
                }
                else
                    return null;
            }
        }
    }
}
