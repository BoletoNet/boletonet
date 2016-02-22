using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoNet.Testes
{
    [TestClass]
    public class BancoSicoobTeste
    {
        //private BoletoBancario GerarBoletoCarteira1()
        //{
        //    DateTime vencimento = new DateTime(2013, 7, 20);

        //    var cedente = new Cedente("00.693.135/0001-80", "TRANSCODIL TRANSPORTE E COMÉRCIO DE DIESEL LTDA", "3416", "0025075", "");

        //    cedente.Codigo = 3214;
        //    cedente.DigitoCedente = 0;
        //    cedente.Carteira = "1";
            
        //    Boleto boleto = new Boleto(vencimento, 1.00m, "1", "000001234", cedente);

        //    boleto.NumeroDocumento = "NF1234";

        //    var boletoBancario = new BoletoBancario();

        //    boletoBancario.CodigoBanco = 756;

        //    boletoBancario.Boleto = boleto;

        //    return boletoBancario;
        //}

        //[TestMethod]
        //public void Santander_Carteira_1_NossoNumero()
        //{
        //    var boletoBancario = GerarBoletoCarteira1();

        //    boletoBancario.Boleto.Valida();

        //    string nossoNumeroValido = "0000012342";

        //    Assert.AreEqual(boletoBancario.Boleto.NossoNumero, nossoNumeroValido, "Nosso número inválido");
        //}

        //[TestMethod]
        //public void Siboob_Carteira_1_LinhaDigitavel()
        //{
        //    var boletoBancario = GerarBoletoCarteira1();

        //    boletoBancario.Boleto.Valida();

        //    string linhaDigitavelValida = "75691.41554 02003.214000 00123.420010 6 57650000000100";

        //    Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.LinhaDigitavel, linhaDigitavelValida, "Linha digitável inválida");
        //}
    }
}
