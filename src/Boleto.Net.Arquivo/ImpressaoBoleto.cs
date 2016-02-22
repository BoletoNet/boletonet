using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;
using System.IO;

namespace BoletoNet.Arquivo
{
    public partial class ImpressaoBoleto : Form
    {
        public ImpressaoBoleto()
        {
            InitializeComponent();
        }

        private void visualizarImagemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormVisualizarImagem form = new FormVisualizarImagem(GerarImagem());
            form.ShowDialog();
            
        }

        private string GerarImagem()
        {
            string address = webBrowser.Url.ToString();
            int width = 670;
            int height = 805;

            int webBrowserWidth = 670;
            int webBrowserHeight = 805;

            Bitmap bmp = WebsiteThumbnailImageGenerator.GetWebSiteThumbnail(address, webBrowserWidth, webBrowserHeight, width, height);

            string file = Path.Combine(Environment.CurrentDirectory, "boleto.bmp");

            bmp.Save(file);

            return file;
        }

        private void enviarImagemPorEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EnviarEmail form = new EnviarEmail(GerarImagem());
            form.ShowDialog();
        }
    }
}