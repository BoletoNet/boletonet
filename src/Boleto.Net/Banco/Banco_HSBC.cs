using System;
using System.Web.UI;
using BoletoNet.Util;

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
                // com até 10 posições e 1 posições para os dígitos verificadores, utilizando 11 posições no máximo (CSB). 
                // com até 13 posições e 3 posições para os dígitos verificadores, utilizando 16 posições no máximo (CNR). 
                switch (boleto.Carteira.ToUpper())
                {
                    case "CSB":
                        if (tamanhoNossoNumero > 10)
                            boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 13);

                        if (tamanhoNossoNumero < 10)
                            boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 10);
                        break;

                    case "CNR":
                        if (tamanhoNossoNumero > 13)
                            throw new NotImplementedException("A quantidade de dígitos do nosso número para a carteira " + boleto.Carteira + ", são 13 números.");

                        if (tamanhoNossoNumero < 13)
                            boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 13);
                        break;
                }

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
                        //banco & moeda & fator & valor & nossonumero & dac_nossonumero & agencia & conta & digitosconta & "00" & "1"
                         string.Format("{0}{1}{2}{3}{4}{5}{6}001", Codigo, boleto.Moeda,
                                      FatorVencimento(boleto), valorBoleto, boleto.NossoNumero + _dacNossoNumero,
                                      Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4),
                                      Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta + boleto.Cedente.ContaBancaria.DigitoConta, 7));                        
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
                string nossoNumero = boleto.NossoNumero.ToString();
                switch (boleto.Carteira.ToUpper())
                {
                    case "CSB":
                        nossoNumero = Utils.FormatCode(boleto.NossoNumero.ToString(), 10);
                        break;

                    case "CNR":
                        nossoNumero = Utils.FormatCode(boleto.NossoNumero.ToString(), 13);
                        break;
                    default:
                        throw new NotImplementedException("Carteira não implementada. Use CSB ou CNR.");
                }

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

                        FFFFFFF = Utils.FormatCode(string.Format("{0}{1}", boleto.Cedente.ContaBancaria.Conta, boleto.Cedente.ContaBancaria.DigitoConta), 7);
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
                s = s + (Convert.ToInt32(seq.Mid( i, 1)) * p);
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

        # region Métodos de geração do arquivo remessa

        # region HEADER

        /// <summary>
        /// HEADER do arquivo CNAB
        /// Gera o HEADER do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarHeaderRemessa(Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            return GerarHeaderRemessa("0", cedente, tipoArquivo, numeroArquivoRemessa);
        }

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            try
            {
                string _header = " ";

                base.GerarHeaderRemessa(numeroConvenio, cedente, tipoArquivo, numeroArquivoRemessa);

                switch (tipoArquivo)
                {

                    case TipoArquivo.CNAB400:
                        if (cedente.Carteira == "1" || cedente.Carteira == "3" || cedente.Carteira == "4")
                            _header = GerarHeaderRemessaCNAB400CSB(numeroConvenio, cedente);
                        else
                            _header = GerarHeaderRemessaCNAB400CNR(numeroConvenio, cedente);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return _header;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do HEADER do arquivo de REMESSA.", ex);
            }
        }

        public string GerarHeaderRemessaCNAB240()
        {
            throw new NotImplementedException("Função não implementada.");
        }

        private string GerarHeaderRemessaCNAB400CNR(string numeroConvenio, Cedente cedente)
        {
            try
            {
                string complemento = new string(' ', 275);
                string _header;

                //Código do registro ==> 001 - 001
                _header = "0";

                //Código da remessa ==> 002 - 002
                _header += "1";

                //Literal de transmissão ==> 003 - 009
                _header += "REMESSA";

                //Código do serviço ==> 010 - 011
                _header += "01";

                //Literal de serviço ==> 012 - 026
                _header += Utils.FitStringLength("COBRANCA CNR", 15, 15, ' ', 0, true, true, false);

                //Número do código do beneficiário ==> 027 - 036
                _header += Utils.FitStringLength(cedente.Codigo, 10, 10, '0', 0, true, true, true);

                //Complemento do tamanho do registro ==> 037 - 046
                _header += new string(' ', 10);

                //Nome do cedente  ==> 047 - 076
                _header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false);

                //Código do Banco ==> 077 - 079 
                _header += Utils.FormatCode(Codigo.ToString(), "0", 3, true);

                //Nome do Banco ==> 080 - 094
                _header += Utils.FitStringLength("HSBC", 15, 15, ' ', 0, true, true, false);

                //Data de Gravação ==> 095 - 102
                _header += DateTime.Now.ToString("ddMMyyyy");

                //Densidade da gravação ==> 103 - 107
                _header += "01600";

                //Literal densidade ==> 108 - 110
                _header += "BPI";

                //Hora da gravação ==> 111 - 116
                _header += DateTime.Now.ToString("HHmmss");

                //Código do formulário ==> 117 - 120
                _header += new string(' ', 4);

                //Periodicidade de vencimento ==> 121 - 121
                _header += "4";

                //Brancos ==> 122 - 122
                _header += " ";

                //Tipo de moeda ==> 123 - 124
                _header += new string(' ', 2);

                //Indicador de valor da parcela==> 125 - 125
                _header += " ";

                //Remessa de documentos ==> 126 - 126
                _header += " ";

                //Montagem dos carnês ==> 127 - 127
                _header += " ";

                //Brancos ==> 128 - 221
                _header += new string(' ', 94);

                //Observação-1 ==> 222 - 263
                _header += new string(' ', 42);

                //Observação-2 ==> 264 - 305
                _header += new string(' ', 42);

                //Observação-3 ==> 306 - 347
                _header += new string(' ', 42);

                //Y2K ==> 348 - 350
                _header += new string(' ', 3);

                //Brancos ==> 351 - 394
                _header += new string(' ', 44);

                //Número sequencial ==> 395 - 400
                _header += Utils.FitStringLength("1", 6, 6, '0', 0, true, true, true);

                _header = Utils.SubstituiCaracteresEspeciais(_header);

                return _header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
        }

        private string GerarHeaderRemessaCNAB400CSB(string numeroConvenio, Cedente cedente)
        {
            try
            {
                string complemento = new string(' ', 275);
                string _header;

                //Código do registro ==> 001 - 001
                _header = "0";

                //Código do Arquivo ==> 002 - 002
                _header += "1";

                //Literal do Arquivo ==> 003 - 009
                _header += "REMESSA";

                //Código do serviço ==> 010 - 011
                _header += "01";

                //Literal de serviço ==> 012 - 026
                _header += Utils.FitStringLength("COBRANCA", 15, 15, ' ', 0, true, true, false);

                //Zero ==> 027 - 027
                _header += "0";

                //Agência Beneficiária ==> 028 - 031
                _header += Utils.FitStringLength(cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true);

                //Sub-conta ==> 032 - 033
                _header += "55";

                //Conta Corrente ==> 034 - 044
                _header += Utils.FitStringLength(cedente.ContaBancaria.Agencia + cedente.ContaBancaria.Conta + cedente.ContaBancaria.DigitoConta, 11, 11, '0', 0, true, true, true);

                //Banco ==> 045 - 046
                _header += "  ";

                //Nome do cedente  ==> 047 - 076
                _header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false);

                //Código do Banco ==> 077 - 079 
                _header += Utils.FormatCode(Codigo.ToString(), "0", 3, true);

                //Nome do Banco ==> 080 - 094
                _header += Utils.FitStringLength("HSBC", 15, 15, ' ', 0, true, true, false);

                //Data de Gravação ==> 095 - 100
                _header += DateTime.Now.ToString("ddMMyy");

                //Densidade da gravação ==> 101 - 105
                _header += "01600";

                //Literal densidade ==> 106 - 108
                _header += "BPI";

                //Banco ==> 109 - 110
                _header += "  ";

                //Sigla Layout ==> 111 - 117
                _header += "LANCV08";

                //Brancos ==> 118 - 394
                _header += new string(' ', 277);

                //Número sequencial ==> 395 - 400
                _header += Utils.FitStringLength("1", 6, 6, '0', 0, true, true, true);

                _header = Utils.SubstituiCaracteresEspeciais(_header).ToUpper();

                return _header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
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
            try
            {
                string _detalhe = "";

                switch (tipoArquivo)
                {
                    case TipoArquivo.CNAB240:
                        _detalhe = GerarDetalheRemessaCNAB240();
                        break;
                    case TipoArquivo.CNAB400:
                        if (boleto.Carteira == "1" || boleto.Carteira == "3" || boleto.Carteira == "4")
                            _detalhe = GerarDetalheRemessaCNAB400CSB(boleto, numeroRegistro, tipoArquivo);
                        else
                            _detalhe = GerarDetalheRemessaCNAB400CNR(boleto, numeroRegistro, tipoArquivo);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return _detalhe;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do DETALHE arquivo de REMESSA.", ex);
            }
        }

        private string GerarDetalheRemessaCNAB240()
        {
            throw new NotImplementedException("Função não implementada.");
        }

        private string GerarDetalheRemessaCNAB400CNR(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                string _detalhe;

                //Código do registro ==> 001 - 001
                _detalhe = "1";

                //Código de inscrição ==> 002 - 003
                _detalhe += "99";

                //Código do beneficiário ==> 004 - 013
                _detalhe += Utils.FitStringLength(boleto.Cedente.Codigo, 10, 10, '0', 0, true, true, true);

                //Brancos ==> 014 - 037
                _detalhe += new string(' ', 24);

                //Zeros ==> 038 - 040
                _detalhe += new string('0', 3);

                //Código do documento ==> 041 - 053
                _detalhe += Utils.FitStringLength(boleto.NumeroDocumento, 13, 13, '0', 0, true, true, true);

                //Brancos ==> 054 - 107
                _detalhe += new string(' ', 54);

                //Carteira ==> 108 - 108
                _detalhe += "0";

                //Código de ocorrência  ==> 109 - 110
                _detalhe += "01";

                //Parcela de ==> 111 - 113
                _detalhe += "001";

                //Quantidade de parcelas ==> 114 - 116
                _detalhe += "001";

                //Parcela até => 117 - 119
                _detalhe += "001";

                //Brancos ==> 120 - 120
                _detalhe += " ";

                //Vencimento ==> 121 - 128
                _detalhe += boleto.DataVencimento.ToString("ddMMyyyy");

                //Valor da parcela ==> 129 - 140
                _detalhe += Utils.FitStringLength(boleto.ValorBoleto.ApenasNumeros(), 12, 12, '0', 0, true, true, true);

                //Banco cobrador ==> 141 - 143
                _detalhe += Utils.FitStringLength(boleto.Banco.Codigo.ToString(), 3, 3, '0', 0, true, true, true);

                //Brancos ==> 144 - 147
                _detalhe += new string(' ', 4);

                //Espécie ==> 148 - 149
                _detalhe += "99";

                //Aceite ==> 150 - 150
                _detalhe += "N";

                //Brancos ==> 151 - 180
                _detalhe += new string(' ', 30);

                //Valor da parcela única ==> 181 - 192
                _detalhe += new string(' ', 12);

                //Vencimento da parcela única ==> 193 - 200
                _detalhe += new string(' ', 8);

                //Brancos ==> 201 - 218
                _detalhe += new string(' ', 18);

                //Código de inscrição ==> 219 - 220
                _detalhe += "98";

                //Brancos ==> 221 - 226
                _detalhe += new string(' ', 6);

                //CEP do pagador ==> 227 - 234
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.CEP, 8, 8, '0', 0, true, true, true).ToUpper();

                //Nome ==> 235 - 274
                _detalhe += Utils.FitStringLength(boleto.Sacado.Nome.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();

                //Logradouro ==> 275 - 314
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.End.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();

                //Bairro ==> 315 - 329
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Bairro.TrimStart(' '), 15, 15, ' ', 0, true, true, false).ToUpper();

                //Brancos ==> 330 - 334
                _detalhe += new string(' ', 5);

                //Cidade ==> 335 - 349
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Cidade.TrimStart(' '), 15, 15, ' ', 0, true, true, false).ToUpper();

                //Estado ==> 350 - 351
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.UF, 2, 2, ' ', 0, true, true, false).ToUpper();

                //Observação ==> 352 - 393
                if (boleto.Instrucoes != null && boleto.Instrucoes.Count > 0)
                    _detalhe += Utils.FitStringLength(boleto.Instrucoes[0].Descricao, 42, 42, ' ', 0, true, true, false).ToUpper();
                else
                    _detalhe += new string(' ', 42);

                //Cadastro da Postagem ==> 394 - 394
                _detalhe += "1";

                //Número sequencial do registro no arquivo ==> 395 - 400
                _detalhe += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);

                _detalhe = Utils.SubstituiCaracteresEspeciais(_detalhe);

                return _detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB400.", ex);
            }
        }

        private string GerarDetalheRemessaCNAB400CSB(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                string _detalhe;

                //Código do registro ==> 001 - 001
                _detalhe = "1";

                //CNPJ ou CPF do cedente ==> 002 - 003
                if (boleto.Cedente.CPFCNPJ.Length <= 11)
                    _detalhe += "01";
                else
                    _detalhe += "02";

                //CNPJ ou CPF do cedente ==> 004 - 017
                _detalhe += Utils.FitStringLength(boleto.Cedente.CPFCNPJ, 14, 14, '0', 0, true, true, true);

                //Zero ==> 018 - 018
                _detalhe += "0";

                //Código da agência cedente ==> 019 - 022
                _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true);

                //Sub-conta ==> 023 - 024
                _detalhe += "55";

                //Conta Corrente ==> 025 - 035
                _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia + boleto.Cedente.ContaBancaria.Conta + boleto.Cedente.ContaBancaria.DigitoConta, 11, 11, '0', 0, true, true, true);

                //Brancos ==> 036 - 037
                _detalhe += "  ";

                //Controle do Participante  ==> 038 - 062
                _detalhe += Utils.FitStringLength(boleto.NumeroDocumento, 25, 25, ' ', 0, true, true, false); 

                //Nosso Número==> 063 - 073
                _detalhe += Utils.FitStringLength(boleto.NossoNumero + Mod11Hsbc(boleto.NossoNumero, 7).ToString(), 11, 11, '0', 0, true, true, false);

                if (boleto.OutrosDescontos > 0)
                {
                    //Desconto Data-(2) ==> 074 - 079
                    _detalhe += boleto.DataOutrosDescontos.ToString("ddMMyy");

                    //Valor do Desconto-(2) ==> 080 - 090
                    _detalhe += Utils.FitStringLength(boleto.OutrosDescontos.ToString("f").Replace(",", "").Replace(".", ""), 11, 11, '0', 0, true, true, true);
                }
                else
                {
                    //Desconto Data-(2) ==> 074 - 079
                    //Valor do Desconto-(2) ==> 080 - 090
                    _detalhe += new string('0', 17);
                }

                //Desconto Data-(3) ==> 091 - 096
                //Valor do Desconto-(3) ==> 097 - 107
                _detalhe += new string('0', 17);

                //Carteira ==> 108 - 108
                _detalhe += Utils.FitStringLength(boleto.Carteira, 1, 1, '0', 0, true, true, true);

                //Código da Ocorrência ==> 109 - 110
                _detalhe += Utils.FitStringLength(boleto.Remessa.CodigoOcorrencia, 2, 2, '0', 0, true, true, true);

                // Seu Número ==> 111 - 120
                _detalhe += Utils.FitStringLength(boleto.NumeroDocumento, 10, 10, ' ', 0, true, true, false);

                //Vencimento ==> 121 - 126
                _detalhe += boleto.DataVencimento.ToString("ddMMyy");

                //Valor do Boleto ==> 127 - 139
                _detalhe += Utils.FitStringLength(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 13, 13, '0', 0, true, true, true);

                //Banco Cobrador
                _detalhe += Utils.FitStringLength(boleto.Banco.Codigo.ToString(), 3, 3, '0', 0, true, true, true);

                //Agência Depositária ==> 143 - 147
                _detalhe += new string('0', 5);

                //Espécie ==> 148 - 149
                _detalhe += "";
                switch (boleto.EspecieDocumento.Codigo)
                {
                    case "2": //DuplicataMercantil, 
                        _detalhe += "01";
                        break;
                    case "12": //NotaPromissoria
                    case "13": //NotaPromissoriaRural
                        _detalhe += "02";
                        break;
                    case "16": //NotaSeguro
                        _detalhe += "03";
                        break;
                    case "17": //Recibo
                        _detalhe += "05";
                        break;
                    case "4": //DuplicataServico
                        _detalhe += "10";
                        break;
                    case "98":  //Cobrança com emissão total do bloqueto pelo cliente
                        _detalhe += "98"; //Cobrança com emissão total do bloqueto pelo cliente
                        break;
                }

                //Aceite ==> 150 - 150
                _detalhe += "A";

                //Data de Emissao ==> 151 - 156
                _detalhe += boleto.DataDocumento.ToString("ddMMyy");

                //Instrução 01 ==> 157 - 158
                if (boleto.Instrucoes != null && boleto.Instrucoes.Count > 0)
                    _detalhe += Utils.FitStringLength(boleto.Instrucoes[0].Codigo.ToString(), 2, 2, '0', 0, true, true, true);
                else
                    _detalhe += "00";

                //Instrução 02 ==> 159 - 160
                if (boleto.Instrucoes != null && boleto.Instrucoes.Count > 1)
                    _detalhe += Utils.FitStringLength(boleto.Instrucoes[1].Codigo.ToString(), 2, 2, '0', 0, true, true, true);
                else
                    _detalhe += "00";

                //Juros de Mora ==> 161 - 173
                if (boleto.JurosMora > 0)
                    _detalhe += Utils.FitStringLength(boleto.JurosMora.ToString("f").Replace(",", "").Replace(".", ""), 13, 13, '0', 0, true, true, true);
                else if (boleto.PercJurosMora > 0)
                    _detalhe += Utils.FitStringLength(boleto.JurosMora.ToString("f").Replace(",", "").Replace(".", ""), 13, 13, '0', 0, true, true, true);
                else
                    _detalhe += new string('0', 13);

                //Desconto Data ==> 174 - 179
                _detalhe += "999999";

                //Valor do Desconto ==> 180 - 192
                _detalhe += Utils.FitStringLength(boleto.ValorDesconto.ToString("f").Replace(",", "").Replace(".", ""), 13, 13, '0', 0, true, true, true);

                //Valor do IOF ==> 193 - 205
                _detalhe += new string(' ', 13);

                //Valor do Abatimento ==> 206 - 218 (preencher somente quando o codigo da ocorrencia for 4 ou 5)
                _detalhe += new string('0', 13);

                //Código de Inscrição ==>   219 - 220
                if (boleto.Sacado.CPFCNPJ.Length <= 11)
                    _detalhe += "01";  // CPF
                else
                    _detalhe += "02"; // CNPJ

                //Número de Inscrição ==> 221 - 234
                _detalhe += Utils.FitStringLength(boleto.Sacado.CPFCNPJ, 14, 14, '0', 0, true, true, true).ToUpper();

                //Nome do Pagador ==> 235 - 274
                _detalhe += Utils.FitStringLength(boleto.Sacado.Nome.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();

                //Endereço do Pagador ==> 275 - 312
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.End.TrimStart(' '), 38, 38, ' ', 0, true, true, false).ToUpper();

                //Instrução de não recebimento do boleto ==> 313 - 314
                _detalhe += "  ";

                //Bairro do Pagador ==> 315 - 326
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Bairro.TrimStart(' '), 12, 12, ' ', 0, true, true, false).ToUpper();

                //CEP do Pagador ==>  327 - 334
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.CEP, 8, 8, ' ', 0, true, true, true);

                //Município do Pagador ==> 335 - 349
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Cidade.TrimStart(' '), 15, 15, ' ', 0, true, true, false).ToUpper();

                //UF Estado do Pagador ==> 350 - 351
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.UF, 2, 2, ' ', 0, true, true, false).ToUpper();

                //Sacador / Avalista ==> 352 - 390
                _detalhe += new string(' ', 39);

                //Tipo de Boleto ==> 391 - 391
                _detalhe += " ";

                //Prazo de Protesto/SERASA ==> 392 - 393
                _detalhe += "  ";

                //Moeda ==> 394 - 394
                _detalhe += "9";

                //Número sequencial do registro no arquivo ==> 395 - 400
                _detalhe += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);

                _detalhe = Utils.SubstituiCaracteresEspeciais(_detalhe);

                return _detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB400.", ex);
            }
        }

        public override string GerarMensagemVariavelRemessa(Boleto boleto, ref int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _detalhe = "";

                switch (tipoArquivo)
                {
                    case TipoArquivo.CNAB240:
                        throw new Exception("Mensagem Variavel nao existe para o tipo CNAB 240.");
                    case TipoArquivo.CNAB400:
                        _detalhe = ""; //GerarMensagemVariavelRemessaCNAB400(boleto, ref numeroRegistro, tipoArquivo);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return _detalhe;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do DETALHE arquivo de REMESSA.", ex);
            }
        }

        private string GerarMensagemVariavelRemessaCNAB400(Boleto boleto, ref int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {

                string _detalhe = "";

                foreach (var _instrucao in boleto.Instrucoes)
                {

                    if (!string.IsNullOrEmpty(_detalhe))
                        _detalhe += Environment.NewLine;

                    //Código do registro = 2 (Recibo do Sacado) 3, 4, 5, 6 e 7 (Ficha de Compensação) ==> 001 - 001
                    _detalhe += "2";

                    //Uso do Banco ==> 002 - 017
                    _detalhe += new string(' ', 16);

                    //Código da Agência Cedente ==> 018 - 021
                    _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true);

                    //Conta Movimento Cedente ==> 022 - 029
                    _detalhe += Utils.FitStringLength(boleto.Cedente.Codigo.ToString(), 8, 8, '0', 0, true, true, true);

                    //Conta Cobrança Cedente ==> 030 - 037
                    if (boleto.Cedente.ContaBancaria.Conta.Length == 9 || (!String.IsNullOrEmpty(boleto.Cedente.ContaBancaria.DigitoConta) && boleto.Cedente.ContaBancaria.Conta.Length == 8))
                        _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta.Substring(0, 7), 8, 8, '0', 0, true, true, true);
                    else
                        _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 8, 8, '0', 0, true, true, true);

                    //Uso do Banco ==> 038 - 047
                    _detalhe += new string(' ', 10);

                    //Sub-sequência do registro ==> 048 - 049
                    _detalhe += "01";

                    //Mensagem variável por título ==> 050 - 099
                    _detalhe += Utils.FitStringLength(_instrucao.Descricao, 50, 50, ' ', 0, true, true, false);

                    //Uso do Banco ==> 100 - 382
                    _detalhe += new string(' ', 283);

                    //Identificador do Complemento ==> 383 - 383
                    _detalhe += "I";

                    //Complemento ==> 385 - 384
                    _detalhe += boleto.Cedente.ContaBancaria.Conta.Substring(boleto.Cedente.ContaBancaria.Conta.Length - 1, 1) + boleto.Cedente.ContaBancaria.DigitoConta;

                    //Brancos ==> 386 - 394
                    _detalhe += new string(' ', 9);

                    //Número sequêncial do registro no arquivo ==> 395 - 400
                    _detalhe += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);
                    numeroRegistro++;

                }

                int CodigoRegistroSacado = 5;
                foreach (var _instrucao in boleto.Sacado.Instrucoes)
                {
                    if (CodigoRegistroSacado > 7)
                        throw new Exception("So pode ter 3 mensagens no recibo do sacdo.");

                    if (!string.IsNullOrEmpty(_detalhe))
                        _detalhe += Environment.NewLine;

                    _detalhe += Environment.NewLine;

                    //((Instrucao_Santander)_instrucao).

                    //Código do registro = 2 (Recibo do Sacado) 3, 4, 5, 6 e 7 (Ficha de Compensação) ==> 001 - 001
                    _detalhe += CodigoRegistroSacado.ToString();
                    CodigoRegistroSacado++;

                    //Uso do Banco ==> 002 - 017
                    _detalhe += new string(' ', 16);

                    //Código da Agência Cedente ==> 018 - 021
                    _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true);

                    //Conta Movimento Cedente ==> 022 - 029
                    _detalhe += Utils.FitStringLength(boleto.Cedente.Codigo.ToString(), 8, 8, '0', 0, true, true, true);

                    //Conta Cobrança Cedente ==> 030 - 037
                    if (boleto.Cedente.ContaBancaria.Conta.Length == 9 || (!String.IsNullOrEmpty(boleto.Cedente.ContaBancaria.DigitoConta) && boleto.Cedente.ContaBancaria.Conta.Length == 8))
                        _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta.Substring(0, 7), 8, 8, '0', 0, true, true, true);
                    else
                        _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 8, 8, '0', 0, true, true, true);

                    //Uso do Banco ==> 038 - 047
                    _detalhe += new string(' ', 10);

                    //Sub-sequência do registro ==> 048 - 049
                    _detalhe += "01";

                    //Mensagem variável por título ==> 050 - 099

                    //Uso do Banco ==> 100 - 382
                    _detalhe += new string(' ', 283);

                    //Identificador do Complemento ==> 383 - 383
                    _detalhe += "I";

                    //Complemento ==> 385 - 384
                    _detalhe += boleto.Cedente.ContaBancaria.Conta.Substring(boleto.Cedente.ContaBancaria.Conta.Length - 1, 1) + boleto.Cedente.ContaBancaria.DigitoConta;

                    //Brancos ==> 386 - 394
                    _detalhe += new string(' ', 9);

                    //Número sequêncial do registro no arquivo ==> 395 - 400
                    _detalhe += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);
                    numeroRegistro++;

                }

                _detalhe = Utils.SubstituiCaracteresEspeciais(_detalhe);

                return _detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB400.", ex);
            }
        }
        
        # endregion DETALHE

        # region TRAILER

        /// <summary>
        /// TRAILER do arquivo CNAB
        /// Gera o TRAILER do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal)
        {
            try
            {
                string _trailer = " ";

                base.GerarTrailerRemessa(numeroRegistro, tipoArquivo, cedente, vltitulostotal);

                switch (tipoArquivo)
                {
                    case TipoArquivo.CNAB240:
                        _trailer = GerarTrailerRemessa240();
                        break;
                    case TipoArquivo.CNAB400:
                        _trailer = GerarTrailerRemessa400(numeroRegistro);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return _trailer;

            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }

        public string GerarTrailerRemessa240()
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public string GerarTrailerRemessa400(int numeroRegistro)
        {
            try
            {
                string _trailer;

                //Código do registro ==> 001 - 001
                _trailer = "9";

                //Brancos ==> 002 - 394
                _trailer += new string(' ', 393);

                //Número sequencial do registro no arquivo ==> 395 - 400
                _trailer += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);

                _trailer = Utils.SubstituiCaracteresEspeciais(_trailer);

                return _trailer;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do arquivo de REMESSA.", ex);
            }
        }

        # endregion

        #endregion

        #region Métodos de processamento do arquivo retorno CNAB400

        /// <summary>
        /// Verifica o tipo de ocorrência para o arquivo remessa
        /// </summary>
        public string OcorrenciaCSB(string codigo)
        {
            switch (codigo)
            {
                case "02":
                    return "Entrada confirmada";
                case "03":
                    return "Entrada rejeitada ou Instrução rejeitada";
                case "06":
                    return "Liquidação normal em dinheiro";
                case "07":
                    return "Liquidação por conta em dinheiro";
                case "09":
                    return "Baixa automática";
                case "10":
                    return "Baixado conforme instruções";
                case "11":
                    return "Títulos em ser (Conciliação Mensal)";
                case "12":
                    return "Abatimento concedido";
                case "13":
                    return "Abatimento cancelado";
                case "14":
                    return "Vencimento prorrogado";
                case "15":
                    return "Liquidação em cartório em dinheiro";
                case "16":
                    return "Liquidação baixado/devolvido em data anterior dinheiro";
                case "17":
                    return "Entregue em cartório";
                case "18":
                    return "Instrução automática de protesto/serasa";
                case "21":
                    return "Instrução de alteração de mora";
                case "22":
                    return "Instrução de protesto/serasa processada/reemitida";
                case "23":
                    return "Cancelamento de protesto/serasa processado";
                case "27":
                    return "Número do cedente ou controle do participante alterado";
                case "31":
                    return "Liquidação normal em cheque/compensação/banco correspondente";
                case "32":
                    return "Liquidação em cartório em cheque";
                case "33":
                    return "Liquidação por conta em cheque";
                case "36":
                    return "Liquidação baixado/devolvido em data anterior em cheque";
                case "37":
                    return "Baixa de título protestado";
                case "38":
                    return "Liquidação de título não registrado em dinheiro";
                case "39":
                    return "Liquidação de título não registrado em cheque";
                case "49":
                    return "Vencimento alterado";
                case "51":
                    return "Título DDA aceito pelo sacado";
                case "52":
                    return "Título DDA não reconhecido pelo sacado";
                case "69":
                    return "Despesas/custas de cartório";
                case "70":
                    return "Ressarcimento sobre títulos";
                case "71":
                    return "Ocorrência/Instrução não permitida para título em garantia de operação";
                case "72":
                    return "Concessão de Desconto Aceito";
                case "73":
                    return "Cancelamento Condição de Desconto Fixo Aceito";
                case "74":
                    return "Cancelamento de Desconto Diário Aceito";
                default:
                    return "";
            }
        }

        public string MotivosRejeicaoCSB(string codigo)
        {
            switch (codigo)
            {
                case "01":
                    return "Valor do desconto não informado/inválido";
                case "02":
                    return "Inexistência de agência do HSBC na praça do sacado";
                case "03":
                    return "CEP do sacado incorreto ou inválido";
                case "04":
                    return "Cadastro do cedente não aceita banco correspondente";
                case "05":
                    return "Tipo de moeda inválido";
                case "06":
                    return "Prazo de protesto/serasa indefinido (não informado)/inválido ou prazo de protesto/serasa inferior ao tempo decorrido da data de vencimento em relação ao envio da instrução de alteração de prazo";
                case "07":
                    return "Data do vencimento inválida";
                case "08":
                    return "Nosso número(número bancário) utilizado não possui vinculação com a conta cobrança";
                case "09":
                    return "Taxa mensal de mora acima do permitido (170%)";
                case "10":
                    return "Taxa de multa acima do permitido (10% ao mês)";
                case "11":
                    return "Data limite de desconto inválida";
                case "12":
                    return "CEP Inválido/Inexistência de Ag HSBC";
                case "13":
                    return "Valor/Taxa de multa inválida";
                case "14":
                    return "Valor diário da multa não informado";
                case "15":
                    return "Quantidade de dias após vencimento para incidência da multa não informada";
                case "16":
                    return "Outras irregularidades";
                case "17":
                    return "Data de início da multa inválida";
                case "18":
                    return "Nosso número (número bancário) já existente para outro título";
                case "19":
                    return "Valor do título inválido";
                case "20":
                    return "Ausência CEP/Endereço/CNPJ ou Sacador Avalista";
                case "21":
                    return "Título sem borderô";
                case "22":
                    return "Número da conta do cedente não cadastrado";
                case "23":
                    return "Instrução não permitida para título em garantia de operação";
                case "24":
                    return "Condição de desconto não permitida para titulo em garantia de Operação";
                case "25":
                    return "Utilizada mais de uma instrução de multa";
                case "26":
                    return "Ausência do endereço do sacado";
                case "27":
                    return "CEP inválido.do sacado";
                case "28":
                    return "Ausência do CPF/CNPJ do sacado em título com instrução de protesto";
                case "29":
                    return "Agência cedente informada inválida";
                case "30":
                    return "Número da conta do cedente inválido";
                case "31":
                    return "Contrato garantia não cadastrado/inválido";
                case "32":
                    return "Tipo de carteira inválido";
                case "33":
                    return "Conta corrente do cedente não compatível com o órgão do contratante";
                case "34":
                    return "Faixa de aplicação não cadastrada/inválida";
                case "35":
                    return "Nosso número (número bancário) inválido";
                case "36":
                    return "Data de emissão do título inválida";
                case "37":
                    return "Valor do título acima de R$ 5.000.000,00 (Cinco milhões de reais)";
                case "38":
                    return "Data de desconto menor que data da emissão";
                case "39":
                    return "Espécie inválida";
                case "40":
                    return "Ausência no nome do sacador avalista";
                case "41":
                    return "Data de início de multa menor que data de emissão";
                case "42":
                    return "Quantidade de moeda variável inválida";
                case "43":
                    return "Controle do participante inválido";
                case "44":
                    return "Nosso número (número bancário) duplicado no mesmo movimento";
                case "45":
                    return "Título não aceito para compor a carteira de garantias";
                case "50":
                    return "Título liquidado";
                case "51":
                    return "Data de emissão da ocorrência inválida";
                case "52":
                    return "Nosso número (número bancário) duplicado";
                case "53":
                    return "Código de ocorrência comandada inválido";
                case "54":
                    return "Valor do desconto concedido inválido";
                case "55":
                    return "Data de prorrogação de vencimento não informada";
                case "56":
                    return "Outras irregularidades";
                case "57":
                    return "Ocorrência não permitida para título em garantia de operações";
                case "58":
                    return "Nosso número (número bancário) comandado na instrução/ocorrência não possui vinculação com a conta cobrança";
                case "59":
                    return "Nosso número (número bancário) comandado na baixa não possui vinculação com a conta cobrança";
                case "60":
                    return "Valor do desconto igual ou maior que o valor do título";
                case "61":
                    return "Titulo com valor em moeda variável não permite condição de desconto";
                case "62":
                    return "Data do desconto informada não coincide com o registro do título";
                case "63":
                    return "Titulo não possui condição de desconto diário";
                case "64":
                    return "Título baixado";
                case "65":
                    return "Título devolvido";
                case "66":
                    return "Valor do título não confere com o registrado";
                case "67":
                    return "Nosso número (número bancário) não informado";
                case "68":
                    return "Nosso número (número bancário) inválido";
                case "69":
                    return "Concessão de abatimento não é permitida para moeda diferente de Real";
                case "70":
                    return "Valor do abatimento concedido inválido(Valor do abatimento zerado, maior ou igual ao valor do título)";
                case "71":
                    return "Cancelamento comandado sobre título sem abatimento";
                case "72":
                    return "Concessão de desconto não é permitida para moeda diferente de real";
                case "73":
                    return "Valor do desconto não informado";
                case "74":
                    return "Cancelamento comandado sobre título sem desconto";
                case "75":
                    return "Data de vencimento alterado inválida";
                case "76":
                    return "Data de prorrogação de vencimento inválida";
                case "77":
                    return "Data da instrução inválida";
                case "78":
                    return "Protesto/Serasa comandado em duplicidade no mesmo dia";
                case "79":
                    return "Título não possui instrução de protesto/serasa ou está com entrada já confirmada em cartório";
                case "80":
                    return "Título não possui condição de desconto";
                case "81":
                    return "Título não possui instrução de abatimento";
                case "82":
                    return "Valor de juros inválido";
                case "83":
                    return "Nosso número (número bancário) inexistente";
                case "84":
                    return "Baixa/liquidação por órgão não autorizado";
                case "85":
                    return "Instrução de protesto/serasa recusada/inválida";
                case "86":
                    return "Instrução não permitida para banco correspondente";
                case "87":
                    return "Valor da instrução inválido";
                case "88":
                    return "Instrução inválida para tipo de carteira";
                case "89":
                    return "Valor do desconto informado não coincide com o registro do título";
                default:
                    return "";
            }
        }

        #region Método de leitura do arquivo retorno

        public override DetalheRetorno LerDetalheRetornoCNAB400(string registro)
        {
            try
            {
                int dataOcorrencia = Utils.ToInt32(registro.Substring(110, 6));
                int dataVencimento = Utils.ToInt32(registro.Substring(146, 6));
                int dataCredito;
                DetalheRetorno detalhe = new DetalheRetorno(registro);
                detalhe.CodigoInscricao = Utils.ToInt32(registro.Substring(1, 2));

                if (detalhe.CodigoInscricao != 99)
                {
                    dataCredito = dataOcorrencia;

                    detalhe.NumeroInscricao = registro.Substring(3, 14);
                    detalhe.Agencia = Utils.ToInt32(registro.Substring(18, 4));
                    detalhe.Conta = Utils.ToInt32(registro.Substring(24, 11));
                    detalhe.UsoEmpresa = registro.Substring(37, 25);
                    detalhe.Carteira = registro.Substring(107, 1);
                    detalhe.CodigoOcorrencia = Utils.ToInt32(registro.Substring(108, 2));
                    detalhe.DescricaoOcorrencia = this.OcorrenciaCSB(registro.Substring(108, 2));
                    detalhe.MotivosRejeicao = this.MotivosRejeicaoCSB(registro.Substring(301, 2));
                    detalhe.DataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));
                    detalhe.NumeroDocumento = registro.Substring(116, 10);
                    if (detalhe.NumeroDocumento.Contains("-"))
                        detalhe.NumeroDocumento = detalhe.NumeroDocumento.Substring(0, detalhe.NumeroDocumento.LastIndexOf('-'));
                    detalhe.NossoNumero = registro.Substring(126, 10);
                    detalhe.DataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));
                    decimal valorTitulo = Convert.ToInt64(registro.Substring(152, 13));
                    detalhe.ValorTitulo = valorTitulo / 100;
                    detalhe.CodigoBanco = Utils.ToInt32(registro.Substring(165, 3));
                    detalhe.AgenciaCobradora = Utils.ToInt32(registro.Substring(168, 5));
                    detalhe.Especie = Utils.ToInt32(registro.Substring(173, 2));
                    decimal tarifaCobranca = Convert.ToUInt64(registro.Substring(175, 13));
                    detalhe.TarifaCobranca = tarifaCobranca / 100;
                    decimal valorAbatimento = Convert.ToUInt64(registro.Substring(227, 13));
                    detalhe.ValorAbatimento = valorAbatimento / 100;
                    decimal valorPrincipal = Convert.ToUInt64(registro.Substring(253, 13));
                    detalhe.ValorPrincipal = valorPrincipal / 100;
                    detalhe.ValorPago = detalhe.ValorPrincipal;
                    decimal jurosMora = Convert.ToUInt64(registro.Substring(266, 13));
                    detalhe.JurosMora = jurosMora / 100;
                    detalhe.DataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));
                    detalhe.DataCredito = Utils.ToDateTime(dataCredito.ToString("##-##-##"));
                    detalhe.InstrucaoCancelada = Utils.ToInt32(registro.Substring(301, 4));
                    detalhe.NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6));
                }
                else
                {
                    dataCredito = Utils.ToInt32(registro.Substring(82, 6));

                    detalhe.NumeroInscricao = registro.Substring(3, 14);
                    detalhe.Agencia = Utils.ToInt32(registro.Substring(17, 5));
                    detalhe.Conta = Utils.ToInt32(registro.Substring(24, 11));
                    detalhe.UsoEmpresa = registro.Substring(37, 16);
                    detalhe.NossoNumeroComDV = registro.Substring(62, 16);
                    detalhe.NossoNumero = registro.Substring(62, 15); //Sem o DV
                    detalhe.Carteira = registro.Substring(107, 1);
                    detalhe.CodigoOcorrencia = Utils.ToInt32(registro.Substring(108, 2));
                    detalhe.DataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));
                    detalhe.NumeroDocumento = registro.Substring(116, 6);
                    if (detalhe.NumeroDocumento.Contains("-"))
                        detalhe.NumeroDocumento = detalhe.NumeroDocumento.Substring(0, detalhe.NumeroDocumento.LastIndexOf('-'));
                    detalhe.DataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));
                    decimal valorTitulo = Convert.ToInt64(registro.Substring(152, 11));
                    detalhe.ValorTitulo = valorTitulo / 100;
                    detalhe.CodigoBanco = Utils.ToInt32(registro.Substring(165, 3));
                    detalhe.AgenciaCobradora = Utils.ToInt32(registro.Substring(168, 5));
                    detalhe.Especie = Utils.ToInt32(registro.Substring(173, 2));
                    decimal iof = Convert.ToUInt64(registro.Substring(175, 9));
                    detalhe.IOF = iof / 100;
                    decimal valorPrincipal = Convert.ToUInt64(registro.Substring(253, 11));
                    detalhe.ValorPrincipal = valorPrincipal / 100;
                    detalhe.ValorPago = detalhe.ValorPrincipal;
                    decimal jurosMora = Convert.ToUInt64(registro.Substring(266, 11));
                    detalhe.JurosMora = jurosMora / 100;
                    detalhe.DataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));
                    detalhe.DataCredito = Utils.ToDateTime(dataCredito.ToString("##-##-##"));
                    detalhe.InstrucaoCancelada = Utils.ToInt32(registro.Substring(301, 4));
                    detalhe.NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6));
                }

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        #endregion

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

