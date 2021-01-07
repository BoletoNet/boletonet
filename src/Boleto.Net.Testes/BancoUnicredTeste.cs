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
            var dataVencimento = boletoBancario.Boleto.DataVencimento;
            var dataVencimentoStr = string.Format("{0}{1}{2}", dataVencimento.Day.ToString("00"), dataVencimento.Month.ToString("00"), dataVencimento.ToString("yyyy"));
            var dataProcessamentoStr = string.Format("{0}{1}{2}", DateTime.Today.Day.ToString("00"), DateTime.Today.Month.ToString("00"), DateTime.Today.ToString("yyyy"));
            var arquivoTeste = "13600000         235342670000170                    0000190000000795020 EMPRESA MODELO S/A            UNICRED                                 1"+dataRemessa+ "00000000000108500000001                                                                  \r\n" +
                               "13600011R01  044 2035342670000170                    0000190000000795020 EMPRESA MODELO S/A                                                                                            00000001"+dataRemessa+"                                         \r\n" +
                               "1360001300001P 010000190000000795020000000000000        09    DOC 123        "+dataVencimentoStr+"000000000105000        N"+dataProcessamentoStr+"300000000000000000000000000000000000000000000000000000000000000000000000000000DOC 123                     0     0000000000 \r\n" +
                               "1360001300002Q 012035342670000170JOSE DA SILVA                           AV. DAS ROSAS, 10                       JARDIM FLORIDO 86300000CORNELIO PROCOPPR1000000000000000                                        000                            \r\n" +
                               "13600015         00000400000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000                                                                                                                             \r\n" +
                               "13699999         000006000010000000                                                                                                                                                                                                             \r\n";
            Assert.AreEqual(arquivoTeste, arquivo);
        }


        [TestMethod]
        public void Uniprime_LerRetorno()
        {
            var retorno = new ArquivoRetorno(TipoArquivo.CNAB400);
            detalheRetorno = new List<DetalheRetorno>();
            var arquivoTeste = "02RETORNO01COBRANCA       00000000000000000001EMPRESA MODELO S/A            084UNIPRIME NORTE 2206190000000007563                                                                                                                                                                                                                                                                          000000         000001\r\n" +
                               "102023989760001900000009000060097606700000000002              0000000000000000002P          00000000000000090222061900010255190000000000000000002P280719000000000020000000000  000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000010000000000000   000000                 0000000000                                                                  000002\r\n" +
                               "102023989760001900000009000060097606700000000001              00000000000000000011          000000000000000902220619000102551900000000000000000011280619000000000020000000000  000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000010000000000000   000000                 0000000000                                                                  000003\r\n" +
                               "9201084          000000020000006860084600000000          00476000065189902000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000                                                                                                                                                                              00000000000000000000000         000557\r\n";
            var buffer = Encoding.ASCII.GetBytes(arquivoTeste);
            var mem = new MemoryStream(buffer);
            retorno.LinhaDeArquivoLida += Retorno_LinhaDeArquivoLida;
            retorno.LerArquivoRetorno(new Banco(84), mem);
            Assert.AreEqual(detalheRetorno[0].IdentificacaoTitulo, "0000000000000000002P");
            Assert.AreEqual(detalheRetorno[1].IdentificacaoTitulo, "00000000000000000011");
        }

        private void Retorno_LinhaDeArquivoLida(object sender, LinhaDeArquivoLidaArgs e)
        {
            var titulo = (DetalheRetorno)e.Detalhe;
            detalheRetorno.Add(titulo);
        }

        [TestMethod]
        public void Uniprime_Homologacao()
        {
            var cedente = new Cedente("35.342.670/0001-70", "EMPRESA MODELO S/A", "0001", "9", "0079502", "0");
            cedente.Codigo = "444601";
            cedente.Convenio = 444601;
            cedente.DigitoCedente = 0;
            cedente.Carteira = "09";

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
                    Carteira = "09",
                    Cedente = cedente,
                    Banco = new Banco(084),
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
                boletoBancarioPDF.CodigoBanco = 084;
                boletoBancarioPDF.Boleto = boleto;
                boletoBancarioPDF.Boleto.Valida();
                var bytes = boletoBancarioPDF.MontaBytesPDF();
                var arquivoBoleto = Path.Combine(Path.GetTempPath(), string.Format("Boleto_Uniprime_{0}.pdf",i));
                if (File.Exists(arquivoBoleto)) File.Delete(arquivoBoleto);
                var sw = new FileStream(arquivoBoleto, FileMode.CreateNew);
                sw.Write(bytes, 0, (int)bytes.Length);
                sw.Flush();
                sw.Close();
                boleto.NossoNumero = i.ToString("00000000000");
            }

            // Gera Remessa e salva na pasta TEMP
            var remessa = new ArquivoRemessa(TipoArquivo.CNAB400);
            var arquivoRemessa = Path.Combine(Path.GetTempPath(), string.Format("Remessa_Uniprime_{0}{1}{2}.REM",DateTime.Today.Day.ToString("00"),DateTime.Today.Month.ToString("00"),DateTime.Today.ToString("yy")));
            if (File.Exists(arquivoRemessa)) File.Delete(arquivoRemessa);
            var swRemessa = new FileStream(arquivoRemessa, FileMode.CreateNew);
            remessa.GerarArquivoRemessa(cedente.Convenio.ToString(), new Banco(84), cedente, bolRemessa, swRemessa, 1);
        }
    }
}
