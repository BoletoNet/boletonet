using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;

namespace BoletoNet.Arquivo
{
    public partial class EnviarEmail : Form
    {
        private string _fileName;

        public EnviarEmail(string fileName)
        {
            InitializeComponent();

            _fileName = fileName;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(textBoxEmail.Text);
            mail.To.Add(new MailAddress(textBoxEmail.Text));
            mail.Subject = "Teste de envio de Boleto Bancário como imagem";
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            mail.Body = "Exemplo de envio de boleto por email como imagem";
            mail.Attachments.Add(new Attachment(_fileName));


            MandaEmail(mail);
           
        }

        void MandaEmail(MailMessage mail)
        {
            try
            {
                SmtpClient objSmtpClient = new SmtpClient();

                objSmtpClient.Host = textBoxSMTP.Text;
                objSmtpClient.Port = 25;
                objSmtpClient.EnableSsl = false;
                objSmtpClient.Credentials = new System.Net.NetworkCredential(textBoxEmail.Text, textBoxPassword.Text);
                objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                objSmtpClient.Timeout = 10000;
                objSmtpClient.Send(mail);

                MessageBox.Show("Boleto como imagem enviado com sucesso");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }
    }
}
