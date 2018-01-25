using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace BoletoNet.Testes.BancoSemear
{
    [TestClass]
    public class BancoSemearTeste
    {
        Boleto boleto = new Boleto();
        Banco banco = new Banco(743);

        [TestMethod]
        public void TesteLinhaDigitavel()
        {
            var boleto = new Boleto();
            var banco = new Banco(743);

            boleto.Cedente = new Cedente()
            {
                Codigo = "743",
                MostrarCNPJnoBoleto = true,
                Nome = "BANCO SEMEAR",
                CPFCNPJ = "65825481000110",
                Carteira = "2",
                DigCedente = "9",
                ContaBancaria = new ContaBancaria()
                {
                    Agencia = "001",
                    DigitoAgencia = "0",
                    Conta = "65456",
                    DigitoConta = "5"
                },
                Endereco = new Endereco()
            };

            boleto.LocalPagamento = "Este boleto poderá ser pago em toda a Rede Bancária até 5 dias após o vencimento. ";
            boleto.Instrucoes.Add(new Instrucao_Bradesco()
            {
                Descricao = "Não receber após o vencimento",
                QuantidadeDias = 3
            });

            boleto.ValorBoleto = 251.51M;
            boleto.ValorCobrado = 251.51M;
            boleto.NossoNumero = "35148373401";
            boleto.NumeroDocumento = "051483734";
            boleto.DataVencimento = new DateTime(2017, 12, 4);
            boleto.DataProcessamento = DateTime.Now;
            boleto.Carteira = "02";

            boleto.Sacado = new Sacado()
            {
                CPFCNPJ = "05461883893",
                Nome = "Joãozinho Testador",
                Endereco = new Endereco()
                {
                    Complemento = "Bla bla",
                    Numero = "80",
                    Bairro = "",
                    CEP = "32310535",
                    Cidade = "Contagem",
                    UF = "Minas Gerais",
                }
            };

            boleto.CodigoBarra.CodigoBanco = "743";
            boleto.CodigoBarra.Moeda = 9;
            boleto.CodigoBarra.CampoLivre = "0001023514837340110996818";
            boleto.CodigoBarra.ValorDocumento = "0000025151";
            boleto.CodigoBarra.FatorVencimento = 7363;

            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);
            banco.FormataLinhaDigitavel(boleto);

            var linhaDigitavel = boleto.CodigoBarra.LinhaDigitavel;

            var boletoBancario = new BoletoBancario
            {
                CodigoBanco = 743,
                Boleto = boleto,
                MostrarEnderecoCedente = true,
                MostrarContraApresentacaoNaDataVencimento = false,
                GerarArquivoRemessa = true
            };

            var boletoHtml = boletoBancario.MontaBytesPDF();

            var arquivo = File.Create("bnet_semear.pdf");
            arquivo.Close();

            File.WriteAllBytes("bnet_semear.pdf", boletoHtml);
        }

        [TestMethod]
        public void TesteDigitoVerificadorNossoNumero()
        {
            var boleto = new Boleto();
            var banco = new Banco(743);
            var resultadoEsperado = "35148373401-6";
            boleto.NossoNumero = "35148373401";
            banco.FormataNossoNumero(boleto);

            Assert.AreEqual(resultadoEsperado, boleto.NossoNumero);
        }
    }
}
