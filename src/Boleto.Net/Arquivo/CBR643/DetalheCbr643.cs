using System;

namespace BoletoNet.Arquivo
{
    public class DetalheCbr643 : LinhaCbr643
    {
        public DetalheCbr643()
        {
            Id = 7;
        }

        /// <summary>
        ///     Prefixo da Ag�ncia
        /// </summary>
        [TextPos(017, 004)]
        public int Agencia { get; set; }

        /// <summary>
        ///     D�gito Verificador - D.V. - do Prefixo da Ag�ncia
        /// </summary>
        [TextPos(021, 001)]
        public string AgenciaDV { get; set; }

        /// <summary>
        ///     N�mero da Conta Corrente do Cedente
        /// </summary>
        [TextPos(022, 008)]
        public int ContaCorrenteCedente { get; set; }

        /// <summary>
        ///     D�gito Verificador - D.V. - do N�mero da Conta Corrente do Cedente
        /// </summary>
        [TextPos(030, 001)]
        public string ContaCorrenteCedenteDV { get; set; }

        /// <summary>
        ///     N�mero do Conv�nio de Cobran�a do Cedente
        /// </summary>
        [TextPos(031, 007)]
        public int NumeroConvenio { get; set; }

        /// <summary>
        ///     N�mero de Controle do Participante
        /// </summary>
        [TextPos(038, 025)]
        public string NumeroControle { get; set; }

        /// <summary>
        ///     Nosso-N�mero
        /// </summary>
        [TextPos(063, 017)]
        public string NossoNumero { get; set; }

        /// <summary>
        ///     Tipo de cobran�a 02
        /// </summary>
        [TextPos(080, 001)]
        public int TipoCobranca { get; set; }

        /// <summary>
        ///     Tipo de cobran�a espec�fico para comando 72 (altera��o de tipo de cobran�a de t�tulos das carteiras  11 e 17) 03
        /// </summary>
        [TextPos(081, 001)]
        public int TipoCobrancaEspecifico { get; set; }

        /// <summary>
        ///     Dias para c�lculo 04
        /// </summary>
        [TextPos(082, 004)]
        public int DiasParaCalculo { get; set; }

        /// <summary>
        ///     Natureza do recebimento 05
        /// </summary>
        [TextPos(086, 002)]
        public int NaturezaRecebimento { get; set; }

        /// <summary>
        ///     Prefixo do T�tulo 06
        /// </summary>
        [TextPos(088, 003)]
        public string PrefixoTitulo { get; set; }

        /// <summary>
        ///     Varia��o da Carteira
        /// </summary>
        [TextPos(091, 003)]
        public int VariacaoCarteira { get; set; }

        /// <summary>
        ///     Conta Cau��o 06
        /// </summary>
        [TextPos(094, 001)]
        public int CotaCaucao { get; set; }

        /// <summary>
        ///     Taxa para desconto
        /// </summary>
        [TextPos(095, 005)]
        public int TaxaDesconto { get; set; }

        /// <summary>
        ///     Taxa IOF
        /// </summary>
        [TextPos(100, 004)]
        public int TaxaIof { get; set; }

        /// <summary>
        ///     Carteira
        /// </summary>
        [TextPos(106, 002)]
        public int Carteira { get; set; }

        /// <summary>
        ///     Comando 07
        /// </summary>
        [TextPos(108, 002)]
        public int Comando { get; set; }

        /// <summary>
        ///     Data de liquida��o (DDMMAA)
        /// </summary>
        [TextPos(110, 006, "ddMMyy")]
        public DateTime DataDeLiquidacao { get; set; }

        /// <summary>
        ///     N�mero do t�tulo dado pelo cedente
        /// </summary>
        [TextPos(116, 010)]
        public string NumeroTitulo { get; set; }

        /// <summary>
        ///     Data de vencimento (DDMMAA)
        /// </summary>
        [TextPos(146, 006, "ddMMyy")]
        public DateTime DataVencimento { get; set; }

        /// <summary>
        ///     v99 Valor do t�tulo
        /// </summary>
        [TextPos(152, 011)]
        public decimal ValorTitulo { get; set; }

        /// <summary>
        ///     C�digo do banco recebedor 08
        /// </summary>
        [TextPos(165, 003)]
        public int CodigoBancoRecebedor { get; set; }

        /// <summary>
        ///     Prefixo da ag�ncia recebedora 08
        /// </summary>
        [TextPos(168, 004)]
        public int AgenciaRecebedora { get; set; }

        /// <summary>
        ///     DV prefixo recebedora
        /// </summary>
        [TextPos(172, 001)]
        public string AgenciaRecebedoraDb { get; set; }

        /// <summary>
        ///     Esp�cie do t�tulo 09
        /// </summary>
        [TextPos(175, 002)]
        public decimal EspeciaTitulo { get; set; }

        /// <summary>
        ///     Data do cr�dito (DDMMAA) 10
        /// </summary>
        [TextPos(175, 006,"ddMMyy")]
        public DateTime DataCredito { get; set; }

        /// <summary>
        ///     v99 Valor da tarifa 06
        /// </summary>
        [TextPos(181, 005)]
        public decimal ValorTarifa { get; set; }

        /// <summary>
        ///     v99 Outras despesas
        /// </summary>
        [TextPos(187, 011)]
        public decimal OutrasDespesas { get; set; }

        /// <summary>
        ///     v99 Juros do desconto
        /// </summary>
        [TextPos(201, 011)]
        public decimal JurosDesconto { get; set; }

        /// <summary>
        ///     v99 IOF do desconto
        /// </summary>
        [TextPos(214, 011)]
        public decimal IofDesconto { get; set; }

        /// <summary>
        ///     v99 Valor do abatimento
        /// </summary>
        [TextPos(227, 011)]
        public decimal ValorAbatimento { get; set; }

        /// <summary>
        ///     v99 Desconto concedido  (diferen�a entre valor do t�tulo e valor recebido)
        /// </summary>
        [TextPos(240, 011)]
        public decimal DescontoConcedido { get; set; }

        /// <summary>
        ///     v99 Valor recebido (valor recebido parcial)
        /// </summary>
        [TextPos(253, 011)]
        public decimal ValorRecebido { get; set; }

        /// <summary>
        ///     v99 Juros de mora
        /// </summary>
        [TextPos(266, 011)]
        public decimal JurosMora { get; set; }

        /// <summary>
        ///     v99 Outros recebimentos
        /// </summary>
        [TextPos(279, 011)]
        public decimal OutrosRecebimentos { get; set; }

        /// <summary>
        ///     v99 Abatimento n�o aproveitado pelo sacado
        /// </summary>
        [TextPos(292, 011)]
        public decimal AbatimentoNaoAproveitadoPeloSacado { get; set; }

        /// <summary>
        ///     v99 Valor do lan�amento
        /// </summary>
        [TextPos(305, 011)]
        public decimal ValorLancamento { get; set; }

        /// <summary>
        ///     Indicativo de d�bito/cr�dito 11
        /// </summary>
        [TextPos(318, 001)]
        public int IndicativoDebitoCredito { get; set; }

        /// <summary>
        ///     Indicador de valor 12
        /// </summary>
        [TextPos(319, 001)]
        public int IndicadorValor { get; set; }

        /// <summary>
        ///     v99 Valor do ajuste 13
        /// </summary>
        [TextPos(320, 010)]
        public decimal ValorAjuste { get; set; }

        /// <summary>
        ///     Indicativo de  Autoriza��o de Liquida��o Parcial  24
        /// </summary>
        [TextPos(391, 001)]
        public int IndicativoDeAutorizacaoDeLiquidacaoParcial { get; set; }


        /// <summary>
        ///     Canal de pagamento do t�tulo utilizado pelo sacado/Meio de Apresenta��o do T�tulo ao Sacado. 15
        /// </summary>
        [TextPos(393, 002)]
        public int CanalDePagamentoDoTitulo { get; set; }
    }
}
