using System;
using BoletoNet.Enums;
using System.Collections.Generic;
using BoletoNet.Excecoes;

namespace BoletoNet
{
    public enum EnumCodigoMovimento_Sicredi
    {
        EntradaConfirmada = 02,                                       //02 Entrada confirmada
        EntradaRejeitada = 03,                                        //03 Entrada rejeitada
        LiquidacaoNormal = 06,                                        //06 Liquidação normal
        BaixadoAutomaticamenteViaArquivo = 09,                        //09 Baixado automaticamente via arquivo
        BaixadoConformeInstrucoesDaCooperativaDeCredito = 10,         //10 Baixado conforme instruções da cooperativa de crédito
        AbatimentoConcedido = 12,                                     //12 Abatimento concedido
        AbatimentoCancelado = 13,                                     //13 Abatimento cancelado
        VencimentoAlterado = 14,                                      //14 Vencimento alterado
        LiquidacaoEmCartorio = 15,                                    //15 Liquidação em cartório
        LiquidacaoAposBaixa = 17,                                     //17 Liquidação após baixa
        ConfirmacaoDeRecebimentoDeInstrucaoDeProtesto = 19,           //19 Confirmação de recebimento de instrução de protesto
        ConfirmacaoDeRecebimentoDeInstrucaoDeSustacaoDeProtesto = 20, //20 Confirmação de recebimento de instrução de sustação de protesto
        EntradaDeTituloEmCartorio = 23,                               //23 Entrada de título em cartório
        EntradaRejeitadaPorCEPIrregular = 24,                         //24 Entrada rejeitada por CEP irregular
        BaixaRejeitada = 27,                                          //27 Baixa rejeitada
        Tarifa = 28,                                                  //28 Tarifa
        RejeicaoDoPagador = 29,                                       //29 Rejeição do pagador
        AlteracaoRejeitada = 30,                                      //30 Alteração rejeitada
        InstrucaoRejeitada = 32,                                      //32 Instrução rejeitada
        ConfirmacaoDePedidoDeAlteracaoDeOutrosDados = 33,             //33 Confirmação de pedido de alteração de outros dados
        RetiradoDeCartorioEManutencaoEmCarteira = 34,                 //34 Retirado de cartório e manutenção em carteira
        AceiteDoPagador = 35                                          //35 Aceite do pagador
    }

    public class CodigoMovimento_Sicredi : AbstractCodigoMovimento, ICodigoMovimento
    {
        #region Construtores
        internal CodigoMovimento_Sicredi()
        {
        }

        public CodigoMovimento_Sicredi(int codigo)
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
                this.Banco = new Banco_Sicredi();

                var movimento = (EnumCodigoMovimento_Sicredi)codigo;
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
            return ObterCorrespondenteFebraban(correspondentesFebraban, (EnumCodigoMovimento_Sicredi)Codigo);
        }

        #region Dicionários
        private Dictionary<EnumCodigoMovimento_Sicredi, TipoOcorrenciaRetorno> correspondentesFebraban = new Dictionary<EnumCodigoMovimento_Sicredi, TipoOcorrenciaRetorno>()
        {
            { EnumCodigoMovimento_Sicredi.EntradaConfirmada                                      ,TipoOcorrenciaRetorno.EntradaConfirmada },
            { EnumCodigoMovimento_Sicredi.EntradaRejeitada                                       ,TipoOcorrenciaRetorno.EntradaRejeitada },
            { EnumCodigoMovimento_Sicredi.LiquidacaoNormal                                       ,TipoOcorrenciaRetorno.Liquidacao },
            { EnumCodigoMovimento_Sicredi.BaixadoAutomaticamenteViaArquivo                       ,TipoOcorrenciaRetorno.Baixa },
            { EnumCodigoMovimento_Sicredi.BaixadoConformeInstrucoesDaCooperativaDeCredito        ,TipoOcorrenciaRetorno.Baixa },
            { EnumCodigoMovimento_Sicredi.AbatimentoConcedido                                    ,TipoOcorrenciaRetorno.ConfirmacaoRecebimentoInstrucaoDeAbatimento },
            { EnumCodigoMovimento_Sicredi.AbatimentoCancelado                                    ,TipoOcorrenciaRetorno.ConfirmacaoRecebimentoInstrucaoDeCancelamentoAbatimento },
            { EnumCodigoMovimento_Sicredi.VencimentoAlterado                                     ,TipoOcorrenciaRetorno.ConfirmacaoRecebimentoInstrucaoAlteracaoDeVencimento },
            { EnumCodigoMovimento_Sicredi.LiquidacaoAposBaixa                                    ,TipoOcorrenciaRetorno.LiquidacaoAposBaixaOuLiquidacaoTituloNaoRegistrado },
            { EnumCodigoMovimento_Sicredi.ConfirmacaoDeRecebimentoDeInstrucaoDeProtesto          ,TipoOcorrenciaRetorno.ConfirmacaoRecebimentoInstrucaoDeProtesto },
            { EnumCodigoMovimento_Sicredi.ConfirmacaoDeRecebimentoDeInstrucaoDeSustacaoDeProtesto,TipoOcorrenciaRetorno.ConfirmacaoRecebimentoInstrucaoDeSustacaoCancelamentoDeProtesto },
            { EnumCodigoMovimento_Sicredi.EntradaDeTituloEmCartorio                              ,TipoOcorrenciaRetorno.RemessaACartorio },
            { EnumCodigoMovimento_Sicredi.Tarifa                                                 ,TipoOcorrenciaRetorno.DebitoDeTarifasCustas },
            { EnumCodigoMovimento_Sicredi.RejeicaoDoPagador                                      ,TipoOcorrenciaRetorno.OcorrenciasDoPagador },
            { EnumCodigoMovimento_Sicredi.AlteracaoRejeitada                                     ,TipoOcorrenciaRetorno.AlteracaoDeDadosRejeitada },
            { EnumCodigoMovimento_Sicredi.InstrucaoRejeitada                                     ,TipoOcorrenciaRetorno.InstrucaoRejeitada },
            { EnumCodigoMovimento_Sicredi.ConfirmacaoDePedidoDeAlteracaoDeOutrosDados            ,TipoOcorrenciaRetorno.ConfirmacaoDaAlteracaoDosDadosDoRateioDeCredito },
            { EnumCodigoMovimento_Sicredi.RetiradoDeCartorioEManutencaoEmCarteira                ,TipoOcorrenciaRetorno.ConfirmacaoDoCancelamentoDosDadosDoRateioDeCredito }
        };

        private Dictionary<EnumCodigoMovimento_Sicredi, string> descricoes = new Dictionary<EnumCodigoMovimento_Sicredi, string>()
        {
            { EnumCodigoMovimento_Sicredi.EntradaConfirmada                                       , "Entrada confirmada"                                             },
            { EnumCodigoMovimento_Sicredi.EntradaRejeitada                                        , "Entrada rejeitada"                                              },
            { EnumCodigoMovimento_Sicredi.LiquidacaoNormal                                        , "Liquidação normal"                                              },
            { EnumCodigoMovimento_Sicredi.BaixadoAutomaticamenteViaArquivo                        , "Baixado automaticamente via arquivo"                            },
            { EnumCodigoMovimento_Sicredi.BaixadoConformeInstrucoesDaCooperativaDeCredito         , "Baixado conforme instruções da cooperativa de crédito"          },
            { EnumCodigoMovimento_Sicredi.AbatimentoConcedido                                     , "Abatimento concedido"                                           },
            { EnumCodigoMovimento_Sicredi.AbatimentoCancelado                                     , "Abatimento cancelado"                                           },
            { EnumCodigoMovimento_Sicredi.VencimentoAlterado                                      , "Vencimento alterado"                                            },
            { EnumCodigoMovimento_Sicredi.LiquidacaoEmCartorio                                    , "Liquidação em cartório"                                         },
            { EnumCodigoMovimento_Sicredi.LiquidacaoAposBaixa                                     , "Liquidação após baixa"                                          },
            { EnumCodigoMovimento_Sicredi.ConfirmacaoDeRecebimentoDeInstrucaoDeProtesto           , "Confirmação de recebimento de instrução de protesto"            },
            { EnumCodigoMovimento_Sicredi.ConfirmacaoDeRecebimentoDeInstrucaoDeSustacaoDeProtesto , "Confirmação de recebimento de instrução de sustação de protesto"},
            { EnumCodigoMovimento_Sicredi.EntradaDeTituloEmCartorio                               , "Entrada de título em cartório"                                  },
            { EnumCodigoMovimento_Sicredi.EntradaRejeitadaPorCEPIrregular                         , "Entrada rejeitada por CEP irregular"                            },
            { EnumCodigoMovimento_Sicredi.BaixaRejeitada                                          , "Baixa rejeitada"                                                },
            { EnumCodigoMovimento_Sicredi.Tarifa                                                  , "Tarifa"                                                         },
            { EnumCodigoMovimento_Sicredi.RejeicaoDoPagador                                       , "Rejeição do pagador"                                            },
            { EnumCodigoMovimento_Sicredi.AlteracaoRejeitada                                      , "Alteração rejeitada"                                            },
            { EnumCodigoMovimento_Sicredi.InstrucaoRejeitada                                      , "Instrução rejeitada"                                            },
            { EnumCodigoMovimento_Sicredi.ConfirmacaoDePedidoDeAlteracaoDeOutrosDados             , "Confirmação de pedido de alteração de outros dados"             },
            { EnumCodigoMovimento_Sicredi.RetiradoDeCartorioEManutencaoEmCarteira                 , "Retirado de cartório e manutenção em carteira"                  },
            { EnumCodigoMovimento_Sicredi.AceiteDoPagador                                         , "Aceite do pagador" }
        }; 
        #endregion
    }
}
