using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using BoletoNet.Util;
using System.Net.Mail;
using System.Net.Mime;

namespace BoletoNet
{

    /// <summary>
    /// Classe responsável por agregar todos os elementos necessários para produção de um boleto bancário
    /// </summary>
    /// <remarks>
    /// Nenhum código de Banco específico deve ser incluído nesta classe. Caso haja a necessidade de incluir um ponto de customização, procure implementar o recurso pela interface IBanco
    /// Atualizada por Olavo Rocha Neto  olavo at exodus.eti.br
    /// </remarks>
    public class BoletoBancario
    {
        String vLocalLogoCedente = String.Empty;

        #region Variaveis

        private Banco _ibanco = null;
        private short _codigoBanco = 0;
        private Boleto _boleto;
        private Cedente _cedente;
        private Sacado _sacado;
        private List<IInstrucao> _instrucoes = new List<IInstrucao>();
        private string _instrucoesHtml = string.Empty;
        private bool _mostrarCodigoCarteira = false;
        private bool _formatoCarne = false;
        #endregion Variaveis

        #region Propriedades

        /// <summary>
        /// "Código do banco em que será gerado o boleto. Ex. 341-Itaú, 237-Bradesco"
        /// </summary>
        public short CodigoBanco
        {
            get { return _codigoBanco; }
            set { _codigoBanco = value; }
        }

        /// <summary>
        /// Mostra o código da carteira
        /// </summary>
        public bool MostrarCodigoCarteira
        {
            get { return _mostrarCodigoCarteira; }
            set { _mostrarCodigoCarteira = value; }
        }

        public bool ExibirDemonstrativo { get; set; }

        /// <summary>
        /// Mostra o código da carteira
        /// </summary>
        public bool FormatoCarne
        {
            get { return _formatoCarne; }
            set { _formatoCarne = value; }
        }

        public Boleto Boleto
        {
            get { return _boleto; }
            set
            {
                _boleto = value;

                if (_ibanco == null)
                {
                    _boleto.Banco = this.Banco;
                    _boleto.BancoCarteira = BancoCarteiraFactory.Fabrica(_boleto.Carteira, Banco.Codigo);
                }

                _cedente = _boleto.Cedente;
                _sacado = _boleto.Sacado;
            }
        }

        public Sacado Sacado
        {
            get { return _sacado; }
        }

        public Cedente Cedente
        {
            get { return _cedente; }
        }

        public Banco Banco
        {
            get
            {
                if ((_ibanco == null) ||
                    (_ibanco.Codigo != _codigoBanco))
                {
                    _ibanco = new Banco(_codigoBanco);
                }

                if (_boleto != null)
                    _boleto.Banco = _ibanco;

                return _ibanco;
            }
        }

        #region Propriedades
        public bool MostrarComprovanteEntregaLivre { get; set; }

        public bool MostrarComprovanteEntrega { get; set; }

        public bool OcultarEnderecoSacado { get; set; }

        public bool OcultarInstrucoes { get; set; }

        public bool OcultarReciboSacado { get; set; }

        public bool GerarArquivoRemessa { get; set; }

        /// <summary> 
        /// Mostra o termo "Contra Apresentação" na data de vencimento do boleto
        /// </summary>
        public bool MostrarContraApresentacaoNaDataVencimento { get; set; }

        public bool MostrarEnderecoCedente { get; set; }

        #endregion Propriedades

        /// <summary> 
        /// Instruções disponíveis no Boleto
        /// </summary>
        public List<IInstrucao> Instrucoes
        {
            get
            {
                return _instrucoes;
            }
        }

        #endregion Propriedades

        //public static string UrlLogo(int banco)
        //{
        //    var page = System.Web.HttpContext.Current.CurrentHandler as Page;
        //    return page.ClientScript.GetWebResourceUrl(typeof(BoletoBancario), "BoletoNet.Imagens." + Utils.FormatCode(banco.ToString(), 3) + ".jpg");
        //}

        #region Html
        public string GeraHtmlInstrucoes()
        {
            try
            {
                var html = new StringBuilder();

                string titulo = "Instruções de Impressão";
                string instrucoes = "Imprimir em impressora jato de tinta (ink jet) ou laser em qualidade normal. (Não use modo econômico).<br>Utilize folha A4 (210 x 297 mm) ou Carta (216 x 279 mm) - Corte na linha indicada<br>";

                html.Append(Html.Instrucoes);
                html.Append("<br />");

                return html.ToString()
                    .Replace("@TITULO", titulo)
                    .Replace("@INSTRUCAO", instrucoes);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a execução da transação.", ex);
            }
        }

        private string GeraHtmlCarne(string telefone, string htmlBoleto)
        {
            var html = new StringBuilder();

            html.Append(Html.Carne);

            return html.ToString()
                .Replace("@TELEFONE", telefone)
                .Replace("#BOLETO#", htmlBoleto);
        }

        public string GeraHtmlReciboSacado()
        {
            try
            {
                var html = new StringBuilder();

                html.Append(Html.ReciboSacadoParte1);
                html.Append("<br />");
                html.Append(Html.ReciboSacadoParte2);
                html.Append(Html.ReciboSacadoParte3);

                if (MostrarEnderecoCedente)
                {
                    html.Append(Html.ReciboSacadoParte10);
                }

                html.Append(Html.ReciboSacadoParte4);
                html.Append(Html.ReciboSacadoParte5);
                html.Append(Html.ReciboSacadoParte6);
                html.Append(Html.ReciboSacadoParte7);

                //if (Instrucoes.Count == 0)
                html.Append(Html.ReciboSacadoParte8);

                //Limpa as intruções para o Sacado
                _instrucoesHtml = "";

                MontaInstrucoes(Boleto.Instrucoes);

                if (Boleto.Sacado.Instrucoes.Count > 0)
                    MontaInstrucoes(Boleto.Sacado.Instrucoes);

                return html.ToString().Replace("@INSTRUCOES", _instrucoesHtml);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a execução da transação.", ex);
            }
        }

        public string GerarHtmlReciboCedente()
        {
            try
            {
                var html = new StringBuilder();

                html.Append(Html.ReciboCedenteParte1);
                html.Append(Html.ReciboCedenteParte2);
                html.Append(Html.ReciboCedenteParte3);
                html.Append(Html.ReciboCedenteParte4);
                html.Append(Html.ReciboCedenteParte5);
                html.Append(Html.ReciboCedenteParte6);
                html.Append(Html.ReciboCedenteParte7);
                html.Append(Html.ReciboCedenteParte8);
                html.Append(Html.ReciboCedenteParte9);
                html.Append(Html.ReciboCedenteParte10);
                html.Append(Html.ReciboCedenteParte11);
                html.Append(Html.ReciboCedenteParte12);

                Boleto.Banco.OnGeraHtmlReciboCedente(html, Boleto);

                //Limpa as intruções para o Cedente
                _instrucoesHtml = "";

                MontaInstrucoes(Boleto.Instrucoes);

                if (Boleto.Cedente.Instrucoes.Count > 0)
                    MontaInstrucoes(Boleto.Cedente.Instrucoes);

                return html.ToString().Replace("@INSTRUCOES", _instrucoesHtml);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro na execução da transação.", ex);
            }
        }

        public string HtmlComprovanteEntrega
        {
            get
            {
                var html = new StringBuilder();

                html.Append(Html.ComprovanteEntrega1);
                html.Append(Html.ComprovanteEntrega2);
                html.Append(Html.ComprovanteEntrega3);
                html.Append(Html.ComprovanteEntrega4);
                html.Append(Html.ComprovanteEntrega5);
                html.Append(Html.ComprovanteEntrega6);

                html.Append(MostrarComprovanteEntregaLivre ? Html.ComprovanteEntrega71 : Html.ComprovanteEntrega7);

                html.Append("<br />");
                return html.ToString();
            }
        }

        private void MontaInstrucoes(IList<IInstrucao> instrucoes)
        {
            if (!string.IsNullOrEmpty(_instrucoesHtml))
                _instrucoesHtml = string.Concat(_instrucoesHtml, "<br />");

            if (instrucoes.Count > 0)
            {
                //_instrucoesHtml = string.Empty;
                //Flavio(fhlviana@hotmail.com) - retirei a tag <span> de cada instrução por não ser mais necessáras desde que dentro
                //da div que contem as instruções a classe cpN se aplica, que é a mesma, em conteudo, da classe cp
                foreach (IInstrucao instrucao in instrucoes)
                {
                    _instrucoesHtml += string.Format("{0}<br />", instrucao.Descricao);

                    //Adiciona a instrução as instruções disponíveis no Boleto
                    Instrucoes.Add(instrucao);
                }

                _instrucoesHtml = Strings.Left(_instrucoesHtml, _instrucoesHtml.Length - 6);
            }
        }

        private string MontaHtml(string urlImagemLogo, string urlImagemBarra, string imagemCodigoBarras)
        {
            var html = new StringBuilder();
            var parametrosBoleto = new BoletoBancario.BoletoRenderParams(this, urlImagemLogo, urlImagemBarra, imagemCodigoBarras);

            //Oculta o cabeçalho das instruções do boleto
            if (!OcultarInstrucoes)
                html.Append(GeraHtmlInstrucoes());

            if (this.ExibirDemonstrativo && this.Boleto.Demonstrativos.Any())
            {
                html.Append(Html.ReciboCedenteRelatorioValores);
                html.Append(Html.ReciboCedenteParte5);

                html.Append(Html.CabecalhoTabelaDemonstrativo);

                var grupoDemonstrativo = new StringBuilder();

                foreach (var relatorio in this.Boleto.Demonstrativos)
                {
                    var first = true;

                    foreach (var item in relatorio.Itens)
                    {
                        grupoDemonstrativo.Append(Html.GrupoDemonstrativo);

                        if (first)
                        {
                            grupoDemonstrativo = grupoDemonstrativo.Replace("@DESCRICAOGRUPO", relatorio.Descricao);

                            first = false;
                        }
                        else
                        {
                            grupoDemonstrativo = grupoDemonstrativo.Replace("@DESCRICAOGRUPO", string.Empty);
                        }

                        grupoDemonstrativo = grupoDemonstrativo.Replace("@DESCRICAOITEM", item.Descricao);
                        grupoDemonstrativo = grupoDemonstrativo.Replace("@REFERENCIAITEM", item.Referencia);
                        grupoDemonstrativo = grupoDemonstrativo.Replace("@VALORITEM", item.Valor.ToString("R$ ##,##0.00"));
                    }

                    grupoDemonstrativo.Append(Html.TotalDemonstrativo);
                    grupoDemonstrativo = grupoDemonstrativo.Replace(
                        "@VALORTOTALGRUPO",
                        relatorio.Itens.Sum(c => c.Valor).ToString("R$ ##,##0.00"));
                }

                html = html.Replace("@ITENSDEMONSTRATIVO", grupoDemonstrativo.ToString());
            }

            if (!FormatoCarne)
            {
                //Mostra o comprovante de entrega
                if (MostrarComprovanteEntrega | MostrarComprovanteEntregaLivre)
                {
                    html.Append(HtmlComprovanteEntrega);
                    //Html da linha pontilhada
                    if (OcultarReciboSacado)
                        html.Append(Html.ReciboSacadoParte8);
                }

                //Oculta o recibo do sacabo do boleto
                if (!OcultarReciboSacado)
                {
                    html.Append(GeraHtmlReciboSacado());

                    //Caso mostre o Endereço do Cedente
                    if (parametrosBoleto.MostrarEnderecoCedente)
                    {
                        if (Cedente.Endereco == null)
                            throw new ArgumentNullException("Endereço do Cedente");

                        string numero = !String.IsNullOrEmpty(Cedente.Endereco.Numero) ? Cedente.Endereco.Numero + ", " : "";
                        parametrosBoleto.EnderecoCedente = string.Concat(Cedente.Endereco.End, " , ", numero);

                        if (Cedente.Endereco.CEP == String.Empty)
                        {
                            parametrosBoleto.EnderecoCedente += string.Format("{0} - {1}/{2}", Cedente.Endereco.Bairro,
                                                             Cedente.Endereco.Cidade, Cedente.Endereco.UF);
                        }
                        else
                        {
                            parametrosBoleto.EnderecoCedente += string.Format("{0} - {1}/{2} - CEP: {3}", Cedente.Endereco.Bairro,
                                                             Cedente.Endereco.Cidade, Cedente.Endereco.UF,
                                                             Utils.FormataCEP(Cedente.Endereco.CEP));
                        }

                    }
                }
            }

            

            if (!FormatoCarne)
                html.Append(GerarHtmlReciboCedente());
            else
            {
                html.Append(GeraHtmlCarne("", GerarHtmlReciboCedente()));
            }

            if (String.IsNullOrEmpty(vLocalLogoCedente))
                parametrosBoleto.LocalLogoCliente = parametrosBoleto.UrlImagemLogo;
            
            return Boleto.Banco.RenderizaBoleto(html, parametrosBoleto).ToString();

        }

        public class BoletoRenderParams
        {
            public BoletoRenderParams(BoletoBancario bb, string urlImagemLogo , string urlImagemBarra,string imagemCodigoBarras)
            {
                this.BoletoBancario = bb;
                this.UrlImagemLogo  = urlImagemLogo ;
                this.UrlImagemBarra = urlImagemBarra;
                this.ImagemCodigoBarras = imagemCodigoBarras;
            }

            internal BoletoBancario BoletoBancario
            {
                get; set;
            }

            internal bool MostrarCodigoCarteira { get { return this.BoletoBancario.MostrarCodigoCarteira; } }
            internal bool MostrarEnderecoCedente { get { return this.BoletoBancario.MostrarEnderecoCedente; } }

            public Boleto Boleto { get { return this.BoletoBancario.Boleto; } }

            public string LocalLogoCliente { get; set; }
            public string UrlImagemLogo { get; set; }
            public string UrlImagemBarra { get; internal set; }

            public string DataVencimento {
                get
                {
                    if (BoletoBancario.MostrarContraApresentacaoNaDataVencimento)
                        return "Contra Apresentação";
                    else
                        return Boleto.DataVencimento.ToString("dd/MM/yyyy");
                }
            }

            public string Sacado {
                get
                {
                    //Flavio(fhlviana@hotmail.com) - adicionei a possibilidade de o boleto não ter, necessáriamente, que informar o CPF ou CNPJ do sacado.
                    //Formata o CPF/CNPJ(se houver) e o Nome do Sacado para apresentação
                    if (Boleto.Sacado.CPFCNPJ == string.Empty)
                    {
                        return Boleto.Sacado.Nome;
                    }
                    else
                    {
                        if (Boleto.Sacado.CPFCNPJ.Length <= 11)
                            return string.Format("{0}  CPF: {1}", Boleto.Sacado.Nome, Utils.FormataCPF(Boleto.Sacado.CPFCNPJ));
                        else
                            return string.Format("{0}  CNPJ: {1}", Boleto.Sacado.Nome, Utils.FormataCNPJ(Boleto.Sacado.CPFCNPJ));
                    }
                }
            }

            public string InfoSacadoString {
                get
                {
                    var expSacado = "";
                    var Sacado = Boleto.Sacado;

                    //Caso não oculte o Endereço do Sacado,
                    if (!this.BoletoBancario.OcultarEnderecoSacado)
                    {
                        String enderecoSacado = "";
                        
                        if (Sacado.Endereco.CEP == String.Empty)
                            enderecoSacado = string.Format("{0} - {1}/{2}", Sacado.Endereco.Bairro, Sacado.Endereco.Cidade, Sacado.Endereco.UF);
                        else
                            enderecoSacado = string.Format("{0} - {1}/{2} - CEP: {3}", Sacado.Endereco.Bairro, Sacado.Endereco.Cidade, Sacado.Endereco.UF, Utils.FormataCEP(Sacado.Endereco.CEP));

                        if (Sacado.Endereco.End != string.Empty && enderecoSacado != string.Empty)
                        {
                            string Numero = !String.IsNullOrEmpty(Sacado.Endereco.Numero) ? ", " + Sacado.Endereco.Numero : "";

                            if (expSacado == string.Empty)
                                expSacado += InfoSacado.Render(Sacado.Endereco.End + Numero, enderecoSacado, false);
                            else
                                expSacado += InfoSacado.Render(Sacado.Endereco.End + Numero, enderecoSacado, true);
                        }
                        //"Informações do Sacado" foi introduzido para possibilitar que o boleto na informe somente o endereço do sacado
                        //como em outras situaçoes onde se imprime matriculas, codigos e etc, sobre o sacado.
                        //Sendo assim o endereço do sacado passa a ser uma Informaçao do Sacado que é adicionada no momento da renderização
                        //de acordo com a flag "OcultarEnderecoSacado"
                    }
                    return expSacado;

                }
            }
            public string ImagemCodigoBarras { get; internal set; }
            public string EnderecoCedente { get; internal set; }
        }
        

        #endregion Html

        #region Geração do Html OffLine

        /// <summary>
        /// Função utilizada para gerar o html do boleto sem que o mesmo esteja dentro de uma página Web.
        /// </summary>
        /// <param name="srcLogo">Local apontado pela imagem de logo.</param>
        /// <param name="srcBarra">Local apontado pela imagem de barra.</param>
        /// <param name="srcCodigoBarra">Local apontado pela imagem do código de barras.</param>
        /// <returns>StringBuilder conténdo o código html do boleto bancário.</returns>
        protected StringBuilder HtmlOffLine(string textoNoComecoDoEmail, string srcLogo, string srcBarra, string srcCodigoBarra, bool usaCSSPDF = false)
        {//protected StringBuilder HtmlOffLine(string srcCorte, string srcLogo, string srcBarra, string srcPonto, string srcBarraInterna, string srcCodigoBarra)
            //this.OnLoad(EventArgs.Empty);

            StringBuilder html = new StringBuilder();
            HtmlOfflineHeader(html, usaCSSPDF);
            if (textoNoComecoDoEmail != null && textoNoComecoDoEmail != "")
            {
                html.Append(textoNoComecoDoEmail);
            }
            html.Append(MontaHtml(srcLogo, srcBarra, "<img src=\"" + srcCodigoBarra + "\" alt=\"Código de Barras\" />"));
            HtmlOfflineFooter(html);
            return html;
        }




        /// <summary>
        /// Monta o Header de um email com pelo menos um boleto dentro.
        /// </summary>
        /// <param name="saida">StringBuilder onde o conteudo sera salvo.</param>
        protected static void HtmlOfflineHeader(StringBuilder html, bool usaCSSPDF = false)
        {
            html.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n");
            html.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">\n");
            html.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\n");
            html.Append("<meta charset=\"utf-8\"/>\n");
            html.Append("<head>");
            html.Append("    <title>Boleto.Net</title>\n");

            #region Css
            {
                string arquivoCSS = usaCSSPDF ? "BoletoNet.BoletoImpressao.BoletoNetPDF.css" : "BoletoNet.BoletoImpressao.BoletoNet.css";
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(arquivoCSS);

                using (StreamReader sr = new StreamReader(stream))
                {
                    html.Append("<style>\n");
                    html.Append(sr.ReadToEnd());
                    html.Append("</style>\n");
                    sr.Close();
                    sr.Dispose();
                }
            }
            #endregion Css

            html.Append("     </head>\n");
            html.Append("<body>\n");
        }


        /// <summary>
        /// Monta o Footer de um email com pelo menos um boleto dentro.
        /// </summary>
        /// <param name="saida">StringBuilder onde o conteudo sera salvo.</param>
        protected static void HtmlOfflineFooter(StringBuilder saida)
        {
            saida.Append("</body>\n");
            saida.Append("</html>\n");
        }


        /// <summary>
        /// Junta varios boletos em uma unica AlternateView, para todos serem mandados juntos no mesmo email
        /// </summary>
        /// <param name="arrayDeBoletos">Array contendo os boletos a serem mesclados</param>
        /// <returns></returns>
        public static AlternateView GeraHtmlDeVariosBoletosParaEmail(BoletoBancario[] arrayDeBoletos)
        {
            return GeraHtmlDeVariosBoletosParaEmail(null, arrayDeBoletos);
        }

        /// <summary>
        /// Junta varios boletos em uma unica AlternateView, para todos serem mandados juntos no mesmo email
        /// </summary>
        /// <param name="textoNoComecoDoEmail">Texto em HTML a ser adicionado no comeco do email</param>
        /// <param name="arrayDeBoletos">Array contendo os boletos a serem mesclados</param>
        /// <returns>AlternateView com os dados de todos os boleto.</returns>
        public static AlternateView GeraHtmlDeVariosBoletosParaEmail(string textoNoComecoDoEmail, BoletoBancario[] arrayDeBoletos)
        {
            var corpoDoEmail = new StringBuilder();

            var linkedResources = new List<LinkedResource>();
            HtmlOfflineHeader(corpoDoEmail);
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
                    var theOutput = umBoleto.MontaHtml(
                        "cid:" + lrImagemLogo.ContentId,
                        "cid:" + lrImagemBarra.ContentId,
                        "<img src=\"cid:" + lrImagemCodigoBarra.ContentId + "\" alt=\"Código de Barras\" />");

                    corpoDoEmail.Append(theOutput);

                    linkedResources.Add(lrImagemLogo);
                    linkedResources.Add(lrImagemBarra);
                    linkedResources.Add(lrImagemCodigoBarra);
                }
            }
            HtmlOfflineFooter(corpoDoEmail);



            AlternateView av = AlternateView.CreateAlternateViewFromString(corpoDoEmail.ToString(), Encoding.Default, "text/html");
            foreach (var theResource in linkedResources)
            {
                av.LinkedResources.Add(theResource);
            }



            return av;
        }


        /// <summary>
        /// Função utilizada gerar o AlternateView necessário para enviar um boleto bancário por e-mail.
        /// </summary>
        /// <returns>AlternateView com os dados do boleto.</returns>
        public AlternateView HtmlBoletoParaEnvioEmail()
        {
            return HtmlBoletoParaEnvioEmail(null);
        }


        /// <summary>
        /// Função utilizada gerar o AlternateView necessário para enviar um boleto bancário por e-mail.
        /// </summary>
        /// <param name="textoNoComecoDoEmail">Texto (em HTML) a ser incluido no começo do Email.</param>
        /// <returns>AlternateView com os dados do boleto.</returns>
        public AlternateView HtmlBoletoParaEnvioEmail(string textoNoComecoDoEmail)
        {
            LinkedResource lrImagemLogo;
            LinkedResource lrImagemBarra;
            LinkedResource lrImagemCodigoBarra;

            GeraGraficosParaEmailOffLine(out lrImagemLogo, out lrImagemBarra, out lrImagemCodigoBarra);
            StringBuilder html = HtmlOffLine(textoNoComecoDoEmail, "cid:" + lrImagemLogo.ContentId, "cid:" + lrImagemBarra.ContentId, "cid:" + lrImagemCodigoBarra.ContentId);

            AlternateView av = AlternateView.CreateAlternateViewFromString(html.ToString(), Encoding.Default, "text/html");

            av.LinkedResources.Add(lrImagemLogo);
            av.LinkedResources.Add(lrImagemBarra);
            av.LinkedResources.Add(lrImagemCodigoBarra);
            return av;
        }

        /// <summary>
        /// Gera as tres imagens necessárias para o Boleto
        /// </summary>
        /// <param name="lrImagemLogo">O Logo do Banco</param>
        /// <param name="lrImagemBarra">A Barra Horizontal</param>
        /// <param name="lrImagemCodigoBarra">O Código de Barras</param>
        void GeraGraficosParaEmailOffLine(out LinkedResource lrImagemLogo, out LinkedResource lrImagemBarra, out LinkedResource lrImagemCodigoBarra)
        {
            var randomSufix = new Random().Next().ToString(); // para podermos colocar no mesmo email varios boletos diferentes

            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("BoletoNet.Imagens." + Utils.FormatCode(_ibanco.Codigo.ToString(), 3) + ".jpg");
            lrImagemLogo = new LinkedResource(stream, MediaTypeNames.Image.Jpeg);
            lrImagemLogo.ContentId = "logo" + randomSufix;


            MemoryStream ms = new MemoryStream(Utils.ConvertImageToByte(Html.barra));
            lrImagemBarra = new LinkedResource(ms, MediaTypeNames.Image.Gif);
            lrImagemBarra.ContentId = "barra" + randomSufix; ;

            C2of5i cb = new C2of5i(Boleto.CodigoBarra.Codigo, 1, 50, Boleto.CodigoBarra.Codigo.Length);
            ms = new MemoryStream(Utils.ConvertImageToByte(cb.ToBitmap()));

            lrImagemCodigoBarra = new LinkedResource(ms, MediaTypeNames.Image.Gif);
            lrImagemCodigoBarra.ContentId = "codigobarra" + randomSufix; ;

        }


        /// <summary>
        /// Função utilizada para gravar em um arquivo local o conteúdo do boleto. Este arquivo pode ser aberto em um browser sem que o site esteja no ar.
        /// </summary>
        /// <param name="fileName">Path do arquivo que deve conter o código html.</param>
        public void MontaHtmlNoArquivoLocal(string fileName)
        {
            using (FileStream f = new FileStream(fileName, FileMode.Create))
            {
                StreamWriter w = new StreamWriter(f, System.Text.Encoding.Default);
                w.Write(MontaHtml());
                w.Close();
                f.Close();
            }
        }

        /// <summary>
        /// Monta o Html do boleto bancário
        /// </summary>
        /// <returns>string</returns>
        public string MontaHtml()
        {
            return MontaHtml(null, null);
        }


        /// <summary>
        /// Monta o Html do boleto bancário
        /// </summary>
        /// <param name="fileName">Caminho do arquivo</param>
        /// <param name="fileName">Caminho do logo do cedente</param>
        /// <returns>Html do boleto gerado</returns>
        public string MontaHtml(string fileName, string logoCedente)
        {
            if (fileName == null)
                fileName = System.IO.Path.GetTempPath();

            if (logoCedente != null)
                vLocalLogoCedente = logoCedente;

            string fnLogo = fileName + @"BoletoNet" + Utils.FormatCode(_ibanco.Codigo.ToString(), 3) + ".jpg";

            if (!System.IO.File.Exists(fnLogo))
            {
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("BoletoNet.Imagens." + Utils.FormatCode(_ibanco.Codigo.ToString(), 3) + ".jpg");
                using (Stream file = File.Create(fnLogo))
                {
                    CopiarStream(stream, file);
                }
            }

            string fnBarra = fileName + @"BoletoNetBarra.gif";
            if (!File.Exists(fnBarra))
            {
                ImageConverter imgConverter = new ImageConverter();
                byte[] imgBuffer = (byte[])imgConverter.ConvertTo(Html.barra, typeof(byte[]));
                MemoryStream ms = new MemoryStream(imgBuffer);

                using (Stream stream = File.Create(fnBarra))
                {
                    CopiarStream(ms, stream);
                    ms.Flush();
                    ms.Dispose();
                }
            }

            string fnCodigoBarras = System.IO.Path.GetTempFileName();
            C2of5i cb = new C2of5i(Boleto.CodigoBarra.Codigo, 1, 50, Boleto.CodigoBarra.Codigo.Length);
            cb.ToBitmap().Save(fnCodigoBarras);

            //return HtmlOffLine(fnCorte, fnLogo, fnBarra, fnPonto, fnBarraInterna, fnCodigoBarras).ToString();
            return HtmlOffLine(null, fnLogo, fnBarra, fnCodigoBarras).ToString();
        }

        /// <summary>
        /// Monta o Html do boleto bancário para View ASP.Net MVC
        /// <code>
        /// <para>Exemplo:</para>
        /// <para>public ActionResult VisualizarBoleto(string Id)</para>
        /// <para>{</para>
        /// <para>    BoletoBancario bb = new BoletoBancario();</para>
        /// <para>    //...</para>
        /// <para>    ViewBag.Boleto = bb.MontaHtml("/Content/Boletos/", "teste1");</para>
        /// <para>    return View();</para>
        /// <para>}</para>
        /// <para>//Na view</para>
        /// <para>@{Layout = null;}@Html.Raw(ViewBag.Boleto)</para>
        /// </code>
        /// </summary>
        /// <param name="Url">Pasta dos boletos. Exemplo MontaHtml("/Content/Boletos/", "000100")</param>
        /// <param name="fileName">Nome do arquivo para o boleto</param>
        /// <returns>Html do boleto gerado</returns>
        /// <desenvolvedor>Sandro Ribeiro</desenvolvedor>
        /// <criacao>16/11/2012</criacao>
        public virtual string MontaHtml(string url, string fileName, bool useMapPathSecure = true)
        {
            //Variável para o caminho físico do servidor
            string pathServer = "";

            //Verifica se o usuário informou uma url válida
            if (url == null)
            {
                //Obriga o usuário a especificar uma url válida
                throw new ArgumentException("Você precisa informar uma pasta padrão.");
            }
            else
            {
                if (useMapPathSecure)
                {
                    //Verifica se o usuário usou barras no início e no final da url
                    if (url.Substring(url.Length - 1, 1) != "/")
                        url = url + "/";
                    if (url.Substring(0, 1) != "/")
                        url = url + "/";
                    //Mapeia o caminho físico dos arquivos
                    pathServer = string.Format("~{0}", url);
                }

                //Verifica se o caminho existe
                if (!Directory.Exists(pathServer))
                    throw new ArgumentException("A o caminho físico '{0}' não existe.", pathServer);
            }
            //Verifica o nome do arquivo
            if (fileName == null)
            {
                fileName = DateTime.Now.Ticks.ToString();
            }
            else
            {
                if (fileName == "")
                    fileName = DateTime.Now.Ticks.ToString();
            }

            //Mantive o padrão 
            //Prepara o arquivo da logo para ser salvo
            string fnLogo = pathServer + @"BoletoNet" + Utils.FormatCode(_ibanco.Codigo.ToString(), 3) + ".jpg";
            //Prepara o arquivo da logo para ser usado no html
            string fnLogoUrl = url + @"BoletoNet" + Utils.FormatCode(_ibanco.Codigo.ToString(), 3) + ".jpg";

            //Salvo a imagem apenas 1 vez com o código do banco
            if (!System.IO.File.Exists(fnLogo))
            {
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("BoletoNet.Imagens." + Utils.FormatCode(_ibanco.Codigo.ToString(), 3) + ".jpg");
                using (Stream file = File.Create(fnLogo))
                {
                    CopiarStream(stream, file);
                }
            }

            //Prepara o arquivo da barra para ser salvo
            string fnBarra = pathServer + @"BoletoNetBarra.gif";
            //Prepara o arquivo da barra para ser usado no html
            string fnBarraUrl = url + @"BoletoNetBarra.gif";

            //Salvo a imagem apenas 1 vez
            if (!File.Exists(fnBarra))
            {
                ImageConverter imgConverter = new ImageConverter();
                byte[] imgBuffer = (byte[])imgConverter.ConvertTo(Html.barra, typeof(byte[]));
                MemoryStream ms = new MemoryStream(imgBuffer);

                using (Stream stream = File.Create(fnBarra))
                {
                    CopiarStream(ms, stream);
                    ms.Flush();
                    ms.Dispose();
                }
            }

            //Prepara o arquivo do código de barras para ser salvo
            string fnCodigoBarras = string.Format("{0}{1}_codigoBarras.jpg", pathServer, fileName);
            //Prepara o arquivo do código de barras para ser usado no html
            string fnCodigoBarrasUrl = string.Format("{0}{1}_codigoBarras.jpg", url, fileName);

            C2of5i cb = new C2of5i(Boleto.CodigoBarra.Codigo, 1, 50, Boleto.CodigoBarra.Codigo.Length);

            //Salva o arquivo conforme o fileName
            cb.ToBitmap().Save(fnCodigoBarras);

            //Retorna o Html para ser usado na view
            return HtmlOffLine(null, fnLogoUrl, fnBarraUrl, fnCodigoBarrasUrl).ToString();
        }

        /// <summary>
        /// Monta o Html do boleto bancário com as imagens embutidas no conteúdo, sem necessidade de links externos
        /// de acordo com o padrão http://en.wikipedia.org/wiki/Data_URI_scheme
        /// </summary>
        /// <param name="convertLinhaDigitavelToImage">Converte a Linha Digitável para imagem, com o objetivo de evitar malwares.</param>
        /// <returns>Html do boleto gerado</returns>
        /// <desenvolvedor>Iuri André Stona</desenvolvedor>
        /// <criacao>23/01/2014</criacao>
        /// <alteracao>08/08/2014</alteracao>

        public string MontaHtmlEmbedded(bool convertLinhaDigitavelToImage = false, bool usaCSSPDF = false)
        {

            var assembly = Assembly.GetExecutingAssembly();

            var streamLogo = assembly.GetManifestResourceStream("BoletoNet.Imagens." + CodigoBanco.ToString("000") + ".jpg");
            string base64Logo = Convert.ToBase64String(new BinaryReader(streamLogo).ReadBytes((int)streamLogo.Length));
            string fnLogo = string.Format("data:image/gif;base64,{0}", base64Logo);

            var streamBarra = assembly.GetManifestResourceStream("BoletoNet.Imagens.barra.gif");
            string base64Barra = Convert.ToBase64String(new BinaryReader(streamBarra).ReadBytes((int)streamBarra.Length));
            string fnBarra = string.Format("data:image/gif;base64,{0}", base64Barra);

            var cb = new C2of5i(Boleto.CodigoBarra.Codigo, 1, 50, Boleto.CodigoBarra.Codigo.Length);
            string base64CodigoBarras = Convert.ToBase64String(cb.ToByte());
            string fnCodigoBarras = string.Format("data:image/gif;base64,{0}", base64CodigoBarras);

            if (convertLinhaDigitavelToImage)
            {

                string linhaDigitavel = Boleto.CodigoBarra.LinhaDigitavel.Replace("  ", " ").Trim();

                var imagemLinha = Utils.DrawText(linhaDigitavel, new Font("Arial", 30, FontStyle.Bold), Color.Black, Color.White);
                string base64Linha = Convert.ToBase64String(Utils.ConvertImageToByte(imagemLinha));

                string fnLinha = string.Format("data:image/gif;base64,{0}", base64Linha);

                Boleto.CodigoBarra.LinhaDigitavel = @"<img style=""max-width:420px; margin-bottom: 2px"" src=" + fnLinha + " />";
            }

            string s = HtmlOffLine(null, fnLogo, fnBarra, fnCodigoBarras, usaCSSPDF).ToString();

            if (convertLinhaDigitavelToImage)
            {
                s = s.Replace(".w500", "");
            }

            return s;
        }

        public byte[] MontaBytesPDF(bool convertLinhaDigitavelToImage = false)
        {

            return (new NReco.PdfGenerator.HtmlToPdfConverter()).GeneratePdf(this.MontaHtmlEmbedded(convertLinhaDigitavelToImage, true));
        }
        #endregion Geração do Html OffLine

        public System.Drawing.Image GeraImagemCodigoBarras(Boleto boleto)
        {
            C2of5i cb = new C2of5i(boleto.CodigoBarra.Codigo, 1, 50, boleto.CodigoBarra.Codigo.Length);
            return cb.ToBitmap();
        }

        private void CopiarStream(Stream entrada, Stream saida)
        {
            int bytesLidos = 0;
            byte[] imgBuffer = new byte[entrada.Length];

            while ((bytesLidos = entrada.Read(imgBuffer, 0, imgBuffer.Length)) > 0)
            {
                saida.Write(imgBuffer, 0, bytesLidos);
            }
        }
    }
}
