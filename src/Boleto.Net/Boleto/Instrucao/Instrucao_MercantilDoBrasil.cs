using System;
using System.Collections;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_MercantilDoBrasil
    {
        Protestar = 9,
        NaoProtestar = 10,
        ImportanciaporDiaDesconto = 30,
        ProtestoFinsFalimentares = 42,
        ProtestarAposNDiasCorridos = 81,
        ProtestarAposNDiasUteis = 82,
        NaoReceberAposNDias = 91,
        DevolverAposNDias = 92,
        JurosdeMora = 998,
        DescontoporDia = 999,
        Multa = 8
    }


    #endregion

    public class Instrucao_MercantilDoBrasil : AbstractInstrucao, IInstrucao
    {
        #region Construtores

        public Instrucao_MercantilDoBrasil()
        {
            try {
                this.Banco = new Banco(104);
            }
            catch (Exception ex) {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public Instrucao_MercantilDoBrasil(int codigo)
        {
            this.carregar(codigo, 0, 0);
        }

        public Instrucao_MercantilDoBrasil(int codigo, int nrDias)
        {
            this.carregar(codigo, nrDias, 0);
        }

        public Instrucao_MercantilDoBrasil(int codigo, decimal valor)
        {
            this.carregar(codigo, 0, valor);
        }

        public Instrucao_MercantilDoBrasil(int codigo, decimal valor, EnumTipoValor tipoValor)
        {
            this.carregar(codigo, 0, valor, tipoValor);
        }

        #endregion

        #region Metodos Privados

        private void carregar(int idInstrucao, int nrDias, decimal valor, EnumTipoValor tipoValor = EnumTipoValor.Percentual)
        {
            try
            {
                this.Banco = new Banco_MercantilDoBrasil();

                //  this.Valida();

                switch ((EnumInstrucoes_MercantilDoBrasil)idInstrucao)
                {
                    case EnumInstrucoes_MercantilDoBrasil.Protestar:
                        this.Codigo = (int)EnumInstrucoes_MercantilDoBrasil.Protestar;
                        this.Descricao = "Protestar após " + nrDias + " dias úteis.";
                        break;
                    case EnumInstrucoes_MercantilDoBrasil.NaoProtestar:
                        this.Codigo = (int)EnumInstrucoes_MercantilDoBrasil.NaoProtestar;
                        this.Descricao = "Não protestar";
                        break;
                    case EnumInstrucoes_MercantilDoBrasil.ImportanciaporDiaDesconto:
                        this.Codigo = (int)EnumInstrucoes_MercantilDoBrasil.ImportanciaporDiaDesconto;
                        this.Descricao = "Importância por dia de desconto.";
                        break;
                    case EnumInstrucoes_MercantilDoBrasil.ProtestoFinsFalimentares:
                        this.Codigo = (int)EnumInstrucoes_MercantilDoBrasil.ProtestoFinsFalimentares;
                        this.Descricao = "Protesto para fins falimentares";
                        break;
                    case EnumInstrucoes_MercantilDoBrasil.ProtestarAposNDiasCorridos:
                        this.Codigo = (int)EnumInstrucoes_MercantilDoBrasil.ProtestarAposNDiasCorridos;
                        this.Descricao = "Protestar após " + nrDias + " dias corridos do vencimento";
                        break;
                    case EnumInstrucoes_MercantilDoBrasil.ProtestarAposNDiasUteis:
                        this.Codigo = (int)EnumInstrucoes_MercantilDoBrasil.ProtestarAposNDiasUteis;
                        this.Descricao = "Protestar após " + nrDias + " dias úteis do vencimento";
                        break;
                    case EnumInstrucoes_MercantilDoBrasil.NaoReceberAposNDias:
                        this.Codigo = (int)EnumInstrucoes_MercantilDoBrasil.NaoReceberAposNDias;
                        this.Descricao = "Não receber após " + nrDias + " dias do vencimento";
                        break;
                    case EnumInstrucoes_MercantilDoBrasil.DevolverAposNDias:
                        this.Codigo = (int)EnumInstrucoes_MercantilDoBrasil.DevolverAposNDias;
                        this.Descricao = "Devolver após " + nrDias + " dias do vencimento";
                        break;
                    case EnumInstrucoes_MercantilDoBrasil.JurosdeMora:
                        this.Codigo = (int)EnumInstrucoes_MercantilDoBrasil.JurosdeMora;
                        this.Descricao = String.Format("Após vencimento cobrar juros de {0} {1} por dia de atraso",
                            (tipoValor.Equals(EnumTipoValor.Reais) ? "R$ " : valor.ToString("F2")),
                            (tipoValor.Equals(EnumTipoValor.Percentual) ? "%" : valor.ToString("F2")));
                        break;
                    case EnumInstrucoes_MercantilDoBrasil.Multa:
                        this.Codigo = (int)EnumInstrucoes_MercantilDoBrasil.Multa;
                        this.Descricao = String.Format("Após vencimento cobrar multa de {0} {1}",
                            (tipoValor.Equals(EnumTipoValor.Reais) ? "R$ " : valor.ToString("F2")),
                            (tipoValor.Equals(EnumTipoValor.Percentual) ? "%" : valor.ToString("F2")));
                        break;
                    case EnumInstrucoes_MercantilDoBrasil.DescontoporDia:
                        this.Codigo = (int)EnumInstrucoes_MercantilDoBrasil.DescontoporDia;
                        this.Descricao = "Conceder desconto de " + valor + "%" + " por dia de antecipação";
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = "( Selecione )";
                        break;
                }

                this.QuantidadeDias = nrDias;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public override void Valida()
        {
            //base.Valida();
        }

        #endregion
    }
}
