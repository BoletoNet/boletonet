using System;
using System.IO;
using System.Net.Mail;
using System.Web.UI;
using BoletoNet;

public partial class EnvioEmail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected MailMessage PreparaMail()
    {
        MailMessage mail = new MailMessage();
        mail.To.Add(new MailAddress(TextBox1.Text));
        mail.Subject = "Teste de envio de Boleto Bancário";
        mail.IsBodyHtml = true;
        mail.Priority = MailPriority.High;
        return mail;
    }

    protected BoletoBancario PreparaBoleto()
    {
        DateTime vencimento = new DateTime(2007, 9, 10);

        Instrucao_Itau item1 = new Instrucao_Itau(9, 5);
        Instrucao_Itau item2 = new Instrucao_Itau(81, 10);
        Cedente c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "0542", "13000");
        //Na carteira 198 o código do Cedente é a conta bancária
        c.Codigo = "13000";

        Boleto b = new Boleto(vencimento, 1642, "198", "92082835", c);
        b.NumeroDocumento = "1008073";

        b.Sacado = new Sacado("000.000.000-00", "Fulano de Silva");
        b.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23";
        b.Sacado.Endereco.Bairro = "Testando";
        b.Sacado.Endereco.Cidade = "Testelândia";
        b.Sacado.Endereco.CEP = "70000000";
        b.Sacado.Endereco.UF = "DF";

        item2.Descricao += " " + item2.QuantidadeDias.ToString() + " dias corridos do vencimento.";
        b.Instrucoes.Add(item1);
        b.Instrucoes.Add(item2);



        BoletoBancario itau = new BoletoBancario();
        itau.CodigoBanco = 341;
        itau.Boleto = b;

        return itau;
    }


    protected void Button1_Click(object sender, EventArgs e)
    {


        BoletoBancario  itau = PreparaBoleto();
        MailMessage mail = PreparaMail();

        if (RadioButton1.Checked)
        {
            mail.Subject += " - On-Line";
            Panel1.Controls.Add(itau);

            System.IO.StringWriter sw = new System.IO.StringWriter();
            HtmlTextWriter htmlTW = new HtmlTextWriter(sw);
            Panel1.RenderControl(htmlTW);
            string html = sw.ToString();
            //
            mail.Body = html;
        }
        else
        {
            mail.Subject += " - Off-Line";
            mail.AlternateViews.Add(itau.HtmlBoletoParaEnvioEmail());
        }

        MandaEmail(mail);
        Label1.Text = "Boleto simples enviado para o email: " + TextBox1.Text;
    }
    protected void Button1_Click2(object sender, EventArgs e)
    {

        BoletoBancario itau = PreparaBoleto();

        // embora estou mandando o mesmo boleto duas vezes, voce pode obviamente mandar boletos distintos
        BoletoBancario[] arrayDeBoletos = new BoletoBancario[] { itau, itau };
        AlternateView  av = BoletoBancario.GeraHtmlDeVariosBoletosParaEmail("Isto é um email com <b>dois</b> boletos", arrayDeBoletos);

        MailMessage  mail = PreparaMail();
        mail.Subject += " - Off-Line - Múltiplo";
        mail.AlternateViews.Add(av);

        MandaEmail(mail);
        Label1.Text = "Boleto múltimplo enviado para o email: " + TextBox1.Text;
    }


    void MandaEmail(MailMessage mail)
    {
        SmtpClient objSmtpClient = new SmtpClient();

        objSmtpClient.Host = "smtp.dominio.com.br";
        objSmtpClient.Port = 25;
        objSmtpClient.EnableSsl = false;
        objSmtpClient.Credentials = new System.Net.NetworkCredential("stiven@callas.com.br", "123456");
        objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        objSmtpClient.Timeout = 10000;
        objSmtpClient.Send(mail);
    }
}
