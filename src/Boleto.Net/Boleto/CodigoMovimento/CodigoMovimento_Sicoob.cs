using System;
using BoletoNet.Enums;
using System.Collections.Generic;
using BoletoNet.Excecoes;

namespace BoletoNet
{
    public enum EnumCodigoMovimento_Sicoob
    {
        EntradaConfirmada = 02,                                                         //02 Entrada confirmada
        EntradaRejeitada = 03,                                                          //03 Entrada rejeitada
        TransferenciaCarteiraEntrada = 04,                                              //04 Transferencia de Carteira - Entrada
        TransferenciaCarteiraBaixa = 05,                                                //05 Transferencia de Carteira - Baixa
        LiquidacaoNormal = 06,                                                          //06 Liquidação normal
        ConfirmacaoDoRecebimentoDaInstrucaoDeDesconto = 07,                             //07 Confirmação do Recebimento da Instrução de Desconto
        ConfirmacaoDoRecebimentoDoCancelamentoDoDesconto = 08,                          //08 Confirmação do Recebimento do Cancelamento do Desconto
        Baixado = 09,                                                                   //09 Baixado
        BaixadoConformeInstrucoesDaCooperativaDeCredito = 10,                           //10 Baixado conforme instruções da cooperativa de crédito
        TituloEmSer = 11,                                                               //11 Títulso em Ser
        AbatimentoConcedido = 12,                                                       //12 Abatimento concedido
        AbatimentoCancelado = 13,                                                       //13 Abatimento cancelado
        VencimentoAlterado = 14,                                                        //14 Vencimento alterado
        LiquidacaoEmCartorio = 15,                                                      //15 Liquidação em cartório
        LiquidacaoAposBaixa = 17,                                                       //17 Liquidação após baixa
        ConfirmacaoDeRecebimentoDeInstrucaoDeProtesto = 19,                             //19 Confirmação de recebimento de instrução de protesto
        ConfirmacaoDeRecebimentoDeInstrucaoDeSustacaoDeProtesto = 20,                   //20 Confirmação de recebimento de instrução de sustação de protesto
        EntradaDeTituloEmCartorio = 23,                                                 //23 Entrada de título em cartório        
        EntradaRejeitadaPorCEPIrregular = 24,                                           //24 Entrada rejeitada por CEP irregular
        ProtestadoEBaixado = 25,                                                        //25 Protestado e Baixado (Baixa por Ter Sido Protestado)
        InstrucaoRejeitada = 26,                                                        //26 instrucao rejeitada
        BaixaRejeitada = 27,                                                            //27 Baixa rejeitada
        Tarifa = 28,                                                                    //28 Tarifa
        RejeicaoDoPagador = 29,                                                         //29 Rejeição do pagador
        AlteracaoRejeitada = 30,                                                        //30 Alteração rejeitada
        ConfirmacaoDePedidoDeAlteracaoDeOutrosDados = 33,                               //33 Confirmação de pedido de alteração de outros dados
        RetiradoDeCartorioEManutencaoEmCarteira = 34,                                   //34 Retirado de cartório e manutenção em carteira
        AceiteDoPagador = 35,                                                           //35 Aceite do pagador
        ConfirmacaoDeEnvioDeEmailSMS = 36,                                              //36 Confirmação de envio de e-mail/SMS
        EnvioDeEmailSMSRejeitado = 37,                                                  //37 Envio de e-mail/SMS rejeitado
        ConfirmacaoDeAlteracaoDoPrazoLimiteDeRecebimento = 38,                          //38 Confirmação de alteração do Prazo Limite de Recebimento
        ConfirmacaoDeDispensaDePrazoLimiteDeRecebimento = 39,                           //39 Confirmação de Dispensa de Prazo Limite de Recebimento
        ConfirmacaoDaAlteracaoDoNumeroDoTituloDadoPeloBeneficiario = 40,                //40 Confirmação da alteração do número do título dado pelo Beneficiário
        ConfirmacaoDaAlteracaoDoNumeroControleDoParticipante = 41,                      //41 Confirmação da alteração do número controle do Participante
        ConfirmacaoDaAlteracaoDosDadosDoPagador = 42,                                   //42 Confirmação da alteração dos dados do Pagador
        ConfirmacaoDaAlteracaoDosDadosDoPagadorAvalista = 43,                           //43 Confirmação da alteração dos dados do Sacador/Avalista
        TituloPagoComChequeDevolvido = 44,                                              //44 Título pago com cheque devolvido
        TituloPagoComChequeCompensado = 45,                                             //45 Título pago com cheque compensado
        InstrucaoParaCancelarProtestoConfirmada = 46,                                   //46 Instrução para cancelar protesto confirmada
        InstrucaoParaProtestoParaFinsFalimentaresConfirmada = 47,                       //47 Instrução para protesto para fins falimentares confirmada
        ConfirmacaoDeInstrucaoDeTransferenciaDeCarteira = 48,                           //48 Confirmação de instrução de transferência de carteira/modalidade de cobrança
        AlteracaoDeContratoDeCobranca = 49,                                             //49 Alteração de contrato de cobrança
        TituloPagoComChequePendenteDeLiquidacao = 50,                                   //50 Título pago com cheque pendente de liquidação
        TituloDDAReconhecidoPeloPagador = 51,                                           //51 Título DDA reconhecido pelo Pagador
        TituloDDANaoReconhecidoPeloPagador = 52,                                        //52 Título DDA não reconhecido pelo Pagador
        TituloDDARecusadoPelaCIP = 53,                                                  //53 Título DDA recusado pela CIP
        ConfirmacaoDaInstrucaoDeBaixaCancelamentoDeTituloNegativadoSemProtesto = 54,    //54 Confirmação da Instrução de Baixa de Título Negativado sem Protesto
        ConfirmacaoDePedidoDeDispensaDeMulta = 55,                                      //55 Confirmação de Pedido de Dispensa de Multa
        ConfirmacaoDoPedidoDeCobrancaDeMulta = 56,                                      //56 Confirmação do Pedido de Cobrança de Multa
        ConfirmacaoDoPedidoDeAlteracaoDeCobrancaDeJuros = 57,                           //57 Confirmação do Pedido de Alteração de Cobrança de Juros
        ConfirmacaoDoPedidoDeAlteracaoDoValorDataDeDesconto = 58,                       //58 Confirmação do Pedido de Alteração do Valor/Data de Desconto
        ConfirmacaoDoPedidoDeAlteracaoDoBeneficiarioDoTitulo = 59,                      //59 Confirmação do Pedido de Alteração do Beneficiário do Título
        ConfirmacaoDoPedidoDeDispensaDeJurosDeMora = 60,                                //60 Confirmação do Pedido de Dispensa de Juros de Mora
        ConfirmacaoDeInstrucaoDeNegativacao = 80,                                       //80 Confirmação de instrução de negativação
        ConfirmacaoDeDesistenciaDeProtesto = 85,                                        //85 Confirmação de desistência de protesto
        ConfirmacaoDeCancelamentoDoProtesto = 86                                        //86 Confirmação de cancelamento do Protesto
    }

    public class CodigoMovimento_Sicoob : AbstractCodigoMovimento, ICodigoMovimento
    {
        #region Construtores
        internal CodigoMovimento_Sicoob()
        {
        }

        public CodigoMovimento_Sicoob(int codigo)
        {
            try
            {
                carregar(codigo);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }
        #endregion

        private void carregar(int codigo)
        {
            try
            {
                this.Banco = new Banco_Sicoob();

                var movimento = (EnumCodigoMovimento_Sicoob)codigo;
                Codigo = codigo;
                Descricao = descricoes[movimento];
            }
            catch (Exception ex)
            {
                throw new BoletoNetException("Código de movimento é inválido", ex);
            }
        }

        public override TipoOcorrenciaRetorno ObterCorrespondenteFebraban()
        {
            return ObterCorrespondenteFebraban(correspondentesFebraban, (EnumCodigoMovimento_Sicoob)Codigo);
        }

        #region Dicionários
        private readonly Dictionary<EnumCodigoMovimento_Sicoob, TipoOcorrenciaRetorno> correspondentesFebraban = new Dictionary<EnumCodigoMovimento_Sicoob, TipoOcorrenciaRetorno>()
        {
            { EnumCodigoMovimento_Sicoob.EntradaConfirmada                                         , TipoOcorrenciaRetorno.EntradaConfirmada                                                    },
            { EnumCodigoMovimento_Sicoob.EntradaRejeitada                                          , TipoOcorrenciaRetorno.EntradaRejeitada                                                     },
            { EnumCodigoMovimento_Sicoob.TransferenciaCarteiraEntrada                              , TipoOcorrenciaRetorno.TransferenciaDeCarteiraEntrada                                       },
            { EnumCodigoMovimento_Sicoob.TransferenciaCarteiraBaixa                                , TipoOcorrenciaRetorno.TransferenciaDeCarteiraBaixa                                         },
            { EnumCodigoMovimento_Sicoob.LiquidacaoNormal                                          , TipoOcorrenciaRetorno.Liquidacao                                                           },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDoRecebimentoDaInstrucaoDeDesconto             , TipoOcorrenciaRetorno.ConfirmacaoDoRecebimentoDaInstrucaoDeDesconto                        },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDoRecebimentoDoCancelamentoDoDesconto          , TipoOcorrenciaRetorno.ConfirmacaoDoRecebimentoDoCancelamentoDoDesconto                     },
            { EnumCodigoMovimento_Sicoob.Baixado                                                   , TipoOcorrenciaRetorno.Baixa                                                                },
            { EnumCodigoMovimento_Sicoob.BaixadoConformeInstrucoesDaCooperativaDeCredito           , TipoOcorrenciaRetorno.Baixa                                                                },
            { EnumCodigoMovimento_Sicoob.TituloEmSer                                               , TipoOcorrenciaRetorno.TitulosEmCarteira                                                    },
            { EnumCodigoMovimento_Sicoob.AbatimentoConcedido                                       , TipoOcorrenciaRetorno.ConfirmacaoRecebimentoInstrucaoDeAbatimento                          },
            { EnumCodigoMovimento_Sicoob.AbatimentoCancelado                                       , TipoOcorrenciaRetorno.ConfirmacaoRecebimentoInstrucaoDeCancelamentoAbatimento              },
            { EnumCodigoMovimento_Sicoob.VencimentoAlterado                                        , TipoOcorrenciaRetorno.ConfirmacaoRecebimentoInstrucaoAlteracaoDeVencimento                 },
            { EnumCodigoMovimento_Sicoob.LiquidacaoEmCartorio                                      , TipoOcorrenciaRetorno.FrancoDePagamento                                                    },
            { EnumCodigoMovimento_Sicoob.LiquidacaoAposBaixa                                       , TipoOcorrenciaRetorno.LiquidacaoAposBaixaOuLiquidacaoTituloNaoRegistrado                   },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDeRecebimentoDeInstrucaoDeProtesto             , TipoOcorrenciaRetorno.ConfirmacaoRecebimentoInstrucaoDeProtesto                            },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDeRecebimentoDeInstrucaoDeSustacaoDeProtesto   , TipoOcorrenciaRetorno.ConfirmacaoRecebimentoInstrucaoDeSustacaoCancelamentoDeProtesto      },
            { EnumCodigoMovimento_Sicoob.EntradaDeTituloEmCartorio                                 , TipoOcorrenciaRetorno.RemessaACartorio                                                     },
            { EnumCodigoMovimento_Sicoob.EntradaRejeitadaPorCEPIrregular                           , TipoOcorrenciaRetorno.EntradaRejeitada                                                     },
            { EnumCodigoMovimento_Sicoob.ProtestadoEBaixado                                        , TipoOcorrenciaRetorno.ProtestadoEBaixado                                                   },
            { EnumCodigoMovimento_Sicoob.InstrucaoRejeitada                                        , TipoOcorrenciaRetorno.InstrucaoRejeitada                                                   },
            { EnumCodigoMovimento_Sicoob.BaixaRejeitada                                            , TipoOcorrenciaRetorno.AlteracaoDeDadosRejeitada                                            },
            { EnumCodigoMovimento_Sicoob.Tarifa                                                    , TipoOcorrenciaRetorno.DebitoDeTarifasCustas                                                },
            { EnumCodigoMovimento_Sicoob.RejeicaoDoPagador                                         , TipoOcorrenciaRetorno.OcorrenciasDoPagador                                                 },
            { EnumCodigoMovimento_Sicoob.AlteracaoRejeitada                                        , TipoOcorrenciaRetorno.AlteracaoDeDadosRejeitada                                            },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDePedidoDeAlteracaoDeOutrosDados               , TipoOcorrenciaRetorno.ConfirmacaoDoPedidoDeAlteracaoDeOutrosDados                          },
            { EnumCodigoMovimento_Sicoob.RetiradoDeCartorioEManutencaoEmCarteira                   , TipoOcorrenciaRetorno.RetiradaDeCartorioEManutencaoEmCarteira                              },
            { EnumCodigoMovimento_Sicoob.AceiteDoPagador                                           , TipoOcorrenciaRetorno.OcorrenciasDoPagador                                                 },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDeEnvioDeEmailSMS                              , TipoOcorrenciaRetorno.ConfirmacaoDeEnvioDeEmailSMS                                         },
            { EnumCodigoMovimento_Sicoob.EnvioDeEmailSMSRejeitado                                  , TipoOcorrenciaRetorno.EnvioDeEmailSMSRejeitado                                             },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDeAlteracaoDoPrazoLimiteDeRecebimento          , TipoOcorrenciaRetorno.ConfirmacaoDeAlteracaoDoPrazoLimiteDeRecebimento                     },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDeDispensaDePrazoLimiteDeRecebimento           , TipoOcorrenciaRetorno.ConfirmacaoDeDispensaDePrazoLimiteDeRecebimento                      },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDaAlteracaoDoNumeroDoTituloDadoPeloBeneficiario, TipoOcorrenciaRetorno.ConfirmacaoDaAlteracaoDonumeroDoTitulodadoPeloBeneficiário           },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDaAlteracaoDoNumeroControleDoParticipante      , TipoOcorrenciaRetorno.ConfirmacaoDaAlteracaoDonumeroControleDoParticipante                 },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDaAlteracaoDosDadosDoPagador                   , TipoOcorrenciaRetorno.ConfirmacaoDaAlteracaoDosdadosDoPagador                              },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDaAlteracaoDosDadosDoPagadorAvalista           , TipoOcorrenciaRetorno.ConfirmacaoDaAlteracaoDosdadosDoSacadorAvalista                      },
            { EnumCodigoMovimento_Sicoob.TituloPagoComChequeDevolvido                              , TipoOcorrenciaRetorno.TitulopagoComChequeDevolvido                                         },
            { EnumCodigoMovimento_Sicoob.TituloPagoComChequeCompensado                             , TipoOcorrenciaRetorno.TitulopagoComChequeCompensado                                        },
            { EnumCodigoMovimento_Sicoob.InstrucaoParaCancelarProtestoConfirmada                   , TipoOcorrenciaRetorno.InstrucaoParacancelarProtestoConfirmada                              },
            { EnumCodigoMovimento_Sicoob.InstrucaoParaProtestoParaFinsFalimentaresConfirmada       , TipoOcorrenciaRetorno.InstrucaoParaprotestoParafinsFalimentaresConfirmada                  },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDeInstrucaoDeTransferenciaDeCarteira           , TipoOcorrenciaRetorno.ConfirmacaoDeinstrucaoDetransferenciaDecarteiraModalidadeDecobrança  },
            { EnumCodigoMovimento_Sicoob.AlteracaoDeContratoDeCobranca                             , TipoOcorrenciaRetorno.AlteracaoDecontratoDecobranca                                        },
            { EnumCodigoMovimento_Sicoob.TituloPagoComChequePendenteDeLiquidacao                   , TipoOcorrenciaRetorno.TitulopagoComchequePendenteDeliquidacao                              },
            { EnumCodigoMovimento_Sicoob.TituloDDAReconhecidoPeloPagador                           , TipoOcorrenciaRetorno.TituloDDAreconhecidoPeloPagador                                      },
            { EnumCodigoMovimento_Sicoob.TituloDDANaoReconhecidoPeloPagador                        , TipoOcorrenciaRetorno.TituloDDANaoReconhecidoPeloPagador                                   },
            { EnumCodigoMovimento_Sicoob.TituloDDARecusadoPelaCIP                                  , TipoOcorrenciaRetorno.TituloDDArecusadoPelaCIP                                             },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDaInstrucaoDeBaixaCancelamentoDeTituloNegativadoSemProtesto, TipoOcorrenciaRetorno.ConfirmacaoDaInstrucaoDeBaixaDeTituloNegativadoSemProtesto },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDePedidoDeDispensaDeMulta                      , TipoOcorrenciaRetorno.ConfirmacaoDePedidoDeDispensaDeMulta                                 },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDoPedidoDeCobrancaDeMulta                      , TipoOcorrenciaRetorno.ConfirmacaoDoPedidoDeCobrancaDeMulta                                 },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDoPedidoDeAlteracaoDeCobrancaDeJuros           , TipoOcorrenciaRetorno.ConfirmacaoDoPedidoDeAlteracaoDeCobrancaDeJuros                      },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDoPedidoDeAlteracaoDoValorDataDeDesconto       , TipoOcorrenciaRetorno.ConfirmacaoDoPedidoDeAlteracaoDoValorDataDeDesconto                  },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDoPedidoDeAlteracaoDoBeneficiarioDoTitulo      , TipoOcorrenciaRetorno.ConfirmacaoDoPedidoDeAlteracaoDoBeneficiarioDoTítulo                 },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDoPedidoDeDispensaDeJurosDeMora                , TipoOcorrenciaRetorno.ConfirmacaoDoPedidoDeDispensaDeJurosDeMora                           },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDeInstrucaoDeNegativacao                       , TipoOcorrenciaRetorno.ConfirmacaoDaInstrucaoDeBaixaDeTituloNegativadoSemProtesto           },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDeDesistenciaDeProtesto                        , TipoOcorrenciaRetorno.ConfirmacaoRecebimentoInstrucaoDeSustacaoCancelamentoDeProtesto      },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDeCancelamentoDoProtesto                       , TipoOcorrenciaRetorno.ConfirmacaoRecebimentoInstrucaoDeSustacaoCancelamentoDeProtesto      }
        };   

        private readonly Dictionary<EnumCodigoMovimento_Sicoob, string> descricoes = new Dictionary<EnumCodigoMovimento_Sicoob, string>()
        {
            { EnumCodigoMovimento_Sicoob.EntradaConfirmada                                         , "Entrada confirmada"                                                            },
            { EnumCodigoMovimento_Sicoob.EntradaRejeitada                                          , "Entrada rejeitada"                                                             },
            { EnumCodigoMovimento_Sicoob.TransferenciaCarteiraEntrada                              , "Transferencia de Carteira - Entrada"                                           },
            { EnumCodigoMovimento_Sicoob.TransferenciaCarteiraBaixa                                , "Transferencia de Carteira - Baixa"                                             },
            { EnumCodigoMovimento_Sicoob.LiquidacaoNormal                                          , "Liquidação normal"                                                             },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDoRecebimentoDaInstrucaoDeDesconto             , "Confirmação do Recebimento da Instrução de Desconto"                           },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDoRecebimentoDoCancelamentoDoDesconto          , "Confirmação do Recebimento do Cancelamento do Desconto"                        },
            { EnumCodigoMovimento_Sicoob.Baixado                                                   , "Baixa de Título"                                                               },
            { EnumCodigoMovimento_Sicoob.BaixadoConformeInstrucoesDaCooperativaDeCredito           , "Baixado conforme instruções da cooperativa de crédito"                         },
            { EnumCodigoMovimento_Sicoob.TituloEmSer                                               , "Título em Ser"                                                                 },
            { EnumCodigoMovimento_Sicoob.AbatimentoConcedido                                       , "Abatimento concedido"                                                          },
            { EnumCodigoMovimento_Sicoob.AbatimentoCancelado                                       , "Abatimento cancelado"                                                          },
            { EnumCodigoMovimento_Sicoob.VencimentoAlterado                                        , "Vencimento alterado"                                                           },
            { EnumCodigoMovimento_Sicoob.LiquidacaoEmCartorio                                      , "Liquidação em cartório"                                                        },
            { EnumCodigoMovimento_Sicoob.LiquidacaoAposBaixa                                       , "Liquidação após baixa"                                                         },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDeRecebimentoDeInstrucaoDeProtesto             , "Confirmação de recebimento de instrução de protesto"                           },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDeRecebimentoDeInstrucaoDeSustacaoDeProtesto   , "Confirmação de recebimento de instrução de sustação de protesto"               },
            { EnumCodigoMovimento_Sicoob.EntradaDeTituloEmCartorio                                 , "Entrada de título em cartório"                                                 },
            { EnumCodigoMovimento_Sicoob.EntradaRejeitadaPorCEPIrregular                           , "Entrada rejeitada por CEP irregular"                                           },
            { EnumCodigoMovimento_Sicoob.ProtestadoEBaixado                                        , "Protestado e Baixado (Baixa por Ter Sido Protestado)"                          },
            { EnumCodigoMovimento_Sicoob.InstrucaoRejeitada                                        , "Instrução rejeitada"                                                           },
            { EnumCodigoMovimento_Sicoob.BaixaRejeitada                                            , "Baixa rejeitada"                                                               },
            { EnumCodigoMovimento_Sicoob.Tarifa                                                    , "Tarifa"                                                                        },
            { EnumCodigoMovimento_Sicoob.RejeicaoDoPagador                                         , "Rejeição do pagador"                                                           },
            { EnumCodigoMovimento_Sicoob.AlteracaoRejeitada                                        , "Alteração rejeitada"                                                           },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDePedidoDeAlteracaoDeOutrosDados               , "Confirmação de pedido de alteração de outros dados"                            },
            { EnumCodigoMovimento_Sicoob.RetiradoDeCartorioEManutencaoEmCarteira                   , "Retirado de cartório e manutenção em carteira"                                 },
            { EnumCodigoMovimento_Sicoob.AceiteDoPagador                                           , "Aceite do pagador"                                                             },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDeEnvioDeEmailSMS                              , "Confirmação de envio de e-mail/SMS"                                            },
            { EnumCodigoMovimento_Sicoob.EnvioDeEmailSMSRejeitado                                  , "Envio de e-mail/SMS rejeitado"                                                 },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDeAlteracaoDoPrazoLimiteDeRecebimento          , "Confirmação de alteração do Prazo Limite de Recebimento"                       },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDeDispensaDePrazoLimiteDeRecebimento           , "Confirmação de Dispensa de Prazo Limite de Recebimento"                        },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDaAlteracaoDoNumeroDoTituloDadoPeloBeneficiario, "Confirmação da alteração do número do título dado pelo Beneficiário"           },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDaAlteracaoDoNumeroControleDoParticipante      , "Confirmação da alteração do número controle do Participante"                   },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDaAlteracaoDosDadosDoPagador                   , "Confirmação da alteração dos dados do Pagador"                                 },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDaAlteracaoDosDadosDoPagadorAvalista           , "Confirmação da alteração dos dados do Sacador/Avalista"                        },
            { EnumCodigoMovimento_Sicoob.TituloPagoComChequeDevolvido                              , "Título pago com cheque devolvido"                                              },
            { EnumCodigoMovimento_Sicoob.TituloPagoComChequeCompensado                             , "Título pago com cheque compensado"                                             },
            { EnumCodigoMovimento_Sicoob.InstrucaoParaCancelarProtestoConfirmada                   , "Instrução para cancelar protesto confirmada"                                   },
            { EnumCodigoMovimento_Sicoob.InstrucaoParaProtestoParaFinsFalimentaresConfirmada       , "Instrução para protesto para fins falimentares confirmada"                     },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDeInstrucaoDeTransferenciaDeCarteira           , "Confirmação de instrução de transferência de carteira/modalidade de cobrança"  },
            { EnumCodigoMovimento_Sicoob.AlteracaoDeContratoDeCobranca                             , "Alteração de contrato de cobrança"                                             },
            { EnumCodigoMovimento_Sicoob.TituloPagoComChequePendenteDeLiquidacao                   , "Título pago com cheque pendente de liquidação"                                 },
            { EnumCodigoMovimento_Sicoob.TituloDDAReconhecidoPeloPagador                           , "Título DDA reconhecido pelo Pagador"                                           },
            { EnumCodigoMovimento_Sicoob.TituloDDANaoReconhecidoPeloPagador                        , "Título DDA não reconhecido pelo Pagador"                                       },
            { EnumCodigoMovimento_Sicoob.TituloDDARecusadoPelaCIP                                  , "Título DDA recusado pela CIP"                                                  },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDaInstrucaoDeBaixaCancelamentoDeTituloNegativadoSemProtesto, "Confirmação da Instrução de Baixa de Título Negativado sem Protesto" },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDePedidoDeDispensaDeMulta                      , "Confirmação de Pedido de Dispensa de Multa"                                    },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDoPedidoDeCobrancaDeMulta                      , "Confirmação do Pedido de Cobrança de Multa"                                    },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDoPedidoDeAlteracaoDeCobrancaDeJuros           , "Confirmação do Pedido de Alteração de Cobrança de Juros"                       },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDoPedidoDeAlteracaoDoValorDataDeDesconto       , "Confirmação do Pedido de Alteração do Valor/Data de Desconto"                  },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDoPedidoDeAlteracaoDoBeneficiarioDoTitulo      , "Confirmação do Pedido de Alteração do Beneficiário do Título"                  },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDoPedidoDeDispensaDeJurosDeMora                , "Confirmação do Pedido de Dispensa de Juros de Mora"                            },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDeInstrucaoDeNegativacao                       , "Confirmação de instrução de negativação"                                       },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDeDesistenciaDeProtesto                        , "Confirmação de desistência de protesto"                                        },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDeCancelamentoDoProtesto                       , "Confirmação de cancelamento do Protesto"                                       }               
        };               
        #endregion
    }
}
