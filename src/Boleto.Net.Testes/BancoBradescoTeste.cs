using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace BoletoNet.Testes
{
    [TestClass]
    public class BancoBradescoTeste
    {
        #region Carteira 16

        private BoletoBancario GerarBoletoCarteira16()
        {
            var vencimento = new DateTime(2015, 7, 20);
            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", "0413", "8", "0002916", "5");
            var boleto = new Boleto(vencimento, 123, "16", "00970171092", cedente);
            boleto.NumeroDocumento = "970171092";

            var boletoBancario = new BoletoBancario();
            boletoBancario.CodigoBanco = 237;
            boletoBancario.Boleto = boleto;

            return boletoBancario;
        }

        [TestMethod]
        public void Bradesco_Carteira_16_NossoNumero()
        {
            var boletoBancario = GerarBoletoCarteira16();

            boletoBancario.Boleto.Valida();

            string nossoNumeroValido = "016/00970171092-1";

            Assert.AreEqual(boletoBancario.Boleto.NossoNumero, nossoNumeroValido, "Nosso número inválido");
        }

        [TestMethod]
        public void Bradesco_Carteira_16_LinhaDigitavel()
        {
            var boletoBancario = GerarBoletoCarteira16();

            boletoBancario.Boleto.Valida();

            string linhaDigitavelValida = "23790.41318 60097.017101 92000.291606 2 64950000012300";

            Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.LinhaDigitavel, linhaDigitavelValida, "Linha digitável inválida");
        }

        [TestMethod]
        public void Bradesco_Carteira_16_CodigoBarra()
        {
            var boletoBancario = GerarBoletoCarteira16();

            boletoBancario.Boleto.Valida();

            string codigoBarraValida = "23792649500000123000413160097017109200029160";

            Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.Codigo, codigoBarraValida, "Código de Barra inválido");
        }

        #endregion

        #region Carteira 09

        private BoletoBancario GerarBoletoCarteira09()
        {
            DateTime vencimento = new DateTime(2012, 6, 8);

            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", "0539", "8", "0032463", "9");

            Boleto boleto = new Boleto(vencimento, 7620, "09", "18194", cedente);

            boleto.NumeroDocumento = "18194";

            var boletoBancario = new BoletoBancario();

            boletoBancario.CodigoBanco = 237;

            boletoBancario.Boleto = boleto;

            return boletoBancario;
        }

        private BoletoBancario GerarBoletoComValorCobradoCarteira09()
        {
            var vencimento = new DateTime(2012, 6, 8);
            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", "0539", "8", "0032463", "9");

            var boleto = new Boleto(vencimento, 5000, "09", "18194", cedente);
            boleto.NumeroDocumento = "18194";
            boleto.ValorMulta = 100;
            boleto.ValorCobrado = 5100;

            var boletoBancario = new BoletoBancario();
            boletoBancario.CodigoBanco = 237;
            boletoBancario.Boleto = boleto;
            return boletoBancario;
        }

        [TestMethod]
        public void Bradesco_Carteira_09_NossoNumero()
        {
            var boletoBancario = GerarBoletoCarteira09();

            boletoBancario.Boleto.Valida();

            string nossoNumeroValido = "009/00000018194-6";

            Assert.AreEqual(boletoBancario.Boleto.NossoNumero, nossoNumeroValido, "Nosso número inválido");
        }

        [TestMethod]
        public void Bradesco_Carteira_09_LinhaDigitavel()
        {
            var boletoBancario = GerarBoletoCarteira09();

            boletoBancario.Boleto.Valida();

            string linhaDigitavelValida = "23790.53909 90000.001819 94003.246306 3 53580000762000";

            Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.LinhaDigitavel, linhaDigitavelValida, "Linha digitável inválida");
        }

        [TestMethod]
        public void Bradesco_Carteira_09_LinhaDigitavel_ValorCobrado()
        {
            var boletoBancario = GerarBoletoComValorCobradoCarteira09();

            boletoBancario.Boleto.Valida();

            var linhaDigitavelValida = "23790.53909 90000.001819 94003.246306 1 53580000510000";

            Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.LinhaDigitavel, linhaDigitavelValida, "Linha digitável inválida");
        }

        [TestMethod]
        public void Bradesco_Carteira_09_CodigoBarra()
        {
            var boletoBancario = GerarBoletoCarteira09();

            boletoBancario.Boleto.Valida();

            string codigoBarraValida = "23793535800007620000539090000001819400324630";

            Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.Codigo, codigoBarraValida, "Código de Barra inválido");
        }

        [TestMethod]
        public void Bradesco_Carteira_09_CodigoBarra_ValorCobrado()
        {
            var boletoBancario = GerarBoletoComValorCobradoCarteira09();

            boletoBancario.Boleto.Valida();

            var codigoBarraValida = "23791535800005100000539090000001819400324630";

            Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.Codigo, codigoBarraValida, "Código de Barra inválido");
        }

        [TestMethod]
        public void Bradesco_Carteira_09_ArquivoRemessa()
        {
            Cedente objCEDENTE = new Cedente(
               "12345678000155",
               "TESTE",
               "1111",
               "11234",
               "1"
               );
            objCEDENTE.Codigo = "123456";
            objCEDENTE.Convenio = 9;

            //Inst�ncia de Boleto
            Boleto objBOLETO = new Boleto();
            //O nosso-numero deve ser de 11 posi��es
            objBOLETO.EspecieDocumento = new EspecieDocumento(237,"12");
            objBOLETO.DataVencimento = DateTime.Now.AddDays(10);
            objBOLETO.ValorBoleto = 90;
            objBOLETO.Carteira ="09";
            objBOLETO.NossoNumero = ("00000012345");
            objBOLETO.Cedente = objCEDENTE;
            //O n� do documento deve ser de 10 posi��es
            objBOLETO.NumeroDocumento = "1234567890";
            objBOLETO.NumeroControle = "100";
            //A data do documento � a data de emiss�o do boleto
            objBOLETO.DataDocumento = DateTime.Now;
            //A data de processamento � a data em que foi processado o documento, portanto � da data de emiss�o do boleto
            objBOLETO.DataProcessamento = DateTime.Now;
            objBOLETO.Sacado = new Sacado("12345678000255", "TESTE SACADO");
            objBOLETO.Sacado.Endereco.End = "END SACADO";
            objBOLETO.Sacado.Endereco.Bairro = "BAIRRO SACADO";
            objBOLETO.Sacado.Endereco.Cidade = "CIDADE SACADO";
            objBOLETO.Sacado.Endereco.CEP = "CEP SACADO";
            objBOLETO.Sacado.Endereco.UF = "RR";

            objBOLETO.PercMulta = 10;
            objBOLETO.JurosMora = 5;

            // nao precisa desta parte no boleto do brasdesco.
            /*objBOLETO.Remessa = new Remessa()
            {
                Ambiente = Remessa.TipoAmbiemte.Producao,
                CodigoOcorrencia = "01",
            };*/

            Boletos objBOLETOS = new Boletos();
            objBOLETOS.Add(objBOLETO);

            var mem = new MemoryStream();
            var objREMESSA = new ArquivoRemessa(TipoArquivo.CNAB400);
            objREMESSA.GerarArquivoRemessa("09", new Banco(237), objCEDENTE, objBOLETOS, mem, 1000);


        }

        [TestMethod]
        public void Bradesco_Carteira_09_ArquivoRetorno400() {
            ArquivoRetornoCNAB400 ret = new ArquivoRetornoCNAB400();

            var arquivo = string.Empty; //@"C:\Temp\2017-Julho-CB180701_009_CB_09_433920_20170718041844_00152068-7.RET";

            if (arquivo != string.Empty) {
                using (FileStream fs = new FileStream(arquivo, FileMode.Open, FileAccess.Read)) {
                    ret.LerArquivoRetorno(new Banco(237), fs);
                }

                foreach (var item in ret.ListaDetalhe) {
                    Console.WriteLine(item.ToString());
                }
            }

        }
        #endregion

        #region Carteira 25

        private BoletoBancario GerarBoletoCarteira25()
        {
            DateTime vencimento = new DateTime( 2015, 10, 21 );

            var cedente = new Cedente( "00.000.000/0000-00", "Empresa Teste", "054", "0", "148870", "8" );

            Boleto boleto = new Boleto( vencimento,(decimal)469.4, "25", "97000005287", cedente );

            boleto.NumeroDocumento = "5..287";

            var boletoBancario = new BoletoBancario();

            boletoBancario.CodigoBanco = 237;

            boletoBancario.Boleto = boleto;

            return boletoBancario;
        }

        [TestMethod]
        public void Bradesco_Carteira_25_NossoNumero()
        {
            var boletoBancario = GerarBoletoCarteira25();

            boletoBancario.Boleto.Valida();

            string nossoNumeroValido = "025/97000005287-P";

            Assert.AreEqual( boletoBancario.Boleto.NossoNumero, nossoNumeroValido, "Nosso número inválido" );
        }

        [TestMethod]
        public void Bradesco_Carteira_25_LinhaDigitavel()
        {
            var boletoBancario = GerarBoletoCarteira25();

            boletoBancario.Boleto.Valida();

            string linhaDigitavelValida = "23790.05420 59700.000520 87014.887001 1 65880000046940";

            Assert.AreEqual( boletoBancario.Boleto.CodigoBarra.LinhaDigitavel, linhaDigitavelValida, "Linha digitável inválida" );
        }

        [TestMethod]
        public void Bradesco_Carteira_25_CodigoBarra()
        {
            var boletoBancario = GerarBoletoCarteira25();

            boletoBancario.Boleto.Valida();

            string codigoBarraValida = "23791658800000469400054259700000528701488700";

            Assert.AreEqual( boletoBancario.Boleto.CodigoBarra.Codigo, codigoBarraValida, "Código de Barra inválido" );
        }
        #endregion

        #region Carteira 26

        private BoletoBancario GerarBoletoCarteira26()
        {
            DateTime vencimento = new DateTime(2015, 10, 21);

            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", "054", "0", "148870", "8");

            Boleto boleto = new Boleto(vencimento, (decimal)469.4, "26", "97000005287", cedente);

            boleto.NumeroDocumento = "5..287";

            var boletoBancario = new BoletoBancario();

            boletoBancario.CodigoBanco = 237;

            boletoBancario.Boleto = boleto;

            return boletoBancario;
        }

        [TestMethod]
        public void Bradesco_Carteira_26_NossoNumero()
        {
            var boletoBancario = GerarBoletoCarteira26();

            boletoBancario.Boleto.Valida();

            string nossoNumeroValido = "026/97000005287-3";

            Assert.AreEqual(boletoBancario.Boleto.NossoNumero, nossoNumeroValido, "Nosso número inválido");
        }

        [TestMethod]
        public void Bradesco_Carteira_26_LinhaDigitavel()
        {
            var boletoBancario = GerarBoletoCarteira26();

            boletoBancario.Boleto.Valida();

            string linhaDigitavelValida = "23790.05420 69700.000529 87014.887001 5 65880000046940";

            Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.LinhaDigitavel, linhaDigitavelValida, "Linha digitável inválida");
        }

        [TestMethod]
        public void Bradesco_Carteira_26_CodigoBarra()
        {
            var boletoBancario = GerarBoletoCarteira26();

            boletoBancario.Boleto.Valida();

            string codigoBarraValida = "23795658800000469400054269700000528701488700";

            Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.Codigo, codigoBarraValida, "Código de Barra inválido");
        }
        #endregion
    }
}
