using System;
using BoletoNet;

public partial class Bancos_Unicred : System.Web.UI.Page
{
    void Page_PreInit(object sender, EventArgs e)
    {
        if (IsPostBack)
            MasterPageFile = "~/MasterPrint.master";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime vencimento = DateTime.Now.AddDays(5); 

        Instrucao_Unicred item1 = new Instrucao_Unicred(9, 5);
        Instrucao_Unicred item2 = new Instrucao_Unicred();

        Cedente c = new Cedente("11.224.046/0001-00", "IDEV WEB SOLUÇÕES", "0165", "", "000000623","2");
        c.Codigo = "13000";
        c.Endereco = new Endereco {
            Bairro = "Norte", CEP = "72548-810", Cidade = "Brasília", Complemento = "Casa", Numero = "12", Email = "contato@idevweb.com.br", End = "Qr 518 Conjunto J", UF = "DF"
        };



        Boleto b = new Boleto(vencimento, 0.1m, "21", "07200004", c);
        b.NumeroDocumento = "0000299621";
        b.NossoNumero = "0000299621";

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

        b.EspecieDocumento = new EspecieDocumento_Unicred("DM");
        b.Aceite = "S";
        b.ValorBoleto = 150.35m;
        boletoBancario.Boleto = b;
        boletoBancario.MostrarEnderecoCedente = true;
        boletoBancario.Boleto.Valida();

        boletoBancario.MostrarComprovanteEntrega = (Request.Url.Query == "?show");
        boletoBancario.FormatoCarne = (Request.Url.Query == "?formatocarne");
    }
}
