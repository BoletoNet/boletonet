using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoletoNet {

    public enum EnumInstrucoes_Cecred {
        /*
        * Código de Movimento Remessa 
        * 01 - Registro de títulos;  
        * 02 - Solicitação de baixa; 
        * 04 - Concessão de abatimento;  
        * 05 - Cancelamento de abatimento;  
        * 06 - Alteração de vencimento de título;  
        * 09 - Instruções para protestar (Nota 09);   
        * 10 - Instrução para sustar protesto;  
        * 12 - Alteração de nome e endereço do Pagador;  
        * 17 – Liquidação de título não registro ou pagamento em duplicidade; 
        * 31 - Conceder desconto; 
        * 32 - Não conceder desconto. 
        */
        CadastroDeTitulo = 1,
        PedidoBaixa = 2,
        ConcessaoAbatimento = 4,
        CancelamentoAbatimentoConcedido = 5,
        AlteracaoVencimento = 6,
        PedidoProtesto = 9,
        SustarProtestoBaixarTitulo = 10,
        AlteracaoNomeEnderecoPagador = 12,
        LiquidacaoDeTituloNaoRegristroOuPagamentoEmDuplicidade = 17,
        ConcederDesconto = 31,
        NaoConcederDesconto = 32,

        OutrasInstrucoes_ExibeMensagem_MoraDiaria = 900,
        OutrasInstrucoes_ExibeMensagem_MultaVencimento = 901


    }

    public class Instrucao_Cecred : AbstractInstrucao, IInstrucao {

        #region Construtores 
        public Instrucao_Cecred() {
            try {
                this.Banco = new Banco(85);
            } catch (Exception ex) {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public Instrucao_Cecred(int codigo) {
            this.carregar(codigo, 0);
        }

        public Instrucao_Cecred(int codigo, int nrDias) {
            this.carregar(codigo, nrDias);
        }
        public Instrucao_Cecred(int codigo, double valor) {
            this.carregar(codigo, valor);
        }

        public Instrucao_Cecred(int codigo, double valor, EnumTipoValor tipoValor) {
            this.carregar(codigo, valor, tipoValor);
        }
        #endregion

        #region Metodos Privados


        private void carregar(int idInstrucao, double valor, EnumTipoValor tipoValor = EnumTipoValor.Percentual) {
            try {
                this.Banco = new Banco(85);
                this.Valida();

                switch ((EnumInstrucoes_Cecred)idInstrucao) {
                    //case EnumInstrucoes_Cecred.OutrasInstrucoes_ExibeMensagem_MoraDiaria:
                    //    this.Codigo = (int)EnumInstrucoes_Cecred.AlteracaoOutrosDados;
                    //    this.Descricao = String.Format("  - APÓS VENCIMENTO COBRAR JUROS DE {0} {1} POR DIA DE ATRASO",
                    //        (tipoValor.Equals(EnumTipoValor.Reais) ? "R$ " : valor.ToString("F2")),
                    //        (tipoValor.Equals(EnumTipoValor.Percentual) ? "%" : valor.ToString("F2")));
                    //    break;
                    //case EnumInstrucoes_Cecred.OutrasInstrucoes_ExibeMensagem_MultaVencimento:
                    //    this.Codigo = (int)EnumInstrucoes_Cecred.AlteracaoOutrosDados;
                    //    this.Descricao = String.Format("  - APÓS VENCIMENTO COBRAR MULTA DE {0} {1}",
                    //        (tipoValor.Equals(EnumTipoValor.Reais) ? "R$ " : valor.ToString("F2")),
                    //        (tipoValor.Equals(EnumTipoValor.Percentual) ? "%" : valor.ToString("F2")));
                    //    break;
                    //case EnumInstrucoes_Cecred.AlteracaoOutrosDados_Desconto:
                    //    this.Codigo = (int)EnumInstrucoes_Cecred.AlteracaoOutrosDados;
                    //    this.Descricao = "  - CONCEDER DESCONTO DE R$ " + valor;
                    //    break;
                    //case EnumInstrucoes_Cecred.AlteracaoOutrosDados_DescontoAntecipacao:
                    //    this.Codigo = (int)EnumInstrucoes_Cecred.AlteracaoOutrosDados;
                    //    this.Descricao = "  - CONCEDER DESCONTO DE R$ " + valor + "POR DIA DE ANTECIPAÇÃO";
                    //    break;
                    //case EnumInstrucoes_Cecred.AlteracaoOutrosDados_JuroDia:
                    //    this.Codigo = (int)EnumInstrucoes_Cecred.AlteracaoOutrosDados;
                    //    this.Descricao = "  - APÓS VENCIMENTO COBRAR JURO DE " + valor + "% POR DIA DE ATRASO";
                    //    break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = " (Selecione) ";
                        break;
                }
            } catch (Exception ex) {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        private void carregar(int idInstrucao, int nrDias) {
            try {
                this.Banco = new Banco_Cecred();
                this.Valida();

                switch ((EnumInstrucoes_Cecred)idInstrucao) {
                    case EnumInstrucoes_Cecred.CadastroDeTitulo:
                        this.Codigo = (int)EnumInstrucoes_Cecred.CadastroDeTitulo;
                        this.Descricao = "";
                        break;
                    case EnumInstrucoes_Cecred.PedidoBaixa:
                        this.Codigo = (int)EnumInstrucoes_Cecred.PedidoBaixa;
                        this.Descricao = "";
                        break;
                    case EnumInstrucoes_Cecred.ConcessaoAbatimento:
                        this.Codigo = (int)EnumInstrucoes_Cecred.ConcessaoAbatimento;
                        this.Descricao = "";
                        break;
                    case EnumInstrucoes_Cecred.CancelamentoAbatimentoConcedido:
                        this.Codigo = (int)EnumInstrucoes_Cecred.CancelamentoAbatimentoConcedido;
                        this.Descricao = "";
                        break;
                    case EnumInstrucoes_Cecred.AlteracaoVencimento:
                        this.Codigo = (int)EnumInstrucoes_Cecred.AlteracaoVencimento;
                        this.Descricao = "";
                        break;
                    case EnumInstrucoes_Cecred.PedidoProtesto:
                        this.Codigo = (int)EnumInstrucoes_Cecred.PedidoProtesto;
                        this.Descricao = "  - PROTESTAR APÓS " + nrDias + " DIAS CORRIDOS DO VENCIMENTO";
                        break;
                    case EnumInstrucoes_Cecred.SustarProtestoBaixarTitulo:
                        this.Codigo = (int)EnumInstrucoes_Cecred.SustarProtestoBaixarTitulo;
                        this.Descricao = "";
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = " (Selecione) ";
                        break;
                }

                this.QuantidadeDias = nrDias;
            } catch (Exception ex) {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }


        public override void Valida() {
            //base.Valida();
        }

        #endregion
    }
}
