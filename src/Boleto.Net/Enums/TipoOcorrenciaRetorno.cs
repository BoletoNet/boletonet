namespace BoletoNet
{
    public enum TipoOcorrenciaRetorno
    {
        EntradaConfirmada = 02,                                                   //'02' = Entrada Confirmada
        EntradaRejeitada = 03,                                                    //'03' = Entrada Rejeitada
        TransferenciaDeCarteiraEntrada = 04,                                      //'04' = Transferência de Carteira/Entrada
        TransferenciaDeCarteiraBaixa = 05,                                        //'05' = Transferência de Carteira/Baixa
        Liquidacao = 06,                                                          //'06' = Liquidação
        ConfirmacaoDoRecebimentoDaInstrucaoDeDesconto = 07,                       //'07' = Confirmação do Recebimento da Instrução de Desconto
        ConfirmacaoDoRecebimentoDoCancelamentoDoDesconto = 08,                    //'08' = Confirmação do Recebimento do Cancelamento do Desconto
        Baixa = 09,                                                               //'09' = Baixa
        TitulosEmCarteira = 11,                                                   //'11' = Títulos em Carteira (Em Ser)
        ConfirmacaoRecebimentoInstrucaoDeAbatimento = 12,                         //'12' = Confirmação Recebimento Instrução de Abatimento
        ConfirmacaoRecebimentoInstrucaoDeCancelamentoAbatimento = 13,             //'13' = Confirmação Recebimento Instrução de Cancelamento Abatimento
        ConfirmacaoRecebimentoInstrucaoAlteracaoDeVencimento = 14,                //'14' = Confirmação Recebimento Instrução Alteração de Vencimento
        FrancoDePagamento = 15,                                                   //'15' = Franco de Pagamento
        LiquidacaoAposBaixaOuLiquidacaoTituloNaoRegistrado = 17,                  //'17' = Liquidação Após Baixa ou Liquidação Título Não Registrado
        ConfirmacaoRecebimentoInstrucaoDeProtesto = 19,                           //'19' = Confirmação Recebimento Instrução de Protesto
        ConfirmacaoRecebimentoInstrucaoDeSustacaoCancelamentoDeProtesto = 20,     //'20' = Confirmação Recebimento Instrução de Sustação/Cancelamento de Protesto
        RemessaACartorio = 23,                                                    //'23' = Remessa a Cartório (Aponte em Cartório)
        RetiradaDeCartorioEManutencaoEmCarteira = 24,                             //'24' = Retirada de Cartório e Manutenção em Carteira
        ProtestadoEBaixado = 25,                                                  //'25' = Protestado e Baixado (Baixa por Ter Sido Protestado)
        InstrucaoRejeitada = 26,                                                  //'26' = Instrução Rejeitada
        ConfirmacaoDoPedidoDeAlteracaoDeOutrosDados = 27,                         //'27' = Confirmação do Pedido de Alteração de Outros Dados
        DebitoDeTarifasCustas = 28,                                               //'28' = Débito de Tarifas/Custas
        OcorrenciasDoPagador = 29,                                                //'29' = Ocorrências do Pagador
        AlteracaoDeDadosRejeitada = 30,                                           //'30' = Alteração de Dados Rejeitada
        ConfirmacaoDaAlteracaoDosDadosDoRateioDeCredito = 33,                     //'33' = Confirmação da Alteração dos Dados do Rateio de Crédito
        ConfirmacaoDoCancelamentoDosDadosDoRateioDeCredito = 34,                  //'34' = Confirmação do Cancelamento dos Dados do Rateio de Crédito
        ConfirmacaoDoDesagendamentoDoDebitoAutomatico = 35,                       //'35' = Confirmação do Desagendamento do Débito Automático
        ConfirmacaoDeEnvioDeEmailSMS = 36,                                        //'36' = Confirmação de envio de e-mail/SMS
        EnvioDeEmailSMSRejeitado = 37,                                            //'37' = Envio de e-mail/SMS rejeitado
        ConfirmacaoDeAlteracaoDoPrazoLimiteDeRecebimento = 38,                    //'38' = Confirmação de alteração do Prazo Limite de Recebimento
        ConfirmacaoDeDispensaDePrazoLimiteDeRecebimento = 39,                     //'39' = Confirmação de Dispensa de Prazo Limite de Recebimento
        ConfirmacaoDaAlteracaoDonumeroDoTitulodadoPeloBeneficiário = 40,          //'40' = Confirmação da alteração do número do título dado pelo Beneficiário
        ConfirmacaoDaAlteracaoDonumeroControleDoParticipante = 41,                //'41' = Confirmação da alteração do número controle do Participante
        ConfirmacaoDaAlteracaoDosdadosDoPagador = 42,                             //'42' = Confirmação da alteração dos dados do Pagador
        ConfirmacaoDaAlteracaoDosdadosDoSacadorAvalista = 43,                     //'43' = Confirmação da alteração dos dados do Sacador/Avalista
        TitulopagoComChequeDevolvido = 44,                                        //'44' = Título pago com cheque devolvido
        TitulopagoComChequeCompensado = 45,                                       //'45' = Título pago com cheque compensado
        InstrucaoParacancelarProtestoConfirmada = 46,                             //'46' = Instrução para cancelar protesto confirmada
        InstrucaoParaprotestoParafinsFalimentaresConfirmada = 47,                 //'47' = Instrução para protesto para fins falimentares confirmada
        ConfirmacaoDeinstrucaoDetransferenciaDecarteiraModalidadeDecobrança = 48, //'48' = Confirmação de instrução de transferência de carteira/modalidade de cobrança
        AlteracaoDecontratoDecobranca = 49,                                       //'49' = Alteração de contrato de cobrança
        TitulopagoComchequePendenteDeliquidacao = 50,                             //'50' = Título pago com cheque pendente de liquidação
        TituloDDAreconhecidoPeloPagador = 51,                                     //'51' = Título DDA reconhecido pelo Pagador
        TituloDDANaoReconhecidoPeloPagador = 52,                                  //'52' = Título DDA não reconhecido pelo Pagador
        TituloDDArecusadoPelaCIP = 53,                                            //'53' = Título DDA recusado pela CIP
        ConfirmacaoDaInstrucaoDeBaixaDeTituloNegativadoSemProtesto = 54,          //'54' = Confirmação da Instrução de Baixa de Título Negativado sem Protesto
        ConfirmacaoDePedidoDeDispensaDeMulta = 55,                                //'55' = Confirmação de Pedido de Dispensa de Multa
        ConfirmacaoDoPedidoDeCobrancaDeMulta = 56,                                //'56' = Confirmação do Pedido de Cobrança de Multa
        ConfirmacaoDoPedidoDeAlteracaoDeCobrancaDeJuros = 57,                     //'57' = Confirmação do Pedido de Alteração de Cobrança de Juros
        ConfirmacaoDoPedidoDeAlteracaoDoValorDataDeDesconto = 58,                 //'58' = Confirmação do Pedido de Alteração do Valor/Data de Desconto
        ConfirmacaoDoPedidoDeAlteracaoDoBeneficiarioDoTítulo = 59,                //'59' = Confirmação do Pedido de Alteração do Beneficiário do Título
        ConfirmacaoDoPedidoDeDispensaDeJurosDeMora = 60,                          //'60' = Confirmação do Pedido de Dispensa de Juros de Mora
        ConfirmacaoDeAlteracaoDoValorNominalDoTitulo = 61,                        //'61' = Confirmação de Alteração do Valor Nominal do Título
        TituloSustadoJudicialmente = 63                                           //'63' = Título Sustado Judicialmente
    }
}
