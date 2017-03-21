namespace BoletoNet
{
    public enum TipoOcorrenciaRemessa
    {
        EntradaDeTitulos = 01, //Entrada de T�tulos
        PedidoDeBaixa = 02, //Pedido de Baixa
        ProtestoParaFinsFalimentares = 03, //Protesto para Fins Falimentares
        Concess�oDeAbatimento = 04, //Concess�o de Abatimento
        CancelamentoDeAbatimento = 05, //Cancelamento de Abatimento
        Altera��oDeVencimento = 06, //Altera��o de Vencimento
        Concess�oDeDesconto = 07, //Concess�o de Desconto
        CancelamentoDeDesconto = 08, //Cancelamento de Desconto
        Protestar = 09, //Protestar
        SustarProtestoBaixarTitulo = 10, //Sustar Protesto e Baixar T�tulo
        SustarProtestoManterEmCarteira = 11, //Sustar Protesto e Manter em Carteira
        Altera��oDeJurosDeMora = 12, //Altera��o de Juros de Mora
        DispensarCobrancaDeJurosDeMora = 13, //Dispensar Cobran�a de Juros de Mora
        Altera��oDeValorPercentualDeMulta = 14, //Altera��o de Valor/Percentual de Multa
        DispensarCobrancaDeMulta = 15, //Dispensar Cobran�a de Multa
        Altera��oDeValorDataDeDesconto = 16, //Altera��o de Valor/Data de Desconto
        NaoConcederDesconto = 17, //N�o conceder Desconto
        AlteracaoDoValorDeAbatimento = 18, //Altera��o do Valor de Abatimento
        PrazoLimiteDeRecebimentoAlterar = 19, //Prazo Limite de Recebimento - Alterar
        PrazoLimiteDeRecebimentoDispensar = 20, //Prazo Limite de Recebimento - Dispensar
        AlterarNumeroDoTituloDadoPeloBeneficiario = 21, //Alterar n�mero do t�tulo dado pelo Benefici�rio
        AlterarNumeroControleDoParticipante = 22, //Alterar n�mero controle do Participante
        AlterarDadosDoPagador = 23, //Alterar dados do Pagador
        AlterarDadosDoSacadorAvalista = 24, //Alterar dados do Sacador/Avalista
        RecusaDaAlegacaoDoPagador = 30, //Recusa da Alega��o do Pagador
        AlteracaoDeOutrosDados = 31, //Altera��o de Outros Dados
        AlteracaoDosDadosDoRateioDeCredito = 33, //Altera��o dos Dados do Rateio de Cr�dito
        PedidoDeCancelamentoDosDadosDoRateioDeCredito = 34, //Pedido de Cancelamento dos Dados do Rateio de Cr�dito
        PedidoDeDesagendamentoDoD�bitoAutomatico = 35, //Pedido de Desagendamento do D�bito Autom�tico
        AlteracaoDeCarteira = 40, //Altera��o de Carteira
        Cancelarprotesto = 41, //Cancelar protesto
        AlteracaoDeEspecieDeTitulo = 42, //Altera��o de Esp�cie de T�tulo
        TransferenciaDeCarteiraModalidadeDeCobranca = 43, //Transfer�ncia de carteira/modalidade de cobran�a
        AlteracaoDeContratoDeCobranca = 44, //Altera��o de contrato de cobran�a
        NegativacaoSemProtesto = 45, //Negativa��o Sem Protesto
        SolicitacaoDeBaixaDeTituloNegativadoSemProtesto = 46, //Solicita��o de Baixa de T�tulo Negativado Sem Protesto
        AlteracaoDoValorNominalDoTitulo = 47, //Altera��o do Valor Nominal do T�tulo
    }
}