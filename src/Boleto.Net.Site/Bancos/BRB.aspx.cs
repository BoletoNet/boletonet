using System;
using BoletoNet;

public partial class Bancos_BRB : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime vencimento = DateTime.Now.AddDays(5);

        Cedente c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "208", "", "010357", "6");
        c.Codigo = "13000";

        Boleto b = new Boleto(vencimento, 0.01m, "COB", "119964", c);
        b.NumeroDocumento = "119964";

        b.Sacado = new Sacado("000.000.000-00", "Nome do seu Cliente ");
        b.Sacado.Endereco.End = "Endere�o do seu Cliente ";
        b.Sacado.Endereco.Bairro = "Bairro";
        b.Sacado.Endereco.Cidade = "Cidade";
        b.Sacado.Endereco.CEP = "00000000";
        b.Sacado.Endereco.UF = "UF";     

        //b.Instrucoes.Add("N�o Receber ap�s o vencimento");
        //b.Instrucoes.Add("Ap�s o Vencimento pague somente no Bradesco");
        //b.Instrucoes.Add("Instru��o 2");
        //b.Instrucoes.Add("Instru��o 3");

        boletoBancario.Boleto = b;
        boletoBancario.Boleto.Valida();

        boletoBancario.MostrarComprovanteEntrega = (Request.Url.Query == "?show");
    }
}
