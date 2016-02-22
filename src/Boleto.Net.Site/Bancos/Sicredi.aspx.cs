using System;
using BoletoNet;

public partial class Bancos_Sicredi : System.Web.UI.Page
{
    void Page_PreInit(object sender, EventArgs e)
    {
        if (IsPostBack)
            MasterPageFile = "~/MasterPrint.master";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime vencimento = DateTime.Now.AddDays(5); 

        Instrucao_Sicredi item1 = new Instrucao_Sicredi(9, 5);
        Instrucao_Sicredi item2 = new Instrucao_Sicredi();

        Cedente c = new Cedente("10.823.650/0001-90", "SAFIRALIFE", "0165", "9", "00623","2","02");
        c.Codigo = "13000";

        Boleto b = new Boleto(vencimento, 0.1m, "112", "07200004", c);
        b.NumeroDocumento = "00000001";

        b.Sacado = new Sacado("000.000.000-00", "Nome do seu Cliente ");
        b.Sacado.Endereco.End = "Endereço do seu Cliente ";
        b.Sacado.Endereco.Bairro = "Bairro";
        b.Sacado.Endereco.Cidade = "Cidade";
        b.Sacado.Endereco.CEP = "00000000";
        b.Sacado.Endereco.UF = "UF";

        // Exemplo de como adicionar mais informações ao sacado
        b.Sacado.InformacoesSacado.Add(new InfoSacado("TÍTULO: " + "2541245"));

        item2.Descricao += " " + item1.QuantidadeDias.ToString() + " dias corridos do vencimento.";
        b.Instrucoes.Add(item1);

        b.EspecieDocumento = new EspecieDocumento_Sicredi("A");
        b.Aceite = "S";
        b.ValorBoleto = 150.35m;
        boletoBancario.Boleto = b;
        boletoBancario.Boleto.Valida();

        boletoBancario.MostrarComprovanteEntrega = (Request.Url.Query == "?show");
        boletoBancario.FormatoCarne = (Request.Url.Query == "?formatocarne");
    }
}
