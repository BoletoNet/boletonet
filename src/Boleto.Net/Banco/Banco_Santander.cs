using System;
using System.Globalization;
using BoletoNet.Util;
using System.Text;

namespace BoletoNet
{
    /// <author>  
    /// Eduardo Frare
    /// Stiven 
    /// Diogo
    /// Miamoto
    /// </author>    


    ///<summary>
    /// Classe referente ao banco Banco_Santander
    ///</summary>
    internal class Banco_Santander : AbstractBanco, IBanco
    {

        /// <summary>
        /// Classe responsavel em criar os campos do Banco Banco_Santander.
        /// </summary>
        internal Banco_Santander()
        {
            this.Codigo = 033;
            this.Digito = "7";
            this.Nome = "Santander";
        }

        internal Banco_Santander(int Codigo)
        {

            this.Codigo = ((Codigo != 353) && (Codigo != 8)) ? 033 : Codigo;
            this.Digito = "0";
            this.Nome = "Banco_Santander";
        }

        #region IBanco Members

        /// <summary>
        /// 
        ///   *******
        /// 
        ///	O Código de barra para cobrança contém 44 posições dispostas da seguinte forma:
        ///    01 a 03 -  3 - 033 fixo - Código do banco
        ///    04 a 04 -  1 - 9 fixo - Código da moeda (R$)
        ///    05 a 05 -  1 - Dígito verificador do Código de barras
        ///    06 a 09 -  4 - Fator de vencimento
        ///    10 a 19 - 10 - Valor
        ///    20 a 20 -  1 - Fixo 9
        ///    21 a 27 -  7 - Código do cedente padrão satander
        ///    28 a 40 - 13 - Nosso número
        ///    41 - 41 - 1 -  IOS  - Seguradoras(Se 7% informar 7. Limitado  a 9%) Demais clientes usar 0 
        ///    42 - 44 - 3 - Tipo de modalidade da carteira 101, 102, 201
        /// 
        ///   *******
        /// 
        /// </summary>
        public override void FormataCodigoBarra(Boleto boleto)
        {
            string codigoBanco = Utils.FormatCode(this.Codigo.ToString(), 3);//3
            string codigoMoeda = boleto.Moeda.ToString();//1
            string fatorVencimento = FatorVencimento(boleto).ToString(); //4
            string valorNominal = Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10);//10
            string fixo = "9";//1
            string codigoCedente = Utils.FormatCode(boleto.Cedente.Codigo.ToString(), 7);//7
            string nossoNumero = Utils.FormatCode(boleto.NossoNumero, 12) + Mod11Santander(Utils.FormatCode(boleto.NossoNumero, 12), 9);//13
            string IOS = boleto.PercentualIOS.ToString();//1
            string tipoCarteira = boleto.Carteira;//3;
            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}",
                codigoBanco, codigoMoeda, fatorVencimento, valorNominal, fixo, codigoCedente, nossoNumero, IOS, tipoCarteira);

            string calculoDV = Mod10Mod11Santander(boleto.CodigoBarra.Codigo, 9).ToString();

            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}",
                codigoBanco, codigoMoeda, calculoDV, fatorVencimento, valorNominal, fixo, codigoCedente, nossoNumero, IOS, tipoCarteira);


        }

        /// <summary>
        /// 
        ///   *******
        /// 
        ///	A Linha Digitavel para cobrança contém 44 posições dispostas da seguinte forma:
        ///   1º Grupo - 
        ///    01 a 03 -  3 - 033 fixo - Código do banco
        ///    04 a 04 -  1 - 9 fixo - Código da moeda (R$) outra moedas 8
        ///    05 a 05 –  1 - Fixo 9
        ///    06 a 09 -  4 - Código cedente padrão santander
        ///    10 a 10 -  1 - Código DV do primeiro grupo
        ///   2º Grupo -
        ///    11 a 13 –  3 - Restante do código cedente
        ///    14 a 20 -  7 - 7 primeiros campos do nosso número
        ///    21 a 21 - 13 - Código DV do segundo grupo
        ///   3º Grupo -  
        ///    22 - 27 - 6 -  Restante do nosso número
        ///    28 - 28 - 1 - IOS  - Seguradoras(Se 7% informar 7. Limitado  a 9%) Demais clientes usar 0 
        ///    29 - 31 - 3 - Tipo de carteira
        ///    32 - 32 - 1 - Código DV do terceiro grupo
        ///   4º Grupo -
        ///    33 - 33 - 1 - Composto pelo DV do código de barras
        ///   5º Grupo -
        ///    34 - 36 - 4 - Fator de vencimento
        ///    37 - 47 - 10 - Valor do título
        ///   *******
        /// 
        /// </summary>
        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            string nossoNumero = Utils.FormatCode(boleto.NossoNumero, 12) + Mod11Santander(Utils.FormatCode(boleto.NossoNumero, 12), 9);//13
            string codigoCedente = Utils.FormatCode(boleto.Cedente.Codigo.ToString(), 7);
            string fatorVencimento = FatorVencimento(boleto).ToString();
            string IOS = boleto.PercentualIOS.ToString();//1

            #region Grupo1

            string codigoBanco = Utils.FormatCode(this.Codigo.ToString(), 3);//3
            string codigoModeda = boleto.Moeda.ToString();//1
            string fixo = "9";//1
            string codigoCedente1 = codigoCedente.Substring(0, 4);//4
            string calculoDV1 = Mod10(string.Format("{0}{1}{2}{3}", codigoBanco, codigoModeda, fixo, codigoCedente1)).ToString();//1
            string grupo1 = string.Format("{0}{1}{2}.{3}{4}", codigoBanco, codigoModeda, fixo, codigoCedente1, calculoDV1);

            #endregion

            #region Grupo2

            string codigoCedente2 = codigoCedente.Substring(4, 3);//3
            string nossoNumero1 = nossoNumero.Substring(0, 7);//7
            string calculoDV2 = Mod10(string.Format("{0}{1}", codigoCedente2, nossoNumero1)).ToString();
            string grupo2 = string.Format("{0}{1}{2}", codigoCedente2, nossoNumero1, calculoDV2);
            grupo2 = " " + grupo2.Substring(0, 5) + "." + grupo2.Substring(5, 6);

            #endregion

            #region Grupo3

            string nossoNumero2 = nossoNumero.Substring(7, 6); //6
            
            string tipoCarteira = boleto.Carteira;//3
            string calculoDV3 = Mod10(string.Format("{0}{1}{2}", nossoNumero2, IOS, tipoCarteira)).ToString();//1
            string grupo3 = string.Format("{0}{1}{2}{3}", nossoNumero2, IOS, tipoCarteira, calculoDV3);
            grupo3 = " " + grupo3.Substring(0, 5) + "." + grupo3.Substring(5, 6) + " ";

            #endregion

            #region Grupo4
            string DVcodigoBanco = Utils.FormatCode(this.Codigo.ToString(), 3);//3
            string DVcodigoMoeda = boleto.Moeda.ToString();//1
            string DVvalorNominal = Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10);//10
            string DVfixo = "9";//1
            string DVcodigoCedente = Utils.FormatCode(boleto.Cedente.Codigo.ToString(), 7).ToString();//7
            string DVnossoNumero = Utils.FormatCode(boleto.NossoNumero, 12) + Mod11Santander(Utils.FormatCode(boleto.NossoNumero, 12), 9);
            string DVtipoCarteira = boleto.Carteira;//3;

            string calculoDVcodigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}",
                DVcodigoBanco, DVcodigoMoeda, fatorVencimento, DVvalorNominal, DVfixo, DVcodigoCedente, DVnossoNumero, IOS, DVtipoCarteira);

            string grupo4 = Mod10Mod11Santander(calculoDVcodigo, 9).ToString() + " ";

            #endregion

            #region Grupo5

             //4
            string valorNominal = Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10);//10

            string grupo5 = string.Format("{0}{1}", fatorVencimento, valorNominal);
            //grupo5 = grupo5.Substring(0, 4) + " " + grupo5.Substring(4, 1)+" "+grupo5.Substring(5,9);



            #endregion

            boleto.CodigoBarra.LinhaDigitavel = string.Format("{0}{1}{2}{3}{4}", grupo1, grupo2, grupo3, grupo4, grupo5);


            //Usado somente no Santander
            boleto.Cedente.ContaBancaria.Conta = boleto.Cedente.Codigo.ToString();

        }

        public override void FormataNossoNumero(Boleto boleto)
        {
            boleto.NossoNumero = string.Format("{0}-{1}", boleto.NossoNumero, Mod11Santander(boleto.NossoNumero, 9));
        }

        public override void FormataNumeroDocumento(Boleto boleto)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public override void ValidaBoleto(Boleto boleto)
        {
            //throw new NotImplementedException("Função não implementada.");
            if (!((boleto.Carteira == "102") || (boleto.Carteira == "101") || (boleto.Carteira == "201")))
            {
                string exceptionMessage = String.Format("A carteira '{0}' não foi implementada. Carteiras válidas: 101, 102 e 201.", boleto.Carteira);
                throw new NotImplementedException(exceptionMessage);
            }

            //Banco 353  - Utilizar somente 08 posições do Nosso Numero (07 posições + DV), zerando os 05 primeiros dígitos
            if (this.Codigo == 353)
            {
                if (boleto.NossoNumero.Length != 7)
                    throw new NotImplementedException("Nosso Número deve ter 7 posições para o banco 353.");
            }

            //Banco 008  - Utilizar somente 09 posições do Nosso Numero (08 posições + DV), zerando os 04 primeiros dígitos
            if (this.Codigo == 8)
            {
                if (boleto.NossoNumero.Length != 8)
                    throw new NotImplementedException("Nosso Número deve ter 7 posições para o banco 008.");
            }

            if (this.Codigo == 33)
            {
                if (boleto.NossoNumero.Length == 7 && boleto.Carteira.Equals("101"))
                    boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero,"0", 12, true);

                if (boleto.NossoNumero.Length != 12)
                    throw new NotSupportedException("Nosso Número deve ter 12 posições para o banco 033.");
            }
            if (boleto.Cedente.Codigo.ToString().Length > 7)
                throw new NotImplementedException("Código cedente deve ter 7 posições.");

            // Atribui o nome do banco ao local de pagamento
			if (string.IsNullOrEmpty(boleto.LocalPagamento))
				boleto.LocalPagamento = "Grupo Santander - GC";

            if (EspecieDocumento.ValidaSigla(boleto.EspecieDocumento) == "")
               boleto.EspecieDocumento = new EspecieDocumento_Santander("2");

            if (boleto.PercentualIOS > 10 & (this.Codigo == 8 || this.Codigo == 33 || this.Codigo == 353))
                throw new Exception("O percentual do IOS é limitado a 9% para o Banco Santander");

            boleto.FormataCampos();
        }

        private static int Mod11Santander(string seq, int lim)
        {
            int ndig = 0;
            int nresto = 0;
            int total = 0;
            int multiplicador = 5;

            while (seq.Length > 0)
            {
                int valorPosicao = Convert.ToInt32(seq.Substring(0, 1));
                total += valorPosicao * multiplicador;
                multiplicador--;

                if (multiplicador == 1)
                {
                    multiplicador = 9;
                }

                seq = seq.Remove(0, 1);
            }

            nresto = total - ((total / 11) * 11);

            if (nresto == 0 || nresto == 1)
                ndig = 0;
            else if (nresto == 10)
                ndig = 1;
            else
                ndig = (11 - nresto);

            return ndig;
        }

        private static int Mod10Mod11Santander(string seq, int lim)
        {
            int ndig = 0;
            int nresto = 0;
            int total = 0;
            int multiplicador = 2;

            char[] posicaoSeq = seq.ToCharArray();
            Array.Reverse(posicaoSeq);
            string sequencia = new string(posicaoSeq);

            while (sequencia.Length > 0)
            {
                int valorPosicao = Convert.ToInt32(sequencia.Substring(0, 1));
                total += valorPosicao * multiplicador;
                multiplicador++;

                if (multiplicador == 10)
                {
                    multiplicador = 2;
                }

                sequencia = sequencia.Remove(0, 1);
            }

            nresto = (total * 10) % 11; //nresto = (((total * 10) / 11) % 10); Jefhtavares em 19/03/14


            if (nresto == 0 || nresto == 1 || nresto == 10)
                ndig = 1;
            else
                ndig = nresto;

            return ndig;
        }

        protected static int Mult10Mod11Santander(string seq, int lim, int flag)
        {
            int mult = 0;
            int total = 0;
            int pos = 1;
            int ndig = 0;
            int nresto = 0;
            string num = string.Empty;

            mult = 1 + (seq.Length % (lim - 1));

            if (mult == 1)
                mult = lim;

            while (pos <= seq.Length)
            {
                num = seq.Mid( pos, 1);
                total += Convert.ToInt32(num) * mult;

                mult -= 1;
                if (mult == 1)
                    mult = lim;

                pos += 1;
            }

            nresto = ((total * 10) % 11);

            if (flag == 1)
                return nresto;
            else
            {
                if (nresto == 0 || nresto == 1)
                    ndig = 0;
                else if (nresto == 10)
                    ndig = 1;
                else
                    ndig = (11 - nresto);

                return ndig;
            }
        }

        /// <summary>
        /// Verifica o tipo de ocorrência para o arquivo remessa
        /// </summary>
        public string Ocorrencia(string codigo)
        {
            switch (codigo)
            {
                case "01":
                    return "01-Título não existe";
                case "02":
                    return "02-Entrada Confirmada";
                case "03":
                    return "03-Entrada Rejeitada";
                case "06":
                    return "06-Liquidação";
                case "07":
                    return "07-Liquidação por conta";
                case "08":
                    return "08-Liquidação por saldo";
                case "09":
                    return "09-Baixa Automatica";
                case "10":
                    return "10-Baixa conf. instrução ou protesto";
                case "11":
                    return "11-Em Ser";
                case "12":
                    return "12-Abatimento Concedido";
                case "13":
                    return "13-Abatimento Cancelado";
                case "14":
                    return "14-Prorrogação de Vencimento";
                case "15":
                    return "15-Enviado para Cartório";
                case "16":
                    return "16-Título já baixado/liquidado";
                case "17":
                    return "17-Liquidado em cartório";
                case "21":
                    return "21-Entrada em cartório";
                case "22":
                    return "22-Retirado de cartório";
                case "24":
                    return "24-Custas de cartório";
                case "25":
                    return "25-Protestar Título";
                case "26":
                    return "26-Sustar protesto";
                default:
                    return "";
            }
        }


        #region Métodos de geração do arquivo remessa

        #region HEADER REMESSA
        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            throw new NotImplementedException("Função não implementada.");
        }
        public override string GerarHeaderRemessa(Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            return GerarHeaderRemessa("0", cedente, tipoArquivo, numeroArquivoRemessa);
        }
        /// <summary>
        /// HEADER do arquivo CNAB
        /// Gera o HEADER do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            try
            {
                string _header = " ";

                base.GerarHeaderRemessa("0", cedente, tipoArquivo, numeroArquivoRemessa);

                switch (tipoArquivo)
                {

                    case TipoArquivo.CNAB240:
                        _header = GerarHeaderRemessaCNAB240(cedente);
                        break;
                    case TipoArquivo.CNAB400:
                        _header = GerarHeaderRemessaCNAB400(0, cedente);
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

        /// <summary>
        ///POS INI/FINAL	DESCRIÇÂO	                   A/N	TAM	DEC	CONTEÚDO	NOTAS
        ///--------------------------------------------------------------------------------
        ///001 - 003	Código do Banco na compensação	    N	003		353 / 008 / 033	
        ///004 - 007	Lote de serviço	                    N	004		0000	1 
        ///008 - 008	Tipo de registro	                N	001		0	2
        ///009 - 016	Reservado (uso Banco)	            A	008		Brancos	  
        ///017 - 017	Tipo de inscrição da empresa	    N	001		1 = CPF,  2 = CNPJ 	
        ///018 - 032	Nº de inscrição da empresa	        N	015			
        ///033 - 047	Código de Transmissão   	        N	015			3 
        ///048 - 072	Reservado (uso Banco)	            A	025		Brancos	
        ///073 - 102	Nome da empresa	                    A	030			
        ///103 - 132	Nome do Banco	                    A	030		Banco Santander 	
        ///133 - 142	Reservado (uso Banco)	            A	010		Brancos	
        ///143 - 143	Código remessa 	                    N	001		1 = Remessa 	
        ///144 - 151	Data de geração do arquivo	        N	008		DDMMAAAA	
        ///152 - 157	Reservado (uso Banco)  	            A	006		Brancos	
        ///158 - 163	Nº sequencial do arquivo 	        N	006			4
        ///164 - 166	Nº da versão do layout do arquivo	N	003		040	
        ///167 - 240	Reservado (uso Banco)	            A	074		Brancos	
        /// </summary>
        public string GerarHeaderRemessaCNAB240(Cedente cedente)
        {
            try
            {
                string header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                header += "0000";
                header += "0";
                header += Utils.FormatCode("", " ", 8);
                header += (cedente.CPFCNPJ.Length == 11 ? "1" : "2");
                header += Utils.FormatCode("", "0", 15);
                header += Utils.FormatCode("", "0", 15);
                header += Utils.FormatCode("", " ", 25);
                header += Utils.FormatCode(cedente.Nome, " ", 30);
                header += Utils.FormatCode("BANCO SANTANDER", " ", 30);
                header += Utils.FormatCode("", " ", 10);
                header += "1";
                header += DateTime.Now.ToString("ddMMyyyy");
                header += Utils.FormatCode("", " ", 6);
                header += "0001";
                header += "040";
                header += Utils.FormatCode("", " ", 74);
                return header;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB240.", ex);
            }
        }

        public string GerarHeaderRemessaCNAB400(int numeroConvenio, Cedente cedente)
        {
            try
            {
                string complemento = new string(' ', 275);
                string _header;

                _header = "01REMESSA01COBRANCA       ";         //fixo
                //Código da transmissão fornecido pelo banco 9(20), mas normalmente é formado por:
                //018-021 = Código da agencia 9(04)
                //022-029 = Código do cedente 9(08)
                //030-037 = Conta cobrança/corrente 9(08)
                _header += Utils.FitStringLength(cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true);
                _header += Utils.FitStringLength(cedente.Codigo.ToString(), 8, 8, '0', 0, true, true, true);
                if (cedente.ContaBancaria.Conta.Length == 9 || (!String.IsNullOrEmpty(cedente.ContaBancaria.DigitoConta) && cedente.ContaBancaria.Conta.Length == 8))
                {
                    _header += Utils.FitStringLength(cedente.ContaBancaria.Conta.Substring(0, 7), 8, 8, '0', 0, true, true, true);
                }
                else
                {
                    _header += Utils.FitStringLength(cedente.ContaBancaria.Conta, 8, 8, '0', 0, true, true, true);
                }
                _header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false).ToUpper();
                _header += "033";
                _header += "SANTANDER      ";                   //Nome do banco X(15)
                _header += DateTime.Now.ToString("ddMMyy");     //data da gravação
                _header += "0000000000000000";                  //zeros 9(16)
                _header += complemento;                         //brancos X(275)
                _header += "000";                               //zeros 9(03)
                _header += "000001";

                _header = Utils.SubstituiCaracteresEspeciais(_header);

                return _header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
        }

        #endregion HEADER REMESSA

        #region HEADER LOTE REMESSA

        public override string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa, TipoArquivo tipoArquivo)
        {
            try
            {
                string header = " ";

                switch (tipoArquivo)
                {

                    case TipoArquivo.CNAB240:
                        header = GerarHeaderLoteRemessaCNAB240(cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.CNAB400:
                        header = GerarHeaderLoteRemessaCNAB400(0, cedente, numeroArquivoRemessa);
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

        private string GerarHeaderLoteRemessaCNAB400(int numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            throw new Exception("Função não implementada.");
        }

        private string GerarHeaderLoteRemessaCNAB240(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                string header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                header += "0000";
                header += "0";
                header += "R";
                header += "  ";
                header += "030";
                header += " ";
                header += "0";
                header += (cedente.CPFCNPJ.Length == 11 ? "1" : "2");
                header += Utils.FormatCode("", "0", 15);
                header += Utils.FormatCode("", " ", 20);
                header += Utils.FormatCode("", "0", 15);
                header += Utils.FormatCode("", " ", 5);
                header += Utils.FormatCode(cedente.Nome, " ", 30);
                header += Utils.FormatCode("", " ", 40);
                header += Utils.FormatCode("", " ", 40);
                header += Utils.FormatCode(numeroArquivoRemessa.ToString(), "0", 8);
                header += DateTime.Now.ToString("ddMMyyyy");
                header += Utils.FormatCode("", " ", 41);
                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar HEADER DO LOTE do arquivo de remessa.", e);
            }
        }

        #endregion HEADER LOTE REMESSA

        #region DETALHE REMESSA

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
                    case TipoArquivo.CNAB240:
                        _detalhe = GerarDetalheRemessaCNAB240();
                        break;
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
                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                string controle_partic = new string(' ', 25);
                string sacador_aval = new string(' ', 30);
                string _detalhe;

                _detalhe = "1";

                // Tipo de inscrição do cedente
                // 01 - CPF DO CEDENTE
                // 02 - CNPJ DO CEDENTE

                if (boleto.Cedente.CPFCNPJ.Length <= 11)
                    _detalhe += "01";
                else
                    _detalhe += "02";

                _detalhe += Utils.FitStringLength(boleto.Cedente.CPFCNPJ, 14, 14, '0', 0, true, true, true);
                //Código da transmissão fornecido pelo banco 9(20), mas normalmente é formado por:
                //018-021 = Código da agencia 9(04)
                //022-029 = Código do cedente 9(08)
                //030-037 = Conta cobrança/corrente 9(08)
                _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true);
                _detalhe += Utils.FitStringLength(boleto.Cedente.Codigo.ToString(), 8, 8, '0', 0, true, true, true);
                if (boleto.Cedente.ContaBancaria.Conta.Length == 9 || (!String.IsNullOrEmpty(boleto.Cedente.ContaBancaria.DigitoConta) && boleto.Cedente.ContaBancaria.Conta.Length == 8))
                    _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta.Substring(0, 7), 8, 8, '0', 0, true, true, true);
                else
                    _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 8, 8, '0', 0, true, true, true);
                 //Bloco if-else adicionado por Jéferson (jefhtavares). Segundo o Banco o código de transmissão muda de acordo com o tamanho (length) da conta corrente

                //Número de controle do participante, controle do cedente X(25)
                _detalhe += controle_partic;
                //NossoNumero com DV, pegar os 8 primeiros dígitos, da direita para esquerda
                string nossoNumero = Utils.FormatCode(boleto.NossoNumero, 12) + Mod11Santander(Utils.FormatCode(boleto.NossoNumero, 12), 9);//13
                _detalhe += Utils.Right(nossoNumero, 8, '0', true);
                _detalhe += "000000"; //Data do segundo desconto 9(06)
                _detalhe += " "; //brancos X(01)
                //(Nota 4) posição 78-78, sempre igual a '4', sendo obrigatório a informação do percentual na posição 79-82 
                /* Jéferson (jefhtavares) em 19/02/2014
                 * Usar '4' na posição 78 quando HOUVER percentual de multa, caso contrário informar 0
                 */
                if (boleto.PercMulta == 0)
                    _detalhe += "0";
                else
                    _detalhe += "4";

                //Percentual de multa por atraso 9(04)V99 = significa 4 posições, sendo 2 decimais
                _detalhe += Utils.FitStringLength(boleto.PercMulta.ToString("0.00").Replace(",", ""), 4, 4, '0', 0, true, true, true);
                _detalhe += "00"; //Unidade de valor mora corrente = 00
                _detalhe += "0000000000000"; //Valor do titulo em outra unidade (consultar banco) 9(13)V99 = significa 13 posições, sendo 2 decimais
                _detalhe += "    "; //brancos X(04)
                _detalhe += "000000"; //Data de cobrança da multa, se zeros, será cobrada após vencimento

                //Código da carteira
                //1 - Eletrônica COM registro
                //3 - Caucionada eletrônica
                //4 - Cobrança SEM registro
                //5 - Rápida COM registro
                //6 - Caucionada rápida
                //7 - Descontada eletrônica
                string carteira;
                switch (boleto.Carteira)
                {
                    case "101": //Carteira 101 (Rápida com registro - impressão pelo beneficiário)
                        carteira = "5";
                        break;
                    case "201": //Carteira 201 (Eletrônica com registro - impressão pelo banco)
                        carteira = "1";
                        break;
                    default:
                        throw new Exception("Carteira não implementada para emissão de remessa");
                }
                _detalhe += carteira;

                //Código de ocorrência
                //01 - Entrada de Título
                //02 - Baixa de Título
                //04 - Concessão de abatimento
                //05 - Cancelamento de abatimento
                //06 - Prorrogação de vencimento
                //07 - Alteração Número de controle do cedente
                //08 - Alteração do seu Número
                //09 - Protestar
                //18 - Sustar protesto
                _detalhe += "01";

                //Nº do documento
                _detalhe += Utils.FitStringLength(boleto.NumeroDocumento, 10, 10, ' ', 0, true, true, false);
                _detalhe += boleto.DataVencimento.ToString("ddMMyy");
                _detalhe += Utils.FitStringLength(boleto.ValorBoleto.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true);
                _detalhe += "033";

                //Se a Código da carteira = 5 (Rápida COM registro), informar, senão = 00000 (zeros 9(05))
                if (!String.IsNullOrEmpty(boleto.Cedente.ContaBancaria.DigitoAgencia))
                {
                    _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true);
                    _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoAgencia, 1, 1, '0', 0, true, true, true);
                }
                else
                {
                    _detalhe += Utils.FitStringLength("0" + boleto.Cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true);
                }
                

                //Espécie de documento
                //01 - Duplicata
                //02 - Nota promissória
                //04 - Apólice / Nota Seguro
                //05 - Recibo
                //06 - Duplicata de serviço
                //07 - Letra de cambio

                string especie_doc = "00";

                //Transcrição para arquivo REMESSA. Verifica qual espécie de documento (Ver EspecieDocumento_Santander.cs)
                switch (boleto.EspecieDocumento.Codigo)
                {
                    case "2": //DuplicataMercantil, 
                        especie_doc = "01";
                        break;
                    case "12": //NotaPromissoria
                    case "13": //NotaPromissoriaRural
                    case "98": //NotaPromissoariaDireta
                        especie_doc = "02";
                        break;
                    case "20": //ApoliceSeguro
                        especie_doc = "03";
                        break;
                    case "17": //Recibo
                        especie_doc = "05";
                        break;
                    case "06": //DuplicataServico
                        especie_doc = "06";
                        break;
                    case "07": //LetraCambio353
                    case "30": //LetraCambio008
                        especie_doc = "07";
                        break;
                    default:    //Cheque ou qualquer outro Código
                        especie_doc = "01";
                        break;
                }

                _detalhe += especie_doc;
                _detalhe += "N"; // Identificação de Título, Aceito ou Não aceito

                //A data informada neste campo deve ser a mesma data de emissão do Título de crédito 
                //(Duplicata de Serviço / Duplicata Mercantil / Nota Fiscal, etc), que deu origem a esta Cobrança. 
                //Existindo divergência, na existência de protesto, a documentação poderá não ser aceita pelo Cartório.
                _detalhe += boleto.DataDocumento.ToString("ddMMyy");

                switch (boleto.Instrucoes.Count)
                {
                    case 0:
                        _detalhe += "0000"; //Não há instruções
                        break;
                    case 1:
                        _detalhe += Utils.FitStringLength(boleto.Instrucoes[0].Codigo.ToString(), 2, 2, '0', 0, true, true, true);
                        _detalhe += "00";
                        break;
                    default:
                        _detalhe += Utils.FitStringLength(boleto.Instrucoes[0].Codigo.ToString(), 2, 2, '0', 0, true, true, true);
                        _detalhe += Utils.FitStringLength(boleto.Instrucoes[1].Codigo.ToString(), 2, 2, '0', 0, true, true, true);
                        break;
                }

                //Valor de mora cobrado por dia de atraso (Juros de 1 dia)
                _detalhe += Utils.FitStringLength(boleto.JurosMora.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true);

                // Data limite para desconto
                _detalhe += boleto.DataVencimento.ToString("ddMMyy");
                _detalhe += Utils.FitStringLength(boleto.ValorDesconto.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true);
                _detalhe += "0000000000000"; // Valor do IOF a ser recolhido pelo banco para Nota de Seguro 9(13)V9(02) = significa 13 posições, sendo 2 decimais
                _detalhe += "0000000000000"; // Valor do Abatimento 9(13)V9(02) = significa 13 posições, sendo 2 decimais

                if (boleto.Sacado.CPFCNPJ.Length <= 11)
                    _detalhe += "01";  // CPF
                else
                    _detalhe += "02"; // CNPJ

                _detalhe += Utils.FitStringLength(boleto.Sacado.CPFCNPJ, 14, 14, '0', 0, true, true, true).ToUpper();
                _detalhe += Utils.FitStringLength(boleto.Sacado.Nome.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.End.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Bairro.TrimStart(' '), 12, 12, ' ', 0, true, true, false).ToUpper();
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.CEP, 8, 8, ' ', 0, true, true, true);
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Cidade.TrimStart(' '), 15, 15, ' ', 0, true, true, false).ToUpper();
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.UF, 2, 2, ' ', 0, true, true, false).ToUpper();

                //Nome do Sacador ou coobrigado
                _detalhe += sacador_aval;
                _detalhe += " "; //brancos X(01)
                // Identificador de complemento de conta cobrança
                _detalhe += "I";
                //Complemento da conta, posições 384-385, com a última posição da conta e o dígito da conta
                _detalhe += boleto.Cedente.ContaBancaria.Conta.Substring(boleto.Cedente.ContaBancaria.Conta.Length - 1, 1) + boleto.Cedente.ContaBancaria.DigitoConta;
                _detalhe += "      "; //brancos X(06)
                string diasProtesto = "00";
                if (boleto.Instrucoes.Count > 0)
                {
                    if (boleto.Instrucoes[0].Codigo == (int)EnumInstrucoes_Santander.Protestar)
                        diasProtesto = Utils.FitStringLength(boleto.Instrucoes[0].QuantidadeDias.ToString(), 2, 2, '0', 0, true, true, true);
                    else if (boleto.Instrucoes.Count > 1 && boleto.Instrucoes[1].Codigo == (int)EnumInstrucoes_Santander.Protestar)
                        diasProtesto = Utils.FitStringLength(boleto.Instrucoes[1].QuantidadeDias.ToString(), 2, 2, '0', 0, true, true, true);
                    else
                        diasProtesto = "00"; //Nº de dias para protesto 9(02)
                }

                _detalhe += diasProtesto;
                _detalhe += " "; //brancos X(01)
                _detalhe += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);

                _detalhe = Utils.SubstituiCaracteresEspeciais(_detalhe);

                return _detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB400.", ex);
            }
        }

        #endregion DETALHE REMESSA

        #region TRAILER CNAB240

        /// <summary>
        ///POS INI/FINAL	DESCRIÇÂO	                   A/N	TAM	DEC	CONTEÚDO	NOTAS
        ///--------------------------------------------------------------------------------
        ///001 - 003	Código do Banco na compensação	    N	003		341	
        ///004 - 007	Lote de serviço	                    N	004		Nota 5 
        ///008 - 008	Registro Trailer de Lote            N	001     5
        ///009 - 017	Complemento de Registros            A	009     Brancos
        ///018 - 023    Qtd. Registros do Lote              N   006     Nota 15     
        ///024 - 041    Soma Valor dos Débitos do Lote      N   018     Nota 14     
        ///042 - 059    Soma Qtd. de Moedas do Lote         N   018     Nota 14
        ///060 - 230    Complemento de Registros            A   171     Brancos
        ///231 - 240    Cód. Ocr. para Retorno              A   010     Brancos
        /// </summary>

        public override string GerarTrailerLoteRemessa(int numeroRegistro)
        {
            try
            {
                string trailer = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                trailer += Utils.FormatCode("", "0", 4, true);
                trailer += "5";
                trailer += Utils.FormatCode("", " ", 9);
                trailer += Utils.FormatCode("", "0", 6, true);
                trailer += Utils.FormatCode("", "0", 18, true);
                trailer += Utils.FormatCode("", "0", 18, true);
                trailer += Utils.FormatCode("", " ", 171);
                trailer += Utils.FormatCode("", " ", 10);
                trailer = Utils.SubstituiCaracteresEspeciais(trailer);

                return trailer;
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do LOTE de REMESSA.", e);
            }
        }

        /// <summary>
        ///POS INI/FINAL	DESCRIÇÂO	                   A/N	TAM	DEC	CONTEÚDO	NOTAS
        ///--------------------------------------------------------------------------------
        ///001 - 003	Código do Banco na compensação	    N	003		341	
        ///004 - 007	Lote de serviço	                    N	004		9999 
        ///008 - 008	Registro Trailer de Arquivo         N	001     9
        ///009 - 017	Complemento de Registros            A	009     Brancos
        ///018 - 023    Qtd. Lotes do Arquivo               N   006     Nota 15     
        ///024 - 029    Qtd. Registros do Arquivo           N   006     Nota 15     
        ///030 - 240    Complemento de Registros            A   211     Brancos
        /// </summary>

        public override string GerarTrailerArquivoRemessa(int numeroRegistro)
        {
            try
            {
                string trailer = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                trailer += "9999";
                trailer += "9";
                trailer += Utils.FormatCode("", " ", 9);
                trailer += Utils.FormatCode("", "0", 6, true);
                trailer += Utils.FormatCode("", "0", 6, true);
                trailer += Utils.FormatCode("", " ", 211);
                trailer = Utils.SubstituiCaracteresEspeciais(trailer);

                return trailer;
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do ARQUIVO de REMESSA.", e);
            }
        }
        #endregion TRAILER CNAB240

        #region TRAILER CNAB400

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
                        _trailer = GerarTrailerRemessa400(numeroRegistro, vltitulostotal);
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

        public string GerarTrailerRemessa400(int numeroRegistro, decimal vltitulostotal)
        {
            try
            {
                string complemento = new string('0', 374);
                string _trailer;

                _trailer = "9";
                _trailer += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true); //Quantidade total de linhas no arquivo
                _trailer += Utils.FitStringLength(vltitulostotal.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true);
                _trailer += complemento;
                _trailer += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true); // Número sequencial do registro no arquivo.

                _trailer = Utils.SubstituiCaracteresEspeciais(_trailer);

                return _trailer;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do arquivo de REMESSA.", ex);
            }
        }

        #endregion TRAILER CNAB400

        #endregion Métodos de geração do arquivo remessa

        #region Método de leitura do arquivo retorno

        public override DetalheRetorno LerDetalheRetornoCNAB400(string registro)
        {
            try
            {
                DetalheRetorno detalhe = new DetalheRetorno(registro);

                //Tipo de Inscrição Empresa
                detalhe.CodigoInscricao = Utils.ToInt32(registro.Substring(1, 2));
                //Nº Inscrição da Empresa
                detalhe.NumeroInscricao = registro.Substring(3, 14);

                //Identificação da Empresa Cedente no Banco
                detalhe.Agencia = Utils.ToInt32(registro.Substring(17, 4));
                detalhe.Conta = Utils.ToInt32(registro.Substring(21, 8));

                //Nº Controle do Participante
                detalhe.NumeroControle = registro.Substring(37, 25);
                //Identificação do Título no Banco
                detalhe.NossoNumeroComDV = registro.Substring(62, 8);
                detalhe.NossoNumero = registro.Substring(62, 7);
                detalhe.DACNossoNumero = registro.Substring(69, 1);
                //Identificação de Ocorrência
                detalhe.CodigoOcorrencia = Utils.ToInt32(registro.Substring(108, 2));

                //Descrição da ocorrência
                detalhe.DescricaoOcorrencia = this.Ocorrencia(registro.Substring(108, 2));

                //Número do Documento
                detalhe.NumeroDocumento = registro.Substring(116, 10);
                //Identificação do Título no Banco
                detalhe.IdentificacaoTitulo = registro.Substring(126, 8);

                //Valor do Título
                decimal valorTitulo = Convert.ToInt64(registro.Substring(152, 13));
                detalhe.ValorTitulo = valorTitulo / 100;
                //Banco Cobrador
                detalhe.CodigoBanco = Utils.ToInt32(registro.Substring(165, 3));
                //Agência Cobradora
                detalhe.AgenciaCobradora = Utils.ToInt32(registro.Substring(168, 5));
                //Espécie do Título
                detalhe.Especie = Utils.ToInt32(registro.Substring(173, 2));
                // IOF
                decimal iof = Convert.ToUInt64(registro.Substring(214, 13));
                detalhe.IOF = iof / 100;
                //Abatimento Concedido sobre o Título (Valor Abatimento Concedido)
                decimal valorAbatimento = Convert.ToUInt64(registro.Substring(227, 13));
                detalhe.ValorAbatimento = valorAbatimento / 100;
                //Desconto Concedido (Valor Desconto Concedido)
                decimal valorDesconto = Convert.ToUInt64(registro.Substring(240, 13));
                detalhe.Descontos = valorDesconto / 100;
                //Valor Pago
                decimal valorPago = Convert.ToUInt64(registro.Substring(253, 13));
                detalhe.ValorPago = valorPago / 100;
                //Juros Mora
                decimal jurosMora = Convert.ToUInt64(registro.Substring(266, 13));
                detalhe.JurosMora = jurosMora / 100;
                //Outros Créditos
                decimal outrosCreditos = Convert.ToUInt64(registro.Substring(279, 13));
                detalhe.OutrosCreditos = outrosCreditos / 100;
                //Data Ocorrência no Banco
                int dataOcorrencia = Utils.ToInt32(registro.Substring(110, 6));
                detalhe.DataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));
                //Data Vencimento do Título
                int dataVencimento = Utils.ToInt32(registro.Substring(146, 6));
                detalhe.DataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));
                // Data do Crédito
                int dataCredito = Utils.ToInt32(registro.Substring(295, 6));
                detalhe.DataCredito = Utils.ToDateTime(dataCredito.ToString("##-##-##"));
                //Nome do Sacado
                detalhe.NomeSacado = registro.Substring(301, 36);

                detalhe.NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6));

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }


        public override DetalheSegmentoTRetornoCNAB240 LerDetalheSegmentoTRetornoCNAB240(string registro)
        {
            try
            {
                DetalheSegmentoTRetornoCNAB240 detalhe = new DetalheSegmentoTRetornoCNAB240(registro);

                if (registro.Substring(13, 1) != "T")
                    throw new Exception("Registro inválido. O detalhe não possuí as características do segmento T.");

                detalhe.CodigoBanco = Convert.ToInt32(registro.Substring(0, 3));
                detalhe.idCodigoMovimento = Convert.ToInt32(registro.Substring(15, 2));
                detalhe.Agencia = Convert.ToInt32(registro.Substring(17, 4));
                detalhe.DigitoAgencia = registro.Substring(21, 1);
                detalhe.Conta = Convert.ToInt32(registro.Substring(22, 9));
                detalhe.DigitoConta = registro.Substring(31, 1);

                detalhe.NossoNumero = registro.Substring(40, 13);
                detalhe.CodigoCarteira = Convert.ToInt32(registro.Substring(43, 1));
                detalhe.NumeroDocumento = registro.Substring(54, 15);
                string dataVencimento = registro.Substring(69, 8);
                //detalhe.DataVencimento = Convert.ToDateTime(dataVencimento.ToString("##-##-####"));
                detalhe.DataVencimento = DateTime.ParseExact(dataVencimento, "ddMMyyyy", CultureInfo.InvariantCulture);
                decimal valorTitulo = Convert.ToInt64(registro.Substring(77, 15));
                detalhe.ValorTitulo = valorTitulo / 100;
                detalhe.IdentificacaoTituloEmpresa = registro.Substring(100, 25);
                detalhe.TipoInscricao = Convert.ToInt32(registro.Substring(127, 1));
                detalhe.NumeroInscricao = registro.Substring(128, 15);
                detalhe.NomeSacado = registro.Substring(143, 40);
                decimal valorTarifas = Convert.ToUInt64(registro.Substring(193, 15));
                detalhe.ValorTarifas = valorTarifas / 100;
                detalhe.CodigoRejeicao = registro.Substring(208, 10);
                detalhe.UsoFebraban = registro.Substring(218, 22);

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO T.", ex);
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

        public override void OnGeraHtmlReciboCedente(StringBuilder html, Boleto boleto)
        {
            //Para SANTANDER, a ficha de compensação não possui código da carteira - por jsoda em 08/12/2012
            html.Replace("Carteira /", "");
        }

        public override string FormataAgenciaCodigoCedente(Cedente cedente)
        {
            if (!cedente.DigitoCedente.Equals(-1))
            {
                return base.FormataAgenciaCodigoCedente(cedente);
            }
            else
            {
                if (String.IsNullOrEmpty(cedente.ContaBancaria.DigitoAgencia))
                {
                    return String.Format("{0}/{1}", cedente.ContaBancaria.Agencia, Utils.FormatCode(cedente.Codigo.ToString(), 6));
                }
                else {
                    //Para banco SANTANDER, a formatação do campo "Agencia/Identif.Cedente" - por jsoda em 07/05/2012
                    return string.Format("{0}-{1}/{2}", cedente.ContaBancaria.Agencia, cedente.ContaBancaria.DigitoAgencia, Utils.FormatCode(cedente.Codigo.ToString(), 6));
                }
                
            }
        }

        public override string FormataDescricaoCarteira(Boleto boleto)
        {
            string descricaoCarteira = new Carteira_Santander(Utils.ToInt32(boleto.Carteira)).Codigo;

            if (string.IsNullOrEmpty(descricaoCarteira))
            {
                throw new Exception("O código da carteira não foi implementado.");
            }

            return string.Format("{0} - {1}", boleto.Carteira,
                    descricaoCarteira);
        }



    }
}