using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoNet.Testes
{
    [TestClass]
    public class BancoUniprimeTeste
    {
        List<DetalheRetorno> detalheRetorno;
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

            var arquivo = Encoding.ASCII.GetString(mem.ToArray());
            var dataRemessa = string.Format("{0}{1}{2}", DateTime.Today.Day.ToString("00"), DateTime.Today.Month.ToString("00"), DateTime.Today.ToString("yy"));
            var dataVencimento = boletoBancario.Boleto.DataVencimento;
            var dataVencimentoStr = string.Format("{0}{1}{2}", dataVencimento.Day.ToString("00"), dataVencimento.Month.ToString("00"), dataVencimento.ToString("yy"));
            var arquivoTeste = "01REMESSA01COBRANCA       00000000000000444601EMPRESA MODELO S A            084UNIPRIME       "+dataRemessa+"        MX0000001                                                                                                                                                                                                                                                                                     000001\r\n"+
                               "1                   00090000100795020DOC 123                  08400000100010001282          1               01000DOC 123"+dataVencimentoStr+"0000000105000        02N010101000000000000000000000000000000000000             00000000000000235342670000170JOSE DA SILVA                           AV. DAS ROSAS, 10                                   86300000      JARDIM FLORIDO000000000000000000000CORNELIO PROCOPIOPR000002\r\n"+
                               "9                                                                                                                                                                                                                                                                                                                                                                                                         000003\r\n";
            Assert.AreEqual(arquivo, arquivoTeste);
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
