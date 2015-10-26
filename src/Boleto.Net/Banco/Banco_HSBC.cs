using System;
using System.Web.UI;
using Microsoft.VisualBasic;

[assembly: WebResource("BoletoNet.Imagens.399.jpg", "image/jpg")]
namespace BoletoNet
{
    /// <summary>
    /// Classe referente ao HSBC
    /// </summary>
    internal class Banco_HSBC : AbstractBanco, IBanco
    {
        #region Variáveis

        private string _dacNossoNumero = string.Empty;
        private int _dacBoleto = 0;

        #endregion

        #region Construtores

        internal Banco_HSBC()
        {
            try
            {
                this.Codigo = 399;
                this.Digito = "9";
                this.Nome = "HSBC";
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao instanciar objeto.", ex);
            }
        }
        #endregion

        #region Métodos de Instância

        /// <summary>
        /// Validações particulares do banco HSBC
        /// </summary>
        public override void ValidaBoleto(Boleto boleto)
        {
            try
            {
                var nossoNumero = Utils.ToInt64(boleto.NossoNumero);
                var tamanhoNossoNumero = nossoNumero.ToString().Length;

                if (string.IsNullOrEmpty(boleto.Carteira))
                    throw new NotImplementedException("Carteira não informada. Utilize a carteira 'CSB' ou 'CNR'");

                //Verifica as carteiras implementadas
                if (!boleto.Carteira.Equals("CSB") &
                    !boleto.Carteira.Equals("CNR"))

                    throw new NotImplementedException("Carteira não implementada. Utilize a carteira 'CSB' ou 'CNR'.");

                //Verifica se o nosso número é válido
                if (Utils.ToString(boleto.NossoNumero) == string.Empty)
                    throw new NotImplementedException("Nosso número inválido");

                //Verifica se o nosso número é válido
                if (nossoNumero == 0)
                    throw new NotImplementedException("Nosso número inválido");
                // Correção: O campo “Código do Documento” deve ser composto somente de código numérico 
                // com até 13 posições e 3 posições para os dígitos verificadores, utilizando 16 posições no máximo. 
                if (tamanhoNossoNumero > 13)
                    throw new NotImplementedException("A quantidade de dígitos do nosso número para a carteira " + boleto.Carteira + ", são 13 números.");

                if (tamanhoNossoNumero < 13)
                    boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 13);

                // Calcula o DAC do Nosso Número
                // Nosso Número = Range(5) + Numero Sequencial(8)
                //_dacNossoNumero = Mod11(boleto.NossoNumero, 7).ToString(); Estava calculando errado, de acordo com o HSBC, quando o resto fosse 1, tinha que gerar digito 0. Criei um mod11Hsbc para isso.
                _dacNossoNumero = Mod11Hsbc(boleto.NossoNumero, 7).ToString();//por Transis em 25/02/15

                //Atribui o nome do banco ao local de pagamento
                boleto.LocalPagamento = "PAGAR EM QUALQUER AGÊNCIA BANCARIA ATÉ O VENCIMENTO OU CANAIS ELETRONICOS DO HSBC";

                //Verifica se data do processamento é valida
				//if (boleto.DataProcessamento.ToString("dd/MM/yyyy") == "01/01/0001")
				if (boleto.DataProcessamento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                    boleto.DataProcessamento = DateTime.Now;

                //Verifica se data do documento é valida
				//if (boleto.DataDocumento.ToString("dd/MM/yyyy") == "01/01/0001")
				if (boleto.DataDocumento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                    boleto.DataDocumento = DateTime.Now;

                FormataCodigoBarra(boleto);
                FormataLinhaDigitavel(boleto);
                FormataNossoNumero(boleto);
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao validar boletos.", e);
            }
        }

        # endregion

        # region Métodos de formatação do boleto

        /// <summary>
        /// Formata o código de barras para carteira CSB
        /// </summary>
        /// <example>
        /// DE ATÉ TAMANHO CONTEÚDO
        /// -----------------------
        /// 01 03 03 Código do HSBC na Câmara de Compensação, igual a 399.
        /// 04 04 01 Tipo de Moeda (9 para moeda Real ou 0 para Moeda Variável).
        /// 05 05 01 Dígito de Autoconferência (DAC).
        /// 06 09 04 Fator de Vencimento.
        /// 10 19 10 Valor do Documento. Se Moeda Variável, o valor deverá ser igual a zeros.
        /// 20 30 11 Número Bancário (Nosso Número).
        /// 31 41 11 Código da Agencia (4) + Conta de Cobrança (7).
        /// 40 43 02 Código da carteira "00".
        /// 44 44 01 Código do aplicativo de cobranca "1"
        /// </example>
        public override void FormataCodigoBarra(Boleto boleto)
        {
            try
            {


                string valorBoleto = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
                valorBoleto = Utils.FormatCode(valorBoleto, 10);

                string numeroDocumento = Utils.FormatCode(boleto.NumeroDocumento.ToString(), 7);

                switch (boleto.Carteira.ToUpper())
                {
                    case "CSB": boleto.CodigoBarra.Codigo =
                            // Código de Barras
                            //banco & moeda & fator & valor & nossonumero & dac_nossonumero & agencia & conta & "00" & "1"
                        string.Format("{0}{1}{2}{3}{4}{5}{6}001", Codigo, boleto.Moeda,
                                      FatorVencimento(boleto), valorBoleto, boleto.NossoNumero + _dacNossoNumero,
                                      Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4),
                                      Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 7));
                        break;
                    case "CNR":
                        // Código de Barras
                        //banco & moeda & fator & valor & codCedente & nossonumero & diadoano & digito do ano & "2"
                        boleto.CodigoBarra.Codigo =
                        string.Format("{0}{1}{2}{3}{4}{5}{6}2",
                                        Codigo,
                                        boleto.Moeda,
                                        FatorVencimento(boleto),
                                        valorBoleto,
                                        Utils.FormatCode(boleto.Cedente.Codigo.ToString(), 7),
                                        Utils.FormatCode(boleto.NossoNumero.ToString(), 13),
                                        Utils.FormatCode(boleto.DataVencimento.DayOfYear.ToString() + boleto.DataVencimento.ToString("yy").Substring(1, 1), 4));
                        break;
                }


                _dacBoleto = Mod11(boleto.CodigoBarra.Codigo, 9, 0);

                boleto.CodigoBarra.Codigo = Strings.Left(boleto.CodigoBarra.Codigo, 4) + _dacBoleto + Strings.Right(boleto.CodigoBarra.Codigo, 39);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao formatar código de barras.", ex);
            }
        }

        /// A linha digitável será composta por cinco campos: para carteira CSB
        ///      1º campo
        ///         01 03 03 Código do HSBC na Câmara de Compensação, igual a 399.
        ///         04 04 01 Tipo de Moeda (9 para moeda Real ou 0 para Moeda Variável).
        ///         05 09 01 Primeira parte do Nosso número (5) - Numero Range.
        ///         10 10 01 DV deste campo.
        ///      2º campo
        ///         11 16 06 Final do Nosso Número Calculado com DV (6).
        ///         17 20 04 Início da conta cobrança - Código da agencia (4).
        ///         21 21 01 DV deste campo.
        ///      3º campo
        ///         22 28 07 Final da conta cobrança - Conta (7).
        ///         29 30 02 Código da carteira "00".
        ///         31 31 01 Código do aplicativo "1".
        ///         32 32 01 DV deste campo.
        ///      4º campo
        ///         33 33 01 DV do código da barras.
        ///      5º campo
        ///         34 37 04 Fator de vencimento.
        ///         38 47 10 Valor do documento. Zeros para Moeda Variável
        /// </summary>
        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            //AAABC.CCCCX DDDDD.DEEEEY FFFFF.FF001Z W GGGGHHHHHHHHHH

            try
            {
                //string numeroDocumento = Utils.FormatCode(boleto.NumeroDocumento.ToString(), 13);
                string nossoNumero = Utils.FormatCode(boleto.NossoNumero.ToString(), 13);
                string codigoCedente = Utils.FormatCode(boleto.Cedente.Codigo.ToString(), 7);

                string C1 = string.Empty;
                string C2 = string.Empty;
                string C3 = string.Empty;
                string C5 = string.Empty;

                string AAA;
                string B;
                string CCCCC;
                string X;


                string DDDDDD; string DD;
                string EEEE; string EEEEEEEE;
                string Y;
                string FFFFFFF; string FFFFF;
                string GGGGG;
                string Z;
                switch (boleto.Carteira.ToUpper())
                {
                    case "CSB":
                        #region AAABC.CCCCX

                        AAA = Utils.FormatCode(Codigo.ToString(), 3);
                        B = boleto.Moeda.ToString();
                        CCCCC = boleto.NossoNumero.Substring(0, 5);
                        X = Mod10(AAA + B + CCCCC).ToString();

                        C1 = string.Format("{0}{1}{2}.", AAA, B, CCCCC.Substring(0, 1));
                        C1 += string.Format("{0}{1} ", CCCCC.Substring(1, 4), X);

                        #endregion AAABC.CCDDX

                        #region DDDDD.DEEEEY

                        DDDDDD = boleto.NossoNumero.Substring(5, 5) + _dacNossoNumero;
                        EEEE = Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4);
                        Y = Mod10(DDDDDD + EEEE).ToString();

                        C2 = string.Format("{0}.", DDDDDD.Substring(0, 5));
                        C2 += string.Format("{0}{1}{2} ", DDDDDD.Substring(5, 1), EEEE, Y);

                        #endregion DDDDD.DEEEEY

                        #region FFFFF.FF001Z

                        FFFFFFF = Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 7);
                        Z = Mod10(FFFFFFF + "001").ToString();

                        C3 = string.Format("{0}.", FFFFFFF.Substring(0, 5));
                        C3 += string.Format("{0}001{1}", FFFFFFF.Substring(5, 2), Z);

                        #endregion FFFFF.FF001Z
                        break;
                    case "CNR":
                        #region AAABC.CCCCX

                        AAA = Utils.FormatCode(Codigo.ToString(), 3);
                        B = boleto.Moeda.ToString();
                        CCCCC = codigoCedente.Substring(0, 5);
                        X = Mod10(AAA + B + CCCCC).ToString();

                        C1 = string.Format("{0}{1}{2}.", AAA, B, CCCCC.Substring(0, 1));
                        C1 += string.Format("{0}{1} ", CCCCC.Substring(1, 4), X);

                        #endregion AAABC.CCDDX

                        #region DDEEE.EEEEEY

                        DD = codigoCedente.Substring(5, 2);
                        EEEEEEEE = nossoNumero.Substring(0, 8);
                        Y = Mod10(DD + EEEEEEEE).ToString();

                        C2 = string.Format("{0}{1}.", DD, EEEEEEEE.Substring(0, 3));
                        C2 += string.Format("{0}{1} ", EEEEEEEE.Substring(3, 5), Y);

                        #endregion DDEEE.EEEEEY

                        #region FFFFF.GGGGGZ

                        FFFFF = nossoNumero.Substring(8, 5);
                        GGGGG = Utils.FormatCode(boleto.DataVencimento.DayOfYear.ToString() + boleto.DataVencimento.ToString("yy").Substring(1, 1), 4) + "2";

                        Z = Mod10(FFFFF + GGGGG).ToString();

                        C3 = string.Format("{0}.", FFFFF);
                        C3 += string.Format("{0}{1}", GGGGG, Z);

                        #endregion FFFFF.GGGGGZ
                        break;
                    default:
                        throw new NotImplementedException("Função não implementada.");
                }


                string W = string.Format(" {0} ", _dacBoleto);

                #region HHHHIIIIIIIIII

                string HHHH = FatorVencimento(boleto).ToString();
                string IIIIIIIIII = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");

                IIIIIIIIII = Utils.FormatCode(IIIIIIIIII, 10);
                C5 = HHHH + IIIIIIIIII;

                #endregion HHHHHHHHHHHHHH

                boleto.CodigoBarra.LinhaDigitavel = C1 + C2 + C3 + W + C5;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao formatar linha digitável.", ex);
            }
        }

        public override void FormataNossoNumero(Boleto boleto)
        {
            try
            {
                switch (boleto.Carteira.ToUpper())
                {
                    case "CSB":
                        boleto.NossoNumero = string.Format("{0}{1}", boleto.NossoNumero, _dacNossoNumero);
                        break;

                    case "CNR": boleto.NossoNumero = string.Format("{0}{1}4{2}", boleto.NossoNumero, Mod11Base9(boleto.NossoNumero).ToString(), Mod11Base9((Int64.Parse(boleto.NossoNumero + Mod11Base9(boleto.NossoNumero).ToString() + "4") + int.Parse(boleto.Cedente.Codigo.ToString()) + int.Parse(boleto.DataVencimento.ToString("ddMMyy"))).ToString())); break;
                    default:
                        throw new NotImplementedException("Carteira não implementada.  Use CSB ou CNR");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao formatar nosso número", ex);
            }
        }

        protected static int Mod11Hsbc(string seq, int b)
        {
            /* Variáveis
             * -------------
             * d - Dígito
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */

            int d, s = 0, p = 2;


            for (int i = seq.Length; i > 0; i--)
            {
                s = s + (Convert.ToInt32(Microsoft.VisualBasic.Strings.Mid(seq, i, 1)) * p);
                if (p == b)
                    p = 2;
                else
                    p = p + 1;
            }

            d = s % 11;

            if ((d == 0) || (d == 1))
                d = 0;
            else
                d = 11 - d;

            return d;
        }

        # endregion

        # region Métodos de geração do arquivo remessa CNAB400

        # region HEADER

        /// <summary>
        /// HEADER do arquivo CNAB
        /// Gera o HEADER do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public string GerarHeaderRemessaCNAB240()
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public string GerarHeaderRemessaCNAB400(int numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            throw new NotImplementedException("Função não implementada.");
        }
        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            throw new NotImplementedException("Função não implementada.");
        }
        # endregion

        # region DETALHE

        /// <summary>
        /// DETALHE do arquivo CNAB
        /// Gera o DETALHE do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public string GerarDetalheRemessaCNAB240()
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public string GerarDetalheRemessaCNAB400(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        # endregion DETALHE

        # region TRAILER

        /// <summary>
        /// TRAILER do arquivo CNAB
        /// Gera o TRAILER do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public string GerarTrailerRemessa240()
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public string GerarTrailerRemessa400(int numeroRegistro)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        # endregion

        #endregion

        #region Métodos de processamento do arquivo retorno CNAB400

        #endregion


        /// <summary>
        /// Efetua as Validações dentro da classe Boleto, para garantir a geração da remessa
        /// </summary>
        public override bool ValidarRemessa(TipoArquivo tipoArquivo, string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            bool vRetorno = true;
            string vMsg = string.Empty;
            ////IMPLEMENTACAO PENDENTE...
            mensagem = vMsg;
            return vRetorno;
        }

    }
}
