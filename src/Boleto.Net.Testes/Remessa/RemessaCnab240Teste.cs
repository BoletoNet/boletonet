using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BoletoNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Boleto.Net.Testes.Remessa
{
    [TestClass]
    public class RemessaCnab240Teste
    {
        [TestMethod]
        public void ArquivoRemessaCnab240ModosTest()
        {
            Cedente cedente = new Cedente(
                "12345678000155",
                "TESTE",
                "3559",
                "9",
                "14100",
                "3"
            ){
                Convenio = 1234567,
                Codigo = "123456",
                Carteira = "17"
            };

            var endereco = new Endereco()
            {
                End = "",
                Bairro = "",
                Cidade = "",
                UF =  "SP",
                CEP = "08090-284",
            };

            var sacado =new Sacado("11300946008", "", endereco);

            //Inst�ncia de Boleto
            var boleto = new BoletoNet.Boleto(
                DateTime.Today,
                5,
                "17",
                "1000",
                String.Empty,
                cedente)
            {
                NumeroDocumento = "1000",
                DataDocumento = DateTime.Today.AddDays(-1),
                Sacado = sacado,
                Banco = new Banco(1)
            };

            var boletos = new Boletos {boleto};

            var mem = new MemoryStream();
            var objREMESSA = new ArquivoRemessa(TipoArquivo.CNAB240EmModoTeste);
            objREMESSA.LinhaDeArquivoGerada+= delegate(object sender, LinhaDeArquivoGeradaArgs args)
            {
                if(args.TipoLinha == EnumTipodeLinha.HeaderDeArquivo || args.TipoLinha == EnumTipodeLinha.HeaderDeLote)
                    Assert.IsTrue(args.Linha.Contains("TS"));
            };
            objREMESSA.GerarArquivoRemessa("09", new Banco(001), cedente, boletos, mem, 1000);

            mem = new MemoryStream();
            objREMESSA = new ArquivoRemessa(TipoArquivo.CNAB240);
            objREMESSA.LinhaDeArquivoGerada += delegate (object sender, LinhaDeArquivoGeradaArgs args)
            {
                if (args.TipoLinha == EnumTipodeLinha.HeaderDeArquivo || args.TipoLinha == EnumTipodeLinha.HeaderDeLote)
                    Assert.IsFalse(args.Linha.Contains("TS"));
            };
            objREMESSA.GerarArquivoRemessa("09", new Banco(001), cedente, boletos, mem, 1000);
        }

    }
}
