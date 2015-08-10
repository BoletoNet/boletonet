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

                //Verifica se o tamanho para o NossoNumero são 10 dígitos (5 range + 5 numero sequencial)
                if (tamanhoNossoNumero > 10)
                    throw new NotImplementedException("A quantidade de dígitos do nosso número para a carteira " + boleto.Carteira + ", são 10 números.");

                if (tamanhoNossoNumero < 10)
                    boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 10);

                // Calcula o DAC do Nosso Número
                // Nosso Número = Range(5) + Numero Sequencial(5)
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


                _dacBoleto = Mod11Hsbc(boleto.CodigoBarra.Codigo, 9, 0);

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

                    case "CNR": boleto.NossoNumero = string.Format("{0}{1}4{2}", boleto.NossoNumero, Mod11Base9(boleto.NossoNumero).ToString(), Mod11Base9((int.Parse(boleto.NossoNumero + Mod11Base9(boleto.NossoNumero).ToString() + "4") + int.Parse(boleto.Cedente.Codigo.ToString()) + int.Parse(boleto.DataVencimento.ToString("ddMMyy"))).ToString())); break;
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
			try
			{
				string _header = " ";

				//base.GerarHeaderRemessa(numeroConvenio, cedente, tipoArquivo, numeroArquivoRemessa);

				switch (tipoArquivo)
				{

					case TipoArquivo.CNAB240:
						_header = GerarHeaderRemessaCNAB240(numeroConvenio, cedente, numeroArquivoRemessa);
						break;
					case TipoArquivo.CNAB400:
						_header = GerarHeaderRemessaCNAB400();
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

		public string GerarHeaderRemessaCNAB240(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
		{
			try
			{
				string registro;
				// O sistema não aceita contas com dois dígitos. Quando existe salva-se conta+dígito juntos no campo conta.
				// Esse tratamento visa verificar se isso ocorreu para esta conta.
				string contaBancaria = cedente.ContaBancaria.Conta;
				string digitoContaBancaria = cedente.ContaBancaria.DigitoConta;
				string contaBancariaComDigito = contaBancaria + digitoContaBancaria;

				if (contaBancaria.Contains("-"))
				{
					string[] contaComDigitoArray = cedente.ContaBancaria.Conta.Split('-');
					contaBancaria = contaComDigitoArray[0];
					digitoContaBancaria = contaComDigitoArray[1];
					contaBancariaComDigito = contaBancaria + digitoContaBancaria;
				}

				registro = "399";                                                                                                               // Código do banco
				registro += "0000";                                                                                                             // Sequencial do Lote de Serviço - 0000 para header
				registro += "0";                                                                                                                // Tipo de registro - 0 para header
				registro += new string(' ', 9);                                                                                                 // Brancos
				registro += (cedente.CPFCNPJ.Length <= 11) ? "1" : "2";                                                                         // Tipo de inscrição: 1 - Física | 2 - Jurídica
				registro += Utils.FitStringLength(cedente.CPFCNPJ, 14, 14, '0', 0, true, true, true);                                           // CPF ou CNPJ
				registro += "COB";                                                                                                              // Convênio - Código do aplicativo no banco
				registro += "CNAB";                                                                                                             // Convênio - Fixo literal igual à CNAB
				registro += Utils.FitStringLength(cedente.ContaBancaria.Agencia + contaBancariaComDigito, 13, 13, '0', 0, true, true, true);    // Convênio - Código da cobrança
				registro += Utils.FitStringLength(cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true);                               // Número da agência da conta bancária
				registro += Utils.FitStringLength(cedente.ContaBancaria.DigitoAgencia, 1, 1, '0', 0, true, true, false);                        // Dígito da agência da conta bancária
				registro += Utils.FitStringLength(contaBancariaComDigito, 12, 12, '0', 0, true, true, true);                                    // Número da conta bancária
				registro += "0";                                                                                                                // Dígito da conta bancária (Orientação do banco: colocar a conta com o dígito no campo anterior e preencher esse campo com 0)
				registro += "0";                                                                                                                // Dígito verificador da Agência/Conta Bancária
				registro += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false);                                             // Nome da empresa
				registro += Utils.FitStringLength("BANCO HSBC", 30, 30, ' ', 0, true, true, false);                                             // Nome do banco
				registro += new string(' ', 10);                                                                                                // Brancos
				registro += "1";                                                                                                                // Código: 1 - Remessa | 2 - Retorno | 3 - Retorno Operação (Desc)
				registro += DateTime.Now.ToString("ddMMyyyy");                                                                                  // Data de geração
				registro += DateTime.Now.ToString("hhMMss");                                                                                    // Hora de geração
				registro += Utils.FitStringLength(numeroArquivoRemessa.ToString(), 6, 6, '0', 0, true, true, true);                             // Número sequencial do arquivo
				registro += "010";                                                                                                              // Número da versão do layout
				registro += "01600";                                                                                                            // Densidade de gravação do arquivo
				registro += "0";                                                                                                                // Duplic não aceitar - Envio para cart cobrança simples
				registro += new string(' ', 11);                                                                                                // Número contrato limite - Apenas para operações de desconto.
				registro += "0";                                                                                                                // Liberação automática Operação Desconto
				registro += new string(' ', 7);                                                                                                 // Reservado banco
				registro += new string(' ', 20);                                                                                                // Reservado empresa
				registro += new string(' ', 29);                                                                                                // Brancos - Uso exclusivo Febraban/CNAB

				registro = Utils.SubstituiCaracteresEspeciais(registro);


				return registro;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a geração do HEADER DE ARQUIVO do arquivo de REMESSA.", ex);
			}
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
		
		# region HEADER DE LOTE

		/// <summary>
		/// HEADER DE LOTE do arquivo CNAB
		/// Gera o HEADER de Lote do arquivo remessa de acordo com o lay-out informado
		/// </summary>
		public override string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa, TipoArquivo tipoArquivo)
		{
			try
			{
				string header = " ";

				base.GerarHeaderLoteRemessa(numeroConvenio, cedente, numeroArquivoRemessa, tipoArquivo);

				switch (tipoArquivo)
				{
					case TipoArquivo.CNAB240:
						header = GerarHeaderLoteRemessaCNAB240(numeroConvenio, cedente, numeroArquivoRemessa);
						break;
					case TipoArquivo.CNAB400:
						header = GerarHeaderLoteRemessaCNAB400();
						break;
					case TipoArquivo.Outro:
						throw new Exception("Tipo de arquivo inexistente.");
				}

				return header;

			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a geração do HEADER DO LOTE do arquivo de REMESSA.", ex);
			}
		}

		// HEADER DE LOTE REMESSA CNAB240
		private string GerarHeaderLoteRemessaCNAB240(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
		{
			try
			{

				// O sistema não aceita contas com dois dígitos. Quando existe salva-se conta+dígito juntos no campo conta.
				// Esse tratamento visa verificar se isso ocorreu para esta conta.
				string contaBancaria = cedente.ContaBancaria.Conta;
				string digitoContaBancaria = cedente.ContaBancaria.DigitoConta;
				string contaBancariaComDigito = contaBancaria + digitoContaBancaria;

				if (contaBancaria.Contains("-"))
				{
					string[] contaComDigitoArray = cedente.ContaBancaria.Conta.Split('-');
					contaBancaria = contaComDigitoArray[0];
					digitoContaBancaria = contaComDigitoArray[1];
					contaBancariaComDigito = contaBancaria + digitoContaBancaria;
				}

				string registro;

				registro = "399";                                                                                                               // Código do banco
				registro += "0001";                                                                                                             // Número sequencial do lote
				registro += "1";                                                                                                                // Código do Registro: 1 - Header de Lote
				registro += "R";                                                                                                                // Tipo de operação: R - Remessa
				registro += "01";                                                                                                               // Tipo de serviço
				registro += "00";                                                                                                               // Forma de lançamento
				registro += "010";                                                                                                              // Número da versão do layout do lote
				registro += " ";                                                                                                                // Uso exclusivo Febraban/CNAB
				registro += (cedente.CPFCNPJ.Length <= 11) ? "1" : "2";                                                                         // Tipo de inscrição: 1 - Física | 2 - Jurídica
				registro += Utils.FitStringLength(cedente.CPFCNPJ, 15, 15, '0', 0, true, true, true);                                           // CPF ou CNPJ
				registro += "COB";                                                                                                              // Convênio - Código do aplicativo no banco
				registro += "CNAB";                                                                                                             // Convênio - Fixo literal igual à CNAB
				registro += Utils.FitStringLength(cedente.ContaBancaria.Agencia + contaBancariaComDigito, 13, 13, '0', 0, true, true, true);    // Convênio - Código da cobrança
				registro += Utils.FitStringLength(cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true);                               // Número da agência da conta bancária
				registro += Utils.FitStringLength(cedente.ContaBancaria.DigitoAgencia, 1, 1, '0', 0, true, true, false);                        // Dígito da agência da conta bancária
				registro += Utils.FitStringLength(contaBancariaComDigito, 12, 12, '0', 0, true, true, true);                                    // Número da conta bancária
				registro += "0";                                                                                                                // Dígito da conta bancária (Orientação do banco: colocar a conta com o dígito no campo anterior e preencher esse campo com 0)
				registro += "0";                                                                                                                // Dígito verificador da Agência/Conta Bancária
				registro += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false);                                             // Nome da empresa
				registro += new string(' ', 40);                                                                                                // Brancos - Mensagem 1
				registro += new string(' ', 40);                                                                                                // Brancos - Mensagem 2
				registro += Utils.FitStringLength(numeroArquivoRemessa.ToString(), 8, 8, '0', 0, true, true, true);                             // Número sequencial do arquivo
				registro += DateTime.Now.ToString("ddMMyyyy");                                                                                  // Data de gravação
				registro += "00000000";                                                                                                         // Data do crédito
				registro += new string(' ', 11);                                                                                                // Número contrato limite - Apenas para operações de desconto.
				registro += new string(' ', 22);                                                                                                // Brancos - Uso exclusivo Febraban/CNAB

				registro = Utils.SubstituiCaracteresEspeciais(registro);

				return registro;

			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a geração do HEADER DE LOTE do arquivo de REMESSA.", ex);
			}
		}

		private string GerarHeaderLoteRemessaCNAB400()
		{
			throw new Exception("Função não implementada.");
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

		public override string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio)
		{
			try
			{
				string registro;

				// O sistema não aceita contas com dois dígitos. Quando existe salva-se conta+dígito juntos no campo conta.
				// Esse tratamento visa verificar se isso ocorreu para esta conta.
				string contaBancaria = boleto.Cedente.ContaBancaria.Conta;
				string digitoContaBancaria = boleto.Cedente.ContaBancaria.DigitoConta;

				if (contaBancaria.Contains("-"))
				{
					string contaDigito = boleto.Cedente.ContaBancaria.Conta.Replace("-", "");
					contaBancaria = contaDigito.Substring(0, contaDigito.Length-1);
					digitoContaBancaria = contaDigito.Substring(contaDigito.Length-1);
				}

				registro = "399";                                                                                                               // Código do banco
				registro += "0001";                                                                                                             // Número sequencial do lote
				registro += "3";                                                                                                                // Código do Registro: 1 - Header de Lote
				registro += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);                                   // Número sequencial do registro do lote
				registro += "P";                                                                                                                // Código do segmento do registro de detalhe
				registro += " ";                                                                                                                // Uso exclusivo Febraban/CNAB
				registro += "01";                                                                                                               // Código de movimento
				registro += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true);                        // Número da agência da conta bancária
				registro += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoAgencia, 1, 1, '0', 0, true, true, false);                 // Dígito da agência da conta bancária
				registro += Utils.FitStringLength(contaBancaria, 12, 12, '0', 0, true, true, true);                                             // Número da conta bancária
				registro += Utils.FitStringLength(digitoContaBancaria, 1, 1, '0', 0, true, true, false);                                        // Dígito da conta bancária
				registro += "0";                                                                                                                // Dígito verificador da Agência/Conta Bancária
                int dacNossoNumero = Mod11Hsbc(boleto.NossoNumero, 7);

				registro += Utils.FitStringLength(boleto.NossoNumero+dacNossoNumero.ToString(), 11, 11, '0', 0, true, true, true);              // Nosso número      
                registro += new string(' ', 9);                                                                                                 // Nosso número      
				registro += "1";                                                                                                                // Código da carteira: 1 - Cobrança Simples
				registro += "1";                                                                                                                // Forma de cadastramento do título no banco
				registro += "1";                                                                                                                // Tipo de documento
				registro += "2";                                                                                                                // Identificação da emissão do bloqueto: 2 - Cliente emite
				registro += "2";                                                                                                                // Identificação da distribuição do bloqueto: 2 - Cliente distribui
				registro += Utils.FitStringLength(boleto.NumeroDocumento, 15, 15, ' ', 0, true, true, false);                                   // Número do documento de cobrança
				registro += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, ' ', 0, true, true, false);                 // Data do vencimento
				registro += Utils.FitStringLength(boleto.ValorBoleto.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true);      // Valor nominal do título
				registro += "00000";                                                                                                            // Agência encarregada da cobrança
				registro += " ";                                                                                                                // Dígito verificador da agência acima
				registro += Utils.FitStringLength(boleto.EspecieDocumento.Codigo.ToString(), 2, 2, '0', 0, true, true, true);                   // Espécie do título
				registro += "N";                                                                                                                // Aceite
				registro += Utils.FitStringLength(boleto.DataDocumento.ToString("ddMMyyyy"), 8, 8, ' ', 0, true, true, false);                  // Data de emissão do título

				if (boleto.JurosMora > 0)
				{
					registro += "1";                                                                                                            // Código do juros de mora: 1 - Valor por dia
					registro += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false);             // Data do juros de mora
					registro += Utils.FitStringLength(boleto.JurosMora.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true);    // Valor do juros de mora
				}
				else
				{
					registro += "3";                                                                                                            // Código do juros de mora: 3 - Isento
					registro += new string('0', 8);                                                                                             // Brancos
					registro += new string('0', 15);                                                                                            // Brancos
				}

				if (boleto.ValorDesconto > 0)
				{
					registro += "1";                                                                                                            // Código do desconto: 1 - Valor fixo até a data informada
					registro += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false);             // Data do desconto
					registro += Utils.FitStringLength(boleto.ValorDesconto.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true);// Valor do desconto
				}
				else
				{
					registro += new string('0', 24);                                                                                            // Brancos
				}

				registro += new string('0', 15);                                                                                                // Valor do IOF a ser recolhido
				registro += new string('0', 15);                                                                                                // Valor do abatimento
				registro += Utils.FitStringLength(boleto.NumeroDocumento, 25, 25, ' ', 0, true, true, false);                                   // Identificação do título na empresa
				registro += "3";                                                                                                                // Código para protesto
				registro += "00";                                                                                                               // Número de dias para protesto
				registro += "0";                                                                                                                // Código para baixa/devolução
				registro += "000";                                                                                                              // Número de dias para baixa/devolução
				registro += "09";                                                                                                               // Código da moeda
				registro += "0000000000";                                                                                                       // Número do contrato da operação de crédito
				registro += " ";                                                                                                                // Uso exclusivo Febraban/CNAB

				registro = Utils.SubstituiCaracteresEspeciais(registro);

				return registro;

			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a geração do SEGMENTO P DO DETALHE do arquivo de REMESSA.", ex);
			}
		}

		public override string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
		{
			try
			{
				string registro;

				registro = "399";                                                                                                               // Código do banco
				registro += "0001";                                                                                                             // Número sequencial do lote
				registro += "3";                                                                                                                // Código do Registro: 1 - Header de Lote
				registro += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);                                   // Número sequencial do registro do lote
				registro += "Q";                                                                                                                // Código do segmento do registro de detalhe
				registro += " ";                                                                                                                // Uso exclusivo Febraban/CNAB
				registro += "01";                                                                                                               // Código de movimento
				registro += (boleto.Sacado.CPFCNPJ.Length <= 11) ? "1" : "2";                                                                   // Tipo de inscrição: 1 - Física | 2 - Jurídica
				registro += Utils.FitStringLength(boleto.Sacado.CPFCNPJ, 15, 15, '0', 0, true, true, true);                                     // CPF ou CNPJ
				registro += Utils.FitStringLength(boleto.Sacado.Nome.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();              // Nome do sacado
				registro += Utils.FitStringLength(boleto.Sacado.Endereco.End.TrimStart(' '), 38, 38, ' ', 0, true, true, false).ToUpper();      // Endereço do sacado
				registro += "  ";                                                                                                               // Uso banco
				registro += Utils.FitStringLength(boleto.Sacado.Endereco.Bairro.TrimStart(' '), 15, 15, ' ', 0, true, true, false).ToUpper();   // Bairro do endereço do sacado
				registro += Utils.FitStringLength(boleto.Sacado.Endereco.CEP, 8, 8, ' ', 0, true, true, false);                                 // CEP do endereço do sacado
				registro += Utils.FitStringLength(boleto.Sacado.Endereco.Cidade.TrimStart(' '), 15, 15, ' ', 0, true, true, false).ToUpper();   // Cidade do endereço do sacado
				registro += Utils.FitStringLength(boleto.Sacado.Endereco.UF, 2, 2, ' ', 0, true, true, false).ToUpper();                        // UF do endereço do sacado
				registro += "0";                                                                                                                // Tipo inscrição do avalista do sacado
				registro += new string('0', 15);                                                                                                // Número de inscrição do avalista do sacado
				registro += new string('0', 40);                                                                                                // Nome do avalista do sacado
				registro += "   ";                                                                                                              // Código banco correspondente na compensação
				registro += new string(' ', 20);                                                                                                // Nosso número no banco correspondente
				registro += new string(' ', 8);                                                                                                 // Uso exclusivo Febraban/CNAB

				registro = Utils.SubstituiCaracteresEspeciais(registro);

				return registro;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a geração do SEGMENTO Q DO DETALHE do arquivo de REMESSA.", ex);
			}
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

		# endregion DETALHE

		# region TRAILER DE LOTE

		/// <summary>
		/// TRAILER DE LOTE do arquivo CNAB
		/// Gera o TRAILER de Lote do arquivo remessa de acordo com o lay-out informado
		/// </summary>
		public override string GerarTrailerLoteRemessa(int quantidadeDeRegistros)
		{
			try
			{
				string registro = "";

				/*switch (tipoArquivo)
				{
					case TipoArquivo.CNAB240:*/
						registro = GerarTrailerLoteRemessaCNAB240(quantidadeDeRegistros);
				/*		break;
					case TipoArquivo.CNAB400:
						registro = GerarTrailerLoteRemessaCNAB400(quantidadeDeRegistros);
						break;
					case TipoArquivo.Outro:
						throw new Exception("Tipo de arquivo inexistente.");
				}*/

				return registro;

			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a geração do TRAILER DO LOTE do arquivo de REMESSA.", ex);
			}
		}

		// TRAILER DE LOTE REMESSA CNAB240
		private string GerarTrailerLoteRemessaCNAB240(int quantidadeDeRegistros)
		{
			try
			{

				string registro;

				registro = "399";                                                                                           // Código do banco
				registro += "0001";                                                                                         // Número sequencial do lote
				registro += "5";                                                                                            // Código do Registro: 5 - Trailer de Lote
				registro += new string(' ', 9);                                                                             // Brancos - Uso exclusivo Febraban/CNAB
				registro += Utils.FitStringLength(quantidadeDeRegistros.ToString(), 6, 6, '0', 0, true, true, true);        // Tipo de operação: R - Remessa
				registro += new string(' ', 217);                                                                           // Brancos - Uso exclusivo Febraban/CNAB

				registro = Utils.SubstituiCaracteresEspeciais(registro);

				return registro;

			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a geração do TRAILER DE LOTE do arquivo de REMESSA.", ex);
			}
		}

		private string GerarTrailerLoteRemessaCNAB400(int quantidadeDeRegistros)
		{
			throw new Exception("Função não implementada.");
		}

		# endregion
				
		# region TRAILER

		/// <summary>
		/// Gera o Trailer de arquivo do arquivo de remessa
		/// </summary>
		public override string GerarTrailerArquivoRemessa(int numeroRegistro)
		{
			try
			{
				return GerarTrailerRemessa240(numeroRegistro); ;
			}
			catch (Exception ex)
			{
				throw new Exception("", ex);
			}
		}

		public override string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal)
		{
			try
			{
				string _trailer = " ";

				switch (tipoArquivo)
				{
					case TipoArquivo.CNAB240:
						_trailer = GerarTrailerRemessa240(numeroRegistro);
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
				throw new Exception("Erro durante a geração do TRAILER do arquivo de REMESSA.", ex);
			}
		}

		public string GerarTrailerRemessa240(int quantidadeDeRegistros)
		{
			try
			{
				string registro;
				
				registro = "399";                                                                                           // Código do banco
				registro += "9999";                                                                                         // Número do trailer
				registro += "9";                                                                                            // Código do Registro: 9 - Trailer de Arquivo
				registro += new string(' ', 9);                                                                             // Brancos - Uso exclusivo Febraban/CNAB
				registro += "000001";                                                                                       // Quantidade de lotes (Registros tipo 1)
				registro += Utils.FitStringLength(quantidadeDeRegistros.ToString(), 6, 6, '0', 0, true, true, true);        // Quantidade de registros (Registros tipo: 0+1+3+5+9)
				registro += "000000";                                                                                       // Quantidade de contas para conciliação
				registro += new string(' ', 205);                                                                           // Brancos - Uso exclusivo Febraban/CNAB

				return registro;
			}
			catch (Exception ex)
			{
				throw new Exception("", ex);
			}
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
