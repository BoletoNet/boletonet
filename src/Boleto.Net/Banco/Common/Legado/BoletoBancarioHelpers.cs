using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;

namespace BoletoNet
{
    public static class BoletoBancarioHelpers
    {
        /// <summary>
        /// [Deprecated] Atalho para a chamada Boleto.Montar() na nova API
        /// <author>Olavo Rocha Neto</author>
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string MontaHtml(this BoletoBancario b)
        {
            return b.Boleto.Montar();
        }

        /// <summary>
        /// Função utilizada gerar o AlternateView necessário para enviar um boleto bancário por e-mail.
        /// </summary>
        /// <returns>AlternateView com os dados do boleto.</returns>
        public static AlternateView HtmlBoletoParaEnvioEmail(this BoletoBancario b)
        {
            return BoletoBancarioHelpers.HtmlBoletoParaEnvioEmail(b,null);
        }


        /// <summary>
        /// Função utilizada gerar o AlternateView necessário para enviar um boleto bancário por e-mail.
        /// </summary>
        /// <param name="textoNoComecoDoEmail">Texto (em HTML) a ser incluido no começo do Email.</param>
        /// <returns>AlternateView com os dados do boleto.</returns>
        public static AlternateView HtmlBoletoParaEnvioEmail(this BoletoBancario b, string textoNoComecoDoEmail)
        {
            LinkedResource lrImagemLogo;
            LinkedResource lrImagemBarra;
            LinkedResource lrImagemCodigoBarra;

            b.GeraGraficosParaEmailOffLine( out lrImagemLogo, out lrImagemBarra, out lrImagemCodigoBarra);
            StringBuilder html = b.HtmlOffLine(textoNoComecoDoEmail, "cid:" + lrImagemLogo.ContentId, "cid:" + lrImagemBarra.ContentId, "cid:" + lrImagemCodigoBarra.ContentId);

            AlternateView av = AlternateView.CreateAlternateViewFromString(html.ToString(), Encoding.Default, "text/html");

            av.LinkedResources.Add(lrImagemLogo);
            av.LinkedResources.Add(lrImagemBarra);
            av.LinkedResources.Add(lrImagemCodigoBarra);
            return av;
        }

        public static StringBuilder HtmlOffLine(this BoletoBancario b, string textoNoComecoDoEmail, string srcLogo, string srcBarra, string srcCodigoBarra, bool usaCSSPDF = false)
        {//protected StringBuilder HtmlOffLine(string srcCorte, string srcLogo, string srcBarra, string srcPonto, string srcBarraInterna, string srcCodigoBarra)
            //this.OnLoad(EventArgs.Empty);

            StringBuilder html = new StringBuilder();
            BoletoBancario.HtmlOfflineHeader(html, usaCSSPDF);
            if (textoNoComecoDoEmail != null && textoNoComecoDoEmail != "")
            {
                html.Append(textoNoComecoDoEmail);
            }

            if (srcLogo != null)
            {
                b.Boleto.Cedente.Logo = srcLogo;
            }

            html.Append(b.Boleto.Montar());
            BoletoBancario.HtmlOfflineFooter(html);
            return html;
        }

        /// <summary>
        /// Gera as tres imagens necessárias para o Boleto
        /// </summary>
        /// <param name="lrImagemLogo">O Logo do Banco</param>
        /// <param name="lrImagemBarra">A Barra Horizontal</param>
        /// <param name="lrImagemCodigoBarra">O Código de Barras</param>
        static void GeraGraficosParaEmailOffLine(this BoletoBancario b, out LinkedResource lrImagemLogo, out LinkedResource lrImagemBarra, out LinkedResource lrImagemCodigoBarra)
        {
            var randomSufix = new Random().Next().ToString(); // para podermos colocar no mesmo email varios boletos diferentes

            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("BoletoNet.Imagens." + Utils.FormatCode(b.Boleto.Banco.Codigo.ToString(), 3) + ".jpg");
            lrImagemLogo = new LinkedResource(stream, MediaTypeNames.Image.Jpeg);
            lrImagemLogo.ContentId = "logo" + randomSufix;


            MemoryStream ms = new MemoryStream(Utils.ConvertImageToByte(Html.barra));
            lrImagemBarra = new LinkedResource(ms, MediaTypeNames.Image.Gif);
            lrImagemBarra.ContentId = "barra" + randomSufix; ;

            C2of5i cb = new C2of5i(b.Boleto.CodigoBarra.Codigo, 1, 50, b.Boleto.CodigoBarra.Codigo.Length);
            ms = new MemoryStream(Utils.ConvertImageToByte(cb.ToBitmap()));

            lrImagemCodigoBarra = new LinkedResource(ms, MediaTypeNames.Image.Gif);
            lrImagemCodigoBarra.ContentId = "codigobarra" + randomSufix; ;

        }

        /// <summary>
        /// Junta varios boletos em uma unica AlternateView, para todos serem mandados juntos no mesmo email
        /// </summary>
        /// <param name="arrayDeBoletos">Array contendo os boletos a serem mesclados</param>
        /// <returns></returns>
        public static AlternateView GeraHtmlDeVariosBoletosParaEmail(this BoletoBancario b, BoletoBancario[] arrayDeBoletos)
        {
            return GeraHtmlDeVariosBoletosParaEmail(null, arrayDeBoletos);
        }

        /// <summary>
        /// Junta varios boletos em uma unica AlternateView, para todos serem mandados juntos no mesmo email
        /// </summary>
        /// <param name="textoNoComecoDoEmail">Texto em HTML a ser adicionado no comeco do email</param>
        /// <param name="arrayDeBoletos">Array contendo os boletos a serem mesclados</param>
        /// <returns>AlternateView com os dados de todos os boleto.</returns>
        public static AlternateView GeraHtmlDeVariosBoletosParaEmail(this BoletoBancario b, string textoNoComecoDoEmail, BoletoBancario[] arrayDeBoletos)
        {
            var corpoDoEmail = new StringBuilder();

            var linkedResources = new List<LinkedResource>();
            BoletoBancario.HtmlOfflineHeader(corpoDoEmail);
            if (textoNoComecoDoEmail != null && textoNoComecoDoEmail != "")
            {
                corpoDoEmail.Append(textoNoComecoDoEmail);
            }
            foreach (var umBoleto in arrayDeBoletos)
            {
                if (umBoleto != null)
                {
                    LinkedResource lrImagemLogo;
                    LinkedResource lrImagemBarra;
                    LinkedResource lrImagemCodigoBarra;
                    umBoleto.GeraGraficosParaEmailOffLine(out lrImagemLogo, out lrImagemBarra, out lrImagemCodigoBarra);
                    //var theOutput = umBoleto.MontaHtml(
                    //    "cid:" + lrImagemLogo.ContentId,
                    //    "cid:" + lrImagemBarra.ContentId,
                    //    "<img src=\"cid:" + lrImagemCodigoBarra.ContentId + "\" alt=\"Código de Barras\" />");
                    var theOutput = umBoleto.Boleto.Montar();

                    corpoDoEmail.Append(theOutput);

                    linkedResources.Add(lrImagemLogo);
                    linkedResources.Add(lrImagemBarra);
                    linkedResources.Add(lrImagemCodigoBarra);
                }
            }
            BoletoBancario.HtmlOfflineFooter(corpoDoEmail);



            AlternateView av = AlternateView.CreateAlternateViewFromString(corpoDoEmail.ToString(), Encoding.Default, "text/html");
            foreach (var theResource in linkedResources)
            {
                av.LinkedResources.Add(theResource);
            }



            return av;
        }
    }

    /// <summary>
    /// Parte da classe Boleto Bancário detinada a guardar os legados para o menor impacto possível nas versões.
    /// </summary>
    public partial class BoletoBancario
    {
        /// <summary>
        /// [deprecated] Definne se o endereço do cedente será ou não exibido na renderização do Boleto
        /// </summary>
        /// <remarks>
        /// Usar Boleto.Opcoes.MostrarEnderecoCedente
        /// </remarks>
        public bool MostrarEnderecoCedente {
            get
            {
                return this.Boleto.Opcoes.MostrarEnderecoCedente;
            }
            set
            {
                if (this.Boleto != null)
                    if (this.Boleto.Opcoes != null)
                        this.Boleto.Opcoes.MostrarEnderecoCedente = value;
            }
        }
    }

    
}
