using System.ComponentModel;

namespace BoletoNet
{
    public enum TipoOcorrenciaBradesco
    {
        [Description("00 - Outros")]
        Outros = 0,

        [Description("02 - Entrada Confirmada")]
        EntradaConfirmada = 2,

        [Description("03 - Entrada Rejeitada")]
        EntradaRejeitada = 3,

        [Description("06 - Liquidação normal")]
        LiquidacaoNomal = 6,

        [Description("09 - Baixado Automaticamente via Arquivo")]
        BaixadoAutomaticamenteViaArquivo = 9,

        [Description("10 - Baixado conforme instruções da Agência")]
        BaixadoConformeIntrucoesAgencia = 10,

        [Description("11 - Em Ser - Arquivo de Títulos Pendentes")]
        EmSerArquivoTitulosPendentes = 11,

        [Description("12 - Abatimento Concedido")]
        AbatimentoConcedido = 12,

        [Description("13 - Abatimento Cancelado")]
        AbatimentoCancelado = 13,

        [Description("14 - Vencimento Alterado")]
        VencimentoAlterado = 14,

        [Description("15 - Liquidação em Cartório")]
        LiquidacaoEmCartorio = 15,

        [Description("17 - Liquidação após baixa ou Título não Registrado")]
        liquidacaoAposBaixaOuTituloNaoRegistrado = 17,

        [Description("18 - Acerto de Depositária")]
        AcertoDeDepositaria = 18,

        [Description("19 - Confirmação Recebimento Instrução de Protesto")]
        ConfirmacaoRecebimentoInstrucoesDeProtesto = 19,

        [Description("20 - Confirmação Recebimento Instrução Sustação de Protesto")]
        ConfirmacaoRecebimentoInstrucaoSustacaoDeProtesto = 20,

        [Description("21 - Acerto do Controle do Participante")]
        AcertoDoControleDoParticipante = 21,

        [Description("23 - Entrada do Título em Cartório")]
        EntradaDoTituloEmCartorio = 23,

        [Description("24 - Entrada rejeitada por CEP Irregular")]
        EntradaRejeitadaPorCepIrregular = 24,

        [Description("27 - Baixa Rejeitada")]
        BaixaRejeitada = 27,

        [Description("28 - Débito de tarifas/custas")]
        DebitoDeTarifasCustas = 28,

        [Description("30 - Alteração de Outros Dados Rejeitados")]
        AlteracaoDeOutrosDadosRejeitados = 30,

        [Description("32 - Instrução Rejeitada")]
        InstruxaoRejeitada = 32,

        [Description("33 - Confirmação Pedido Alteração Outros Dados")]
        ConfirmacaoPedidoAlteracaoOutrosDados = 33,

        [Description("34 - Retirado de Cartório e Manutenção Carteira")]
        RetiradoDeCartorioEManutencaoCarteira = 34,

        [Description("35 - Desagendamento do débito automático")]
        Desagendamento_DebitoAutomatico = 35,

        [Description("40 - Estorno de pagamento")]
        EstornoDePagamento = 40,

        [Description("55 - Sustado judicial")]
        SustadoJudicial = 55,

        [Description("68 - Acerto dos dados do rateio de Crédito")]
        AcertoDosDados_RateioDeCredito = 68,

        [Description("69 - Cancelamento dos dados do rateio")]
        CancelamentoDosDados_Rateio = 69
    }
}