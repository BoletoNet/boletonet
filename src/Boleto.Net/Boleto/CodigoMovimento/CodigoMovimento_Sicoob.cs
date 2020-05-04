using System;
using BoletoNet.Enums;
using System.Collections.Generic;
using BoletoNet.Excecoes;

namespace BoletoNet
{
    public enum EnumCodigoMovimento_Sicoob
    {
        EntradaConfirmada = 02,                                       //02 Entrada confirmada
        EntradaRejeitada = 03,                                        //03 Entrada rejeitada
        LiquidacaoNormal = 06,                                        //06 Liquidação normal
        Baixado = 09,                                                 //09 Baixado
        BaixadoConformeInstrucoesDaCooperativaDeCredito = 10,         //10 Baixado conforme instruções da cooperativa de crédito
        TituloEmSer = 11,                                             //11 Títulso em Ser
        AbatimentoConcedido = 12,                                     //12 Abatimento concedido
        AbatimentoCancelado = 13,                                     //13 Abatimento cancelado
        VencimentoAlterado = 14,                                      //14 Vencimento alterado
        LiquidacaoEmCartorio = 15,                                    //15 Liquidação em cartório
        LiquidacaoAposBaixa = 17,                                     //17 Liquidação após baixa
        ConfirmacaoDeRecebimentoDeInstrucaoDeProtesto = 19,           //19 Confirmação de recebimento de instrução de protesto
        ConfirmacaoDeRecebimentoDeInstrucaoDeSustacaoDeProtesto = 20, //20 Confirmação de recebimento de instrução de sustação de protesto
        EntradaDeTituloEmCartorio = 23,                               //23 Entrada de título em cartório        
        EntradaRejeitadaPorCEPIrregular = 24,                         //24 Entrada rejeitada por CEP irregular
        ProtestadoEBaixado = 25,                                      //25 Protestado e Baixado (Baixa por Ter Sido Protestado)
        BaixaRejeitada = 27,                                          //27 Baixa rejeitada
        Tarifa = 28,                                                  //28 Tarifa
        RejeicaoDoPagador = 29,                                       //29 Rejeição do pagador
        AlteracaoRejeitada = 30,                                      //30 Alteração rejeitada
        InstrucaoRejeitada = 32,                                      //32 Instrução rejeitada
        ConfirmacaoDePedidoDeAlteracaoDeOutrosDados = 33,             //33 Confirmação de pedido de alteração de outros dados
        RetiradoDeCartorioEManutencaoEmCarteira = 34,                 //34 Retirado de cartório e manutenção em carteira
        AceiteDoPagador = 35                                          //35 Aceite do pagador
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
        private Dictionary<EnumCodigoMovimento_Sicoob, TipoOcorrenciaRetorno> correspondentesFebraban = new Dictionary<EnumCodigoMovimento_Sicoob, TipoOcorrenciaRetorno>()
        {
            { EnumCodigoMovimento_Sicoob.EntradaConfirmada                                      ,TipoOcorrenciaRetorno.EntradaConfirmada },
            { EnumCodigoMovimento_Sicoob.EntradaRejeitada                                       ,TipoOcorrenciaRetorno.EntradaRejeitada },
            { EnumCodigoMovimento_Sicoob.LiquidacaoNormal                                       ,TipoOcorrenciaRetorno.Liquidacao },
            { EnumCodigoMovimento_Sicoob.Baixado                                                ,TipoOcorrenciaRetorno.Baixa },
            { EnumCodigoMovimento_Sicoob.BaixadoConformeInstrucoesDaCooperativaDeCredito        ,TipoOcorrenciaRetorno.Baixa },
            { EnumCodigoMovimento_Sicoob.AbatimentoConcedido                                    ,TipoOcorrenciaRetorno.ConfirmacaoRecebimentoInstrucaoDeAbatimento },
            { EnumCodigoMovimento_Sicoob.AbatimentoCancelado                                    ,TipoOcorrenciaRetorno.ConfirmacaoRecebimentoInstrucaoDeCancelamentoAbatimento },
            { EnumCodigoMovimento_Sicoob.VencimentoAlterado                                     ,TipoOcorrenciaRetorno.ConfirmacaoRecebimentoInstrucaoAlteracaoDeVencimento },
            { EnumCodigoMovimento_Sicoob.LiquidacaoAposBaixa                                    ,TipoOcorrenciaRetorno.LiquidacaoAposBaixaOuLiquidacaoTituloNaoRegistrado },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDeRecebimentoDeInstrucaoDeProtesto          ,TipoOcorrenciaRetorno.ConfirmacaoRecebimentoInstrucaoDeProtesto },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDeRecebimentoDeInstrucaoDeSustacaoDeProtesto,TipoOcorrenciaRetorno.ConfirmacaoRecebimentoInstrucaoDeSustacaoCancelamentoDeProtesto },
            { EnumCodigoMovimento_Sicoob.EntradaDeTituloEmCartorio                              ,TipoOcorrenciaRetorno.RemessaACartorio },
            { EnumCodigoMovimento_Sicoob.Tarifa                                                 ,TipoOcorrenciaRetorno.DebitoDeTarifasCustas },
            { EnumCodigoMovimento_Sicoob.RejeicaoDoPagador                                      ,TipoOcorrenciaRetorno.OcorrenciasDoPagador },
            { EnumCodigoMovimento_Sicoob.AlteracaoRejeitada                                     ,TipoOcorrenciaRetorno.AlteracaoDeDadosRejeitada },
            { EnumCodigoMovimento_Sicoob.InstrucaoRejeitada                                     ,TipoOcorrenciaRetorno.InstrucaoRejeitada },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDePedidoDeAlteracaoDeOutrosDados            ,TipoOcorrenciaRetorno.ConfirmacaoDaAlteracaoDosDadosDoRateioDeCredito },
            { EnumCodigoMovimento_Sicoob.RetiradoDeCartorioEManutencaoEmCarteira                ,TipoOcorrenciaRetorno.ConfirmacaoDoCancelamentoDosDadosDoRateioDeCredito },
            { EnumCodigoMovimento_Sicoob.TituloEmSer                                            ,TipoOcorrenciaRetorno.TitulosEmCarteira },
            { EnumCodigoMovimento_Sicoob.ProtestadoEBaixado                                     ,TipoOcorrenciaRetorno.ProtestadoEBaixado},
        };

        private Dictionary<EnumCodigoMovimento_Sicoob, string> descricoes = new Dictionary<EnumCodigoMovimento_Sicoob, string>()
        {
            { EnumCodigoMovimento_Sicoob.EntradaConfirmada                                       , "Entrada confirmada"                                             },
            { EnumCodigoMovimento_Sicoob.EntradaRejeitada                                        , "Entrada rejeitada"                                              },
            { EnumCodigoMovimento_Sicoob.LiquidacaoNormal                                        , "Liquidação normal"                                              },
            { EnumCodigoMovimento_Sicoob.Baixado                                                 , "Baixa de Título"                                                },
            { EnumCodigoMovimento_Sicoob.BaixadoConformeInstrucoesDaCooperativaDeCredito         , "Baixado conforme instruções da cooperativa de crédito"          },
            { EnumCodigoMovimento_Sicoob.AbatimentoConcedido                                     , "Abatimento concedido"                                           },
            { EnumCodigoMovimento_Sicoob.AbatimentoCancelado                                     , "Abatimento cancelado"                                           },
            { EnumCodigoMovimento_Sicoob.VencimentoAlterado                                      , "Vencimento alterado"                                            },
            { EnumCodigoMovimento_Sicoob.LiquidacaoEmCartorio                                    , "Liquidação em cartório"                                         },
            { EnumCodigoMovimento_Sicoob.LiquidacaoAposBaixa                                     , "Liquidação após baixa"                                          },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDeRecebimentoDeInstrucaoDeProtesto           , "Confirmação de recebimento de instrução de protesto"            },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDeRecebimentoDeInstrucaoDeSustacaoDeProtesto , "Confirmação de recebimento de instrução de sustação de protesto"},
            { EnumCodigoMovimento_Sicoob.EntradaDeTituloEmCartorio                               , "Entrada de título em cartório"                                  },
            { EnumCodigoMovimento_Sicoob.EntradaRejeitadaPorCEPIrregular                         , "Entrada rejeitada por CEP irregular"                            },
            { EnumCodigoMovimento_Sicoob.ProtestadoEBaixado                                      , "Protestado e Baixado (Baixa por Ter Sido Protestado)"           },
            { EnumCodigoMovimento_Sicoob.BaixaRejeitada                                          , "Baixa rejeitada"                                                },
            { EnumCodigoMovimento_Sicoob.Tarifa                                                  , "Tarifa"                                                         },
            { EnumCodigoMovimento_Sicoob.RejeicaoDoPagador                                       , "Rejeição do pagador"                                            },
            { EnumCodigoMovimento_Sicoob.AlteracaoRejeitada                                      , "Alteração rejeitada"                                            },
            { EnumCodigoMovimento_Sicoob.InstrucaoRejeitada                                      , "Instrução rejeitada"                                            },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDePedidoDeAlteracaoDeOutrosDados             , "Confirmação de pedido de alteração de outros dados"             },
            { EnumCodigoMovimento_Sicoob.RetiradoDeCartorioEManutencaoEmCarteira                 , "Retirado de cartório e manutenção em carteira"                  },
            { EnumCodigoMovimento_Sicoob.AceiteDoPagador                                         , "Aceite do pagador" },
            { EnumCodigoMovimento_Sicoob.TituloEmSer                                             , "Título em Ser" }
        };
        #endregion
    }
}
