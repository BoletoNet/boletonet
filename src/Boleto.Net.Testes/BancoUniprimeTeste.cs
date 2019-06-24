using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoNet.Testes
{
    [TestClass]
    public class BancoUniprimeTeste
    {
        private BoletoBancario GerarBoletoCarteira1()
        {
            var cedente = new Cedente("35.342.670/0001-70", "EMPRESA MODELO S/A", "0001", "9", "0079502", "0");
            cedente.Codigo = "444601";
            cedente.Convenio = 444601;
            cedente.DigitoCedente = 0;
            cedente.Carteira = "09";

            var sacado = new Sacado("35.342.670/0001-70", "JOSE DA SILVA");
            sacado.Endereco = new Endereco() { End = "AV. DAS ROSAS", Numero = "10", Bairro = "JARDIM FLORIDO", Cidade = "CORNELIO PROCOPIO", CEP = "86300-000", UF = "PR", Email = "teste@boleto.net" };
            var boleto = new Boleto(DateTime.Today.AddDays(30), 1.00m, "09", "10001000128", cedente);
            boleto.Sacado = sacado;
            boleto.ContaBancaria = new ContaBancaria("0001", "0079502");
            boleto.NumeroDocumento = "DOC 123";
            boleto.ValorBoleto = 1050;
            var boletoBancario = new BoletoBancario();
            boletoBancario.CodigoBanco = 084;
            boletoBancario.Boleto = boleto;
            return boletoBancario;
        }

        [TestMethod]
        public void Uniprime_Carteira_9_NossoNumero_Digito_N()
        {
            var boletoBancario = GerarBoletoCarteira1();
            boletoBancario.Boleto.Valida();
            string nossoNumeroValido = "009/10001000128-2";
            Assert.AreEqual(boletoBancario.Boleto.NossoNumero, nossoNumeroValido, "Nosso número inválido");
        }

        [TestMethod]
        public void Uniprime_Carteira_9_NossoNumero_Digito_P()
        {
            var boletoBancario = GerarBoletoCarteira1();
            boletoBancario.Boleto.NossoNumero = "00000000001";
            boletoBancario.Boleto.Valida();
            string nossoNumeroValido = "009/00000000001-P";
            Assert.AreEqual(boletoBancario.Boleto.NossoNumero, nossoNumeroValido, "Nosso número inválido");
        }

        [TestMethod]
        public void Uniprime_Carteira_9_LinhaDigitavel()
        {
            var boletoBancario = GerarBoletoCarteira1();
            boletoBancario.Boleto.DataVencimento = new DateTime(2018, 09, 25);
            boletoBancario.Boleto.Valida();
            string linhaDigitavelValida = "08490.00104 91000.100015 28007.950208 7 76580000105000";
            Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.LinhaDigitavel, linhaDigitavelValida, "Linha digitável inválida");
        }

        [TestMethod]
        public void Uniprime_GerarRemessa()
        {
            var boletoBancario = GerarBoletoCarteira1();
            var remessa = new ArquivoRemessa(TipoArquivo.CNAB400);
            var mem = new MemoryStream();
            var boletos = new Boletos();
            boletos.Add(boletoBancario.Boleto);
            remessa.GerarArquivoRemessa(boletoBancario.Cedente.Convenio.ToString(), new Banco(84), boletoBancario.Cedente, boletos, mem, 1);
            Assert.Inconclusive();
        }
    }
}
