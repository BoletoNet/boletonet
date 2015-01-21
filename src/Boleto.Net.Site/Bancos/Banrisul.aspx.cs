using System;
using BoletoNet;

public partial class Bancos_Banrisul : System.Web.UI.Page
{
    void Page_PreInit(object sender, EventArgs e)
    {
        if (IsPostBack)
            MasterPageFile = "~/MasterPrint.master";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime vencimento = DateTime.Now.AddDays(5);

        Cedente c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "1234", "5", "12345678", "9");

        c.Codigo = "00000000504";

        Boleto b = new Boleto(vencimento, 45.50m, "18", "12345678901", c);

        b.Sacado = new Sacado("000.000.000-00", "Fulano de Silva");
        b.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23";
        b.Sacado.Endereco.Bairro = "Testando";
        b.Sacado.Endereco.Cidade = "Testelândia";
        b.Sacado.Endereco.CEP = "70000000";
        b.Sacado.Endereco.UF = "DF";

        //Adiciona as instruções ao boleto
        #region Instruções
        //Protestar
        Instrucao_Banrisul item = new Instrucao_Banrisul(9, 10, 0);
        b.Instrucoes.Add(item);
        #endregion Instruções

        b.NumeroDocumento = "12345678901";

        boletoBancario.Boleto = b;
        boletoBancario.Boleto.Valida();

        boletoBancario.MostrarComprovanteEntrega = (Request.Url.Query == "?show");
    }
}
