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
                        string.Format("{0}{1}{2}{3}{4}{5}{6}{7}001", Codigo, boleto.Moeda,
                                      FatorVencimento(boleto), valorBoleto, boleto.NossoNumero + _dacNossoNumero,
                                      Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4),
                                      Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 5),
                                      Utils.FormatCode(boleto.Cedente.ContaBancaria.DigitoConta, 2));
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

        # region Métodos de geração do arquivo remessa CNAB400

        # region HEADER

        /// <summary>
        /// HEADER do arquivo CNAB
        /// Gera o HEADER do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            try
            {
                string _header = " ";

                base.GerarHeaderRemessa(numeroConvenio, cedente, tipoArquivo, numeroArquivoRemessa);

                switch (tipoArquivo)
                {
                    case TipoArquivo.CNAB400:
                        _header = GerarHeaderRemessaCNAB400(cedente, numeroArquivoRemessa);
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

        public string GerarHeaderRemessaCNAB400(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                string _header = "";
                _header += "0";
                _header += "1";
                _header += "REMESSA";
                _header += "01";
                _header += Utils.FitStringLength("COBRANCA", 15, 15, ' ', 0, true, true, false);
                _header += "0";
                _header += Utils.FitStringLength(cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true);
                _header += "55";
                _header += Utils.FitStringLength(cedente.ContaBancaria.Conta, 11, 11, '0', 0, true, true, true);
                _header += "  ";
                _header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false);
                _header += "399";
                _header += Utils.FitStringLength("HSBC", 15, 15, ' ', 0, true, true, false);
                _header += DateTime.Now.ToString("ddMMyy");
                _header += "01600";
                _header += "BPI";
                _header += "  ";
                _header += "LANCV08";
                _header += Utils.FitStringLength(string.Empty, 277, 277, ' ', 0, true, true, false);
                _header += "000001";

                _header = Utils.SubstituiCaracteresEspeciais(_header);

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
                string _detalhe = " ";

                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                switch (tipoArquivo)
                {
                    case TipoArquivo.CNAB400:
                        _detalhe = GerarDetalheRemessaCNAB400(boleto, numeroRegistro, tipoArquivo);
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

        public string GerarDetalheRemessaCNAB240()
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public string GerarDetalheRemessaCNAB400(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string vCpfCnpjEmi = "00";
                if (boleto.Cedente.CPFCNPJ.Length.Equals(11))
                    vCpfCnpjEmi = "01"; //Cpf é sempre 11;
                else if (boleto.Cedente.CPFCNPJ.Length.Equals(14))
                    vCpfCnpjEmi = "02"; //Cnpj é sempre 14;
                else
                    vCpfCnpjEmi = "99";
                
                 string vCpfCnpjSac = "00";
                if (boleto.Sacado.CPFCNPJ.Length.Equals(11)) 
                    vCpfCnpjSac = "01"; //Cpf é sempre 11;
                else if (boleto.Sacado.CPFCNPJ.Length.Equals(14))
                    vCpfCnpjSac = "02"; //Cnpj é sempre 14;
                else 
                    vCpfCnpjSac = "99"; //outros;




                string _detalhe = "";
                _detalhe += "1";
                _detalhe += vCpfCnpjEmi;
                _detalhe += Utils.FitStringLength(boleto.Cedente.CPFCNPJ, 14, 14, '0', 0, true, true, true);
                _detalhe += "0";
                _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true);
                _detalhe += "55";
                _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 11, 11, '0', 0, true, true, true);
                _detalhe += "  ";
                _detalhe += Utils.FitStringLength(boleto.NumeroDocumento, 25, 25, ' ', 0, true, true, false);
                _detalhe += Utils.FitStringLength(boleto.NossoNumero, 11, 11, ' ', 0, true, true, false);
                _detalhe += boleto.DataVencimento.ToString("ddMMyy");                
                _detalhe += Utils.FitStringLength(boleto.ValorDesconto.ToString("0.00").Replace(",", ""), 11, 11, '0', 0, true, true, true);
                _detalhe += boleto.DataVencimento.ToString("ddMMyy");
                _detalhe += Utils.FitStringLength(boleto.ValorDesconto.ToString("0.00").Replace(",", ""), 11, 11, '0', 0, true, true, true);
                _detalhe += "1";
                _detalhe += "01";
                _detalhe += Utils.FitStringLength(string.Empty, 10, 10, ' ', 0, true, true, false);
                _detalhe += boleto.DataVencimento.ToString("ddMMyy");
                _detalhe += Utils.FitStringLength(boleto.ValorBoleto.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true);
                _detalhe += "399";
                _detalhe += "00000";
                if (boleto.EspecieDocumento.Sigla == "PD")
                    _detalhe += "98"; //98-PD - Cobrança com emissão total do bloqueto pelo cliente.
                else
                    _detalhe += "01"; //01-DP – Duplicata Mercantil 
                _detalhe += "A";
                _detalhe += DateTime.Today.ToString("ddMMyy");

                //
                string vInstrucao1 = "";
                string vInstrucao2 = "";
                //string vInstrucao3 = "0";
                switch (boleto.Instrucoes.Count)
                {
                    case 1:
                        vInstrucao1 = boleto.Instrucoes[0].Codigo.ToString();
                        break;
                    case 2:
                        vInstrucao1 = boleto.Instrucoes[0].Codigo.ToString();
                        vInstrucao2 = boleto.Instrucoes[1].Codigo.ToString();
                        break;
                }
                _detalhe += Utils.FitStringLength(vInstrucao1, 2, 2, ' ', 0, true, true, false);
                _detalhe += Utils.FitStringLength(vInstrucao2, 2, 2, ' ', 0, true, true, false);

                _detalhe += Utils.FitStringLength(boleto.JurosMora.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true);
                _detalhe += boleto.DataVencimento.ToString("ddMMyy");
                _detalhe += Utils.FitStringLength(boleto.ValorDesconto.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true);
                _detalhe += Utils.FitStringLength(boleto.IOF.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true);
                _detalhe += Utils.FitStringLength("0", 13, 13, '0', 0, true, true, true);
                _detalhe += vCpfCnpjSac;

                _detalhe += Utils.FitStringLength(boleto.Sacado.CPFCNPJ, 14, 14, ' ', 0, true, true, false);
                _detalhe += Utils.FitStringLength(boleto.Sacado.Nome, 40, 40, ' ', 0, true, true, false);
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.End, 38, 38, ' ', 0, true, true, false);
                _detalhe += "  ";

                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Bairro, 12, 12, ' ', 0, true, true, false);
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.CEP, 8, 8, ' ', 0, true, true, false);
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Cidade, 15, 15, ' ', 0, true, true, false);
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.UF, 2, 2, ' ', 0, true, true, false);
                _detalhe += Utils.FitStringLength(string.Empty, 39, 39, ' ', 0, true, true, false);
                _detalhe += " ";
                _detalhe += "  ";
                _detalhe += boleto.Moeda;
                _detalhe += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);


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
                throw new Exception("Erro durante a geração do TRAILER do arquivo de REMESSA.", ex);
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
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0001, 001, 0, "9", ' '));            //001-001
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0002, 393, 0, string.Empty, ' '));   //002-393
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0395, 006, 0, numeroRegistro, '0')); //395-400
                //
                reg.CodificarLinha();
                //
                string vLinha = reg.LinhaRegistro;
                string _trailer = Utils.SubstituiCaracteresEspeciais(vLinha);
                //
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

