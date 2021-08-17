using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace BoletoNet.Testes
{
    [TestClass]
    public class BancoC6Teste
    {
        private const int CODIGO_BANCO = (int)Enums.Bancos.C6Bank;
        private readonly List<MassaTestes> massaTestes;

        public BancoC6Teste()
        {
            massaTestes = new List<MassaTestes>()
            {
                new MassaTestes
                {
                    Cedente = new MassaTestes.CedenteTeste
                    {
                        Agencia = "0001",
                        ContaCorrente = "000000000001"
                    },
                    Boleto = new MassaTestes.BoletoTeste
                    {
                        Carteira = "21",
                        Vcto = new DateTime(2020, 4, 25),
                        Valor = 420.00m,
                        NossoNumero = "0000113522",
                        CodigoBarrasEsperado = "33691823600000420000000000000010000113522213",
                        LinhaDigitavelEsperada = "33690.00009 00000.010009 01135.222139 1 82360000042000"
                    }
                },

                new MassaTestes
                {
                    Cedente = new MassaTestes.CedenteTeste
                    {
                        Agencia = "0001",
                        ContaCorrente = "000000000002"
                    },
                    Boleto = new MassaTestes.BoletoTeste
                    {
                        Carteira = "24",
                        Vcto = new DateTime(2021, 7, 29),
                        Valor = 456.12m,
                        NossoNumero = "0000000002",
                        CodigoBarrasEsperado = "33691869600000456120000000000020000000002243",
                        LinhaDigitavelEsperada = "33690.00009 00000.020008 00000.022434 1 86960000045612"
                    }
                }
            };
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

        #endregion Classe para representar massa de testes de boletos a serem testados

        private BoletoBancario GerarBoletoCarteira500(MassaTestes item)
        {
            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", item.Cedente.Agencia, item.Cedente.ContaCorrente);
            cedente.Codigo = item.Cedente.ContaCorrente;

            Boleto boleto = new Boleto(item.Boleto.Vcto, item.Boleto.Valor, item.Boleto.Carteira, item.Boleto.NossoNumero, cedente);

            const int modalidadeIdentificadorLayoutEmissaoBanco = 3;
            boleto.TipoModalidade = modalidadeIdentificadorLayoutEmissaoBanco.ToString();

            var boletoBancario = new BoletoBancario();
            boletoBancario.CodigoBanco = CODIGO_BANCO;
            boletoBancario.Boleto = boleto;

            return boletoBancario;
        }

        [TestMethod]
        public void NossoNumeroTest()
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
        public void LinhaDigitavelTest()
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
        public void CodigoBarraTest()
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