using BoletoNet.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
//Envio por email
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Web.UI;

[assembly: WebResource("BoletoNet.BoletoImpressao.BoletoNet.css", "text/css", PerformSubstitution = true)]
[assembly: WebResource("BoletoNet.Imagens.barra.gif", "image/gif")]
//[assembly: WebResource("BoletoNet.Imagens.corte.gif", "image/gif")]
//[assembly: WebResource("BoletoNet.Imagens.barrainterna.gif", "image/gif")]
//[assembly: WebResource("BoletoNet.Imagens.ponto.gif", "image/gif")]

namespace BoletoNet
{
    using System.Linq;

    [Serializable(),
    Designer(typeof(BoletoBancarioDesigner)),
    ToolboxBitmap(typeof(BoletoBancario)),
    ToolboxData("<{0}:BoletoBancario Runat=\"server\"></{0}:BoletoBancario>")]
    public class BoletoBancario : System.Web.UI.Control
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
        private bool _ajustaTamanhoFonte = false;
        private bool _removeSimboloMoedaValorDocumento = false;
        private string _ajustaTamanhoFonteHtml;
        #endregion Variaveis

        #region Propriedades

        [Browsable(true), Description("Remove o símbolo R$ do Valor do Documento")]
        public bool RemoveSimboloMoedaValorDocumento
        {
            get { return _removeSimboloMoedaValorDocumento; }
            set { _removeSimboloMoedaValorDocumento = value; }
        }

        [Browsable(true), Description("Código do banco em que será gerado o boleto. Ex. 341-Itaú, 237-Bradesco")]
        public short CodigoBanco
        {
            get { return _codigoBanco; }
            set { _codigoBanco = value; }
        }

        /// <summary>
        /// Mostra o código da carteira
        /// </summary>
        [Browsable(true), Description("Mostra a descrição da carteira")]
        public bool MostrarCodigoCarteira
        {
            get { return _mostrarCodigoCarteira; }
            set { _mostrarCodigoCarteira = value; }
        }

        [Browsable(true), Description("Gera um relatório com os valores que deram origem ao boleto")]
        public bool ExibirDemonstrativo { get; set; }

        /// <summary>
        /// Mostra o código da carteira
        /// </summary>
        [Browsable(true), Description("Formata o boleto no layout de carnê")]
        public bool FormatoCarne
        {
            get { return _formatoCarne; }
            set { _formatoCarne = value; }
        }

        [Browsable(false)]
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

        [Browsable(false)]
        public Sacado Sacado
        {
            get { return _sacado; }
        }

        [Browsable(false)]
        public Cedente Cedente
        {
            get { return _cedente; }
        }

        [Browsable(false)]
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

        /// <summary>
        /// Caminho onde se encontra a ferramenta WkHtmlToPdf.
        /// Se os arquivos do WkHtmlToPdf não estiverem presentes no caminho,
        /// eles serão criados automaticamente pelo NReco PdfGenerator.
        /// </summary>
        [Browsable(true), Description("Caminho onde se encontra a ferramenta WkHtmlToPdf.")]
        public string PdfToolPath { get; set; }

        /// <summary>
        /// Caminho onde a NReco gera os arquivos temporários necessários para a construção do PDF.
        /// Se o caminho não for especificado a ferramenta utilizará a pasta retornada pelo método Path.GetTempPath().
        /// </summary>
        [Browsable(true), Description("Caminho onde a NReco gera os arquivos temporários necessários para a construção do PDF.")]
        public string TempFilesPath { get; set; }

        #region Propriedades
        [Browsable(true), Description("Mostra o comprovante de entrega sem dados para marcar")]
        public bool MostrarComprovanteEntregaLivre
        {
            get { return Utils.ToBool(ViewState["1"]); }
            set { ViewState["1"] = value; }
        }

        [Browsable(true), Description("Mostra o comprovante de entrega")]
        public bool MostrarComprovanteEntrega
        {
            get { return Utils.ToBool(ViewState["2"]); }
            set { ViewState["2"] = value; }
        }

        [Browsable(true), Description("Oculta as intruções do boleto")]
        public bool OcultarEnderecoSacado
        {
            get { return Utils.ToBool(ViewState["3"]); }
            set { ViewState["3"] = value; }
        }

        [Browsable(true), Description("Oculta as intruções do boleto")]
        public bool OcultarInstrucoes
        {
            get { return Utils.ToBool(ViewState["4"]); }
            set { ViewState["4"] = value; }
        }

        [Browsable(true), Description("Oculta o recibo do sacado do boleto")]
        public bool OcultarReciboSacado
        {
            get { return Utils.ToBool(ViewState["5"]); }
            set { ViewState["5"] = value; }
        }

        [Browsable(true), Description("Gerar arquivo de remessa")]
        public bool GerarArquivoRemessa
        {
            get { return Utils.ToBool(ViewState["6"]); }
            set { ViewState["6"] = value; }
        }
        /// <summary> 
        /// Mostra o termo "Contra Apresentação" na data de vencimento do boleto
        /// </summary>
        public bool MostrarContraApresentacaoNaDataVencimento
        {
            get { return Utils.ToBool(ViewState["7"]); }
            set { ViewState["7"] = value; }
        }

        [Browsable(true), Description("Mostra o endereço do Cedente")]
        public bool MostrarEnderecoCedente
        {
            get { return Utils.ToBool(ViewState["8"]); }
            set { ViewState["8"] = value; }
        }
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

        public static string UrlLogo(int banco)
        {
            var page = System.Web.HttpContext.Current.CurrentHandler as Page;
            return page.ClientScript.GetWebResourceUrl(typeof(BoletoBancario), "BoletoNet.Imagens." + Utils.FormatCode(banco.ToString(), 3) + ".jpg");
        }

        #region Override
        protected override void OnPreRender(EventArgs e)
        {
            string alias = "BoletoNet.BoletoImpressao.BoletoNet.css";
            string csslink = "<link rel=\"stylesheet\" type=\"text/css\" href=\"" +
                Page.ClientScript.GetWebResourceUrl(typeof(BoletoBancario), alias) + "\" />";

            var include = new LiteralControl(csslink);
            Page.Header.Controls.Add(include);

            base.OnPreRender(e);
        }

        protected override void OnLoad(EventArgs e)
        {
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "Execution")]
        protected override void Render(HtmlTextWriter output)
        {
            if (_ibanco == null)
            {
                output.Write("<b>Erro gerando o boleto bancário: faltou definir o banco.</b>");
                return;
            }

            string urlImagemLogo = Page.ClientScript.GetWebResourceUrl(typeof(BoletoBancario), "BoletoNet.Imagens." + Utils.FormatCode(_ibanco.Codigo.ToString(), 3) + ".jpg");
            string urlImagemBarra = Page.ClientScript.GetWebResourceUrl(typeof(BoletoBancario), "BoletoNet.Imagens.barra.gif");
            //string urlImagemBarraInterna = Page.ClientScript.GetWebResourceUrl(typeof(BoletoBancario), "BoletoNet.Imagens.barrainterna.gif");
            //string urlImagemCorte = Page.ClientScript.GetWebResourceUrl(typeof(BoletoBancario), "BoletoNet.Imagens.corte.gif");
            //string urlImagemPonto = Page.ClientScript.GetWebResourceUrl(typeof(BoletoBancario), "BoletoNet.Imagens.ponto.gif");

            //Atribui os valores ao html do boleto bancário
            //output.Write(MontaHtml(urlImagemCorte, urlImagemLogo, urlImagemBarra, urlImagemPonto, urlImagemBarraInterna,
            //    "<img src=\"ImagemCodigoBarra.ashx?code=" + Boleto.CodigoBarra.Codigo + "\" alt=\"Código de Barras\" />"));
            output.Write(MontaHtml(urlImagemLogo, urlImagemBarra, "<img src=\"ImagemCodigoBarra.ashx?code=" + Boleto.CodigoBarra.Codigo + "\" alt=\"Código de Barras\" />"));
        }
        #endregion Override

        /// <summary>
        /// Ajusta o tamanho dos textos e labels do boleto bancário.
        /// </summary>
        /// <param name="tamanhoFonteTextos">Padrão 10px</param>
        /// <param name="tamanhoFonteRotulos">Padrão 9.8px. É o maior tamanho sem que o exista uma quebra de linha no rótulo "Data processamento"</param>
        /// <param name="tamanhoFonteInstrucaoImpressao">Padrão 10px</param>
        /// <param name="tamanhoFonteInstrucoes">Padrão 10px</param>
        public void AjustaTamanhoFonte(double tamanhoFonteTextos = 10, double tamanhoFonteRotulos = 9.8, double tamanhoFonteInstrucaoImpressao = 9, double tamanhoFonteInstrucoes = 10)
        {
            _ajustaTamanhoFonte = true;

            var html = new StringBuilder();

            html.AppendLine("<style>");
            html.AppendFormat(".cp$1 font-size: {0}px !important; $2", tamanhoFonteTextos);
            html.AppendFormat(".ctN$1 font-size: {0}px !important; $2", tamanhoFonteRotulos);
            html.AppendFormat(".cpN$1 font-size: {0}px !important; $2", tamanhoFonteTextos);
            html.AppendFormat(".ti$1 font-size: {0}px !important; $2", tamanhoFonteInstrucaoImpressao);
            html.AppendFormat(".ct$1 font-size: {0}px !important; $2", tamanhoFonteRotulos);
            html.AppendFormat(".t$1 font-size: {0}px !important; $2", tamanhoFonteRotulos);
            html.AppendFormat(".it$1 font-size: {0}px !important; $2", tamanhoFonteInstrucoes);
            html.AppendLine("</style>");

            _ajustaTamanhoFonteHtml = html.ToString().Replace("$1", "{").Replace("$2", "}");
        }


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

        private string GeraHtmlCarne(string htmlBoleto)
        {
            var html = new StringBuilder();

            html.Append(Html.Carne);

            return html.ToString()                
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

        public string GeraHtmlReciboCedente()
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

                //Para Banco Itaú, o texto "(Texto de responsabilidade do cedente)" deve ser
                //"(Todas as informações deste bloqueto são de exclusiva responsabilidade do cedente)".
                if (Boleto.Banco.Codigo == 341)
                {
                    html.Replace("(Texto de responsabilidade do cedente)", "(Todas as informações deste bloqueto são de exclusiva responsabilidade do cedente)");
                }

                //Para carteiras "17-019" e "18-019" do Banco do Brasil, a ficha de compensação não possui código da carteira
                //na formatação do campo.
                if (Boleto.Banco.Codigo == 1 & (Boleto.Carteira.Equals("17-019") | Boleto.Carteira.Equals("17-027") | Boleto.Carteira.Equals("18-019") | Boleto.Carteira.Equals("17-159") | Boleto.Carteira.Equals("17-140") | Boleto.Carteira.Equals("17-067")))
                {
                    html.Replace("Carteira /", "");
                    html.Replace("@NOSSONUMERO", "@NOSSONUMEROBB");
                }
                else
                {
                    //Para SANTANDER, a ficha de compensação não possui código da carteira - por jsoda em 08/12/2012
                    if (Boleto.Banco.Codigo == 33)
                    {
                        html.Replace("Carteira /", "");
                    }
                }

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
            string enderecoCedente = "";

            if (_ajustaTamanhoFonte)
            {
                html.Append(_ajustaTamanhoFonteHtml);
            }

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
                    if (MostrarEnderecoCedente)
                    {
                        if (Cedente.Endereco == null)
                            throw new ArgumentNullException("Endereço do Cedente");

                        string Numero = !String.IsNullOrEmpty(Cedente.Endereco.Numero) ? Cedente.Endereco.Numero + ", " : "";
                        enderecoCedente = string.Concat(Cedente.Endereco.End, " , ", Numero);

                        if (Cedente.Endereco.CEP == String.Empty)
                        {
                            enderecoCedente += string.Format("{0} - {1}/{2}", Cedente.Endereco.Bairro,
                                                             Cedente.Endereco.Cidade, Cedente.Endereco.UF);
                        }
                        else
                        {
                            enderecoCedente += string.Format("{0} - {1}/{2} - CEP: {3}", Cedente.Endereco.Bairro,
                                                             Cedente.Endereco.Cidade, Cedente.Endereco.UF,
                                                             Utils.FormataCEP(Cedente.Endereco.CEP));
                        }

                    }
                }
            }

            string sacado = "";
            //Flavio(fhlviana@hotmail.com) - adicionei a possibilidade de o boleto não ter, necessáriamente, que informar o CPF ou CNPJ do sacado.
            //Formata o CPF/CNPJ(se houver) e o Nome do Sacado para apresentação
            if (Sacado.CPFCNPJ == string.Empty)
            {
                sacado = Sacado.Nome;
            }
            else
            {
                if (Sacado.CPFCNPJ.Length <= 11)
                    sacado = string.Format("{0}  CPF: {1}", Sacado.Nome, Utils.FormataCPF(Sacado.CPFCNPJ));
                else
                    sacado = string.Format("{0}  CNPJ: {1}", Sacado.Nome, Utils.FormataCNPJ(Sacado.CPFCNPJ));
            }

            String infoSacado = Sacado.InformacoesSacado.GeraHTML(false);

            //Caso não oculte o Endereço do Sacado,
            if (!OcultarEnderecoSacado)
            {
                String enderecoSacado = "";

                if (Sacado.Endereco.CEP == String.Empty)
                    enderecoSacado = string.Format("{0} - {1}/{2}", Sacado.Endereco.Bairro, Sacado.Endereco.Cidade, Sacado.Endereco.UF);
                else
                    enderecoSacado = string.Format("{0} - {1}/{2} - CEP: {3}", Sacado.Endereco.Bairro,
                    Sacado.Endereco.Cidade, Sacado.Endereco.UF, Utils.FormataCEP(Sacado.Endereco.CEP));

                if (Sacado.Endereco.End != string.Empty && enderecoSacado != string.Empty)
                {
                    string Numero = !String.IsNullOrEmpty(Sacado.Endereco.Numero) ? ", " + Sacado.Endereco.Numero : "";

                    if (infoSacado == string.Empty)
                        infoSacado += InfoSacado.Render(Sacado.Endereco.End + Numero, enderecoSacado, false);
                    else
                        infoSacado += InfoSacado.Render(Sacado.Endereco.End + Numero, enderecoSacado, true);
                }
                //"Informações do Sacado" foi introduzido para possibilitar que o boleto na informe somente o endereço do sacado
                //como em outras situaçoes onde se imprime matriculas, codigos e etc, sobre o sacado.
                //Sendo assim o endereço do sacado passa a ser uma Informaçao do Sacado que é adicionada no momento da renderização
                //de acordo com a flag "OcultarEnderecoSacado"
            }

            string agenciaConta = Utils.FormataAgenciaConta(Cedente.ContaBancaria.Agencia, Cedente.ContaBancaria.DigitoAgencia, Cedente.ContaBancaria.Conta, Cedente.ContaBancaria.DigitoConta);

            // Trecho adicionado por Fabrício Nogueira de Almeida :fna - fnalmeida@gmail.com - 09/12/2008
            /* Esse código foi inserido pq no campo Agência/Cod Cedente, estava sendo impresso sempre a agência / número da conta
			 * No boleto da caixa que eu fiz, coloquei no método validarBoleto um trecho para calcular o dígito do cedente, e adicionei esse atributo na classe cedente
			 * O trecho abaixo testa se esse digito foi calculado, se foi insere no campo Agencia/Cod Cedente, a agência e o código com seu digito
			 * caso contrário mostra a agência / conta, como era anteriormente.
			 * Com esse código ele ira atender as necessidades do boleto caixa e não afetará os demais
			 * Caso queira que apareça o Agência/cod. cedente para outros boletos, basta calcular e setar o digito, como foi feito no boleto Caixa 
			 */

            string agenciaCodigoCedente;

            if (!Cedente.DigitoCedente.Equals(-1))
            {
                if (!String.IsNullOrEmpty(Cedente.ContaBancaria.OperacaConta))
                    agenciaCodigoCedente = string.Format("{0}/{1}.{2}-{3}", Cedente.ContaBancaria.Agencia, Cedente.ContaBancaria.OperacaConta, Utils.FormatCode(Cedente.Codigo.ToString(), 6), Cedente.DigitoCedente.ToString());

                switch (Boleto.Banco.Codigo)
                {
                    case 748:
                        agenciaCodigoCedente = string.Format("{0}.{1}.{2}", Cedente.ContaBancaria.Agencia, Cedente.ContaBancaria.OperacaConta, Utils.FormatCode(Cedente.ContaBancaria.Conta, 5));
                        break;
                    case 41:
                        agenciaCodigoCedente = string.Format("{0}.{1}/{2}.{3}.{4}", Cedente.ContaBancaria.Agencia, Cedente.ContaBancaria.DigitoAgencia, Cedente.Codigo.Substring(4, 6), Cedente.Codigo.Substring(10, 1), Cedente.DigitoCedente);
                        break;
                    case 1:
                        agenciaCodigoCedente = string.Format("{0}-{1}/{2}-{3}", Cedente.ContaBancaria.Agencia, Cedente.ContaBancaria.DigitoAgencia, Utils.FormatCode(Cedente.ContaBancaria.Conta, 6), Cedente.ContaBancaria.DigitoConta);
                        break;
                    case 399:
                        agenciaCodigoCedente = string.Format("{0}/{1}", Cedente.ContaBancaria.Agencia, Utils.FormatCode(Cedente.Codigo.ToString() + Cedente.DigitoCedente.ToString(), 7));
                        break;
                    default:
                        agenciaCodigoCedente = string.Format("{0}/{1}-{2}", Cedente.ContaBancaria.Agencia, Utils.FormatCode(Cedente.Codigo.ToString(), 6), Cedente.DigitoCedente.ToString());
                        break;
                }
            }
            else
            {
                //Para banco SANTANDER, a formatação do campo "Agencia/Identif.Cedente" - por jsoda em 07/05/2012
                if (Boleto.Banco.Codigo == 33)
                {
                    agenciaCodigoCedente = string.Format("{0}-{1}/{2}", Cedente.ContaBancaria.Agencia, Cedente.ContaBancaria.DigitoAgencia, Utils.FormatCode(Cedente.Codigo.ToString(), 6));
                    if (String.IsNullOrEmpty(Cedente.ContaBancaria.DigitoAgencia))
                        agenciaCodigoCedente = String.Format("{0}/{1}", Cedente.ContaBancaria.Agencia, Utils.FormatCode(Cedente.Codigo.ToString(), 6));
                }
                else if (Boleto.Banco.Codigo == 399)
                    //agenciaCodigoCedente = Utils.FormatCode(Cedente.Codigo.ToString(), 7); -> para Banco HSBC mostra apenas código Cedente - por Ponce em 08/06/2012
                    agenciaCodigoCedente = String.Format("{0}/{1}", Cedente.ContaBancaria.Agencia, Utils.FormatCode(Cedente.Codigo.ToString(), 7)); //Solicitação do HSBC que mostrasse agencia/Conta - por Transis em 24/02/15
                else if (Boleto.Banco.Codigo == 748)
                    agenciaCodigoCedente = string.Format("{0}.{1}.{2}", Cedente.ContaBancaria.Agencia, Cedente.ContaBancaria.OperacaConta, Utils.FormatCode(Cedente.ContaBancaria.Conta, 5));
                else
                    agenciaCodigoCedente = agenciaConta;
            }

            if (!FormatoCarne)
                html.Append(GeraHtmlReciboCedente());
            else
            {
                html.Append(GeraHtmlCarne(GeraHtmlReciboCedente()));
            }

            string dataVencimento = Boleto.DataVencimento.ToString("dd/MM/yyyy");

            if (MostrarContraApresentacaoNaDataVencimento)
                dataVencimento = "Contra Apresentação";

            if (String.IsNullOrEmpty(vLocalLogoCedente))
                vLocalLogoCedente = urlImagemLogo;

            var valorBoleto = (Boleto.ValorBoleto == 0 ? "" : Boleto.ValorBoleto.ToString("C", CultureInfo.GetCultureInfo("PT-BR")));

            return html.Replace("@CODIGOBANCO", Utils.FormatCode(_ibanco.Codigo.ToString(), 3))
                .Replace("@DIGITOBANCO", _ibanco.Digito)
                //.Replace("@URLIMAGEMBARRAINTERNA", urlImagemBarraInterna)
                //.Replace("@URLIMAGEMCORTE", urlImagemCorte)
                //.Replace("@URLIMAGEMPONTO", urlImagemPonto)
                .Replace("@URLIMAGEMLOGO", urlImagemLogo)
                .Replace("@URLIMGCEDENTE", vLocalLogoCedente)
                .Replace("@URLIMAGEMBARRA", urlImagemBarra)
                .Replace("@LINHADIGITAVEL", Boleto.CodigoBarra.LinhaDigitavel)
                .Replace("@LOCALPAGAMENTO", Boleto.LocalPagamento)
                .Replace("@DATAVENCIMENTO", dataVencimento)
                .Replace(
                    "@CEDENTE_BOLETO",
                    !Cedente.MostrarCNPJnoBoleto
                        ? Cedente.Nome
                        : string.Format("{0}&nbsp;&nbsp;&nbsp;CNPJ: {1}", Cedente.Nome, Cedente.CPFCNPJcomMascara))
                .Replace("@CEDENTE", Cedente.Nome)
                .Replace("@DATADOCUMENTO", Boleto.DataDocumento.ToString("dd/MM/yyyy"))
                .Replace("@NUMERODOCUMENTO", Boleto.NumeroDocumento)
                .Replace("@ESPECIEDOCUMENTO", EspecieDocumento.ValidaSigla(Boleto.EspecieDocumento))
                .Replace("@DATAPROCESSAMENTO", Boleto.DataProcessamento.ToString("dd/MM/yyyy"))

            #region Implementação para o Banco do Brasil

                //Variável inserida para atender às especificações das carteiras "17-019", "17-027" e "18-019" do Banco do Brasil
                //apenas para a ficha de compensação.
                //Como a variável não existirá se não forem as carteiras "17-019", "17-027", "17-019", "17-035", "17-140", "17-159", "17-067", "17-167" e "18-019", não foi colocado o [if].
                .Replace(
                    "@NOSSONUMEROBB",
                    Boleto.Banco.Codigo == 1
                    & (Boleto.Carteira.Equals("17-019") | Boleto.Carteira.Equals("17-027") | Boleto.Carteira.Equals("17-035")
                        | Boleto.Carteira.Equals("18-019") | Boleto.Carteira.Equals("17-140") | Boleto.Carteira.Equals("17-159")
                        | Boleto.Carteira.Equals("17-067") | Boleto.Carteira.Equals("17-167"))
                        ? Boleto.NossoNumero.Substring(3)
                        : string.Empty)
            #endregion Implementação para o Banco do Brasil

                .Replace("@NOSSONUMERO", Boleto.NossoNumero)
                .Replace("@CARTEIRA", FormataDescricaoCarteira())
                .Replace("@ESPECIE", Boleto.Especie)
                .Replace("@QUANTIDADE", (Boleto.QuantidadeMoeda == 0 ? "" : Boleto.QuantidadeMoeda.ToString()))
                .Replace("@VALORDOCUMENTO", Boleto.ValorMoeda)
                .Replace("@=VALORDOCUMENTO", valorBoleto)
                .Replace(
                    "@VALORCOBRADO",
                    (Boleto.ValorCobrado == 0 ? "" : Boleto.ValorCobrado.ToString("C", CultureInfo.GetCultureInfo("PT-BR"))))
                .Replace("@OUTROSACRESCIMOS", "")
                .Replace("@OUTRASDEDUCOES", "")
                .Replace(
                    "@DESCONTOS",
                    (Boleto.ValorDesconto == 0 ? "" : Boleto.ValorDesconto.ToString("C", CultureInfo.GetCultureInfo("PT-BR"))))
                .Replace("@AGENCIACONTA", agenciaCodigoCedente)
                .Replace("@SACADO", sacado)
                .Replace("@INFOSACADO", infoSacado)
                .Replace("@AGENCIACODIGOCEDENTE", agenciaCodigoCedente)
                .Replace("@CPFCNPJ", Cedente.CPFCNPJ)
                .Replace(
                    "@MORAMULTA",
                    (Boleto.ValorMulta == 0 ? "" : Boleto.ValorMulta.ToString("C", CultureInfo.GetCultureInfo("PT-BR"))))
                .Replace("@AUTENTICACAOMECANICA", "")
                .Replace("@USODOBANCO", Boleto.UsoBanco)
                .Replace("@IMAGEMCODIGOBARRA", imagemCodigoBarras)
                .Replace("@ACEITE", Boleto.Aceite)
                .ToString()
                .Replace("@ENDERECOCEDENTE", MostrarEnderecoCedente ? enderecoCedente : "")
                .Replace(
                    "@AVALISTA",
                    string.Format(
                        "{0} - {1}",
                        Boleto.Avalista != null ? Boleto.Avalista.Nome : string.Empty,
                        Boleto.Avalista != null ? Boleto.Avalista.CPFCNPJ : string.Empty))
                .Replace("Ar\">R$", RemoveSimboloMoedaValorDocumento ? "Ar\">" : "Ar\">R$")
                .Replace("@PARCELATOTAL",Boleto.NumeroParcela != 0 && Boleto.TotalParcela != 0 ? Boleto.NumeroParcela + " / " + Boleto.TotalParcela : string.Empty);
                

        }

        private string FormataDescricaoCarteira()
        {

            if (MostrarCodigoCarteira)
            {
                string descricaoCarteira = "";
                int carteira = Utils.ToInt32(Boleto.Carteira);

                switch (Banco.Codigo)
                {
                    case 1:
                        descricaoCarteira = new Carteira_BancoBrasil(carteira).Codigo;
                        break;
                    case 353:
                    case 8:
                    case 33:
                        descricaoCarteira = new Carteira_Santander(carteira).Codigo;
                        break;
                    case 104:
                        descricaoCarteira = new Carteira_Caixa(carteira).Codigo;
                        break;
                    case 341:
                        descricaoCarteira = new Carteira_Itau(carteira).Codigo;
                        break;

                    default:
                        throw new Exception(string.Format("A descrição para o banco {0} não foi implementada.", Boleto.Banco));
                        throw new Exception(string.Format("A descrição da carteira {0} / banco {1} não foi implementada (marque false na propriedade MostrarCodigoCarteira)", carteira, Banco.Codigo));

                }

                if (string.IsNullOrEmpty(descricaoCarteira))
                {
                    throw new Exception("O código da carteira não foi implementado.");
                }

                return string.Format("{0} - {1}", Boleto.Carteira,
                     descricaoCarteira);
            }
            else
            {
                return Boleto.Carteira;
            }
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
        protected StringBuilder HtmlOffLine(string textoNoComecoDoEmail, string srcLogo, string srcBarra, string srcCodigoBarra, bool usaCSSPDF = false, bool imprimeHeader = true, bool imprimeFooter = true)
        {//protected StringBuilder HtmlOffLine(string srcCorte, string srcLogo, string srcBarra, string srcPonto, string srcBarraInterna, string srcCodigoBarra)
            this.OnLoad(EventArgs.Empty);

            StringBuilder html = new StringBuilder();
            if (imprimeHeader)
                HtmlOfflineHeader(html, usaCSSPDF);
            if (textoNoComecoDoEmail != null && textoNoComecoDoEmail != "")
            {
                html.Append(textoNoComecoDoEmail);
            }
            html.Append(MontaHtml(srcLogo, srcBarra, "<img src=\"" + srcCodigoBarra + "\" alt=\"Código de Barras\" />"));
            if (imprimeFooter)
                HtmlOfflineFooter(html);
            return html;
        }




        /// <summary>
        /// Monta o Header de um email com pelo menos um boleto dentro.
        /// </summary>
        /// <param name="saida">StringBuilder onde o conteudo sera salvo.</param>
        protected static void HtmlOfflineHeader(StringBuilder html, bool usaCSSPDF = false, string titulo = "Boleto.Net")
        {
            html.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n");
            html.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">\n");
            html.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\n");
            html.Append("<meta charset=\"utf-8\"/>\n");
            html.Append("<head>");
            html.Append("    <title>" + titulo + "</title>\n");

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
            this.OnLoad(EventArgs.Empty);

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

            this.OnLoad(EventArgs.Empty);

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
        public string MontaHtml(string url, string fileName, bool useMapPathSecure = true)
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
                    pathServer = MapPathSecure(string.Format("~{0}", url));
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
            this.OnLoad(EventArgs.Empty);

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
        /// <param name="somenteHtmlBoleto">Quando true imprime apenas a table do boleto ignorando o CSS e o html/head/body. Utilizado para dimunir o tamanho do html quando imprimir mais de um boleto</param>
        /// <returns>Html do boleto gerado</returns>
        /// <desenvolvedor>Iuri André Stona</desenvolvedor>
        /// <criacao>23/01/2014</criacao>
        /// <alteracao>08/08/2014</alteracao>

        public string MontaHtmlEmbedded(bool convertLinhaDigitavelToImage = false, bool usaCSSPDF = false, bool imprimeHeader = true,  bool imprimeFooter = true)
        {
            OnLoad(EventArgs.Empty);
            Stream streamBarra = null;
            try
            {
                var assembly = Assembly.GetExecutingAssembly();

                string base64Logo = Convert.ToBase64String(ObterLogoDoBanco(CodigoBanco));
                string fnLogo = string.Format("data:image/gif;base64,{0}", base64Logo);

                streamBarra = assembly.GetManifestResourceStream("BoletoNet.Imagens.barra.gif");
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

                string s = HtmlOffLine(null, fnLogo, fnBarra, fnCodigoBarras, usaCSSPDF, imprimeHeader, imprimeFooter).ToString();

                if (convertLinhaDigitavelToImage)
                {
                    s = s.Replace(".w500", "");
                }

                return s;

            }
            finally
            {
                if (streamBarra != null)
                {
                    streamBarra.Close();
                }
            }
        }

        public byte[] MontaBytesPDF(bool convertLinhaDigitavelToImage = false)
        {
            var converter = new NReco.PdfGenerator.HtmlToPdfConverter();

            if (!string.IsNullOrWhiteSpace(TempFilesPath))
            {
                converter.TempFilesPath = this.TempFilesPath;
            }

            if (!string.IsNullOrEmpty(this.PdfToolPath))
            {
                converter.PdfToolPath = this.PdfToolPath;
            }
            return converter.GeneratePdf(this.MontaHtmlEmbedded(convertLinhaDigitavelToImage, true));
        }

        /// <summary>
        /// Lista de Boletos, objetos do tipo
        /// BoletoBancario
        /// </summary>
        /// <param name="boletos">Lista de Boletos, objetos do tipo BoletoBancario</param>
        /// <param name="tituloNaView">Título Que aparecerá na Aba do Navegador</param>
        /// <param name="CustomSwitches">Custom WkHtmlToPdf global options</param>
        /// <param name="tituloPDF">Título No Início do PDF</param>
        /// <param name="PretoBranco">Preto e Branco = true</param>
        /// <param name="convertLinhaDigitavelToImage">bool Converter a Linha Digitavel Em Imagem</param>
        /// <param name="BoletosPorPagina">Quantidade de Boletos até o próximo LineBreak. Lembre de Utilizar o Zoom para ajustar a página</param>
        /// <param name="ZoomPercent">Percentual da Scala do PDF. Para 3 Boletos na mesma Página com Carnê utilize 80%</param>				
        /// <returns>byte[], Vetor de bytes do PDF</returns>
        public byte[] MontaBytesListaBoletosPDF(List<BoletoBancario> boletos, string tituloNaView = "", string CustomSwitches = "", string tituloPDF = "", bool PretoBranco = false, bool convertLinhaDigitavelToImage = false, int BoletosPorPagina = 1, float ZoomPercent = 100)
        {
            StringBuilder htmlBoletos = new StringBuilder();
            HtmlOfflineHeader(htmlBoletos, true, tituloNaView);

            if (!string.IsNullOrEmpty(tituloPDF))
            {
                htmlBoletos.Append("<br/><center><h1>");
                htmlBoletos.Append(tituloPDF);
                htmlBoletos.Append("</h1></center><br/>");
            }
            int qtdeBoletosPagina = 0;
            foreach (BoletoBancario boleto in boletos)
            {
                qtdeBoletosPagina++;

                if (qtdeBoletosPagina % BoletosPorPagina == 0)
                {
                    htmlBoletos.Append("<div class='break'>");
                    qtdeBoletosPagina = 0;
                }
                else
                {
                    htmlBoletos.Append("<div>");
                }
                htmlBoletos.Append(boleto.MontaHtmlEmbedded(convertLinhaDigitavelToImage, true, false, false));
                htmlBoletos.Append("</div>");
            }
            HtmlOfflineFooter(htmlBoletos);

            var converter = new NReco.PdfGenerator.HtmlToPdfConverter()
            {
                CustomWkHtmlArgs = CustomSwitches,
                Grayscale = PretoBranco,
                Zoom = ZoomPercent / 100
            };
            if (!string.IsNullOrEmpty(this.PdfToolPath))
            {
                converter.PdfToolPath = this.PdfToolPath;
            }
            if (!string.IsNullOrEmpty(this.TempFilesPath))
            {
                converter.TempFilesPath = this.TempFilesPath;
            }

            return converter.GeneratePdf(htmlBoletos.ToString());
        }

        #endregion Geração do Html OffLine

        public System.Drawing.Image GeraImagemCodigoBarras(Boleto boleto)
        {
            C2of5i cb = new C2of5i(boleto.CodigoBarra.Codigo, 1, 50, boleto.CodigoBarra.Codigo.Length);
            return cb.ToBitmap();
        }

        /// <summary>
        /// Obtem o array de bytes da logo do banco.
        /// </summary>
        /// <returns>bytes da logo</returns>
        public static byte[] ObterLogoDoBanco(short codigoBanco)
        {
            Stream streamLogo = null;
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                streamLogo = assembly.GetManifestResourceStream(string.Format("BoletoNet.Imagens.{0}.jpg", codigoBanco.ToString("000")));
                return new BinaryReader(streamLogo).ReadBytes((int)streamLogo.Length);
            }
            finally
            {
                if (streamLogo != null)
                {
                    streamLogo.Close();
                }
            }

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
