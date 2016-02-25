using System.ComponentModel;

namespace BoletoNet
{
    public enum CategoriaLancamento
    {
        //Débitos
        [Description("Cheques")]
        DebitoCheques = 101,

        DebitoEncargos = 102,
        DebitoEstornos = 103,
        DebitoLancamentoAvisado = 104,

        [Description("Tarifas")]
        DebitoTarifas = 105,
        DebitoAplicacao = 106,
        DebitoEmprestimoOuFinanciamento = 107,
        DebitoCambio = 108,
        DebitoCpmf = 109,
        DebitoIof = 110,
        DebitoImpostoRenda = 111,
        DebitoPagamentoFornecedores = 112,
        DebitoPagamentosSalario = 113,
        DebitoSaqueEletronico = 114,
        DebitoAcoes = 115,
        DebitoTransferenciaEntreContas = 117,
        DebitoDevolucaoCompensacao = 118,
        DebitoDevolucaoChequeDepositado = 119,
        DebitoTransferenciaInterbancariaDocTed = 120,
        DebitoAntecipacaoAFornecedores = 121,
        DebitoOcAerops = 122,

        //Créditos
        CreditoDepositos = 201,
        CreditoLiquidoDeCobranca = 202,
        CreditoDevolucaoDeCheques = 203,
        CreditoEstornos = 204,
        CreditoLancamentoAvisado = 205,
        CreditoResgateDeAplicacao = 206,
        CreditoEmprestimoOuFinanciamento = 207,
        CreditoCambio = 208,
        CreditoTransferenciaInterbancariaDocTed = 209,
        CreditoAcoes = 210,
        CreditoDividendos = 211,
        CreditoSeguro = 212,
        CreditoTransferenciaEntreContas = 213,
        CreditoDepositosEspeciais = 214,
        CreditoDevolucaoDaCompensacao = 215,
        CreditoOct = 216,
        CreditoPagamentosFornecedores = 217,
        CreditoPagamentosDiversos = 218,
        CreditoPagamentosSalarios = 219,
    }
}
