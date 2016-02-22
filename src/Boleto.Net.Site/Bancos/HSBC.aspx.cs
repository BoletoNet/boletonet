using System;
using BoletoNet;

public partial class Bancos_HSBC : System.Web.UI.Page
{
    void Page_PreInit(object sender, EventArgs e)
    {
        if (IsPostBack)
            MasterPageFile = "~/MasterPrint.master";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime vencimento = DateTime.Now.AddDays(5);

        Cedente c = new Cedente("00.000.000/0000-00", "Minha empresa", "0000", "", "00000", "00");
        // Código fornecido pela agencia, NÃO é o numero da conta
        c.Codigo = "0000000"; // 7 posicoes

        Boleto b = new Boleto(vencimento, 2, "CNR", "888888888", c); //cod documento
        b.NumeroDocumento = "9999999999999"; // nr documento

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
