using BoletoNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        private static BoletoBancario GerarBoleto()
        {
            DateTime vencimento = DateTime.Now.AddDays(5);

            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", "0812", string.Empty, "81168", "0");
            cedente.Codigo = "08121581168";

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
