using System;
using System.Collections.Generic;
using BoletoNet.Enums;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumCodigoMovimento_Cecred
    {
        EntradaConfirmada = 2,
        EntradaRejeitada = 3,
        TransferenciaCarteiraEntrada = 4,
        TransferenciaCarteiraBaixa = 5,
        Liquidacao = 6,
        ConfirmacaoRecebimentoDesconto = 7,
        ConfirmacaoRecebimentoCancelamentoDesconto = 8,
        Baixa = 9,
        TitulosCarteira = 11,
        ConfirmacaoRecebimentoInstrucaoAbatimento = 12,
        ConfirmacaoRecebimentoInstrucaoCancelamentoAbatimento = 13,
        ConfirmacaoRecebimentoInstrucaoAlteracaoVencimento = 14,
        LiquidacaoAposBaixa = 17,
        ConfirmacaoRecebimentoInstrucaoProtesto = 19,
        ConfirmacaoRecebimentoInstrucaoSustacaoProtesto = 20,
        RemessaCartorio = 23,
        RetiradaCartorioManutencaoCarteira = 24,
        ProtestadoBaixado = 25,
        InstrucaoRejeitada = 26,
        ConfirmaçãoPedidoAlteracaoOutrosDados = 27,
        DebitoTarifas = 28,
        AlteracaoDadosRejeitada = 30,
        AlteracaoDadosSacado = 42,
        InstrucaoCancelarProtesto = 46
    }

    #endregion 

    public class CodigoMovimento_Cecred : AbstractCodigoMovimento, ICodigoMovimento
    {
        #region Construtores 

        public CodigoMovimento_Cecred()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public CodigoMovimento_Cecred(int codigo)
        {
            try
            {
                this.carregar(codigo);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }
        #endregion

        #region Metodos Privados

        private void carregar(int codigo)
        {
            try
            {
                this.Banco = new Banco_Brasil();

                switch ((EnumCodigoMovimento_Cecred)codigo)
                {
                    case EnumCodigoMovimento_Cecred.EntradaConfirmada:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.EntradaConfirmada;
                        this.Descricao = "Entrada confirmada";
                        break;
                    case EnumCodigoMovimento_Cecred.EntradaRejeitada:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.EntradaRejeitada;
                        this.Descricao = "Entrada rejeitada";
                        break;
                    case EnumCodigoMovimento_Cecred.TransferenciaCarteiraEntrada:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.TransferenciaCarteiraEntrada;
                        this.Descricao = "Transferência de carteira/entrada";
                        break;
                    case EnumCodigoMovimento_Cecred.TransferenciaCarteiraBaixa:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.TransferenciaCarteiraBaixa;
                        this.Descricao = "Transferência de carteira/baixa";
                        break;
                    case EnumCodigoMovimento_Cecred.Liquidacao:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.Liquidacao;
                        this.Descricao = "Liquidação";
                        break;
                    case EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoDesconto:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoDesconto;
                        this.Descricao = "Confirmação do Recebimento da Instrução de Desconto";
                        break;
                    case EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoCancelamentoDesconto:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoDesconto;
                        this.Descricao = "Confirmação do Recebimento do Cancelamento do Desconto";
                        break;
                    case EnumCodigoMovimento_Cecred.Baixa:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.Baixa;
                        this.Descricao = "Baixa";
                        break;
                    case EnumCodigoMovimento_Cecred.TitulosCarteira:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.TitulosCarteira;
                        this.Descricao = "Títulos em carteira";
                        break;
                    case EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoAbatimento:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoAbatimento;
                        this.Descricao = "Confirmação recebimento instrução de abatimento";
                        break;
                    case EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoCancelamentoAbatimento:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoCancelamentoAbatimento;
                        this.Descricao = "Confirmação recebimento instrução de cancelamento de abatimento";
                        break;
                    case EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoAlteracaoVencimento:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoAlteracaoVencimento;
                        this.Descricao = "Confirmação recebimento instrução alteração de vencimento";
                        break;
                    case EnumCodigoMovimento_Cecred.LiquidacaoAposBaixa:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.LiquidacaoAposBaixa;
                        this.Descricao = "Liquidação Após Baixa ou Liquidação Título Não Registrado";
                        break;
                    case EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoProtesto:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoProtesto;
                        this.Descricao = "Confirmação de recebimento de instrução de protesto";
                        break;
                    case EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoSustacaoProtesto:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoSustacaoProtesto;
                        this.Descricao = "Confirmação de recebimento de instrução de sustação de protesto";
                        break;
                    case EnumCodigoMovimento_Cecred.RemessaCartorio:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.RemessaCartorio;
                        this.Descricao = "Remessa a cartório/aponte em cartório";
                        break;
                    case EnumCodigoMovimento_Cecred.RetiradaCartorioManutencaoCarteira:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.RetiradaCartorioManutencaoCarteira;
                        this.Descricao = "Retirada de cartório e manutenção em carteira";
                        break;
                    case EnumCodigoMovimento_Cecred.ProtestadoBaixado:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ProtestadoBaixado;
                        this.Descricao = "Protestado e baixado/baixa por ter sido protestado";
                        break;
                    case EnumCodigoMovimento_Cecred.InstrucaoRejeitada:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.InstrucaoRejeitada;
                        this.Descricao = "Instrução rejeitada";
                        break;
                    case EnumCodigoMovimento_Cecred.ConfirmaçãoPedidoAlteracaoOutrosDados:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmaçãoPedidoAlteracaoOutrosDados;
                        this.Descricao = "Confirmação do pedido de alteração de outros dados";
                        break;
                    case EnumCodigoMovimento_Cecred.DebitoTarifas:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.DebitoTarifas;
                        this.Descricao = "Debito de tarifas/custas";
                        break;
                    case EnumCodigoMovimento_Cecred.AlteracaoDadosRejeitada:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.AlteracaoDadosRejeitada;
                        this.Descricao = "Alteração de dados rejeitada";
                        break;
                    case EnumCodigoMovimento_Cecred.AlteracaoDadosSacado:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.AlteracaoDadosSacado;
                        this.Descricao = "Confirmação da alteração dos dados do Sacado";
                        break;
                    case EnumCodigoMovimento_Cecred.InstrucaoCancelarProtesto:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.InstrucaoCancelarProtesto;
                        this.Descricao = "Instrução para cancelar protesto confirmada";
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = "( Selecione )";
                        break;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        private void Ler(int codigo)
        {
            try
            {
                switch (codigo)
                {
                    case 2:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.EntradaConfirmada;
                        this.Descricao = "Entrada confirmada";
                        break;
                    case 3:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.EntradaRejeitada;
                        this.Descricao = "Entrada rejeitada";
                        break;
                    case 4:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.TransferenciaCarteiraEntrada;
                        this.Descricao = "Transferência de carteira/entrada";
                        break;
                    case 5:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.TransferenciaCarteiraBaixa;
                        this.Descricao = "Transferência de carteira/baixa";
                        break;
                    case 6:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.Liquidacao;
                        this.Descricao = "Liquidação";
                        break;
                    case 7:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoDesconto;
                        this.Descricao = "Confirmação do Recebimento da Instrução de Desconto";
                        break;
                    case 8:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoCancelamentoDesconto;
                        this.Descricao = "Confirmação do Recebimento do Cancelamento do Desconto";
                        break;
                    case 9:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.Baixa;
                        this.Descricao = "Baixa";
                        break;
                    case 11:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.TitulosCarteira;
                        this.Descricao = "Títulos em carteira em ser";
                        break;
                    case 12:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoAbatimento;
                        this.Descricao = "Confirmação recebimento instrução de abatimento";
                        break;
                    case 13:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoCancelamentoAbatimento;
                        this.Descricao = "Confirmação recebimento instrução de cancelamento de abatimento";
                        break;
                    case 14:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoAlteracaoVencimento;
                        this.Descricao = "Confirmação recebimento instrução alteração de vencimento";
                        break;
                    case 17:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.LiquidacaoAposBaixa;
                        this.Descricao = "Liquidação após baixa";
                        break;
                    case 19:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoProtesto;
                        this.Descricao = "Confirmação de recebimento de instrução de protesto";
                        break;
                    case 20:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoSustacaoProtesto;
                        this.Descricao = "Confirmação de recebimento de instrução de sustação de protesto";
                        break;
                    case 23:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.RemessaCartorio;
                        this.Descricao = "Remessa a cartório/aponte em cartório";
                        break;
                    case 24:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.RetiradaCartorioManutencaoCarteira;
                        this.Descricao = "Retirada de cartório e manutenção em carteira";
                        break;
                    case 25:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ProtestadoBaixado;
                        this.Descricao = "Protestado e baixado/baixa por ter sido protestado";
                        break;
                    case 26:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.InstrucaoRejeitada;
                        this.Descricao = "Instrução rejeitada";
                        break;
                    case 27:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmaçãoPedidoAlteracaoOutrosDados;
                        this.Descricao = "Confirmação do pedido de alteração de outros dados";
                        break;
                    case 28:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.DebitoTarifas;
                        this.Descricao = "Debito de tarifas/custas";
                        break;
                    case 30:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.AlteracaoDadosRejeitada;
                        this.Descricao = "Alteração de dados rejeitada";
                        break;
                    case 42:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.AlteracaoDadosSacado;
                        this.Descricao = "Confirmação da alteração dos dados do Sacado";
                        break;
                    case 46:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.InstrucaoCancelarProtesto;
                        this.Descricao = "Instrução para cancelar protesto confirmada";
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = "( Selecione )";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }


        #endregion

        public override TipoOcorrenciaRetorno ObterCorrespondenteFebraban()
        {
            return ObterCorrespondenteFebraban(correspondentesFebraban, (EnumCodigoMovimento_Cecred)Codigo);
        }

        private Dictionary<EnumCodigoMovimento_Cecred, TipoOcorrenciaRetorno> correspondentesFebraban = new Dictionary<EnumCodigoMovimento_Cecred, TipoOcorrenciaRetorno>()
        {
            { EnumCodigoMovimento_Cecred.EntradaConfirmada                                     ,TipoOcorrenciaRetorno.EntradaConfirmada                                      },
            { EnumCodigoMovimento_Cecred.EntradaRejeitada                                      ,TipoOcorrenciaRetorno.EntradaRejeitada                                       },
            { EnumCodigoMovimento_Cecred.TransferenciaCarteiraEntrada                          ,TipoOcorrenciaRetorno.TransferenciaDeCarteiraEntrada                           },
            { EnumCodigoMovimento_Cecred.TransferenciaCarteiraBaixa                            ,TipoOcorrenciaRetorno.TransferenciaDeCarteiraBaixa                             },
            { EnumCodigoMovimento_Cecred.Liquidacao                                            ,TipoOcorrenciaRetorno.Liquidacao                                             },
            { EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoDesconto                        ,TipoOcorrenciaRetorno.ConfirmacaoDoRecebimentoDaInstrucaoDeDesconto },
            { EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoCancelamentoDesconto            ,TipoOcorrenciaRetorno.ConfirmacaoDoRecebimentoDoCancelamentoDoDesconto             },
            { EnumCodigoMovimento_Cecred.Baixa                                                 ,TipoOcorrenciaRetorno.Baixa                                                  },
            { EnumCodigoMovimento_Cecred.TitulosCarteira                                       ,TipoOcorrenciaRetorno.TitulosEmCarteira                                        },
            { EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoAbatimento             ,TipoOcorrenciaRetorno.ConfirmacaoRecebimentoInstrucaoDeAbatimento              },
            { EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoCancelamentoAbatimento ,TipoOcorrenciaRetorno.ConfirmacaoRecebimentoInstrucaoDeCancelamentoAbatimento  },
            { EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoAlteracaoVencimento    ,TipoOcorrenciaRetorno.ConfirmacaoRecebimentoInstrucaoAlteracaoDeVencimento     },
            { EnumCodigoMovimento_Cecred.LiquidacaoAposBaixa                                   ,TipoOcorrenciaRetorno.LiquidacaoAposBaixaOuLiquidacaoTituloNaoRegistrado },
            { EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoProtesto               ,TipoOcorrenciaRetorno.ConfirmacaoRecebimentoInstrucaoDeProtesto                },
            { EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoSustacaoProtesto       ,TipoOcorrenciaRetorno.ConfirmacaoRecebimentoInstrucaoDeSustacaoCancelamentoDeProtesto        },
            { EnumCodigoMovimento_Cecred.RemessaCartorio                                       ,TipoOcorrenciaRetorno.RemessaACartorio                                        },
            { EnumCodigoMovimento_Cecred.RetiradaCartorioManutencaoCarteira                    ,TipoOcorrenciaRetorno.RetiradaDeCartorioEManutencaoEmCarteira                     },
            { EnumCodigoMovimento_Cecred.ProtestadoBaixado                                     ,TipoOcorrenciaRetorno.ProtestadoEBaixado                                      },
            { EnumCodigoMovimento_Cecred.InstrucaoRejeitada                                    ,TipoOcorrenciaRetorno.InstrucaoRejeitada                                     },
            { EnumCodigoMovimento_Cecred.ConfirmaçãoPedidoAlteracaoOutrosDados                 ,TipoOcorrenciaRetorno.ConfirmacaoDoPedidoDeAlteracaoDeOutrosDados                  },
            { EnumCodigoMovimento_Cecred.DebitoTarifas                                         ,TipoOcorrenciaRetorno.DebitoDeTarifasCustas },
            { EnumCodigoMovimento_Cecred.AlteracaoDadosRejeitada                               ,TipoOcorrenciaRetorno.AlteracaoDeDadosRejeitada                                },
            { EnumCodigoMovimento_Cecred.AlteracaoDadosSacado                                  ,TipoOcorrenciaRetorno.ConfirmacaoDaAlteracaoDosdadosDoPagador },
            { EnumCodigoMovimento_Cecred.InstrucaoCancelarProtesto                             ,TipoOcorrenciaRetorno.InstrucaoParacancelarProtestoConfirmada }
        };
    }
}
