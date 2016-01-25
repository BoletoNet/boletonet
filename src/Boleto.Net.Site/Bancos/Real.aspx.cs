using System;
using BoletoNet;

public partial class Bancos_Real : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime vencimento = DateTime.Now.AddDays(5); 

        Cedente c = new Cedente("00.000.000/0000-00", "Coloque a Raz�o Social da sua empresa aqui", "1234", "12345");
        c.Codigo = "12345";

        Boleto b = new Boleto(vencimento, 0.1m, "57", "123456", c, new EspecieDocumento(356, "9"));
        b.NumeroDocumento = "1234567";

        b.Sacado = new Sacado("000.000.000-00", "Nome do seu Cliente ");
        b.Sacado.Endereco.End = "Endere�o do seu Cliente ";
        b.Sacado.Endereco.Bairro = "Bairro";
        b.Sacado.Endereco.Cidade = "Cidade";
        b.Sacado.Endereco.CEP = "00000000";
        b.Sacado.Endereco.UF = "UF";

        //b.Instrucoes.Add("N�o Receber ap�s o vencimento");
        //b.Instrucoes.Add("Ap�s o Vencimento pague somente no Real");
        //b.Instrucoes.Add("Instru��o 2");
        //b.Instrucoes.Add("Instru��o 3");
        real.Boleto = b;

        EspeciesDocumento ed = EspecieDocumento_Real.CarregaTodas();

        real.Boleto.Valida();

        real.MostrarComprovanteEntrega = (Request.Url.Query == "?show");
    }
}
