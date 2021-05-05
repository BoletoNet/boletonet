using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoNet.Testes
{
    [TestClass]
    public class BancoUnicredTeste
    {
        List<DetalheRetorno> detalheRetorno;
        private BoletoBancario GerarBoletoCarteira1()
        {
            var cedente = new Cedente("35.342.670/0001-70", "EMPRESA MODELO S/A", "0001", "9", "0079502", "0");
            cedente.CodigoTransmissao = "001";

            var sacado = new Sacado("35.342.670/0001-70", "JOSE DA SILVA");
            sacado.Endereco = new Endereco() { End = "AV. DAS ROSAS", Numero = "10", Bairro = "JARDIM FLORIDO", Cidade = "CORNELIO PROCOPIO", CEP = "86300-000", UF = "PR", Email = "teste@boleto.net" };
            var boleto = new Boleto(DateTime.Today.AddDays(30), 1.00m, "09", "0000000000", cedente);
            boleto.Sacado = sacado;
            boleto.ContaBancaria = new ContaBancaria("00019", "00795020");
            boleto.NumeroDocumento = "DOC 123";
            boleto.ValorBoleto = 1050;
            var boletoBancario = new BoletoBancario();
            boletoBancario.CodigoBanco = 136;
            boletoBancario.Boleto = boleto;
            boleto.Valida();
            return boletoBancario;
        }


        [TestMethod]
        public void Unicred_GerarRemessa()
        {
            var boletoBancario = GerarBoletoCarteira1();
            var remessa = new ArquivoRemessa(TipoArquivo.CNAB240);
            var mem = new MemoryStream();
            var boletos = new Boletos();
            boletos.Add(boletoBancario.Boleto);
            remessa.GerarArquivoRemessa(boletoBancario.Cedente.Convenio.ToString(), new Banco(136), boletoBancario.Cedente, boletos, mem, 1);

            var arquivo = Encoding.ASCII.GetString(mem.ToArray());
            var dataRemessa = string.Format("{0}{1}{2}", DateTime.Today.Day.ToString("00"), DateTime.Today.Month.ToString("00"), DateTime.Today.ToString("yyyy"));
            var horaRemessa = string.Format("{0}{1}00", DateTime.Now.Hour.ToString("00"), DateTime.Now.Minute.ToString("00"));
            var dataVencimento = boletoBancario.Boleto.DataVencimento;
            var dataVencimentoStr = string.Format("{0}{1}{2}", dataVencimento.Day.ToString("00"), dataVencimento.Month.ToString("00"), dataVencimento.ToString("yyyy"));
            var dataProcessamentoStr = string.Format("{0}{1}{2}", DateTime.Today.Day.ToString("00"), DateTime.Today.Month.ToString("00"), DateTime.Today.ToString("yyyy"));
            var arquivoTeste = "13600000         235342670000170                    00001900000000000010EMPRESA MODELO S/A            UNICRED                                 1" + dataRemessa + horaRemessa + "00000108500000000                                                                  \r\n" +
                               "13600011R01  044 2035342670000170                    00001900000000795020EMPRESA MODELO S/A                                                                                            00000001" + dataRemessa + "        00                               \r\n" +
                               "1360001300001P 010000190000000795020000000000000        09    DOC 123        " + dataVencimentoStr + "000000000105000      N N" + dataProcessamentoStr + "5        000000000000000000000000000000000000000               000000000000000DOC 123                  000    090000000000 \r\n" +
                               "1360001300002Q 012035342670000170JOSE DA SILVA                           AV. DAS ROSAS, 10                       JARDIM FLORIDO 86300000CORNELIO PROCOPPR1000000000000000                                                                       \r\n" +
                               "13600015         0000040000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000                                                                                                                     \r\n" +
                               "13699999         000001000006000000                                                                                                                                                                                                             \r\n";
            Assert.AreEqual(arquivoTeste, arquivo);
        }
    }
}
