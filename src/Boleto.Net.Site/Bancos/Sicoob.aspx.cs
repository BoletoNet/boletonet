using System;
using BoletoNet;

public partial class Bancos_Sicoob : System.Web.UI.Page
{
    void Page_PreInit(object sender, EventArgs e)
    {
        if (IsPostBack)
            MasterPageFile = "~/MasterPrint.master";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime vencimento = DateTime.Now.AddDays(5);

       
        Cedente c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "4444", "", "", "");

        c.Codigo = "123456"; 
        c.DigitoCedente = 7;
        c.Carteira = "1";

        Boleto b = new Boleto(vencimento, 10, "1", "897654321", c);
        b.NumeroDocumento = "119964";

        b.Sacado = new Sacado("000.000.000-00", "Nome do seu Cliente ");
        b.Sacado.Endereco.End = "Endereço do seu Cliente ";
        b.Sacado.Endereco.Bairro = "Bairro";
        b.Sacado.Endereco.Cidade = "Cidade";
        b.Sacado.Endereco.CEP = "00000000";
        b.Sacado.Endereco.UF = "UF";

  
        boletoBancario.Boleto = b;
        boletoBancario.Boleto.Valida();

        boletoBancario.MostrarComprovanteEntrega = (Request.Url.Query == "?show");
    }
}
