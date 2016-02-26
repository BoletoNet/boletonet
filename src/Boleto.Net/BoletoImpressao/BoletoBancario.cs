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
    public partial class BoletoBancario
    {
        #region Variaveis

        protected Boleto _boleto;

        #endregion Variaveis

        #region Propriedades

        /// <summary>
        /// "Código do banco em que será gerado o boleto. Ex. 341-Itaú, 237-Bradesco"
        /// </summary>
        public short CodigoBanco {
            get
            {
                return this.Boleto.Banco.Codigo;
            }
            set
            {
                if (this.Boleto == null)
                    this._boleto = new Boleto() { Banco = new Banco(value) };
                else {
                    this._boleto.ResetBanco(value);
                }
            }
        }

        /// <summary>
        /// Mostra o código da carteira
        /// </summary>
        public Boleto Boleto
        {
            get {
                return _boleto; }
            set
            {
                if (_boleto != null)
                {
                    if ( _boleto.Banco != null && value.Banco == null)
                    {
                        //aplica um merge do banco no boleto exitente antes de atribuir (por razões de legado) desde que o valor inicial seja nulo
                        value.BancoCarteira = null; //zera o conteudo de BancoCarteira antes
                        value.Banco = _boleto.Banco;
                    }
                }

                _boleto = value;
            }
        }

        public Sacado Sacado
        {
            get { return _boleto?.Sacado; }
        }

        public Cedente Cedente
        {
            get { return _boleto?.Cedente; }
        }

        

        #endregion Propriedades

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

                if (Boleto.Opcoes.MostrarEnderecoCedente)
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
                var _instrucoesHtml = new StringBuilder() ;

                _instrucoesHtml.Append(MontaInstrucoes(Boleto.Instrucoes));

                if (Boleto.Sacado?.Instrucoes.Count > 0)
                    _instrucoesHtml.Append(MontaInstrucoes(Boleto.Sacado.Instrucoes));

                return html.ToString().Replace("@INSTRUCOES", _instrucoesHtml.ToString());
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
                var _instrucoesHtml = new StringBuilder();

                _instrucoesHtml.Append(MontaInstrucoes(Boleto.Instrucoes));

                if (Boleto.Cedente.Instrucoes.Count > 0)
                    _instrucoesHtml.Append(MontaInstrucoes(Boleto.Cedente.Instrucoes));

                return html.ToString().Replace("@INSTRUCOES", _instrucoesHtml.ToString());
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

                html.Append(Boleto.Opcoes.MostrarComprovanteEntregaLivre ? Html.ComprovanteEntrega71 : Html.ComprovanteEntrega7);

                html.Append("<br />");
                return html.ToString();
            }
        }

        private string MontaInstrucoes(IList<IInstrucao> instrucoes)
        {
            var _instrucoesHtml = "";
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
                    //Instrucoes.Add(instrucao); não necessita mais disso Olavo(olavo at exodus.eti.br)
                }

                _instrucoesHtml = Strings.Left(_instrucoesHtml, _instrucoesHtml.Length - 6);
            }

            return _instrucoesHtml;
        }

        protected string MontaHtml(string urlImagemLogo, string urlImagemBarra, string imagemCodigoBarras)
        {
            var html = new StringBuilder();
            var parametrosBoleto = new BoletoBancario.BoletoRenderParams(this, urlImagemLogo, urlImagemBarra, imagemCodigoBarras);

            //Oculta o cabeçalho das instruções do boleto
            if (!Boleto.Opcoes.OcultarInstrucoes)
                html.Append(GeraHtmlInstrucoes());

            if (this.Boleto.Opcoes.ExibirDemonstrativo && this.Boleto.Demonstrativos.Any())
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

            if (Boleto.Opcoes.Formato != FormatoBoleto.Carne)
            {
                //Mostra o comprovante de entrega
                if (Boleto.Opcoes.MostrarComprovanteEntrega | Boleto.Opcoes.MostrarComprovanteEntregaLivre)
                {
                    html.Append(HtmlComprovanteEntrega);
                    //Html da linha pontilhada
                    if (Boleto.Opcoes.OcultarReciboSacado)
                        html.Append(Html.ReciboSacadoParte8);
                }

                //Oculta o recibo do sacabo do boleto
                if (!Boleto.Opcoes.OcultarReciboSacado)
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

            

            if (Boleto.Opcoes.Formato != FormatoBoleto.Carne)
                html.Append(GerarHtmlReciboCedente());
            else
            {
                html.Append(GeraHtmlCarne("", GerarHtmlReciboCedente()));
            }

            if (String.IsNullOrEmpty(Boleto.Cedente.Logo))
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

            internal bool MostrarCodigoCarteira { get { return this.Boleto.Opcoes.MostrarCodigoCarteira; } }
            internal bool MostrarEnderecoCedente { get { return this.Boleto.Opcoes.MostrarEnderecoCedente; } }

            public Boleto Boleto { get { return this.BoletoBancario.Boleto; } }

            public string LocalLogoCliente { get; set; }
            public string UrlImagemLogo { get; set; }
            public string UrlImagemBarra { get; internal set; }

            public string DataVencimento {
                get
                {
                    if (Boleto.Opcoes.MostrarContraApresentacaoNaDataVencimento)
                        return "Contra Apresentação";
                    else
                        return Boleto.DataVencimento.ToString("dd/MM/yyyy");
                }
            }

            public string Sacado {
                get
                {
                    //Flavio(fhlviana@hotmail.com) - adicionei a possibilidade de o boleto não ter, necessáriamente, que informar o CPF ou CNPJ do sacado. (Ajustado por olavo at exodus.eti.br)
                    //Formata o CPF/CNPJ(se houver) e o Nome do Sacado para apresentação
                    if (string.IsNullOrEmpty( Boleto.Sacado?.CPFCNPJ ) )
                    {
                        return Boleto.Sacado?.Nome ?? "";
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
                    var sacado = Boleto.Sacado;

                    if (sacado == null)
                        return "";

                    //Caso não oculte o Endereço do Sacado,
                    if (!Boleto.Opcoes.OcultarEnderecoSacado)
                    {
                        String enderecoSacado = "";
                        
                        if (sacado.Endereco.CEP == String.Empty)
                            enderecoSacado = string.Format("{0} - {1}/{2}", sacado.Endereco.Bairro, sacado.Endereco.Cidade, sacado.Endereco.UF);
                        else
                            enderecoSacado = string.Format("{0} - {1}/{2} - CEP: {3}", sacado.Endereco.Bairro, sacado.Endereco.Cidade, sacado.Endereco.UF, Utils.FormataCEP(sacado.Endereco.CEP));

                        if (sacado.Endereco.End != string.Empty && enderecoSacado != string.Empty)
                        {
                            string Numero = !String.IsNullOrEmpty(sacado.Endereco.Numero) ? ", " + sacado.Endereco.Numero : "";

                            if (expSacado == string.Empty)
                                expSacado += InfoSacado.Render(sacado.Endereco.End + Numero, enderecoSacado, false);
                            else
                                expSacado += InfoSacado.Render(sacado.Endereco.End + Numero, enderecoSacado, true);
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
        protected StringBuilder HtmlOffLine(string srcLogo, string srcBarra, string srcCodigoBarra, bool usaPdf = false)
        {
            StringBuilder html = new StringBuilder();
            HtmlOfflineHeader(html, usaPdf);
            html.Append(MontaHtml(srcLogo, srcBarra, "<img src=\"" + srcCodigoBarra + "\" alt=\"Código de Barras\" />"));
            HtmlOfflineFooter(html);
            return html;
        }

        /// <summary>
        /// Monta o Header de um email com pelo menos um boleto dentro.
        /// </summary>
        /// <param name="saida">StringBuilder onde o conteudo sera salvo.</param>
        protected static void HtmlOfflineHeader(StringBuilder html, bool usaPdf = false)
        {
            html.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n");
            html.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">\n");
            html.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\n");
            html.Append("<meta charset=\"utf-8\"/>\n");
            html.Append("<head>");
            html.Append("    <title>Boleto.Net</title>\n");

            #region Css
            {
                string arquivoCSS = usaPdf ? "BoletoNet.BoletoImpressao.BoletoNetPDF.css" : "BoletoNet.BoletoImpressao.BoletoNet.css";
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
        /// Função utilizada para gravar em um arquivo local o conteúdo do boleto. Este arquivo pode ser aberto em um browser sem que o site esteja no ar.
        /// </summary>
        /// <param name="fileName">Path do arquivo que deve conter o código html.</param>
        public void MontaHtmlNoArquivoLocal(string fileName)
        {
            using (FileStream f = new FileStream(fileName, FileMode.Create))
            {
                StreamWriter w = new StreamWriter(f, System.Text.Encoding.Default);
                w.Write(MontaHtmlEmbedded());
            }
        }

        /// <summary>
        /// Monta o Html do boleto bancário com as imagens embutidas no conteúdo, sem necessidade de links externos
        /// de acordo com o padrão http://en.wikipedia.org/wiki/Data_URI_scheme
        /// </summary>
        /// <param name="convertLinhaDigitavelToImage">Converte a Linha Digitável para imagem, com o objetivo de evitar malwares.</param>
        /// <returns>Html do boleto gerado</returns>
        /// <desenvolvedor>Iuri André Stona/Olavo Rocha Neto</desenvolvedor>
        /// <criacao>23/01/2014</criacao>
        /// <alteracao>17/02/2016</alteracao>
        public string MontaHtmlEmbedded(bool convertLinhaDigitavelToImage = false, bool usaPdf = false, string logoCedente = null)
        {
            var assembly = Assembly.GetExecutingAssembly();

            if (logoCedente != null)
                Boleto.Cedente.Logo = logoCedente;

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

            string s = HtmlOffLine(fnLogo, fnBarra, fnCodigoBarras, usaPdf).ToString();

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

    }
}
