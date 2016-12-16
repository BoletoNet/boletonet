using BoletoNet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesCobrancaRegistrada
{
    public class Class1
    {
        static void Main()
        {
            int numeroArquivoRemessa = 1234;

            var objCedente = new Cedente("17508675000149", "AUTO EXPRESSO TECNOLOGIA S.A.", "3403", "", "13004354", "4");
            //objCedente.Codigo = "123456";
            objCedente.Convenio = 6265650;
            objCedente.CodigoTransmissao = "340300006265650";
            objCedente.NumeroSequencial = numeroArquivoRemessa;
            
            var remessa = new Remessa();
            remessa.CodigoOcorrencia = "01";

            //Instância de Boleto
            var objBOLETO = new Boleto();
            objBOLETO.EspecieDocumento = new EspecieDocumento(33, "DS");
            objBOLETO.DataVencimento = DateTime.ParseExact("11/12/2016", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            objBOLETO.DataMulta = objBOLETO.DataVencimento;
            objBOLETO.ValorBoleto = 24;
            objBOLETO.Carteira = "101";
            objBOLETO.NossoNumero = ("160000000548");
            objBOLETO.Cedente = objCedente;
            objBOLETO.NumeroDocumento = "";
            objBOLETO.DataDocumento = DateTime.ParseExact("01/12/2016", "dd/MM/yyyy", CultureInfo.InvariantCulture); 
            objBOLETO.DataProcessamento = DateTime.ParseExact("01/12/2016", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            objBOLETO.Sacado = new Sacado("29438268049", "JAIME BERGAMANN SCALO");
            objBOLETO.Sacado.Endereco.End = "PROF. MILTON GUERREIRO 270 BLOCO B";
            objBOLETO.Sacado.Endereco.Bairro = "SANTA TEREZA";
            objBOLETO.Sacado.Endereco.Cidade = "PORTO ALEGRE";
            objBOLETO.Sacado.Endereco.CEP = "90850350";
            objBOLETO.Sacado.Endereco.UF = "RS";
            
            objBOLETO.PercMulta = 2;
            objBOLETO.JurosMora = 1;
            objBOLETO.CodJurosMora = "2";
            objBOLETO.NumeroDiasBaixa = 29;

            objBOLETO.Remessa = remessa;

            var objBOLETO2 = new Boleto();
            objBOLETO2.EspecieDocumento = new EspecieDocumento(33, "DS");
            objBOLETO2.DataVencimento = DateTime.ParseExact("12/12/2016", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            objBOLETO2.DataMulta = objBOLETO2.DataVencimento;
            objBOLETO2.ValorBoleto = 24;
            objBOLETO2.Carteira = "101";
            objBOLETO2.NossoNumero = ("160000000549");
            objBOLETO2.Cedente = objCedente;
            objBOLETO2.NumeroDocumento = "";
            objBOLETO2.DataDocumento = DateTime.Now;
            objBOLETO2.DataProcessamento = DateTime.ParseExact("02/12/2016", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            objBOLETO2.Sacado = new Sacado("29438268049", "JAIME BERGAMANN SCALO");
            objBOLETO2.Sacado.Endereco.End = "PROF. MILTON GUERREIRO 270 BLOCO B";
            objBOLETO2.Sacado.Endereco.Bairro = "SANTA TEREZA";
            objBOLETO2.Sacado.Endereco.Cidade = "PORTO ALEGRE";
            objBOLETO2.Sacado.Endereco.CEP = "90850350";
            objBOLETO2.Sacado.Endereco.UF = "RS";

            objBOLETO2.PercMulta = 2;
            objBOLETO2.JurosMora = 1;
            objBOLETO2.CodJurosMora = "2";
            objBOLETO2.NumeroDiasBaixa = 29;

            objBOLETO2.Remessa = remessa;

            Boletos objBOLETOS = new Boletos();
            objBOLETOS.Add(objBOLETO);
            objBOLETOS.Add(objBOLETO2);

            var ms = new MemoryStream();

            FileStream file = new FileStream("d:\\ztemp\\cnab240Santander.txt", FileMode.Create, FileAccess.Write);

            var objREMESSA = new ArquivoRemessa(TipoArquivo.CNAB240);
            objREMESSA.GerarArquivoRemessa("101", new Banco(33), objCedente, objBOLETOS, file, numeroArquivoRemessa);
            //ms.WriteTo(file);
        }

    }
}
