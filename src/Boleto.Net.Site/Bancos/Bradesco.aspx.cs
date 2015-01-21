using System;
using BoletoNet;

public partial class Bancos_Bradesco : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime vencimento = DateTime.Now.AddDays(5);

        Instrucao_Bradesco item = new Instrucao_Bradesco(9, 5);

        Cedente c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "1234", "5", "123456", "7");
        c.Codigo = "13000";


        //Carteiras 
        Boleto b = new Boleto(vencimento, 1.01m, "09", "01000000001", c);
        b.NumeroDocumento = "01000000001";

        b.Sacado = new Sacado("000.000.000-00", "Nome do seu Cliente ");
        b.Sacado.Endereco.End = "Endereço do seu Cliente ";
        b.Sacado.Endereco.Bairro = "Bairro";
        b.Sacado.Endereco.Cidade = "Cidade";
        b.Sacado.Endereco.CEP = "00000000";
        b.Sacado.Endereco.UF = "UF";     

        item.Descricao += " após " + item.QuantidadeDias.ToString() + " dias corridos do vencimento.";
        b.Instrucoes.Add(item); //"Não Receber após o vencimento");

        Instrucao i = new Instrucao(237);
        i.Descricao = "Nova Instrução";
        b.Instrucoes.Add(i);

        /* 
         * A data de vencimento não é usada
         * Usado para mostrar no lugar da data de vencimento o termo "Contra Apresentação";
         * Usado na carteira 06
         */
        boletoBancario.MostrarContraApresentacaoNaDataVencimento = true;

        boletoBancario.Boleto = b;
        boletoBancario.Boleto.Valida();

        boletoBancario.MostrarComprovanteEntrega = (Request.Url.Query == "?show");
    }
}
