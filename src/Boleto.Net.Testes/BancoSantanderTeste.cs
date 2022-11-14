using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoNet.Testes
{
    [TestClass]
    public class BancoSantanderTeste
    {
        #region Carteira 101

        private BoletoBancario GerarBoletoCarteira101()
        {
            DateTime vencimento = new DateTime(2022, 12, 4);

            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", "32651", "3309894123", "4");

            cedente.Codigo = "3309894";

            Boleto boleto = new Boleto(vencimento, 2701.40m, "101", "000000020061", cedente);

            boleto.NumeroDocumento = "20061";

            var boletoBancario = new BoletoBancario();

            boletoBancario.CodigoBanco = 33;

            boletoBancario.Boleto = boleto;

            return boletoBancario;
        }

        [TestMethod]
        public void Santander_Carteira_101_NossoNumero()
        {
            var boletoBancario = GerarBoletoCarteira101();

            boletoBancario.Boleto.Valida();

            //string nossoNumeroValido = "000000020061-0";
            string nossoNumeroValido = "000000020061-1";

            Assert.AreEqual(boletoBancario.Boleto.NossoNumero, nossoNumeroValido, "Nosso número inválido");
        }

        [TestMethod]
        public void Santander_Carteira_101_LinhaDigitavel()
        {
            var boletoBancario = GerarBoletoCarteira101();

            boletoBancario.Boleto.Valida();

            string linhaDigitavelValida = "03399.33095 89400.000009 20061.101018 1 91890000270140";

            Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.LinhaDigitavel, linhaDigitavelValida, "Linha digitável inválida");
        }

        //[TestMethod]
        public void Santander_Carteira_101_CodigoBarra()
        {
            var boletoBancario = GerarBoletoCarteira101();

            boletoBancario.Boleto.Valida();

            string codigoBarraValida = "03391553700002701409330989400000002006110101";

            Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.Codigo, codigoBarraValida, "Código de Barra inválido");
        }
        #endregion

        #region Carteira 102
        private BoletoBancario GerarBoletoCarteira101_Com_IOS_8()
        {
            DateTime vencimento = new DateTime(2021, 8, 20);

            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", "3719", "", "") { Codigo = "3780457" };

            Boleto boleto = new Boleto(vencimento, 1052.50m, "101", "000000252084", cedente);

            boleto.PercentualIOS = 8;

            boleto.DataProcessamento = new DateTime(2021, 7, 20);

            //boleto.NumeroDocumento = "20061";

            var boletoBancario = new BoletoBancario();

            boletoBancario.CodigoBanco = 33;

            boletoBancario.Boleto = boleto;

            return boletoBancario;
        }

        [TestMethod]
        public void Santander_Carteira_101_LinhaDigitavel_Com_IOS_8()
        {
            var boletoBancario = GerarBoletoCarteira101_Com_IOS_8();

            boletoBancario.Boleto.Valida();

            string linhaDigitavelValida = "03399.37807 45700.000024 52084.281014 8 87180000105250";

            Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.LinhaDigitavel, linhaDigitavelValida, "Linha digitável inválida");
        }

        [TestMethod]
        public void Santander_Carteira_101_CodigoBarra_Com_IOS_8()
        {
            var boletoBancario = GerarBoletoCarteira101_Com_IOS_8();

            boletoBancario.Boleto.Valida();

            string codigoBarra = "03398871800001052509378045700000025208428101";

            Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.Codigo, codigoBarra, "Código de Barra inválido");
        }

        [TestMethod]
        public void Santander_Mod11_Com_IOS_6()
        {
            int dv = AbstractBanco.Mod11("0339204600000273719028203356661245780020102", 9, 0);

            Assert.AreEqual(dv, 6, "Linha digitável inválida");
        }

        [TestMethod]
        public void Santander_Mod11_Com_IOS_8()
        {
            int dv = AbstractBanco.Mod11("0339567700001052509378045700000025208428101", 9, 0);

            Assert.AreEqual(dv, 8, "Linha digitável inválida");
        }

        #endregion

        #region Remessa

        private BoletoBancario GerarBoletoRemessa()
        {
            DateTime vencimento = new DateTime(2022, 12, 4);

            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", "32651", "3309894123", "4");
            cedente.ContaBancaria.DigitoAgencia = "0";

            cedente.Codigo = "3309894";

            Boleto boleto = new Boleto(vencimento, 2701.40m, "101", "000000020061", cedente);

            boleto.NumeroDocumento = "20061";
            
            boleto.Sacado = new Sacado("87425264188", "Sacado teste", new Endereco()
            {
                CEP = "78945612",
                Cidade = "Teste",
                UF = "PR",
                End = "End teste",
                Bairro = "Centro"
            });

            boleto.Instrucoes.Add(new Instrucao_Santander()
            {
                Codigo = 1,
                Descricao = "teste"
            });

            var boletoBancario = new BoletoBancario();

            boletoBancario.CodigoBanco = 33;

            boletoBancario.Boleto = boleto;

            return boletoBancario;
        }

        [TestMethod]
        public void BancoSantander_GerarRemessaCNAB400()
        {
            var boletos = Enumerable.Range(0, 5).Select(o => {
                var boleto = GerarBoletoRemessa();
                return boleto;
            });

            Boletos itensRemessa = new Boletos();
            itensRemessa.AddRange(boletos.Select(o => o.Boleto));

            var banco = itensRemessa.First().Banco;
            var cedente = itensRemessa.First().Cedente;
            cedente.CodigoTransmissao = "01";

            ArquivoRemessa arquivoRemessa = new ArquivoRemessa(TipoArquivo.CNAB400);
            arquivoRemessa.LinhaDeArquivoGerada += (object sender, LinhaDeArquivoGeradaArgs e) =>
            {
                Debug.WriteLine(e.Linha);
            };

            using (var stream = new MemoryStream())
            {
                arquivoRemessa.GerarArquivoRemessa("08111081111", banco, cedente, itensRemessa, stream, 1);
                var conteudo = Encoding.ASCII.GetString(stream.ToArray());
                Debug.WriteLine(conteudo);
            }
        }
        #endregion
    }
}
