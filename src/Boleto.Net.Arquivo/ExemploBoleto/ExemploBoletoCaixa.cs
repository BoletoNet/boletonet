using System;
using System.Collections.Generic;

namespace BoletoNet.Arquivo
{
    public static class ExemploBoletoCaixa
    {
        private static ContaBancaria ContaBancaria { get; } = new ContaBancaria
        {
            Agencia = "345",
            DigitoAgencia = "6",
            Conta = "87654321",
            DigitoConta = "0"
        };

        private static Cedente Cedente { get; } = new Cedente
        {
            ContaBancaria = ContaBancaria,
            CPFCNPJ = "00.000.000/0000-00",
            Nome =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin sed magna eu magna fringilla convallis nec sit amet neque. Aenean efficitur in dui vitae tincidunt.",
            Codigo = "000000"
        };

        private static Sacado Sacado { get; } = new Sacado
        {
            Nome = "Fulano de Silva",
            CPFCNPJ = "000.000.000-00",
            Endereco = new Endereco
            {
                End = "SSS 154 Bloco J Casa 23",
                Bairro = "Testando",
                Cidade = "Testelândia",
                CEP = "70000000",
                UF = "RS"
            }
        };

        public static Banco Banco { get; } = new Banco(104);

        private static Instrucao_Caixa InstrucaoCaixa { get; } = new Instrucao_Caixa
        {
            Codigo = (int)EnumInstrucoes_Caixa.ProtestarAposNDiasUteis,
            QuantidadeDias = 5,
            Banco = Banco
        };

        public static Boleto Boleto
        {
            get
            {
                var boleto = new Boleto
                {
                    Cedente = Cedente,
                    DataProcessamento = DateTime.Now,
                    DataVencimento = DateTime.Now.AddDays(15),
                    ValorBoleto = (decimal)9999.99,
                    Carteira = "RG",
                    NossoNumero = "10400000000000000",
                    EspecieDocumento = new EspecieDocumento(104),
                    Sacado = Sacado,
                    Instrucoes = new List<IInstrucao> { InstrucaoCaixa },
                    Banco = Banco
                };
                boleto.Valida();
                return boleto;
            }
        }

        public static BoletoBancario BoletoBancario { get; } = new BoletoBancario
        {
            CodigoBanco = (short)Banco.Codigo,
            Boleto = Boleto
        };
    }
}
