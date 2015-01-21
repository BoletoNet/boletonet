using System;
using System.Collections;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_Caixa
    {
        Protestar = 9,                      // Emite aviso ao sacado após N dias do vencto, e envia ao cartório após 5 dias úteis
        NaoProtestar = 10,                  // Inibe protesto, quando houver instrução permanente na conta corrente
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

    public class Instrucao_Caixa : AbstractInstrucao, IInstrucao
    {

        #region Construtores

        public Instrucao_Caixa()
        {
            try
            {
                this.Banco = new Banco(104);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public Instrucao_Caixa(int codigo)
        {
            this.carregar(codigo, 0, 0);
        }

        public Instrucao_Caixa(int codigo, int nrDias)
        {
            this.carregar(codigo, nrDias, 0);
        }

        public Instrucao_Caixa(int codigo, decimal valor)
        {
            this.carregar(codigo, 0, valor);
        }

        #endregion

        #region Metodos Privados

        private void carregar(int idInstrucao, int nrDias, decimal valor)
        {
            try
            {
                this.Banco = new Banco_Caixa();

                //  this.Valida();

                switch ((EnumInstrucoes_Caixa)idInstrucao)
                {
                    case EnumInstrucoes_Caixa.Protestar:
                        this.Codigo = (int)EnumInstrucoes_Caixa.Protestar;
                        this.Descricao = "Protestar após " + nrDias + " dias úteis.";
                        break;
                    case EnumInstrucoes_Caixa.NaoProtestar:
                        this.Codigo = (int)EnumInstrucoes_Caixa.NaoProtestar;
                        this.Descricao = "Não protestar";
                        break;
                    case EnumInstrucoes_Caixa.ImportanciaporDiaDesconto:
                        this.Codigo = (int)EnumInstrucoes_Caixa.ImportanciaporDiaDesconto;
                        this.Descricao = "Importância por dia de desconto.";
                        break;
                    case EnumInstrucoes_Caixa.ProtestoFinsFalimentares:
                        this.Codigo = (int)EnumInstrucoes_Caixa.ProtestoFinsFalimentares;
                        this.Descricao = "Protesto para fins falimentares";
                        break;
                    case EnumInstrucoes_Caixa.ProtestarAposNDiasCorridos:
                        this.Codigo = (int)EnumInstrucoes_Caixa.ProtestarAposNDiasCorridos;
                        this.Descricao = "Protestar após " + nrDias + " dias corridos do vencimento";
                        break;
                    case EnumInstrucoes_Caixa.ProtestarAposNDiasUteis:
                        this.Codigo = (int)EnumInstrucoes_Caixa.ProtestarAposNDiasUteis;
                        this.Descricao = "Protestar após " + nrDias + " dias úteis do vencimento";
                        break;
                    case EnumInstrucoes_Caixa.NaoReceberAposNDias:
                        this.Codigo = (int)EnumInstrucoes_Caixa.NaoReceberAposNDias;
                        this.Descricao = "Não receber após " + nrDias + " dias do vencimento";
                        break;
                    case EnumInstrucoes_Caixa.DevolverAposNDias:
                        this.Codigo = (int)EnumInstrucoes_Caixa.DevolverAposNDias;
                        this.Descricao = "Devolver após " + nrDias + " dias do vencimento";
                        break;
                    case EnumInstrucoes_Caixa.JurosdeMora:
                        this.Codigo = (int)EnumInstrucoes_Caixa.JurosdeMora;
                        this.Descricao = "Após vencimento cobrar Juros de " + valor + "%";
                        break;
                    case EnumInstrucoes_Caixa.Multa:
                        this.Codigo = (int)EnumInstrucoes_Caixa.Multa;
                        this.Descricao = "Após vencimento cobrar Multa de " + valor + "%";
                        break;
                    case EnumInstrucoes_Caixa.DescontoporDia:
                        this.Codigo = (int)EnumInstrucoes_Caixa.DescontoporDia;
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
