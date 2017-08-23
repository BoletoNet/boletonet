using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BoletoNet.Testes
{
    [TestClass]
    public class BancoVotorantimTeste
    {
        const int CODIGO_BANCO = 655;
        private List<MassaTestes> massaTestes;

        public BancoVotorantimTeste()
        {
            massaTestes = new List<MassaTestes>();

            //insere testes
            massaTestes.Add(new MassaTestes { Cedente = new MassaTestes.CedenteTeste { Agencia = "0001", ContaCorrente = "0000000400" }, Boleto = new MassaTestes.BoletoTeste { Carteira = "500", Vcto = new DateTime(2017, 8, 18), Valor = 2820.12m, NossoNumero = "0000003158", CodigoBarrasEsperado = "65591725500002820120000000400500000000315800", LinhaDigitavelEsperada = "65590.00002 00400.500005 00003.158003 1 72550000282012" } });
            massaTestes.Add(new MassaTestes { Cedente = new MassaTestes.CedenteTeste { Agencia = "0001", ContaCorrente = "0000000078" }, Boleto = new MassaTestes.BoletoTeste { Carteira = "500", Vcto = new DateTime(2017, 8, 14), Valor = 15.20m, NossoNumero = "0000021226", CodigoBarrasEsperado = "65594725100000015200000000078500000002122600", LinhaDigitavelEsperada = "65590.00002 00078.500006 00021.226006 4 72510000001520" } });
            massaTestes.Add(new MassaTestes { Cedente = new MassaTestes.CedenteTeste { Agencia = "0001", ContaCorrente = "0000000116" }, Boleto = new MassaTestes.BoletoTeste { Carteira = "500", Vcto = new DateTime(2017, 8, 14), Valor = 10.20m, NossoNumero = "0000021226", CodigoBarrasEsperado = "65591725100000010200000000116500000002122600", LinhaDigitavelEsperada = "65590.00002 00116.500000 00021.226006 1 72510000001020" } });
        }

        #region Classe para representar massa de testes de boletos a serem testados
        private class MassaTestes
        {
            public CedenteTeste Cedente { get; set; }

            public BoletoTeste Boleto { get; set; }

            public class CedenteTeste
            {
                public string Agencia { get; set; }
                public string ContaCorrente { get; set; }
            }

            public class BoletoTeste
            {
                public DateTime Vcto { get; set; }

                public decimal Valor { get; set; }

                public string Carteira { get; set; }

                public string NossoNumero { get; set; }

                public string CodigoBarrasEsperado { get; set; }

                public string LinhaDigitavelEsperada { get; set; }
            }
        }
        #endregion


        private BoletoBancario GerarBoletoCarteira500(MassaTestes item)
        {
            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", item.Cedente.Agencia, item.Cedente.ContaCorrente);
            cedente.Convenio = Convert.ToInt64(item.Cedente.ContaCorrente);

            Boleto boleto = new Boleto(item.Boleto.Vcto, item.Boleto.Valor, item.Boleto.Carteira, item.Boleto.NossoNumero, cedente);

            //boleto.NumeroDocumento = "20061";

            var boletoBancario = new BoletoBancario();
            boletoBancario.CodigoBanco = CODIGO_BANCO;
            boletoBancario.Boleto = boleto;

            return boletoBancario;
        }

        [TestMethod]
        public void Votorantim_Carteira_500_NossoNumero()
        {

            foreach (MassaTestes item in massaTestes)
            {
                var boletoBancario = GerarBoletoCarteira500(item);
                boletoBancario.Boleto.Valida();

                string nossoNumeroValido = item.Boleto.NossoNumero;

                Assert.AreEqual(boletoBancario.Boleto.NossoNumero, nossoNumeroValido, "Nosso número inválido");
            }

        }

        [TestMethod]
        public void Votorantim_Carteira_500_LinhaDigitavel()
        {
            foreach (MassaTestes item in massaTestes)
            {
                var boletoBancario = GerarBoletoCarteira500(item);
                boletoBancario.Boleto.Valida();

                string linhaDigitavelValida = item.Boleto.LinhaDigitavelEsperada;

                Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.LinhaDigitavel, linhaDigitavelValida, "Linha digitável inválida");
            }

        }

        [TestMethod]
        public void Votorantim_Carteira_500_CodigoBarra()
        {
            foreach (MassaTestes item in massaTestes)
            {
                var boletoBancario = GerarBoletoCarteira500(item);
                boletoBancario.Boleto.Valida();

                string codigoBarraValida = item.Boleto.CodigoBarrasEsperado;

                Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.Codigo, codigoBarraValida, "Código de Barras inválido");
            }
             
        }

    }
}
