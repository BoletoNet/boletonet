using System;
using System.Globalization;
using System.Web.UI;
using BoletoNet.Util;
using System.Linq;
using BoletoNet.Excecoes;

[assembly: WebResource("BoletoNet.Imagens.033.jpg", "image/jpg")]
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
            //string nossoNumero = Utils.FormatCode(boleto.NossoNumero, 12) + Mod11Santander(Utils.FormatCode(boleto.NossoNumero, 12));//13
            string nossoNumero = Utils.FormatCode(boleto.NossoNumero, 12) + CalcularDVSantander(Utils.FormatCode(boleto.NossoNumero, 12));//13
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
            //string nossoNumero = Utils.FormatCode(boleto.NossoNumero, 12) + Mod11Santander(Utils.FormatCode(boleto.NossoNumero, 12));//13
            string nossoNumero = Utils.FormatCode(boleto.NossoNumero, 12) + CalcularDVSantander(Utils.FormatCode(boleto.NossoNumero, 12));//13
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
            //string DVnossoNumero = Utils.FormatCode(boleto.NossoNumero, 12) + Mod11Santander(Utils.FormatCode(boleto.NossoNumero, 12));
            string DVnossoNumero = Utils.FormatCode(boleto.NossoNumero, 12) + CalcularDVSantander(Utils.FormatCode(boleto.NossoNumero, 12));
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
        }

        public override void FormataNossoNumero(Boleto boleto)
        {
            //boleto.NossoNumero = string.Format("{0}-{1}", boleto.NossoNumero, Mod11Santander(boleto.NossoNumero));
            boleto.NossoNumero = string.Format("{0}-{1}", boleto.NossoNumero, CalcularDVSantander(boleto.NossoNumero));
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
                if (boleto.NossoNumero.Length < 12 && (boleto.Carteira.Equals("101") || boleto.Carteira.Equals("102")))
                    boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, "0", 12, true);

                if (boleto.NossoNumero.Length != 12)
                    throw new NotSupportedException("Nosso Número deve ter 12 posições para o banco 033.");
            }

            if (boleto.Cedente.Codigo.ToString().Length > 8)
                throw new NotImplementedException("Código cedente deve ter 8 posições.");

            // Atribui o nome do banco ao local de pagamento
            if (string.IsNullOrEmpty(boleto.LocalPagamento))
                boleto.LocalPagamento = "Grupo Santander - GC";

            if (EspecieDocumento.ValidaSigla(boleto.EspecieDocumento) == "")
                boleto.EspecieDocumento = new EspecieDocumento_Santander("2");

            if (boleto.PercentualIOS > 10 & (this.Codigo == 8 || this.Codigo == 33 || this.Codigo == 353))
                throw new Exception("O percentual do IOS é limitado a 9% para o Banco Santander");

            boleto.FormataCampos();
        }

        private static int Mod11Santander(string nossoNumero)
        {
            var digito = 0;
            var multiplicador = 2;
            var total = 0;
            var nossoNumeroArray = nossoNumero.ToCharArray().Reverse();

            foreach (var numero in nossoNumeroArray)
            {
                total += multiplicador * numero;

                if (++multiplicador > 9)
                {
                    multiplicador = 2;
                }
            }

            var modulo = total % 11;

            if (modulo > 1)
            {
                digito = 11 - modulo;
            }

            return digito;
        }

        private static string CalcularDVSantander(string texto)
        {
            //Codigo retirado do projeto Boleto2net
            string digito;
            int pesoMaximo = 9, soma = 0, peso = 2;
            for (var i = texto.Length - 1; i >= 0; i--)
            {
                soma = soma + Convert.ToInt32(texto.Substring(i, 1)) * peso;
                if (peso == pesoMaximo)
                    peso = 2;
                else
                    peso = peso + 1;
            }
            var resto = soma % 11;
            if (resto <= 1)
                digito = "0";
            else
                digito = (11 - resto).ToString();
            return digito;
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
                num = seq.Mid(pos, 1);
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
                        _header = GerarHeaderRemessaCNAB240(cedente, numeroArquivoRemessa);
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
        /// Gera Header da Remessa para o CNAB 240
        /// </summary>
        /// <param name="cedente"></param>
        /// <param name="numeroArquivoRemessa"></param>
        /// <returns></returns>
        public string GerarHeaderRemessaCNAB240(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                //Código do Banco na compensação ==> 001 - 003
                string header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);

                //Lote de serviço ==> 004 - 007
                header += "0000";

                //Tipo de registro ==> 008 - 008
                header += "0";

                //Reservado (uso Banco) ==> 009 - 016
                header += Utils.FormatCode("", " ", 8);

                //Tipo de inscrição da empresa ==> 017 - 017
                header += (cedente.CPFCNPJ.Length == 11 ? "1" : "2");

                //Nº de inscrição da empresa ==> 018 – 032
                header += Utils.FormatCode(cedente.CPFCNPJ, "0", 15, true);

                //Código de Transmissão ==> 033 – 047
                header += Utils.FormatCode(cedente.CodigoTransmissao, "0", 15, true);

                //Reservado (uso Banco) ==> 048 - 072
                header += Utils.FormatCode("", " ", 25);

                //Nome da empresa ==> 073 - 102
                header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false).ToUpper();

                //Nome do Banco ==> 103 - 132
                header += Utils.FitStringLength("BANCO SANTANDER", 30, 30, ' ', 0, true, true, false);

                //Reservado (uso Banco) ==> 133 - 142
                header += Utils.FormatCode("", " ", 10);

                //Código remessa ==> 143 - 143
                header += "1";

                //Data de geração do arquivo ==> 144 - 151
                header += DateTime.Now.ToString("ddMMyyyy");

                //Reservado (uso Banco) ==> 152 - 157
                header += Utils.FormatCode("", " ", 6);
                //Nº seqüencial do arquivo ==> 158 - 163
                header += Utils.FormatCode(numeroArquivoRemessa.ToString(), "0", 6, true);

                //Nº da versão do layout do arquivo ==> 164 - 166
                header += "040";

                //Reservado (uso Banco) ==> 167 - 240
                header += Utils.FormatCode("", " ", 74);

                return Utils.SubstituiCaracteresEspeciais(header);

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

                //Código do registro ==> 001 - 001
                _header = "0";

                //Código da remessa ==> 002 - 002
                _header += "1";

                //Literal de transmissão ==> 003 - 009
                _header += "REMESSA";

                //Código do serviço ==> 010 - 011
                _header += "01";

                //Literal de serviço ==> 012 - 026
                _header += Utils.FitStringLength("COBRANCA", 15, 15, ' ', 0, true, true, false);

                //Código de Transmissão ==> 027 - 046
                _header += Utils.FitStringLength(cedente.CodigoTransmissao, 20, 20, '0', 0, true, true, true);

                //Nome do cedente  ==> 047 - 076
                _header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false);

                //Código do Banco ==> 077 - 079 
                _header += Utils.FormatCode(Codigo.ToString(), "0", 3, true);

                //Nome do Banco ==> 080 - 094
                _header += Utils.FitStringLength("SANTANDER", 15, 15, ' ', 0, true, true, false);

                //Data de Gravação ==> 095 - 100
                _header += Utils.FitStringLength(DateTime.Now.ToString("ddMMyy"), 6, 6, ' ', 0, true, true, false);

                //Zeros ==> 101 - 116
                _header += Utils.FitStringLength("0", 16, 16, '0', 0, true, true, false);

                //Mensagem 1 ==> 117 - 163
                _header += Utils.FitStringLength(" ", 47, 47, ' ', 0, true, true, false);

                //Mensagem 2 ==> 164 - 210
                _header += Utils.FitStringLength(" ", 47, 47, ' ', 0, true, true, false);

                //Mensagem 3 ==> 211 - 257
                _header += Utils.FitStringLength(" ", 47, 47, ' ', 0, true, true, false);

                //Mensagem 4 ==> 258 - 304
                _header += Utils.FitStringLength(" ", 47, 47, ' ', 0, true, true, false);

                //Mensagem 5 ==> 305 - 351
                _header += Utils.FitStringLength(" ", 47, 47, ' ', 0, true, true, false);

                //Mensagem 6 ==> 352 - 391
                _header += Utils.FitStringLength(" ", 40, 40, ' ', 0, true, true, false);

                //Número da versão da remessa (opcional) ==> 392 - 394
                _header += Utils.FitStringLength("0", 3, 3, '0', 0, true, true, true);

                //Número sequencial do registro no arquivo ==> 395 - 400
                _header += Utils.FitStringLength("1", 6, 6, '0', 0, true, true, true);

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
                //Código do Banco na compensação ==> 001 - 003
                string header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);

                //Numero do lote remessa ==> 004 - 007
                header += "0001";

                //Tipo de registro ==> 008 - 008
                header += "1";

                //Tipo de operação ==> 009 - 009
                header += "R";

                //Tipo de serviço ==> 010 - 011
                header += "01";

                //Reservado (uso Banco) ==> 012 - 013
                header += "  ";

                //Nº da versão do layout do lote ==> 014 - 016
                header += "030";

                //Reservado (uso Banco) ==> 017 - 017
                header += " ";

                //Tipo de inscrição da empresa ==> 018 - 018
                header += (cedente.CPFCNPJ.Length == 11 ? "1" : "2");

                //Nº de inscrição da empresa ==> 019 - 033
                header += Utils.FormatCode(cedente.CPFCNPJ, "0", 15, true);

                //Reservado (uso Banco) ==> 034 – 053
                header += Utils.FormatCode("", " ", 20);

                //Código de Transmissão ==> 054 - 068
                header += Utils.FormatCode(cedente.CodigoTransmissao, "0", 15, true);

                //Reservado uso Banco ==> 069 – 073
                header += Utils.FormatCode("", " ", 5);

                //Nome do Cedente ==> 074 - 103
                header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false).ToUpper();

                //Mensagem 1 ==> 104 - 143
                header += Utils.FormatCode("", " ", 40);

                //Mensagem 2 ==> 144 - 183
                header += Utils.FormatCode("", " ", 40);
                //Número remessa/retorno ==> 184 - 191
                header += Utils.FormatCode(cedente.NumeroSequencial.ToString(), "0", 8, true);

                //Data da gravação remessa/retorno ==> 192 - 199
                header += DateTime.Now.ToString("ddMMyyyy");

                //Reservado (uso Banco) ==> 200 - 240
                header += Utils.FormatCode("", " ", 41);

                return Utils.SubstituiCaracteresEspeciais(header);
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar HEADER DO LOTE do arquivo de remessa.", e);
            }
        }

        #endregion HEADER LOTE REMESSA

        #region DETALHE REMESSA

        public override string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio)
        {
            try
            {
                string _segmentoP;

                //Código do Banco na compensação ==> 001-003
                _segmentoP = Utils.FormatCode(Codigo.ToString(), "0", 3, true);

                //Numero do lote remessa ==> 004 - 007
                _segmentoP += Utils.FitStringLength("1", 4, 4, '0', 0, true, true, true);

                //Tipo de registro => 008 - 008
                _segmentoP += "3";

                //Nº seqüencial do registro de lote ==> 009 - 013
                _segmentoP += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);

                //Cód. Segmento do registro detalhe ==> 014 - 014
                _segmentoP += "P";

                //Reservado (uso Banco) ==> 015 - 015
                _segmentoP += " ";

                //Código de movimento remessa ==> 016 - 017
                _segmentoP += ObterCodigoDaOcorrencia(boleto);

                //Agência do Cedente ==> 018 –021
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true);

                //Dígito da Agência do Cedente ==> 022 –022
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoAgencia, 1, 1, '0', 0, true, true, true);

                //Número da conta corrente ==> 023 - 031
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 9, 9, '0', 0, true, true, true);

                //Dígito verificador da conta ==> 032 – 032
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true);

                //Conta cobrança ==> 033 - 041
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 9, 9, '0', 0, true, true, true);

                //Dígito da conta cobrança ==> 042 - 042
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true);

                //Reservado (uso Banco) ==> 043 - 044
                _segmentoP += "  ";

                boleto.Valida();
                //Identificação do título no Banco ==> 045 –057
                _segmentoP += Utils.FitStringLength(boleto.NossoNumero.Replace("-", ""), 13, 13, '0', 0, true, true, true);

                //Tipo de cobrança ==> 058 - 058
                _segmentoP += "5";

                //Forma de Cadastramento ==> 059 - 059
                _segmentoP += "1";

                //Tipo de documento ==> 060 - 060
                _segmentoP += "1";

                //Reservado (uso Banco) ==> 061 –061
                _segmentoP += " ";

                //Reservado (uso Banco) ==> 062 - 062
                _segmentoP += " ";

                //Nº do documento ==> 063 - 077
                _segmentoP += Utils.FitStringLength(boleto.NumeroDocumento, 15, 15, ' ', 0, true, true, false);

                //Data de vencimento do título ==> 078 - 085
                _segmentoP += boleto.DataVencimento.ToString("ddMMyyyy");

                //Valor nominal do título ==> 086 - 100
                _segmentoP += Utils.FitStringLength(boleto.ValorBoleto.ApenasNumeros(), 15, 15, '0', 0, true, true, true);

                //Agência encarregada da cobrança ==> 101 - 104
                _segmentoP += "0000";

                //Dígito da Agência do Cedente ==> 105 – 105
                _segmentoP += "0";

                //Reservado (uso Banco) ==> 106 - 106
                _segmentoP += " ";

                //Espécie do título ==> 107 – 108
                _segmentoP += Utils.FitStringLength(boleto.EspecieDocumento.Codigo, 2, 2, '0', 0, true, true, true);

                //Identif. de título Aceito/Não Aceito ==> 109 - 109
                _segmentoP += "N";

                //Data da emissão do título ==> 110 - 117
                _segmentoP += boleto.DataDocumento.ToString("ddMMyyyy");

                if (boleto.JurosMora > 0)
                {
                    //Código do juros de mora ==> 118 - 118
                    if (!String.IsNullOrWhiteSpace(boleto.CodJurosMora)) //Possibilita passar o código 2 para JurosMora ao Mes, senão for setado, assume o valor padrão 1 para JurosMora ao Dia
                        _segmentoP += Utils.FitStringLength(boleto.CodJurosMora.ToString(), 1, 1, '0', 0, true, true, true); 
                    else
                        _segmentoP += "1";

                    //Data do juros de mora ==> 119 - 126
                    _segmentoP += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false);

                    //Valor da mora/dia ou Taxa mensal ==> 127 - 141
                    _segmentoP += Utils.FitStringLength(boleto.JurosMora.ApenasNumeros(), 15, 15, '0', 0, true, true, true);
                }
                else
                {
                    //Código do juros de mora ==> 118 - 118
                    _segmentoP += "3";

                    //Data do juros de mora ==> 119 - 126
                    _segmentoP += "00000000";

                    //Valor da mora/dia ou Taxa mensal ==> 127 - 141
                    _segmentoP += "000000000000000";
                }

                if (boleto.ValorDesconto > 0)
                {
                    //Código do desconto 1 ==> 142 - 142
                    _segmentoP += "1";

                    //Data de desconto 1 ==> 143 - 150
                    _segmentoP += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false);

                    //Valor ou Percentual do desconto concedido ==> 151 - 165
                    _segmentoP += Utils.FitStringLength(boleto.ValorDesconto.ApenasNumeros(), 15, 15, '0', 0, true, true, true);
                }
                else
                    _segmentoP += "0".PadLeft(24, '0');


                //Valor do IOF a ser recolhido ==> 166 - 180
                _segmentoP += "0".PadLeft(15, '0');

                //Valor do abatimento ==> 181 - 195
                _segmentoP += "0".PadLeft(15, '0');

                //Identificação do título na empresa ==> 196 - 220
                _segmentoP += Utils.FitStringLength(boleto.NumeroDocumento, 25, 25, ' ', 0, true, true, false);

                string codigo_protesto = "0";
                string dias_protesto = "00";

                foreach (var instrucao in boleto.Instrucoes)
                {
                    switch ((EnumInstrucoes_Santander)instrucao.Codigo)
                    {
                        case EnumInstrucoes_Santander.Protestar:
                            codigo_protesto = "1";
                            dias_protesto = Utils.FitStringLength(instrucao.QuantidadeDias.ToString(), 2, 2, '0', 0, true, true, true); //Para código '1' – é possível, de 6 a 29 dias
                            break;
                        default:
                            break;
                    }
                }

                //Código para protesto ==> 221 - 221
                _segmentoP += codigo_protesto;

                //Número de dias para protesto ==> 222 - 223
                _segmentoP += dias_protesto;

                //Código para Baixa/Devolução ==> 222 - 223
                _segmentoP += "3";

                //Reservado (uso Banco) ==> 225 – 225
                _segmentoP += "0";

                //Número de dias para Baixa/Devolução ==> 226 - 227
                _segmentoP += Utils.FitStringLength(boleto.NumeroDiasBaixa.ToString(), 2, 2, '0', 0, true, true, true); 

                //Código da moeda ==> 228 - 229
                _segmentoP += "00";

                //Reservado (uso Banco) ==> 230 –240
                _segmentoP += " ".PadLeft(11, ' ');


                _segmentoP = Utils.SubstituiCaracteresEspeciais(_segmentoP);

                return _segmentoP;

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

                string _segmentoQ;

                //Código do Banco na compensação ==> 001 - 003
                _segmentoQ = Utils.FormatCode(Codigo.ToString(), "0", 3, true);

                //Numero do lote remessa ==> 004 - 007
                _segmentoQ += Utils.FitStringLength("1", 4, 4, '0', 0, true, true, true);

                //Tipo de registro ==> 008 - 008
                _segmentoQ += "3";

                //Nº seqüencial do registro no lote ==> 009 - 013
                _segmentoQ += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);

                //Cód. segmento do registro detalhe ==> 014 - 014
                _segmentoQ += "Q";

                //Reservado (uso Banco) ==> 015 - 015
                _segmentoQ += " ";

                //Código de movimento remessa ==> 016 - 017
                _segmentoQ += ObterCodigoDaOcorrencia(boleto);

                if (boleto.Sacado.CPFCNPJ.Length <= 11)
                    //Tipo de inscrição do sacado ==> 018 - 018
                    _segmentoQ += "1";
                else
                    //Tipo de inscrição do sacado ==> 018 - 018
                    _segmentoQ += "2";

                //Número de inscrição do sacado ==> 019 - 033
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.CPFCNPJ, 15, 15, '0', 0, true, true, true);

                //Nome sacado ==> 034 - 073
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Nome.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();

                //Endereço sacado ==> 074 - 113
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.End.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();

                //Bairro sacado ==> 114 - 128
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.Bairro.TrimStart(' '), 15, 15, ' ', 0, true, true, false).ToUpper();

                //Cep sacado ==> 129 - 133
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.CEP.Substring(0, 5), 5, 5, ' ', 0, true, true, false).ToUpper();

                //Sufixo do Cep do sacado ==> 134 - 136
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.CEP.Substring(5, 3), 3, 3, ' ', 0, true, true, false).ToUpper();

                //Cidade do sacado ==> 137 - 151
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.Cidade.TrimStart(' '), 15, 15, ' ', 0, true, true, false).ToUpper();

                //Unidade da federação do sacado ==> 152 - 153
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.UF, 2, 2, ' ', 0, true, true, false).ToUpper();

                //Tipo de inscrição sacador/avalista ==> 154 - 154
                _segmentoQ += "0";

                //Nº de inscrição sacador/avalista ==> 155 - 169
                _segmentoQ += "0".PadLeft(15, '0');

                //Nome do sacador/avalista ==> 170 - 209
                _segmentoQ += " ".PadLeft(40, ' ');

                //Identificador de carne ==> 210 –212
                _segmentoQ += "0".PadLeft(3, '0');

                //Seqüencial da Parcela ou número inicial da parcela ==> 213 –215
                _segmentoQ += "0".PadLeft(3, '0');

                //Quantidade total de parcelas ==> 216 –218
                _segmentoQ += "0".PadLeft(3, '0');

                //Número do plano ==> 219 – 221
                _segmentoQ += "0".PadLeft(3, '0');

                //Reservado (uso Banco) ==> 222 - 240
                _segmentoQ += " ".PadLeft(19, ' ');

                return Utils.SubstituiCaracteresEspeciais(_segmentoQ);

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do SEGMENTO Q DO DETALHE do arquivo de REMESSA.", ex);
            }
        }
        public override string GerarDetalheSegmentoRRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _brancos110 = new string(' ', 110);
                string _brancos9 = new string(' ', 9);

                string _segmentoR;

                //Código do Banco na compensação ==> 001 - 003
                _segmentoR = Utils.FormatCode(Codigo.ToString(), "0", 3, true);

                //Numero do lote remessa ==> 004 - 007
                _segmentoR += Utils.FitStringLength("1", 4, 4, '0', 0, true, true, true);

                //Tipo de registro ==> 008 - 008
                _segmentoR += "3";

                //Nº seqüencial do registro de lote ==> 009 - 013
                _segmentoR += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);

                //Código segmento do registro detalhe ==> 014 - 014
                _segmentoR += "R";

                //Reservado (uso Banco) ==> 015 - 015
                _segmentoR += " ";

                //Código de movimento ==> 016 - 017
                _segmentoR += ObterCodigoDaOcorrencia(boleto);
                                               
                //Suelton - 14/12/2018 - Implementação do 2 desconto por antecipação
                if (boleto.DataDescontoAntecipacao2.HasValue && boleto.ValorDescontoAntecipacao2.HasValue)
                {
                    _segmentoR += "1" + //'1' = Valor Fixo Até a Data Informada
                        Utils.FitStringLength(boleto.DataDescontoAntecipacao2.Value.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false) +
                        Utils.FitStringLength(boleto.ValorDescontoAntecipacao2.ApenasNumeros(), 15, 15, '0', 0, true, true, true);
                }
                else
                {
                    // Desconto 2
                    _segmentoR += "000000000000000000000000"; //24 zeros
                }

                //Suelton - 14/12/2018 - Implementação do 3 desconto por antecipação
                if (boleto.DataDescontoAntecipacao3.HasValue && boleto.ValorDescontoAntecipacao3.HasValue)
                {
                    _segmentoR += "1" + //'1' = Valor Fixo Até a Data Informada
                        Utils.FitStringLength(boleto.DataDescontoAntecipacao3.Value.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false) +
                        Utils.FitStringLength(boleto.ValorDescontoAntecipacao3.ApenasNumeros(), 15, 15, '0', 0, true, true, true);
                }
                else
                {
                    // Desconto 3
                    _segmentoR += "000000000000000000000000"; //24 zeros
                }

                #region Deprecado
                //Com a implementação dos campo descontos 2 e 3 não é mais recomendado utilizar o campo outros descontos
                //a lógica já foi toda implementada nesses campos
                //else if (boleto.OutrosDescontos > 0)
                //{
                //    //Código do desconto 2 ==> 018 - 018
                //    _segmentoR += "1";

                //    //Data do desconto 2 ==> 019 - 026
                //    _segmentoR += boleto.DataOutrosDescontos.ToString("ddMMyyyy");

                //    //Valor/Percentual a ser concedido ==> 027 - 041
                //    _segmentoR += Utils.FitStringLength(boleto.OutrosDescontos.ApenasNumeros(), 15, 15, '0', 0, true, true, true);
                //}
                //else
                //{
                //    //Código do desconto 2 ==> 018 - 018
                //    _segmentoR += "0";

                //    //Data do desconto 2 ==> 019 - 026
                //    _segmentoR += "0".PadLeft(8, '0');

                //    //Valor/Percentual a ser concedido ==> 027 - 041
                //    _segmentoR += "0".PadLeft(15, '0');


                //}

                ////Reservado (uso Banco) ==> 042 – 065
                //_segmentoR += " ".PadLeft(24, ' '); 
                #endregion

                if (boleto.PercMulta > 0)
                {
                    //Código da multa ==> 066 - 066
                    _segmentoR += "2";

                    //Data da multa ==> 067 - 074
                    _segmentoR += boleto.DataMulta.ToString("ddMMyyyy");

                    //Valor/Percentual a ser aplicado ==> 075 - 089
                    _segmentoR += Utils.FitStringLength(boleto.PercMulta.ApenasNumeros(), 15, 15, '0', 0, true, true, true);
                }
                else if (boleto.ValorMulta > 0)
                {
                    //Código da multa ==> 066 - 066
                    _segmentoR += "1";

                    //Data da multa ==> 067 - 074
                    _segmentoR += boleto.DataMulta.ToString("ddMMyyyy");

                    //Valor/Percentual a ser aplicado ==> 075 - 089
                    _segmentoR += Utils.FitStringLength(boleto.ValorMulta.ApenasNumeros(), 15, 15, '0', 0, true, true, true);
                }
                else
                {
                    //Código da multa ==> 066 - 066
                    _segmentoR += "0";

                    //Data da multa ==> 067 - 074
                    _segmentoR += "0".PadLeft(8, '0');

                    //Valor/Percentual a ser aplicado ==> 075 - 089
                    _segmentoR += "0".PadLeft(15, '0');
                }

                //Reservado (uso Banco) ==> 090 - 099
                _segmentoR += " ".PadLeft(10, ' ');

                //Mensagem 3 ==> 100 - 139
                _segmentoR += " ".PadLeft(40, ' ');

                //Mensagem 4 ==> 140 - 179
                _segmentoR += " ".PadLeft(40, ' ');

                //Reservado ==> 180 - 240
                _segmentoR += " ".PadLeft(61, ' ');

                _segmentoR = Utils.SubstituiCaracteresEspeciais(_segmentoR);

                return _segmentoR;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do SEGMENTO R DO DETALHE do arquivo de REMESSA.", ex);
            }
        }
        public override string GerarDetalheSegmentoSRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {

                string _segmentoS;

                //Código do Banco na compensação ==> 001 - 003
                _segmentoS = Utils.FormatCode(Codigo.ToString(), "0", 3, true);

                //Numero do lote remessa ==> 004 - 007
                _segmentoS += Utils.FitStringLength("1", 4, 4, '0', 0, true, true, true);

                //Tipo de registro ==> 008 - 008
                _segmentoS += "3";

                //Nº seqüencial do registro de lote ==> 009 - 013
                _segmentoS += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);

                //Cód. Segmento do registro detalhe ==> 014 - 014
                _segmentoS += "S";

                //Reservado (uso Banco) ==> 015 - 015
                _segmentoS += " ";

                //Código de movimento ==> 016 - 017
                _segmentoS += ObterCodigoDaOcorrencia(boleto);

                //Identificação da impressão ==> 018 - 018
                _segmentoS += "2";

                //Mensagem 5 ==> 019 - 058
                //Mensagem 6 ==> 059 - 098
                //Mensagem 7 ==> 099 - 138
                for (int i = 0; i < 3; i++)
                {
                    if (boleto.Instrucoes.Count > i)
                        _segmentoS += Utils.FitStringLength(boleto.Instrucoes[i].Descricao, 40, 40, ' ', 0, true, true, false);
                    else
                        _segmentoS += Utils.FitStringLength(" ", 40, 40, ' ', 0, true, true, false);
                }

                //Mensagem 8 ==> 139 - 178
                _segmentoS += " ".PadLeft(40, ' ');

                //Mensagem 9 ==> 179 - 218
                _segmentoS += " ".PadLeft(40, ' ');

                //Reservado (uso Banco) ==> 219 - 240
                _segmentoS += " ".PadLeft(22, ' ');

                _segmentoS = Utils.SubstituiCaracteresEspeciais(_segmentoS);

                return _segmentoS;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do SEGMENTO S DO DETALHE do arquivo de REMESSA.", ex);
            }
        }

        /// <summary>
        /// DETALHE do arquivo CNAB
        /// Gera o DETALHE do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _detalhe = "";

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
                        _detalhe = GerarMensagemVariavelRemessaCNAB400(boleto, ref numeroRegistro, tipoArquivo);
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

                //string controle_partic = new string(' ', 25);
                string sacador_aval = new string(' ', 30);
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

                //Código da agência cedente ==> 018 - 021
                _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true);

                //Conta movimento cedente ==> 022 - 029
                _detalhe += Utils.FitStringLength(boleto.Cedente.Codigo.ToString(), 8, 8, '0', 0, true, true, true);

                //Conta cobrança cedente ==> 030 - 037                
                if (boleto.Cedente.ContaBancaria.Conta.Length == 9 || (!String.IsNullOrEmpty(boleto.Cedente.ContaBancaria.DigitoConta) && boleto.Cedente.ContaBancaria.Conta.Length == 8))
                    _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta.Substring(0, 8), 8, 8, '0', 0, true, true, true); //alterado por diegodariolli - 15/03/2018 - estava cortando um digito
                else
                    _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 8, 8, '0', 0, true, true, true);
                //Bloco if-else adicionado por Jéferson (jefhtavares). Segundo o Banco o código de transmissão muda de acordo com o tamanho (length) da conta corrente

                //Número de controle do participante, controle do cedente ==> 038 - 062
                var numeroControle = string.Empty;
                if (!string.IsNullOrEmpty(boleto.NumeroControle))
                    numeroControle = boleto.NumeroControle;

                _detalhe += Utils.FitStringLength(numeroControle, 25, 25, ' ', 0, true, true, false); //alterado por diegodariolli - 15/03/2018 - estava passando vazio impossibilitando controle interno

                //NossoNumero com DV, pegar os 8 primeiros dígitos, da direita para esquerda ==> 063 - 070
                //string nossoNumero = Utils.FormatCode(boleto.NossoNumero, 12) + Mod11Santander(Utils.FormatCode(boleto.NossoNumero, 12));//13
                string nossoNumero = Utils.FormatCode(boleto.NossoNumero, 12) + CalcularDVSantander(Utils.FormatCode(boleto.NossoNumero, 12));//13
                _detalhe += Utils.Right(nossoNumero, 8, '0', true);

                //Data do segundo desconto 9(06) ==> 071 - 076
                _detalhe += "000000";

                //brancos ==> 077 - 077
                _detalhe += " ";

                //Informação de multa = 4, senão houver informar zero ==> 078 - 078
                if (boleto.PercMulta == 0)
                    _detalhe += "0";
                else
                    _detalhe += "4";

                //Percentual multa por atraso % ==> 079 - 082
                _detalhe += Utils.FitStringLength(boleto.PercMulta.ApenasNumeros(), 4, 4, '0', 0, true, true, true);
                //Unidade de valor moeda corrente ==> 083 - 084
                _detalhe += "00";

                //Valor do título em outra unidade ==> 082 - 097
                _detalhe += "0000000000000";

                //Brancos ==> 098 - 101
                _detalhe += "    ";

                //Data para cobrança de multa ==> 102 - 107
                _detalhe += "000000";

                //Código da carteira ==> 108 - 108
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

                //Código de ocorrência  ==> 109 - 110
                //01 - Entrada de Título
                //02 - Baixa de Título
                //04 - Concessão de abatimento
                //05 - Cancelamento de abatimento
                //06 - Prorrogação de vencimento
                //07 - Alteração Número de controle do cedente
                //08 - Alteração do seu Número
                //09 - Protestar
                //18 - Sustar protesto
                _detalhe += ObterCodigoDaOcorrencia(boleto);

                //Nº do documento ==> 111 - 120
                _detalhe += Utils.FitStringLength(boleto.NumeroDocumento, 10, 10, ' ', 0, true, true, false);

                //Data de vencimento do título ==> 121 - 126
                _detalhe += boleto.DataVencimento.ToString("ddMMyy");

                //Valor do título - moeda corrente ==> 127 - 139
                _detalhe += Utils.FitStringLength(boleto.ValorBoleto.ApenasNumeros(), 13, 13, '0', 0, true, true, true);

                //Número do Banco cobrador ==> 140 - 142
                _detalhe += "033";

                //Código da agência cobradora do Banco Santander informar somente se carteira for igual a 5, caso contrário, informar zeros. ==> 143 - 147
                if (carteira == "5") //alterado por diegodariolli - 15/03/2018 - verificação já feita anteriormente
                {
                    _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true);
                    _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoAgencia, 1, 1, '0', 0, true, true, true);
                }
                else
                {
                    _detalhe += Utils.FitStringLength("0", 5, 5, '0', 0, true, true, true); //alterado por diegodariolli - 15/03/2018
                }


                //Espécie de documento ==> 148 - 149
                //01 - Duplicata
                //02 - Nota promissória
                //04 - Apólice / Nota Seguro
                //05 - Recibo
                //06 - Duplicata de serviço
                //07 - Letra de cambio
                string especie_doc = "00";

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

                //Tipo de aceite ==> 150 - 150
                _detalhe += "N";

                //Data da emissão do título ==> 151 - 156
                _detalhe += boleto.DataDocumento.ToString("ddMMyy");

                //Primeira instrução cobrança ==> 157 - 158
                if (boleto.Instrucoes.Count > 0)
                    _detalhe += Utils.FitStringLength(boleto.Instrucoes[0].Codigo.ToString(), 2, 2, '0', 0, true, true, true);
                else
                    _detalhe += "00"; //Não há instruções

                //Segunda instrução cobrança==> 159 - 160
                if (boleto.Instrucoes.Count > 1)
                    _detalhe += Utils.FitStringLength(boleto.Instrucoes[1].Codigo.ToString(), 2, 2, '0', 0, true, true, true);
                else
                    _detalhe += "00"; //Não há instruções

                //Valor de mora a ser cobrado por dia de atraso == > 161 - 173
                _detalhe += Utils.FitStringLength(boleto.JurosMora.ApenasNumeros(), 13, 13, '0', 0, true, true, true);

                //Data limite para concessão de desconto ==> 174 - 179
                var dataLimiteConcessaoDesconto = "000000";
                if (boleto.ValorDesconto > 0)
                {
                    dataLimiteConcessaoDesconto = boleto.DataVencimento.ToString("ddMMyy");
                }

                _detalhe += dataLimiteConcessaoDesconto;

                //Valor de desconto a ser concedido ==> 180 - 192
                _detalhe += Utils.FitStringLength(boleto.ValorDesconto.ApenasNumeros(), 13, 13, '0', 0, true, true, true);

                //Valor do IOF a ser recolhido pelo Banco para nota de seguro ==> 192 - 205
                _detalhe += "0000000000000";

                //Valor do abatimento a ser concedido ou valor do segundo desconto. ==> 206 - 218
                _detalhe += "0000000000000";

                //Tipo de inscrição do sacado  ==> 219 - 220
                if (boleto.Sacado.CPFCNPJ.Length <= 11)
                    _detalhe += "01";  // CPF
                else
                    _detalhe += "02"; // CNPJ

                //CNPJ ou CPF do sacado ==> 221 - 234
                _detalhe += Utils.FitStringLength(boleto.Sacado.CPFCNPJ, 14, 14, '0', 0, true, true, true).ToUpper();

                //Nome do sacado ==> 235 - 274
                _detalhe += Utils.FitStringLength(boleto.Sacado.Nome.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();

                //Endereço do sacado ==> 275 - 314
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.EndComNumeroEComplemento.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();

                //Bairro do sacado (opcional) ==> 315 - 326
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Bairro.TrimStart(' '), 12, 12, ' ', 0, true, true, false).ToUpper();

                //CEP do sacado 327 - 334
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.CEP, 8, 8, ' ', 0, true, true, true);

                //Município do sacado ==> 335 - 349
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Cidade.TrimStart(' '), 15, 15, ' ', 0, true, true, false).ToUpper();

                //UF Estado do sacado ==> 350 - 351
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.UF, 2, 2, ' ', 0, true, true, false).ToUpper();

                //Nome do Sacador ou coobrigado ==> 352 - 381
                _detalhe += sacador_aval;

                //Brancos ==> 382 - 382
                _detalhe += " ";

                // Identificador de complemento de conta cobrança ==> 383 - 383
                _detalhe += "I";

                //Complemento da conta ==> 384 - 385
                _detalhe += boleto.Cedente.ContaBancaria.Conta.Substring(boleto.Cedente.ContaBancaria.Conta.Length - 1, 1) + boleto.Cedente.ContaBancaria.DigitoConta;

                //Brancos ==> 386 - 391
                _detalhe += "      "; //brancos X(06)

                //Número de dias para protesto ==> 392 - 393
                var instrucao = boleto.Instrucoes.FirstOrDefault(x => x.Codigo == 6);
                if (instrucao != null)
                {
                    _detalhe += Utils.FitStringLength(((Instrucao_Santander)instrucao).QuantidadeDias.ToString(), 2, 2, '0', 0, true, true, true);
                }
                else
                    _detalhe += "00";

                //Brancos ==> 394 - 394
                _detalhe += " "; //brancos X(01)

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

        public string GerarMensagemVariavelRemessaCNAB400(Boleto boleto, ref int numeroRegistro, TipoArquivo tipoArquivo)
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
                //Código do Banco na compensação ==> 001 - 003
                string trailer = Utils.FormatCode(Codigo.ToString(), "0", 3, true);

                //Numero do lote remessa ==> 004 - 007
                trailer += Utils.FormatCode("1", "0", 4, true);

                //Tipo de registro ==> 008 - 008
                trailer += "5";

                //Reservado (uso Banco) ==> 009 - 017
                trailer += Utils.FormatCode("", " ", 9);

                //Quantidade de registros do lote ==> 018 - 023
                trailer += Utils.FormatCode(numeroRegistro.ToString(), "0", 6, true);

                //Reservado (uso Banco) ==> 024 - 240
                trailer += Utils.FormatCode("", " ", 217);

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
                //Código do Banco na compensação ==> 001 - 003
                string trailer = Utils.FormatCode(Codigo.ToString(), "0", 3, true);

                //Numero do lote remessa ==> 004 - 007
                trailer += "9999";

                //Tipo de registro ==> 008 - 008
                trailer += "9";

                //Reservado (uso Banco) ==> 009 - 017
                trailer += Utils.FormatCode("", " ", 9);

                //Quantidade de lotes do arquivo ==> 018 - 023
                trailer += Utils.FormatCode("1", "0", 6, true);

                //Quantidade de registros do arquivo ==> 024 - 029
                trailer += Utils.FormatCode(numeroRegistro.ToString(), "0", 6, true);

                //Reservado (uso Banco) ==> 030 - 240
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

                //Código do registro ==> 001 - 001
                _trailer = "9";
                //Quantidade de documentos no arquivo ==> 002 - 007
                _trailer += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);

                //Valor total dos títulos ==> 008 - 020
                _trailer += Utils.FitStringLength(vltitulostotal.ApenasNumeros(), 13, 13, '0', 0, true, true, true);

                //Zeros ==> 021 - 394
                _trailer += complemento;

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
                //detalhe.DACNossoNumero = registro.Substring(69, 1);
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
                detalhe.CodigoCarteira = Convert.ToInt32(registro.Substring(53, 1));
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

        public override DetalheSegmentoURetornoCNAB240 LerDetalheSegmentoURetornoCNAB240(string registro)
        {
            try
            {
                DetalheSegmentoURetornoCNAB240 detalhe = new DetalheSegmentoURetornoCNAB240(registro);

                if (registro.Substring(13, 1) != "U")
                    throw new Exception("Registro inválido. O detalhe não possuí as características do segmento U.");

                detalhe.CodigoOcorrenciaSacado = registro.Substring(15, 2);
                int DataCredito = Convert.ToInt32(registro.Substring(145, 8));
                detalhe.DataCredito = Convert.ToDateTime(DataCredito.ToString("##-##-####"));
                int DataOcorrencia = Convert.ToInt32(registro.Substring(137, 8));
                detalhe.DataOcorrencia = Convert.ToDateTime(DataOcorrencia.ToString("##-##-####"));
                int DataOcorrenciaSacado = Convert.ToInt32(registro.Substring(157, 8));
                if (DataOcorrenciaSacado > 0)
                    detalhe.DataOcorrenciaSacado = Convert.ToDateTime(DataOcorrenciaSacado.ToString("##-##-####"));
                else
                    detalhe.DataOcorrenciaSacado = DateTime.Now;

                decimal JurosMultaEncargos = Convert.ToUInt64(registro.Substring(17, 15));
                detalhe.JurosMultaEncargos = JurosMultaEncargos / 100;
                decimal ValorDescontoConcedido = Convert.ToUInt64(registro.Substring(32, 15));
                detalhe.ValorDescontoConcedido = ValorDescontoConcedido / 100;
                decimal ValorAbatimentoConcedido = Convert.ToUInt64(registro.Substring(47, 15));
                detalhe.ValorAbatimentoConcedido = ValorAbatimentoConcedido / 100;
                decimal ValorIOFRecolhido = Convert.ToUInt64(registro.Substring(62, 15));
                detalhe.ValorIOFRecolhido = ValorIOFRecolhido / 100;
                decimal ValorPagoPeloSacado = Convert.ToUInt64(registro.Substring(77, 15));
                detalhe.ValorPagoPeloSacado = ValorPagoPeloSacado / 100;
                decimal ValorLiquidoASerCreditado = Convert.ToUInt64(registro.Substring(92, 15));
                detalhe.ValorLiquidoASerCreditado = ValorLiquidoASerCreditado / 100;
                decimal ValorOutrasDespesas = Convert.ToUInt64(registro.Substring(107, 15));
                detalhe.ValorOutrasDespesas = ValorOutrasDespesas / 100;

                decimal ValorOutrosCreditos = Convert.ToUInt64(registro.Substring(122, 15));
                detalhe.ValorOutrosCreditos = ValorOutrosCreditos / 100;

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO U.", ex);
            }


        }

        public override DetalheSegmentoYRetornoCNAB240 LerDetalheSegmentoYRetornoCNAB240(string registro)
        {
            try
            {
                DetalheSegmentoYRetornoCNAB240 detalhe = new DetalheSegmentoYRetornoCNAB240(registro);

                if (registro.Substring(13, 1) != "Y")
                    throw new Exception("Registro inválido. O detalhe não possuí as características do segmento Y.");

                detalhe.CodigoMovimento = Convert.ToInt32(registro.Substring(15, 2));
                detalhe.IdentificacaoRegistro = Convert.ToInt32(registro.Substring(17, 4));
                detalhe.IdentificacaoCheque1 = registro.Substring(19, 34);
                detalhe.IdentificacaoCheque2 = registro.Substring(43, 34);
                detalhe.IdentificacaoCheque3 = registro.Substring(87, 34);
                detalhe.IdentificacaoCheque4 = registro.Substring(121, 34);
                detalhe.IdentificacaoCheque5 = registro.Substring(155, 34);
                detalhe.IdentificacaoCheque6 = registro.Substring(189, 34);

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO Y.", ex);
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

        public override long ObterNossoNumeroSemConvenioOuDigitoVerificador(long convenio, string nossoNumero)
        {
            if (string.IsNullOrEmpty(nossoNumero) || nossoNumero.Length != 13)
                throw new TamanhoNossoNumeroInvalidoException();

            var nossoNumeroSemDV = nossoNumero.Substring(0, 12);

            long numero;
            if (long.TryParse(nossoNumeroSemDV, out numero))
                return numero;
            throw new NossoNumeroInvalidoException();
        }
    }
}