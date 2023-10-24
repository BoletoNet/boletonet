using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text;
using BoletoNet.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoNet.Testes
{
    [TestClass]
    public class BancoSerFinanceTeste
    {
        List<DetalheRetorno> detalheRetorno;
        private BoletoBancario GerarBoletoCarteira110()
        {
            var cedente = new Cedente("35.342.670/0001-70", "EMPRESA MODELO S/A", "0001", "9", "0079502", "0");
            cedente.Codigo = "444601";
            cedente.DigitoCedente = 0;
            cedente.Carteira = "110";

            var sacado = new Sacado("35.342.670/0001-70", "JOSE DA SILVA");
            sacado.Endereco = new Endereco() { End = "AV. DAS ROSAS", Numero = "10", Bairro = "JARDIM FLORIDO", Cidade = "CORNELIO PROCOPIO", CEP = "86300-000", UF = "PR", Email = "teste@boleto.net" };
            var boleto = new Boleto(DateTime.Today.AddDays(30), 1.00m, "110", "1234567890", cedente);
            boleto.Sacado = sacado;
            boleto.ContaBancaria = new ContaBancaria("00012", "9079509");
            boleto.NumeroDocumento = "DOC 123";
            boleto.ValorBoleto = 777.88M;
            boleto.TipoEmissao = TipoEmissao.EmissaoPeloBanco;
            var boletoBancario = new BoletoBancario();
            boletoBancario.CodigoBanco = 530;
            boletoBancario.Boleto = boleto;
            return boletoBancario;
        }


        [TestMethod]
        public void SerFinance_Carteira_110_NossoNumero()
        {
            var boletoBancario = GerarBoletoCarteira110();
            boletoBancario.Boleto.Valida();
            string nossoNumeroValido = "110/1234567890-9";
            Assert.AreEqual(boletoBancario.Boleto.NossoNumero, nossoNumeroValido, "Nosso número inválido");
        }

        [TestMethod]
        public void SerFinance_GerarRemessa()
        {
            var boletoBancario = GerarBoletoCarteira110();
            boletoBancario.Boleto.ValorMulta = 1;
            var remessa = new ArquivoRemessa(TipoArquivo.CNAB240);
            var mem = new MemoryStream();
            var boletos = new Boletos();
            boletos.Add(boletoBancario.Boleto);
            remessa.GerarArquivoRemessa(boletoBancario.Cedente.Convenio.ToString(), new Banco(530), boletoBancario.Cedente, boletos, mem, 1);

            var arquivo = Encoding.ASCII.GetString(mem.ToArray());
            var dataRemessa = string.Format("{0}{1}{2}", DateTime.Today.Day.ToString("00"), DateTime.Today.Month.ToString("00"), DateTime.Today.Year.ToString("0000"));
            var dataVencimento = boletoBancario.Boleto.DataVencimento;
            var dataVencimentoStr = string.Format("{0}{1}{2}", dataVencimento.Day.ToString("00"), dataVencimento.Month.ToString("00"), dataVencimento.Year.ToString("0000"));
            var arquivoTeste = "53000000000000000235342670000170444601              00001900000007950200EMPRESA MODELO S A            Ser Finance                             1" + dataRemessa + "02041500000110101600                                                                     \r\n" +
                               "53000011 01  030 2353426700001700444601              00001900000007950200EMPRESA MODELO S A                                                                                            000000000000000000000000                                 \r\n" +
                               "5300001300001P 01000019000000079502005000001100123456789011211DOC 123        " + dataVencimentoStr + "00000000007778800001901A01010001101010001000000000000000100000000000000000000000000000000000000000000000000000DOC 123                  3002000090000000000 \r\n" +
                               "5300001320000Q 012035342670000170JOSE DA SILVA                           AV. DAS ROSAS, 10                       JARDIM FLORIDO 86300000CORNELIO PROCOPPR0000000000000000000000000000000000000000000000000000000 000                            \r\n" +
                               "5300001300003R 01100000000000000000000000100000000000000000000000101010001000000000000100                                                                                                                                             0         \r\n" +
                               "53000015         00000500000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000                                                                                                                             \r\n" +
                               "53099999         0000010000070000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\r\n";
            var horaArquivo = arquivo.Substring(151, 6);
            arquivoTeste = arquivoTeste.Remove(151, 6).Insert(151, horaArquivo);
            Assert.AreEqual(arquivo, arquivoTeste);
        }

        [TestMethod]
        public void SerFinance_LerRetorno()
        {
            var retorno = new ArquivoRetorno(TipoArquivo.CNAB240);
            detalheRetorno = new List<DetalheRetorno>();
            var arquivoTeste = "02RETORNO01COBRANCA       00000000000000000001EMPRESA MODELO S/A            530UNIPRIME NORTE 2206190000000007563                                                                                                                                                                                                                                                                          000000         000001\r\n" +
                               "102023989760001900000009000060097606700000000002              0000000000000000002P          00000000000000090222061900010255190000000000000000002P280719000000000020000000000  000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000010000000000000   000000                 0000000000                                                                  000002\r\n" +
                               "102023989760001900000009000060097606700000000001              00000000000000000011          000000000000000902220619000102551900000000000000000011280619000000000020000000000  000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000010000000000000   000000                 0000000000                                                                  000003\r\n" +
                               "9201084          000000020000006860084600000000          00476000065189902000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000                                                                                                                                                                              00000000000000000000000         000557\r\n";
            //var buffer = Encoding.ASCII.GetBytes(arquivoTeste);
            //var mem = new MemoryStream(buffer);
            var mem = File.OpenRead(@"c:\temp\serfinance.txt");


            retorno.LinhaDeArquivoLida += Retorno_LinhaDeArquivoLida;
            retorno.LerArquivoRetorno(new Banco(530), mem);
            Assert.AreEqual(detalheRetorno[0].IdentificacaoTitulo, "0000000000000000002P");
            Assert.AreEqual(detalheRetorno[1].IdentificacaoTitulo, "00000000000000000011");
        }

        private void Retorno_LinhaDeArquivoLida(object sender, LinhaDeArquivoLidaArgs e)
        {
            var titulo = (DetalheRetorno)e.Detalhe;
            detalheRetorno.Add(titulo);
        }

        [TestMethod]
        public void SerFinance_Homologacao()
        {
            var cedente = new Cedente("35.342.670/0001-70", "EMPRESA MODELO S/A", "0001", "9", "0079502", "0");
            cedente.Codigo = "444601";
            cedente.Convenio = 444601;
            cedente.DigitoCedente = 0;
            cedente.Carteira = "110";

            var sacado = new Sacado("35.342.670/0001-70", "JOSE DA SILVA");
            sacado.Endereco = new Endereco() { End = "AV. DAS ROSAS", Numero = "10", Bairro = "JARDIM FLORIDO", Cidade = "CORNELIO PROCOPIO", CEP = "86300-000", UF = "PR", Email = "teste@boleto.net" };

            // Gera 5 boletos e salva em PDF na pasta TEMP
            var bolRemessa = new Boletos();
            for (int i = 1; i <= 5; i++)
            {
                var boleto = new Boleto()
                {
                    NumeroDocumento = "DOC " + i.ToString("00000"),
                    DataVencimento = DateTime.Today.AddDays(i),
                    ValorBoleto = 200 * i,
                    NossoNumero = i.ToString("00000000000"),
                    Carteira = "110",
                    Cedente = cedente,
                    Banco = new Banco(530),
                    Sacado = new Sacado()
                    {
                        CPFCNPJ = "35.342.670/0001-70",
                        Nome = "JOSE DA SILVA",
                        Endereco = new Endereco()
                        {
                            End = "AV. DAS ROSAS",
                            Numero = "10",
                            Bairro = "JARDIM FLORIDO",
                            Cidade = "CORNELIO PROCOPIO",
                            CEP = "86300-000",
                            UF = "PR",
                            Email = "teste@boleto.net"
                        }
                    }
                };
                bolRemessa.Add(boleto);
                var boletoBancarioPDF = new BoletoBancario();
                boletoBancarioPDF.CodigoBanco = 530;
                boletoBancarioPDF.Boleto = boleto;
                boletoBancarioPDF.Boleto.Valida();
                var bytes = boletoBancarioPDF.MontaBytesPDF();
                var arquivoBoleto = Path.Combine(Path.GetTempPath(), string.Format("Boleto_SerFinance_{0}.pdf",i));
                if (File.Exists(arquivoBoleto)) File.Delete(arquivoBoleto);
                var sw = new FileStream(arquivoBoleto, FileMode.CreateNew);
                sw.Write(bytes, 0, (int)bytes.Length);
                sw.Flush();
                sw.Close();
                boleto.NossoNumero = i.ToString("00000000000");
            }

            // Gera Remessa e salva na pasta TEMP
            var remessa = new ArquivoRemessa(TipoArquivo.CNAB400);
            var arquivoRemessa = Path.Combine(Path.GetTempPath(), string.Format("Remessa_SerFinance_{0}{1}{2}.REM",DateTime.Today.Day.ToString("00"),DateTime.Today.Month.ToString("00"),DateTime.Today.ToString("yy")));
            if (File.Exists(arquivoRemessa)) File.Delete(arquivoRemessa);
            var swRemessa = new FileStream(arquivoRemessa, FileMode.CreateNew);
            remessa.GerarArquivoRemessa(cedente.Convenio.ToString(), new Banco(530), cedente, bolRemessa, swRemessa, 1);
        }
    }
}
