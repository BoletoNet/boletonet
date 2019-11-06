using System;
using System.Drawing;
using System.IO;
using BoletoNet;

public partial class Bancos_Itau : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime vencimento = DateTime.Now.AddDays(1);

        Instrucao_Itau item1 = new Instrucao_Itau(9, 5);
        Instrucao_Itau item2 = new Instrucao_Itau(81, 10);
        Cedente c = new Cedente("10.823.650/0001-90", "SAFIRALIFE", "4406", "22324");
        //Na carteira 198 o código do Cedente é a conta bancária
        c.Codigo = "13000";

        //Mostrar endereço do Cedente
        //c.Endereco = new Endereco();
        //c.Endereco.End = "SQN 416 Bloco M Ap 132";
        //c.Endereco.Bairro = "Asa Norte";
        //c.Endereco.CEP = "70879110";
        //c.Endereco.Cidade = "Brasília";
        //c.Endereco.UF = "DF";

        //boletoBancario.MostrarEnderecoCedente = true;

        Boleto b = new Boleto(vencimento, 0.1m, "176", "00000001", c, new EspecieDocumento(341, "1"));
        b.NumeroDocumento = "00000001";

        b.Sacado = new Sacado("000.000.000-00", "Nome do seu Cliente ");
        b.Sacado.Endereco.End = "Endereço do seu Cliente ";
        b.Sacado.Endereco.Bairro = "Bairro";
        b.Sacado.Endereco.Cidade = "Cidade";
        b.Sacado.Endereco.CEP = "00000000";
        b.Sacado.Endereco.UF = "UF";

        // Exemplo de como adicionar mais informações ao sacado
        b.Sacado.InformacoesSacado.Add(new InfoSacado("TÍTULO: " + "2541245"));

        item2.Descricao += " " + item2.QuantidadeDias.ToString() + " dias corridos do vencimento.";
        b.Instrucoes.Add(item1);
        b.Instrucoes.Add(item2);

        // juros/descontos

        if (b.ValorDesconto == 0)
        {
            Instrucao_Itau item3 = new Instrucao_Itau(999, 1);
            item3.Descricao += ("1,00 por dia de antecipação.");
            b.Instrucoes.Add(item3);
        }


        boletoBancario.Boleto = b;
        boletoBancario.Boleto.Valida();

        //boletoBancario.MostrarCodigoCarteira = true;
        boletoBancario.MostrarComprovanteEntrega = (Request.Url.Query == "?show");
        boletoBancario.FormatoCarne = (Request.Url.Query == "?formatocarne");
        boletoBancario.OcultarInstrucoes = true;
        boletoBancario.FormatoPropaganda = (Request.Url.Query == "?formatopropaganda");
        boletoBancario.ImagemPropaganda = GetBase64StringForImage(Server.MapPath("/prop.jpg"));

        var bytes = boletoBancario.MontaBytesPDF();
        Response.Clear();
        Response.ContentType = "application/pdf";
        Response.AddHeader("Content-Disposition", "attachment;filename=\"FileName.pdf\"");
        Response.BinaryWrite(bytes);
        Response.Flush();
        Response.End();
    }

    protected static string GetBase64StringForImage(string imgPath)
    {
        byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
        string base64String = Convert.ToBase64String(imageBytes);
        return base64String;
    }
}
