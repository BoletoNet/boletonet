using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BoletoNet;

namespace Boleto.Net.Testes {
    [TestClass]
    public class CecredTeste {
        [TestMethod]
        public void Cecred_GerarBoletoCarteira01() {

            DateTime vencimento = new DateTime(2016, 03, 11);

            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", "0109", "265071", "1");
            cedente.Convenio = 0110041;
            cedente.Endereco = new Endereco() {
                Bairro = "BAIRRO CEDENTE",
                CEP = "88309-600",
                Cidade = "CIDADE CEDENTE",
                Complemento = "CASA",
                Email = "cedente@teste.com.br",
                End = "Teste CEDENTE",
                Numero = "999",
                UF = "SC"
            };
            cedente.Codigo = cedente.Convenio.ToString();

            cedente.Carteira = "01";
            string nossoNumero = "000000127"; // Tamanho nosso numero 9

            var sacado = new Sacado("000.000.000-00", "Sacado Teste");
            sacado.Endereco = new Endereco() {
                Bairro = "BAIRRO SACADO",
                CEP = "88309-600",
                Cidade = "CIDADE SACADO",
                Complemento = "CASA",
                Email = "sacado@teste.com.br",
                End = "Teste SACADO",
                Numero = "999",
                UF = "SC"
            };

            BoletoNet.Boleto boleto = new BoletoNet.Boleto(vencimento, 10.99m, cedente.Carteira, nossoNumero, cedente);
            boleto.NumeroDocumento = "ABC123";
            boleto.Sacado = sacado;
            BoletoNet.BoletoBancario boletoBancario = new BoletoBancario();
            boletoBancario.CodigoBanco = 85;    // CECRED
            boletoBancario.Boleto = boleto;

            try {
                boletoBancario.Boleto.Valida();
                var htmlString = boletoBancario.MontaHtml();

                //string _arquivo = string.Empty;
                //_arquivo = string.Format(@"C:\temp\boleto_{0}.html", boletoBancario.Boleto.NumeroDocumento);

                //using (System.IO.FileStream f = new System.IO.FileStream(_arquivo, System.IO.FileMode.Create)) {
                //    System.IO.StreamWriter w = new System.IO.StreamWriter(f, System.Text.Encoding.UTF8);
                //    w.Write(htmlString);
                //}

                Assert.IsTrue(!string.IsNullOrEmpty(htmlString));
            } catch {

            }

        }
    }
}
