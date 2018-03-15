using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_Safra
    {
        CadastroTitulo = 1,
        PedidoBaixa = 2,
        ConcessaoAbatimento = 4,
        CancelamentoAbatimento = 5,
        AlteracaoVencimento = 6,
        ConcessaoDesconto = 7,
        AlteracaoSeuNumero = 8,
        Protestar = 9,
        NaoProtestar = 10,
        NaoCobrarJurosMora = 11,
        CobrarJurosMora = 16,
        AlteracaoOutrosDados = 31,
        Conceder_MultaVencimento = 901

        /*
        ProtestoFinsFamiliares = 3,
        CancelamentoDesconto = 8,
        SustarProtestoBaixarTitulo = 10,
        SustarProtestoManterCarteira = 11,
        AlteracaoJurosMora = 12,
        DispensarCobrancaJurosMora = 13,
        AlteracaoValorPercentualMulta = 14,
        DispensarCobrancaMulta = 15,
        AlteracaoValorDataDesconto = 16,
        NaoconcederDesconto = 17,
        AlteracaoValorAbatimento = 18,
        PrazoLimiteRecebimentoAlterar = 19,
        PrazoLimiteRecebimentoDispensar = 20,
        AlterarNumerotituloDadoBeneficiario = 21,
        AlterarNumeroControleParticipante = 22,
        AlterarDadosPagador = 23,
        AlterarDadosSacadorAvalista = 24,
        RecusaAlegacaoPagador = 30,        
        AlteracaoDadosRateioCredito = 33,
        PedidoCancelamentoDadosRateioCredito = 34,
        PedidoDesagendamentoDebitoAutomatico = 35,
        AlteracaoCarteira = 40,
        CancelarProtesto = 41,
        AlteracaoEspecieTitulo = 42,
        TransferenciaCarteiraModalidadeCobranca = 43,
        AlteracaoContratoCobranca = 44,
        NegativacaoSemProtesto = 45,
        SolicitacaoBaixaTituloNegativadoSemProtesto = 46,
        AlteracaoValorNominalTitulo = 47,
        AlteracaoValorMinimoPercentual = 48,
        AlteracaoValorMaximoPercentual = 49 */

    }

    #endregion

    public class Instrucao_Safra : AbstractInstrucao, IInstrucao
    {
        #region Construtores

        public Instrucao_Safra()
        {
            try
            {
                this.Banco = new Banco(422);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public Instrucao_Safra(int codigo)
        {
            this.carregar(codigo, 0);
        }

        public Instrucao_Safra(int codigo, int nrDias)
        {
            this.carregar(codigo, nrDias);
        }

        public Instrucao_Safra(int codigo, double valor)
        {
            this.carregar(codigo, valor);
        }

        public Instrucao_Safra(int codigo, double valor, EnumTipoValor tipoValor)
        {
            this.carregar(codigo, valor, tipoValor);
        }
        #endregion Construtores

        #region Metodos Privados

        private void carregar(int idInstrucao, double valor, EnumTipoValor tipoValor = EnumTipoValor.Percentual)
        {
            try
            {
                this.Banco = new Banco_Safra();
                this.Valida();

                switch ((EnumInstrucoes_Safra)idInstrucao)
                {
                    case EnumInstrucoes_Safra.CobrarJurosMora:
                        this.Codigo = (int)EnumInstrucoes_Safra.CobrarJurosMora;
                        this.Descricao = String.Format("  - APÓS VENCIMENTO COBRAR JUROS DE {0} {1} POR DIA DE ATRASO",
                            (tipoValor.Equals(EnumTipoValor.Reais) ? "R$ " : valor.ToString("F2")),
                            (tipoValor.Equals(EnumTipoValor.Percentual) ? "%" : valor.ToString("F2")));
                        break;
                    case EnumInstrucoes_Safra.Conceder_MultaVencimento:
                        this.Codigo = (int)EnumInstrucoes_Safra.Conceder_MultaVencimento;
                        this.Descricao = String.Format("  - APÓS VENCIMENTO COBRAR MULTA DE {0} {1}",
                            (tipoValor.Equals(EnumTipoValor.Reais) ? "R$ " : valor.ToString("F2")),
                            (tipoValor.Equals(EnumTipoValor.Percentual) ? "%" : valor.ToString("F2")));
                        break;
                    case EnumInstrucoes_Safra.ConcessaoDesconto:
                        this.Codigo = (int)EnumInstrucoes_Safra.ConcessaoDesconto;
                        this.Descricao = "  - CONCEDER DESCONTO DE R$ " + valor;
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = " (Selecione) ";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        private void carregar(int idInstrucao, int nrDias)
        {
            try
            {
                this.Banco = new Banco_Safra();
                this.Valida();

                switch ((EnumInstrucoes_Safra)idInstrucao)
                {
                    case EnumInstrucoes_Safra.CadastroTitulo:
                        this.Codigo = (int)EnumInstrucoes_Safra.CadastroTitulo;
                        this.Descricao = "";
                        break;
                    case EnumInstrucoes_Safra.PedidoBaixa:
                        this.Codigo = (int)EnumInstrucoes_Safra.PedidoBaixa;
                        this.Descricao = "";
                        break;
                    case EnumInstrucoes_Safra.ConcessaoAbatimento:
                        this.Codigo = (int)EnumInstrucoes_Safra.ConcessaoAbatimento;
                        this.Descricao = "";
                        break;
                    case EnumInstrucoes_Safra.CancelamentoAbatimento:
                        this.Codigo = (int)EnumInstrucoes_Safra.CancelamentoAbatimento;
                        this.Descricao = "";
                        break;
                    case EnumInstrucoes_Safra.AlteracaoVencimento:
                        this.Codigo = (int)EnumInstrucoes_Safra.AlteracaoVencimento;
                        this.Descricao = "";
                        break;
                    case EnumInstrucoes_Safra.Protestar:
                        this.Codigo = (int)EnumInstrucoes_Safra.Protestar;
                        this.Descricao = "  - PROTESTAR APÓS " + nrDias + " DIAS ÚTEIS DO VENCIMENTO";
                        break;
                    case EnumInstrucoes_Safra.NaoProtestar:
                        this.Codigo = (int)EnumInstrucoes_Safra.NaoProtestar;
                        this.Descricao = "";
                        break;
                    case EnumInstrucoes_Safra.AlteracaoSeuNumero:
                        this.Codigo = (int)EnumInstrucoes_Safra.AlteracaoSeuNumero;
                        this.Descricao = "";
                        break;
                    case EnumInstrucoes_Safra.AlteracaoOutrosDados:
                        this.Codigo = (int)EnumInstrucoes_Safra.AlteracaoOutrosDados;
                        this.Descricao = "";
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = " (Selecione) ";
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
