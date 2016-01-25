using System;
using BoletoNet;

public partial class Bancos_Sudameris : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime vencimento = DateTime.Now.AddDays(5);

        Cedente c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "0501", "6703255");

        c.Codigo = "13000";

        //Nosso n�mero com 7 d�gitos
        string nn = "0003020";
        //Nosso n�mero com 13 d�gitos
        //nn = "0000000003025";

        Boleto b = new Boleto(vencimento, 1642, "198", nn, c);// EnumEspecieDocumento_Sudameris.DuplicataMercantil);
        b.NumeroDocumento = "1008073";

        b.Sacado = new Sacado("000.000.000-00", "Nome do seu Cliente ");
        b.Sacado.Endereco.End = "Endere�o do seu Cliente ";
        b.Sacado.Endereco.Bairro = "Bairro";
        b.Sacado.Endereco.Cidade = "Cidade";
        b.Sacado.Endereco.CEP = "00000000";
        b.Sacado.Endereco.UF = "UF";

        //b.Instrucoes.Add("N�o Receber ap�s o vencimento");
        //b.Instrucoes.Add("Ap�s o Vencimento pague somente no Sudameris");
        //b.Instrucoes.Add("Instru��o 2");
        //b.Instrucoes.Add("Instru��o 3");

        boletoBancario.Boleto = b;
        boletoBancario.Boleto.Valida();

        boletoBancario.MostrarComprovanteEntrega = (Request.Url.Query == "?show");
    }
}
