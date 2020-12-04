namespace BoletoNet
{
    public enum TipoOcorrenciaRemessa
    {
        EntradaDeTitulos = 01, //Entrada de Títulos
        PedidoDeBaixa = 02, //Pedido de Baixa
        ProtestoParaFinsFalimentares = 03, //Protesto para Fins Falimentares
        ConcesssaoDeAbatimento = 04, //Concessão de Abatimento
        CancelamentoDeAbatimento = 05, //Cancelamento de Abatimento
        AlteracaoDeVencimento = 06, //Alteração de Vencimento
        ConcessaoDeDesconto = 07, //Concessão de Desconto
        CancelamentoDeDesconto = 08, //Cancelamento de Desconto
        Protestar = 09, //Protestar
        SustarProtestoBaixarTitulo = 10, //Sustar Protesto e Baixar Título
        SustarProtestoManterEmCarteira = 11, //Sustar Protesto e Manter em Carteira
        AlteracaoDeJurosDeMora = 12, //Alteração de Juros de Mora
        DispensarCobrancaDeJurosDeMora = 13, //Dispensar Cobrança de Juros de Mora
        AlteracaoDeValorPercentualDeMulta = 14, //Alteração de Valor/Percentual de Multa
        DispensarCobrancaDeMulta = 15, //Dispensar Cobrança de Multa
        AlteracaoDeValorDataDeDesconto = 16, //Alteração de Valor/Data de Desconto
        NaoConcederDesconto = 17, //Não conceder Desconto
        AlteracaoDoValorDeAbatimento = 18, //Alteração do Valor de Abatimento
        PrazoLimiteDeRecebimentoAlterar = 19, //Prazo Limite de Recebimento - Alterar
        PrazoLimiteDeRecebimentoDispensar = 20, //Prazo Limite de Recebimento - Dispensar
        AlterarNumeroDoTituloDadoPeloBeneficiario = 21, //Alterar número do título dado pelo Beneficiário
        AlterarNumeroControleDoParticipante = 22, //Alterar número controle do Participante
        AlterarDadosDoPagador = 23, //Alterar dados do Pagador
        AlterarDadosDoSacadorAvalista = 24, //Alterar dados do Sacador/Avalista
        RecusaDaAlegacaoDoPagador = 30, //Recusa da Alegação do Pagador
        AlteracaoDeOutrosDados = 31, //Alteração de Outros Dados
        AlteracaoDosDadosDoRateioDeCredito = 33, //Alteração dos Dados do Rateio de Crédito
        PedidoDeCancelamentoDosDadosDoRateioDeCredito = 34, //Pedido de Cancelamento dos Dados do Rateio de Crédito
        PedidoDeDesagendamentoDoDebitoAutomatico = 35, //Pedido de Desagendamento do Débito Automático
        AlteracaoDeCarteira = 40, //Alteração de Carteira
        Cancelarprotesto = 41, //Cancelar protesto
        AlteracaoDeEspecieDeTitulo = 42, //Alteração de Espécie de Título
        TransferenciaDeCarteiraModalidadeDeCobranca = 43, //Transferência de carteira/modalidade de cobrança
        AlteracaoDeContratoDeCobranca = 44, //Alteração de contrato de cobrança
        NegativacaoSemProtesto = 45, //Negativação Sem Protesto
        SolicitacaoDeBaixaDeTituloNegativadoSemProtesto = 46, //Solicitação de Baixa de Título Negativado Sem Protesto
        AlteracaoDoValorNominalDoTitulo = 47, //Alteração do Valor Nominal do Título
    }
}