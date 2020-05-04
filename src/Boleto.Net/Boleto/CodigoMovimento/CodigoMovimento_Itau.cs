using BoletoNet.Excecoes;
using System;
using System.Collections.Generic;

namespace BoletoNet
{
    public enum EnumCodigoMovimento_Itau
    {
        EntradaConfirmadaComPossibilidadeDeMensagem = 02,                               //02 - ENTRADA CONFIRMADA COM POSSIBILIDADE DE MENSAGEM (NOTA 20 – TABELA 10)
        EntradaRejeitada = 03,                                                          //03 - ENTRADA REJEITADA (NOTA 20 - TABELA 1)
        AlteracaoDeDadosNovaEntradaOuAlteracaoExclusaoDeDadosAcatada = 04,              //04 - ALTERAÇÃO DE DADOS - NOVA ENTRADA OU ALTERAÇÃO/EXCLUSÃO DE DADOS ACATADA
        AlteracaoDeDadosBaixa = 05,                                                     //05 - ALTERAÇÃO DE DADOS – BAIXA
        LiquidacaoNormal = 06,                                                          //06 - LIQUIDAÇÃO NORMAL
        LiquidacaoParcialCobrancaInteligente = 07,                                      //07 - LIQUIDAÇÃO PARCIAL – COBRANÇA INTELIGENTE (B2B)
        LiquidacaoEmCartorio = 08,                                                      //08 - LIQUIDAÇÃO EM CARTÓRIO
        BaixaSimples = 09,                                                              //09 - BAIXA SIMPLES
        BaixaPorTerSidoLiquidado = 10,                                                  //10 - BAIXA POR TER SIDO LIQUIDADO
        EmSer = 11,                                                                     //11 - EM SER (SÓ NO RETORNO MENSAL)
        AbatimentoConcedido = 12,                                                       //12 - ABATIMENTO CONCEDIDO
        AbatimentoCancelado = 13,                                                       //13 - ABATIMENTO CANCELADO
        VencimentoAlterado = 14,                                                        //14 - VENCIMENTO ALTERADO
        BaixasRejeitadas = 15,                                                          //15 - BAIXAS REJEITADAS (NOTA 20 - TABELA 4)
        InstrucoesRejeitadas = 16,                                                      //16 - INSTRUÇÕES REJEITADAS (NOTA 20 - TABELA 3)
        AlteracaoExclusaoDeDadosRejeitados = 17,                                        //17 - ALTERAÇÃO/EXCLUSÃO DE DADOS REJEITADOS (NOTA 20 - TABELA 2)
        CobrancaContratualInstrucoesAlteracoesRejeitadasPendentes = 18,                 //18 - COBRANÇA CONTRATUAL - INSTRUÇÕES/ALTERAÇÕES REJEITADAS/PENDENTES (NOTA 20 - TABELA 5)
        ConfirmaRecebimentoDeInstrucaoDeProtesto = 19,                                  //19 - CONFIRMA RECEBIMENTO DE INSTRUÇÃO DE PROTESTO
        ConfirmaRecebimentoDeInstrucaoDeSustacaoDeProtestoTarifa = 20,                  //20 - CONFIRMA RECEBIMENTO DE INSTRUÇÃO DE SUSTAÇÃO DE PROTESTO /TARIFA
        ConfirmaRecebimentoDeInstrucaoDeNaoProtestar = 21,                              //21 - CONFIRMA RECEBIMENTO DE INSTRUÇÃO DE NÃO PROTESTAR
        TituloEnviadoACartorioTarifa = 23,                                              //23 - TÍTULO ENVIADO A CARTÓRIO/TARIFA
        InstrucaoDeProtestoRejeitadaSustadaPendente = 24,                               //24 - INSTRUÇÃO DE PROTESTO REJEITADA / SUSTADA / PENDENTE (NOTA 20 - TABELA 7)
        AlegacoesDoSacado = 25,                                                         //25 - ALEGAÇÕES DO SACADO (NOTA 20 - TABELA 6)
        TarifaDeAvisoDeCobranca = 26,                                                   //26 - TARIFA DE AVISO DE COBRANÇA
        TarifaDeExtratoPosicao = 27,                                                    //27 - TARIFA DE EXTRATO POSIÇÃO (B40X)
        TarifaDeRelacaoDasLiquidacoes = 28,                                             //28 - TARIFA DE RELAÇÃO DAS LIQUIDAÇÕES
        TarifaDeManutencaoDeTitulosVencidos = 29,                                       //29 - TARIFA DE MANUTENÇÃO DE TÍTULOS VENCIDOS
        DebitoMensalDeTarifas = 30,                                                     //30 - DÉBITO MENSAL DE TARIFAS (PARA ENTRADAS E BAIXAS)
        BaixaPorTerSidoProtestado = 32,                                                 //32 - BAIXA POR TER SIDO PROTESTADO
        CustasDeProtesto = 33,                                                          //33 - CUSTAS DE PROTESTO
        CustasDeSustacao = 34,                                                          //34 - CUSTAS DE SUSTAÇÃO
        CustasDeCartorioDistribuidor = 35,                                              //35 - CUSTAS DE CARTÓRIO DISTRIBUIDOR
        CustasDeEdital = 36,                                                            //36 - CUSTAS DE EDITAL
        TarifaDeEmissaoDeBoletoTarifaDeEnvioDeDuplicata = 37,                           //37 - TARIFA DE EMISSÃO DE BOLETO/TARIFA DE ENVIO DE DUPLICATA
        TarifaDeInstrucao = 38,                                                         //38 - TARIFA DE INSTRUÇÃO
        TarifaDeOcorrencias = 39,                                                       //39 - TARIFA DE OCORRÊNCIAS
        TarifaMensalDeEmissaoDeBoletoTarifaMensalDeEnvioDeDuplicata = 40,               //40 - TARIFA MENSAL DE EMISSÃO DE BOLETO/TARIFA MENSAL DE ENVIO DE DUPLICATA
        DebitoMensalDeTarifasExtratoDePosicao = 41,                                     //41 - DÉBITO MENSAL DE TARIFAS – EXTRATO DE POSIÇÃO (B4EP/B4OX)
        DebitoMensalDeTarifasOutrasInstrucoes = 42,                                     //42 - DÉBITO MENSAL DE TARIFAS – OUTRAS INSTRUÇÕES
        DebitoMensalDeTarifasManutencaoDeTitulosVencidos = 43,                          //43 - DÉBITO MENSAL DE TARIFAS – MANUTENÇÃO DE TÍTULOS VENCIDOS
        DebitoMensalDeTarifasOutrasOcorrencias = 44,                                    //44 - DÉBITO MENSAL DE TARIFAS – OUTRAS OCORRÊNCIAS
        DebitoMensalDeTarifasProtesto = 45,                                             //45 - DÉBITO MENSAL DE TARIFAS – PROTESTO
        DebitoMensalDeTarifasSustacaoDeProtesto = 46,                                   //46 - DÉBITO MENSAL DE TARIFAS – SUSTAÇÃO DE PROTESTO
        BaixaComTransferenciaParaDesconto = 47,                                         //47 - BAIXA COM TRANSFERÊNCIA PARA DESCONTO
        CustasDeSustacaoJudicial = 48,                                                  //48 - CUSTAS DE SUSTAÇÃO JUDICIAL
        TarifaMensalRefAEntradasBancosCorrespondentesNaCarteira = 51,                   //51 - TARIFA MENSAL REF A ENTRADAS BANCOS CORRESPONDENTES NA CARTEIRA
        TarifaMensalBaixasNaCarteira = 52,                                              //52 - TARIFA MENSAL BAIXAS NA CARTEIRA
        TarifaMensalBaixasEmBancosCorrespondentesNaCarteira = 53,                       //53 - TARIFA MENSAL BAIXAS EM BANCOS CORRESPONDENTES NA CARTEIRA
        TarifaMensalDeLiquidacoesNaCarteira = 54,                                       //54 - TARIFA MENSAL DE LIQUIDAÇÕES NA CARTEIRA
        TarifaMensalDeLiquidacoesEmBancosCorrespondentesNaCarteira = 55,                //55 - TARIFA MENSAL DE LIQUIDAÇÕES EM BANCOS CORRESPONDENTES NA CARTEIRA
        CustasDeIrregularidade = 56,                                                    //56 - CUSTAS DE IRREGULARIDADE
        InstrucaoCancelada = 57,                                                        //57 - INSTRUÇÃO CANCELADA (NOTA 20 – TABELA 8)
        BaixaPorCreditoEmCCAtravesDoSispag = 59,                                        //59 - BAIXA POR CRÉDITO EM C/C ATRAVÉS DO SISPAG
        EntradaRejeitadaCarne = 60,                                                     //60 - ENTRADA REJEITADA CARNÊ (NOTA 20 – TABELA 1)
        TarifaEmissaoAvisoDeMovimentacaoDeTitulos = 61,                                 //61 - TARIFA EMISSÃO AVISO DE MOVIMENTAÇÃO DE TÍTULOS (2154)
        DebitoMensalDeTarifaAvisoDeMovimentacaoDeTitulos = 62,                          //62 - DÉBITO MENSAL DE TARIFA - AVISO DE MOVIMENTAÇÃO DE TÍTULOS (2154)
        TituloSustadoJudicialmente = 63,                                                //63 - TÍTULO SUSTADO JUDICIALMENTE
        EntradaConfirmadaComRateioDeCredito = 64,                                       //64 - ENTRADA CONFIRMADA COM RATEIO DE CRÉDITO
        ChequeDevolvido = 69,                                                           //69 - CHEQUE DEVOLVIDO (NOTA 20 - TABELA 9)
        AguardandoAvaliacao = 71,                                                       //71 - ENTRADA REGISTRADA, AGUARDANDO AVALIAÇÃO
        BaixaPorCreditoEmCCAtravesDoSispagSemTituloCorrespondente = 72,                 //72 - BAIXA POR CRÉDITO EM C/C ATRAVÉS DO SISPAG SEM TÍTULO CORRESPONDENTE
        ConfirmacaoDeEntradaNaCobrancaSimplesEntradaNaoAceitaNaCobrancaContratual = 73, //73 - CONFIRMAÇÃO DE ENTRADA NA COBRANÇA SIMPLES – ENTRADA NÃO ACEITA NA COBRANÇA CONTRATUAL
        InstrucaoNegativacaoExpressaRejeitada = 74,                                     //74 - INSTRUÇÃO DE NEGATIVAÇÃO EXPRESSA REJEITADA (NOTA 25 – TABELA 3)
        RecebimentoInstrucaoEntradaNegativacaoExpressa = 75,                            //75 - CONFIRMA O RECEBIMENTO DE INSTRUÇÃO DE ENTRADA EM NEGATIVAÇÃO EXPRESSA
        ChequeCompensado = 76,                                                           //76 - CHEQUE COMPENSADO
        RecebimentoInstrucaoExclusaoNegativacaoExpressa = 77,                           //77 - CONFIRMA O RECEBIMENTO DE INSTRUÇÃO DE EXCLUSÃO DE ENTRADA EM NEGATIVAÇÃO EXPRESSA
        RecebimentoInstrucaoCancelamentoNegativacaoExpressa = 78,                       //78 - CONFIRMA O RECEBIMENTO DE INSTRUÇÃO DE CANCELAMENTO DA NEGATIVAÇÃO EXPRESSA
        NegativacaoExpressaInformal = 79,                                               //79 - NEGATIVAÇÃO EXPRESSA INFORMACIONAL (NOTA 25 – TABELA 12)
        EntradaNegativacaoExpressaTarifa = 80,                                          //80 - CONFIRMAÇÃO DE ENTRADA EM NEGATIVAÇÃO EXPRESSA – TARIFA
        AlteracaoBloqueadaNegativacaoExpresso = 81,                                     //81 - Alteração Bloqueada - TITULO COM NEGATIVAÇÃO EXPRESSA OU PROTESTO
        CancelamentoNegativacaoExpressaTarifa = 82,                                     //82 - CONFIRMAÇÃO DE CANCELAMENTO EM NEGATIVAÇÃO EXPRESSA – TARIFA
        ExclusaoNegativacaoExpressaTarifa = 83,                                         //83 - CONFIRMAÇÃO DA EXCLUSÃO/CANCELAMENTO DA NEGATIVAÇÃO EXPRESSA POR LIQUIDAÇÃO - TARIFA
        ConfirmaRecebimentoInstrucaoNaoNegativar = 94                                   //94 - CONFIRMA RECEBIMENTO DE INSTRUÇÃO DE NÃO NEGATIVAR
    }
    public class CodigoMovimento_Itau : AbstractCodigoMovimento, ICodigoMovimento
    {
        public CodigoMovimento_Itau(int codigo)
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
        

        private void carregar(int codigo)
        {
            try
            {
                this.Banco = new Banco_Itau();

                var movimento = (EnumCodigoMovimento_Itau)codigo;
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
            return ObterCorrespondenteFebraban(correspondentesFebraban, (EnumCodigoMovimento_Itau)Codigo);
        }

        private static Dictionary<EnumCodigoMovimento_Itau, string> descricoes = new Dictionary<EnumCodigoMovimento_Itau, string>()
        {
            { EnumCodigoMovimento_Itau.EntradaConfirmadaComPossibilidadeDeMensagem                               , "Entrada Confirmada Com Possibilidade De Mensagem" },
            { EnumCodigoMovimento_Itau.EntradaRejeitada                                                          , "Entrada Rejeitada" },
            { EnumCodigoMovimento_Itau.AlteracaoDeDadosNovaEntradaOuAlteracaoExclusaoDeDadosAcatada              , "Alteração De Dados - Nova Entrada Ou Alteração/Exclusão De Dados Acatada" },
            { EnumCodigoMovimento_Itau.AlteracaoDeDadosBaixa                                                     , "Alteração De Dados – Baixa" },
            { EnumCodigoMovimento_Itau.LiquidacaoNormal                                                          , "Liquidação Normal" },
            { EnumCodigoMovimento_Itau.LiquidacaoParcialCobrancaInteligente                                      , "Liquidação Parcial – Cobrança Inteligente(B2b)" },
            { EnumCodigoMovimento_Itau.LiquidacaoEmCartorio                                                      , "Liquidação Em Cartório" },
            { EnumCodigoMovimento_Itau.BaixaSimples                                                              , "Baixa Simples" },
            { EnumCodigoMovimento_Itau.BaixaPorTerSidoLiquidado                                                  , "Baixa Por Ter Sido Liquidado" },
            { EnumCodigoMovimento_Itau.EmSer                                                                     , "Em Ser" },
            { EnumCodigoMovimento_Itau.AbatimentoConcedido                                                       , "Abatimento Concedido" },
            { EnumCodigoMovimento_Itau.AbatimentoCancelado                                                       , "Abatimento Cancelado" },
            { EnumCodigoMovimento_Itau.VencimentoAlterado                                                        , "Vencimento Alterado" },
            { EnumCodigoMovimento_Itau.BaixasRejeitadas                                                          , "Baixas Rejeitadas" },
            { EnumCodigoMovimento_Itau.InstrucoesRejeitadas                                                      , "Instruções Rejeitadas" },
            { EnumCodigoMovimento_Itau.AlteracaoExclusaoDeDadosRejeitados                                        , "Alteração/Exclusão De Dados Rejeitados" },
            { EnumCodigoMovimento_Itau.CobrancaContratualInstrucoesAlteracoesRejeitadasPendentes                 , "Cobrança Contratual - Instruções/Alterações Rejeitadas/Pendentes" },
            { EnumCodigoMovimento_Itau.ConfirmaRecebimentoDeInstrucaoDeProtesto                                  , "Confirma Recebimento De Instrução De Protesto" },
            { EnumCodigoMovimento_Itau.ConfirmaRecebimentoDeInstrucaoDeSustacaoDeProtestoTarifa                  , "Confirma Recebimento De Instrução De Sustação De Protesto /Tarifa" },
            { EnumCodigoMovimento_Itau.ConfirmaRecebimentoDeInstrucaoDeNaoProtestar                              , "Confirma Recebimento De Instrução De Não Protestar" },
            { EnumCodigoMovimento_Itau.TituloEnviadoACartorioTarifa                                              , "Título Enviado a Cartório/Tarifa" },
            { EnumCodigoMovimento_Itau.InstrucaoDeProtestoRejeitadaSustadaPendente                               , "Instrução De Protesto Rejeitada / Sustada / Pendente" },
            { EnumCodigoMovimento_Itau.AlegacoesDoSacado                                                         , "Alegações Do Sacado" },
            { EnumCodigoMovimento_Itau.TarifaDeAvisoDeCobranca                                                   , "Tarifa De Aviso De Cobrança" },
            { EnumCodigoMovimento_Itau.TarifaDeExtratoPosicao                                                    , "Tarifa De Extrato Posição(B40x)" },
            { EnumCodigoMovimento_Itau.TarifaDeRelacaoDasLiquidacoes                                             , "Tarifa De Relação Das Liquidações" },
            { EnumCodigoMovimento_Itau.TarifaDeManutencaoDeTitulosVencidos                                       , "Tarifa De Manutenção De Títulos Vencidos" },
            { EnumCodigoMovimento_Itau.DebitoMensalDeTarifas                                                     , "Débito Mensal De Tarifas(Para Entradas E Baixas)" },
            { EnumCodigoMovimento_Itau.BaixaPorTerSidoProtestado                                                 , "Baixa Por Ter Sido Protestado" },
            { EnumCodigoMovimento_Itau.CustasDeProtesto                                                          , "Custas De Protesto" },
            { EnumCodigoMovimento_Itau.CustasDeSustacao                                                          , "Custas De Sustação" },
            { EnumCodigoMovimento_Itau.CustasDeCartorioDistribuidor                                              , "Custas De Cartório Distribuidor" },
            { EnumCodigoMovimento_Itau.CustasDeEdital                                                            , "Custas De Edital" },
            { EnumCodigoMovimento_Itau.TarifaDeEmissaoDeBoletoTarifaDeEnvioDeDuplicata                           , "Tarifa De Emissão De Boleto/Tarifa De Envio De Duplicata" },
            { EnumCodigoMovimento_Itau.TarifaDeInstrucao                                                         , "Tarifa De Instrução" },
            { EnumCodigoMovimento_Itau.TarifaDeOcorrencias                                                       , "Tarifa De Ocorrências" },
            { EnumCodigoMovimento_Itau.TarifaMensalDeEmissaoDeBoletoTarifaMensalDeEnvioDeDuplicata               , "Tarifa Mensal De Emissão De Boleto/Tarifa Mensal De Envio De Duplicata" },
            { EnumCodigoMovimento_Itau.DebitoMensalDeTarifasExtratoDePosicao                                     , "Débito Mensal De Tarifas – Extrato De Posição(B4ep/B4ox)" },
            { EnumCodigoMovimento_Itau.DebitoMensalDeTarifasOutrasInstrucoes                                     , "Débito Mensal De Tarifas – Outras Instruções" },
            { EnumCodigoMovimento_Itau.DebitoMensalDeTarifasManutencaoDeTitulosVencidos                          , "Débito Mensal De Tarifas – Manutenção De Títulos Vencidos" },
            { EnumCodigoMovimento_Itau.DebitoMensalDeTarifasOutrasOcorrencias                                    , "Débito Mensal De Tarifas – Outras Ocorrências" },
            { EnumCodigoMovimento_Itau.DebitoMensalDeTarifasProtesto                                             , "Débito Mensal De Tarifas – Protesto" },
            { EnumCodigoMovimento_Itau.DebitoMensalDeTarifasSustacaoDeProtesto                                   , "Débito Mensal De Tarifas – Sustação De Protesto" },
            { EnumCodigoMovimento_Itau.BaixaComTransferenciaParaDesconto                                         , "Baixa Com Transferência Para Desconto" },
            { EnumCodigoMovimento_Itau.CustasDeSustacaoJudicial                                                  , "Custas De Sustação Judicial" },
            { EnumCodigoMovimento_Itau.TarifaMensalRefAEntradasBancosCorrespondentesNaCarteira                   , "Tarifa Mensal Ref a Entradas Bancos Correspondentes Na Carteira" },
            { EnumCodigoMovimento_Itau.TarifaMensalBaixasNaCarteira                                              , "Tarifa Mensal Baixas Na Carteira" },
            { EnumCodigoMovimento_Itau.TarifaMensalBaixasEmBancosCorrespondentesNaCarteira                       , "Tarifa Mensal Baixas Em Bancos Correspondentes Na Carteira" },
            { EnumCodigoMovimento_Itau.TarifaMensalDeLiquidacoesNaCarteira                                       , "Tarifa Mensal De Liquidações Na Carteira" },
            { EnumCodigoMovimento_Itau.TarifaMensalDeLiquidacoesEmBancosCorrespondentesNaCarteira                , "Tarifa Mensal De Liquidações Em Bancos Correspondentes Na Carteira" },
            { EnumCodigoMovimento_Itau.CustasDeIrregularidade                                                    , "Custas De Irregularidade" },
            { EnumCodigoMovimento_Itau.InstrucaoCancelada                                                        , "Instrução Cancelada" },
            { EnumCodigoMovimento_Itau.BaixaPorCreditoEmCCAtravesDoSispag                                        , "Baixa Por Crédito Em C/C Através Do Sispag" },
            { EnumCodigoMovimento_Itau.EntradaRejeitadaCarne                                                     , "Entrada Rejeitada Carnê" },
            { EnumCodigoMovimento_Itau.TarifaEmissaoAvisoDeMovimentacaoDeTitulos                                 , "Tarifa Emissão Aviso De Movimentação De Títulos" },
            { EnumCodigoMovimento_Itau.DebitoMensalDeTarifaAvisoDeMovimentacaoDeTitulos                          , "Débito Mensal De Tarifa - Aviso De Movimentação De Títulos" },
            { EnumCodigoMovimento_Itau.TituloSustadoJudicialmente                                                , "Título Sustado Judicialmente" },
            { EnumCodigoMovimento_Itau.EntradaConfirmadaComRateioDeCredito                                       , "Entrada Confirmada Com Rateio De Crédito" },
            { EnumCodigoMovimento_Itau.ChequeDevolvido                                                           , "Cheque Devolvido" },
            { EnumCodigoMovimento_Itau.AguardandoAvaliacao                                                       , "Entrada Registrada, Aguardando Avaliação" },
            { EnumCodigoMovimento_Itau.BaixaPorCreditoEmCCAtravesDoSispagSemTituloCorrespondente                 , "Baixa Por Crédito Em C/C Através Do Sispag Sem Título Correspondente" },
            { EnumCodigoMovimento_Itau.ConfirmacaoDeEntradaNaCobrancaSimplesEntradaNaoAceitaNaCobrancaContratual , "Confirmação De Entrada Na Cobrança Simples – Entrada Não Aceita Na Cobrança Contratual" },
            { EnumCodigoMovimento_Itau.ChequeCompensado                                                          , "Cheque Compensado" },
            { EnumCodigoMovimento_Itau.InstrucaoNegativacaoExpressaRejeitada                                     , "Instrução de negativação expressa rejeitada" },
            { EnumCodigoMovimento_Itau.RecebimentoInstrucaoEntradaNegativacaoExpressa                            , "Recebimento de instrução de negativação expressa" },
            { EnumCodigoMovimento_Itau.RecebimentoInstrucaoExclusaoNegativacaoExpressa                           , "Recebimento de exclusão de negativação expressa" },
            { EnumCodigoMovimento_Itau.RecebimentoInstrucaoCancelamentoNegativacaoExpressa                       , "Recebimento de cancelamento de negativação expressa" },
            { EnumCodigoMovimento_Itau.NegativacaoExpressaInformal                                               , "Negativacao expressa informal" },
            { EnumCodigoMovimento_Itau.AlteracaoBloqueadaNegativacaoExpresso                                     , "Alteração Bloqueada - Título com negativação expressa ou protesto" },
            { EnumCodigoMovimento_Itau.EntradaNegativacaoExpressaTarifa                                          , "Recebimento de instrução de negativação expressa - tarifa" },
            { EnumCodigoMovimento_Itau.ExclusaoNegativacaoExpressaTarifa                                         , "Recebimento de exclusão de negativação expressa - tarifa" },
            { EnumCodigoMovimento_Itau.CancelamentoNegativacaoExpressaTarifa                                     , "Recebimento de cancelamento de negativação expressa - tarifa" },
            { EnumCodigoMovimento_Itau.ConfirmaRecebimentoInstrucaoNaoNegativar                                  , "Confirma Recebimento de instrução de não negativar" }
        };

        private static Dictionary<EnumCodigoMovimento_Itau, TipoOcorrenciaRetorno> correspondentesFebraban = new Dictionary<EnumCodigoMovimento_Itau, TipoOcorrenciaRetorno>()
        {
            { EnumCodigoMovimento_Itau.EntradaConfirmadaComPossibilidadeDeMensagem, TipoOcorrenciaRetorno.EntradaConfirmada },
            { EnumCodigoMovimento_Itau.EntradaRejeitada                           , TipoOcorrenciaRetorno.EntradaRejeitada },
            { EnumCodigoMovimento_Itau.LiquidacaoNormal                           , TipoOcorrenciaRetorno.Liquidacao },
            { EnumCodigoMovimento_Itau.BaixaSimples                               , TipoOcorrenciaRetorno.Baixa },
            { EnumCodigoMovimento_Itau.TarifaDeRelacaoDasLiquidacoes              , TipoOcorrenciaRetorno.DebitoDeTarifasCustas },
            { EnumCodigoMovimento_Itau.TituloSustadoJudicialmente                 , TipoOcorrenciaRetorno.TituloSustadoJudicialmente }
        };
    }
}
