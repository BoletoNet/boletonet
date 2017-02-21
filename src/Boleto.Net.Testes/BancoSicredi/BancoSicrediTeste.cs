using BoletoNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Boleto.Net.Testes.BancoSicredi
{
    [TestClass]
    public class BancoSicrediTeste
    {
        [TestMethod]
        public void ValidarBoleto()
        {
            BoletoBancario boletoBancario = GerarBoleto();
            boletoBancario.Boleto.Valida();
        }

        [TestMethod]
        public void GerarRemessaCNAB400()
        {
            var boletos = Enumerable.Range(0, 3).Select(o => GerarBoleto());

            Boletos itensRemessa = new Boletos();
            itensRemessa.AddRange(boletos.Select(o => o.Boleto));

            var banco = itensRemessa.First().Banco;
            var cedente = itensRemessa.First().Cedente;

            ArquivoRemessa arquivoRemessa = new ArquivoRemessa(TipoArquivo.CNAB400);
            arquivoRemessa.LinhaDeArquivoGerada += (object sender, LinhaDeArquivoGeradaArgs e) =>
            {
                Debug.WriteLine(e.Linha);
            };
            byte[] arquivo;
            using (var stream = new MemoryStream())
            {                
                arquivoRemessa.GerarArquivoRemessa("8120683004", banco, cedente, itensRemessa, stream, 1);
                arquivo = stream.ToArray();
            }
        }

        private static BoletoBancario GerarBoleto()
        {
            DateTime vencimento = DateTime.Now.AddDays(5);

            var agencia = "0811";
            var conta = "81111";

            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", agencia, string.Empty, conta, "0");
            cedente.Codigo = "08111081111";

            BoletoNet.Boleto boleto = new BoletoNet.Boleto(vencimento, 722.71M, "1", "17200096", cedente);

            boleto.NumeroDocumento = "901900-069";
            boleto.DataDocumento = DateTime.Now.AddDays(-15);

            var boletoBancario = new BoletoBancario();

            boletoBancario.CodigoBanco = 748;

            boletoBancario.Boleto = boleto;
            return boletoBancario;
        }
    }
}
