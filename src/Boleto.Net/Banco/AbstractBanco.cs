using System;
using System.Threading;

namespace BoletoNet
{
    public abstract class AbstractBanco
    {

        #region Variaveis

        private int _codigo = 0;
        private string _digito = "0";
        private string _nome = string.Empty;
        private Cedente _cedente = null;

        #endregion Variaveis

        #region Propriedades

        /// <summary>
        /// C�digo do Banco
        /// 237 - Bradesco; 341 - Ita�
        /// </summary>
        public virtual int Codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }

        /// <summary>
        /// D�gito do Banco
        /// </summary>
        public virtual string Digito
        {
            get { return _digito; }
            protected set { _digito = value; }
        }

        /// <summary>
        /// Nome da Institui��o Financeira
        /// </summary>
        public virtual string Nome
        {
            get { return _nome; }
            protected set { _nome = value; }
        }

        /// <summary>
        /// Cedente
        /// </summary>
        public virtual Cedente Cedente
        {
            get { return _cedente; }
            protected set { _cedente = value; }
        }

        /// <summary>
        /// Permitir a comunica��o entre bancos comerciais estaduais agindo como chave comum na troca de informa��es entre eles
        /// </summary>
        public string ChaveASBACE { get; set; }
        #endregion Propriedades

        # region M�todos

        /// <summary>
        /// Retorna o campo que compos o c�digo de barras que para todos os bancos s�o iguais foramado por:
        /// </summary>
        /// <returns></returns>
        public virtual string CampoFixo()
        {
            throw new NotImplementedException("Fun��o n�o implementada");
        }
        
        public virtual bool ValidarRemessa(TipoArquivo tipoArquivo, string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            throw new NotImplementedException("Fun��o n�o implementada na classe filha. Implemente na classe que est� sendo criada.");
        }
        
        /// <summary>
        /// Gera os registros de header do aquivo de remessa
        /// </summary>
        public virtual string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            string _header = "";
            return _header;
        }
        public virtual string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            string _header = "";
            return _header;
        }
        /// <summary>
        /// Gera registros de detalhe do arquivo remessa
        /// </summary>
        public virtual string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            string _remessa = "";
            return _remessa;
        }
        /// <summary>
        /// Gera os registros de Trailer do arquivo de remessa
        /// </summary>
        public virtual string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal)
        {
            string _trailer = "";
            return _trailer;
        }

        /// <summary>
        /// Gera os registros de Trailer do arquivo de remessa com total de registros detalhe
        /// </summary>
        public virtual string GerarTrailerRemessaComDetalhes(int numeroRegistro, int numeroRegistroDetalhes, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal)
        {
            string _trailer = "";
            return _trailer;
        }



        /// <summary>
        /// Gera os registros de header de aquivo do arquivo de remessa
        /// </summary>
        public virtual string GerarHeaderRemessa(Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            string _headerArquivo = "";
            return _headerArquivo;
        }
        /// <summary>
        /// Gera os registros de header de lote do arquivo de remessa
        /// </summary>
        public virtual string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            string _headerLote = "";
            return _headerLote;
        }
        /// <summary>
        /// Gera os registros de header de lote do arquivo de remessa
        /// </summary>
        public virtual string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa, TipoArquivo tipoArquivo)
        {
            string _headerLote = "";
            return _headerLote;
        }
        /// <summary>
        /// Gera os registros de header de lote do arquivo de remessa
        /// </summary>
        public virtual string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa, TipoArquivo tipoArquivo, Boleto boletos)
        {
            string _headerLote = "";
            return _headerLote;
        }

        /// <summary>
        /// Gera registros de detalhe do arquivo remessa - SEGMENTO A
        /// </summary>
        public virtual string GerarDetalheSegmentoARemessa(Boleto boleto, int numeroRegistro)
        {
            string _segmentoP = "";
            return _segmentoP;
        }
        /// <summary>
        /// Gera registros de detalhe do arquivo remessa - SEGMENTO B
        /// </summary>
        public virtual string GerarDetalheSegmentoBRemessa(Boleto boleto, int numeroRegistro)
        {
            string _segmentoP = "";
            return _segmentoP;
        }

        /// <summary>
        /// Gera registros de detalhe do arquivo remessa - SEGMENTO P
        /// </summary>
        public virtual string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio, Cedente cedente)
        {
            string _segmentoP = "";
            return _segmentoP;
        }
        public virtual string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio, Cedente cedente, Boleto boletos)
        {
            string _segmentoP = "";
            return _segmentoP;
        }
        /// <summary>
        /// Gera registros de detalhe do arquivo remessa - SEGMENTO P
        /// </summary>
        public virtual string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio)
        {
            string _segmentoP = "";
            return _segmentoP;
        }
        /// <summary>
        /// Gera registros de detalhe do arquivo remessa - SEGMENTO Q
        /// </summary>
        public virtual string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente)
        {
            string _segmentoQ = "";
            return _segmentoQ;
        }
        /// <summary>
        /// Gera registros de detalhe do arquivo remessa - SEGMENTO Q
        /// </summary>
        public virtual string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, Sacado sacado)
        {
            string _segmentoQ = "";
            return _segmentoQ;
        }
        /// <summary>
        /// Gera registros de detalhe do arquivo remessa - SEGMENTO Q
        /// </summary>
        public virtual string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            string _segmentoQ = "";
            return _segmentoQ;
        }
        /// <summary>
        /// Gera registros de detalhe do arquivo remessa - SEGMENTO R
        /// </summary>
        public virtual string GerarDetalheSegmentoRRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            string _segmentoR = "";
            return _segmentoR;
        }
        /// <summary>
        /// Gera os registros de Trailer de arquivo do arquivo de remessa
        /// </summary>
        public virtual string GerarTrailerArquivoRemessa(int numeroRegistro)
        {
            string _trailerArquivo = "";
            return _trailerArquivo;
        }
        public virtual string GerarTrailerArquivoRemessa(int numeroRegistro, Boleto boletos)
        {
            string _trailerArquivo = "";
            return _trailerArquivo;
        }
        /// <summary>
        /// Gera os registros de Trailer de lote do arquivo de remessa
        /// </summary>
        public virtual string GerarTrailerLoteRemessa(int numeroRegistro)
        {
            string _trailerLote = "";
            return _trailerLote;
        }
        /// <summary>
        /// Gera os registros de Trailer de lote do arquivo de remessa
        /// </summary>
        public virtual string GerarTrailerLoteRemessa(int numeroRegistro, Boleto boletos)
        {
            string _trailerLote = "";
            return _trailerLote;
        }
        /// <summary>
        /// Formata c�digo de barras
        /// </summary>      
        public virtual void FormataCodigoBarra(Boleto boleto)
        {
            throw new NotImplementedException("Fun��o n�o implementada na classe filha. Implemente na classe que est� sendo criada.");
        }
        /// <summary>
        /// Formata linha digit�vel
        /// </summary>
        public virtual void FormataLinhaDigitavel(Boleto boleto)
        {
            throw new NotImplementedException("Fun��o n�o implementada na classe filha. Implemente na classe que est� sendo criada.");
        }
        /// <summary>
        /// Formata nosso n�mero
        /// </summary>
        public virtual void FormataNossoNumero(Boleto boleto)
        {
            throw new NotImplementedException("Fun��o n�o implementada na classe filha. Implemente na classe que est� sendo criada.");
        }
        /// <summary>
        /// Formata n�mero do documento
        /// </summary>
        public virtual void FormataNumeroDocumento(Boleto boleto)
        {
            throw new NotImplementedException("Fun��o n�o implementada na classe filha. Implemente na classe que est� sendo criada.");
        }
        /// <summary>
        /// Valida o boleto
        /// </summary>
        public virtual void ValidaBoleto(Boleto boleto)
        {
            throw new NotImplementedException("Fun��o n�o implementada na classe filha. Implemente na classe que est� sendo criada.");
        }

        public virtual DetalheSegmentoWRetornoCNAB240 LerDetalheSegmentoWRetornoCNAB240(string registro)
        {
            var detalhe = new DetalheSegmentoWRetornoCNAB240();

            detalhe.LerDetalheSegmentoWRetornoCNAB240(registro);

            return detalhe;
        }

        public virtual DetalheSegmentoTRetornoCNAB240 LerDetalheSegmentoTRetornoCNAB240(string registro)
        {
            var detalhe = new DetalheSegmentoTRetornoCNAB240(registro);

            detalhe.LerDetalheSegmentoTRetornoCNAB240(registro);

            return detalhe;
        }

        public virtual DetalheSegmentoURetornoCNAB240 LerDetalheSegmentoURetornoCNAB240(string registro)
        {
            var detalhe = new DetalheSegmentoURetornoCNAB240(registro);

            detalhe.LerDetalheSegmentoURetornoCNAB240(registro);

            return detalhe;
        }

        public virtual DetalheRetorno LerDetalheRetornoCNAB400(string registro)
        {
            try
            {
                int dataOcorrencia = Utils.ToInt32(registro.Substring(110, 6));
                int dataVencimento = Utils.ToInt32(registro.Substring(146, 6));
                int dataCredito = Utils.ToInt32(registro.Substring(295, 6));

                DetalheRetorno detalhe = new DetalheRetorno(registro);

                detalhe.CodigoInscricao = Utils.ToInt32(registro.Substring(1, 2));
                detalhe.NumeroInscricao = registro.Substring(3, 14);
                detalhe.Agencia = Utils.ToInt32(registro.Substring(17, 4));
                detalhe.Conta = Utils.ToInt32(registro.Substring(23, 5));
                detalhe.DACConta = Utils.ToInt32(registro.Substring(28, 1));
                detalhe.UsoEmpresa = registro.Substring(37, 25);
                //
                detalhe.NossoNumeroComDV = registro.Substring(85, 9);
                detalhe.NossoNumero = registro.Substring(85, 8); //Sem o DV
                detalhe.DACNossoNumero = registro.Substring(93, 1); //DV
                //
                detalhe.Carteira = registro.Substring(107, 1);
                detalhe.CodigoOcorrencia = Utils.ToInt32(registro.Substring(108, 2));
                detalhe.DataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));
                detalhe.NumeroDocumento = registro.Substring(116, 10);
                detalhe.NossoNumero = registro.Substring(126, 9);
                detalhe.DataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));
                decimal valorTitulo = Convert.ToInt64(registro.Substring(152, 13));
                detalhe.ValorTitulo = valorTitulo / 100;
                detalhe.CodigoBanco = Utils.ToInt32(registro.Substring(165, 3));
                detalhe.AgenciaCobradora = Utils.ToInt32(registro.Substring(168, 4));
                detalhe.Especie = Utils.ToInt32(registro.Substring(173, 2));
                decimal tarifaCobranca = Convert.ToUInt64(registro.Substring(175, 13));
                detalhe.TarifaCobranca = tarifaCobranca / 100;
                // 26 brancos
                decimal iof = Convert.ToUInt64(registro.Substring(214, 13));
                detalhe.IOF = iof / 100;
                decimal valorAbatimento = Convert.ToUInt64(registro.Substring(227, 13));
                detalhe.ValorAbatimento = valorAbatimento / 100;
                decimal valorPrincipal = Convert.ToUInt64(registro.Substring(253, 13));
                detalhe.ValorPrincipal = valorPrincipal / 100;
                decimal jurosMora = Convert.ToUInt64(registro.Substring(266, 13));
                detalhe.JurosMora = jurosMora / 100;
                detalhe.DataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));
                // 293 - 3 brancos
                detalhe.DataCredito = Utils.ToDateTime(dataCredito.ToString("##-##-##"));
                detalhe.InstrucaoCancelada = Utils.ToInt32(registro.Substring(301, 4));
                // 306 - 6 brancos
                // 311 - 13 zeros
                detalhe.NomeSacado = registro.Substring(324, 30);
                // 354 - 23 brancos
                detalhe.Erros = registro.Substring(377, 8);
                // 377 - Registros rejeitados ou alega��o do sacado
                // 386 - 7 brancos

                detalhe.CodigoLiquidacao = registro.Substring(392, 2);
                detalhe.NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6));

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }
        # endregion

        /// <summary>
        /// Fator de vencimento do boleto.
        /// </summary>
        /// <param name="boleto"></param>
        /// <returns>Retorno Fator de Vencimento</returns>
        /// <remarks>
        ///     Wellington(wcarvalho@novatela.com.br) 
        ///     Com base na proposta feita pela CENEGESC de acordo com o comunicado FEBRABAN de n� 082/2012 de 14/06/2012 segue regra para implanta��o.
        ///     No dia 21/02/20025 o fator vencimento chegar� em 9999 assim atigindo o tempo de utiliza��o, para contornar esse problema foi definido com uma nova regra
        ///     de utiliza�ao criando um range de uso o range funcionara controlando a emiss�o dos boletos.
        ///     Exemplo:
        ///         Data Atual: 12/03/2014 = 6000
        ///         Para os boletos vencidos, anterior a data atual � de 3000 fatores cerca de =/- 8 anos. Os boletos que forem gerados acima dos 3000 n�o ser�o aceitos pelas institui��es financeiras.
        ///         Para os boletos a vencer, posterior a data atual � de 5500 fatores cerca de +/- 15 anos. Os boletos que forem gerados acima dos 5500 n�o ser�o aceitos pelas institui��es financeiras.
        ///     Quando o fator de vencimento atingir 9999 ele retorna para 1000
        ///     Exemplo:
        ///         21/02/2025 = 9999
        ///         22/02/2025 = 1000
        ///         23/02/2025 = 1001
        ///         ...
        ///         05/03/2025 = 1011
        /// </remarks>
        public static long FatorVencimento(Boleto boleto)
        {
            var dateBase = new DateTime(1997, 10, 7, 0, 0, 0);

            //Verifica se a data esta dentro do range utilizavel
            var dataAtual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            long rangeUtilizavel = Utils.DateDiff(DateInterval.Day, dataAtual, boleto.DataVencimento);

            if (rangeUtilizavel > 5500 || rangeUtilizavel < -3000)
                throw new Exception("Data do vencimento fora do range de utiliza��o proposto pela CENEGESC. Comunicado FEBRABAN de n� 082/2012 de 14/06/2012");

            while (boleto.DataVencimento > dateBase.AddDays(9999))
                dateBase = boleto.DataVencimento.AddDays(-(((Utils.DateDiff(DateInterval.Day, dateBase, boleto.DataVencimento) - 9999) - 1) + 1000));

            return Utils.DateDiff(DateInterval.Day, dateBase, boleto.DataVencimento);
        }

        #region Mod
        internal static int Mod10(string seq)
        {
            /* Vari�veis
             * -------------
             * d - D�gito
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */

            int d, s = 0, p = 2, r;

            for (int i = seq.Length; i > 0; i--)
            {
                r = (Convert.ToInt32(Microsoft.VisualBasic.Strings.Mid(seq, i, 1)) * p);

                if (r > 9)
                    r = (r / 10) + (r % 10);

                s += r;

                if (p == 2)
                    p = 1;
                else
                    p = p + 1;
            }
            d = ((10 - (s % 10)) % 10);
            return d;
        }

        protected static int Mod11(string seq)
        {
            /* Vari�veis
             * -------------
             * d - D�gito
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */

            int d, s = 0, p = 2, b = 9;

            for (int i = 0; i < seq.Length; i++)
            {
                s = s + (Convert.ToInt32(seq[i]) * p);
                if (p < b)
                    p = p + 1;
                else
                    p = 2;
            }

            d = 11 - (s % 11);
            if (d > 9)
                d = 0;
            return d;
        }

        public static int Mod11Peso2a9(string seq)
        {
            /* Vari�veis
             * -------------
             * d - D�gito
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */

            int d, r, s = 0, p = 2, b = 9;
            string n;

            for (int i = seq.Length; i > 0; i--)
            {
                n = Microsoft.VisualBasic.Strings.Mid(seq, i, 1);

                s = s + (Convert.ToInt32(n) * p);

                if (p < b)
                    p = p + 1;
                else
                    p = 2;
            }

            r = ((s * 10) % 11);

            if (r == 0 || r == 1 || r == 10)
                d = 1;
            else
                d = r;

            return d;

        }

        protected static int Mod11(string seq, int b)
        {
            /* Vari�veis
             * -------------
             * d - D�gito
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

            d = 11 - (s % 11);


            if ((d > 9) || (d == 0) || (d == 1))
                d = 1;

            return d;
        }

        protected static int Mod11Base9(string seq)
        {
            /* Vari�veis
             * -------------
             * d - D�gito
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */

            int d, s = 0, p = 2, b = 9;


            for (int i = seq.Length - 1; i >= 0; i--)
            {
                string aux = Convert.ToString(seq[i]);
                s += (Convert.ToInt32(aux) * p);
                if (p >= b)
                    p = 2;
                else
                    p = p + 1;
            }

            if (s < 11)
            {
                d = 11 - s;
                return d;
            }
            else
            {
                d = 11 - (s % 11);
                if ((d > 9) || (d == 0))
                    d = 0;

                return d;
            }
        }

        public static int Mod11(string seq, int lim, int flag)
        {
            int mult = 0;
            int total = 0;
            int pos = 1;
            //int res = 0;
            int ndig = 0;
            int nresto = 0;
            string num = string.Empty;

            mult = 1 + (seq.Length % (lim - 1));

            if (mult == 1)
                mult = lim;


            while (pos <= seq.Length)
            {
                num = Microsoft.VisualBasic.Strings.Mid(seq, pos, 1);
                total += Convert.ToInt32(num) * mult;

                mult -= 1;
                if (mult == 1)
                    mult = lim;

                pos += 1;
            }
            nresto = (total % 11);
            if (flag == 1)
                return nresto;
            else
            {
                if (nresto == 0 || nresto == 1 || nresto == 10)
                    ndig = 1;
                else
                    ndig = (11 - nresto);

                return ndig;
            }
        }

        protected static int Mult10Mod11(string seq, int lim, int flag)
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
                num = Microsoft.VisualBasic.Strings.Mid(seq, pos, 1);
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
                if (nresto == 0 || nresto == 1 || nresto == 10)
                    ndig = 1;
                else
                    ndig = nresto;

                return ndig;
            }
        }

        /// <summary>
        /// Encontra multiplo de 10 igual ou superior a soma e subtrai multiplo da soma devolvendo o resultado
        /// </summary>
        /// <param name="soma"></param>
        /// <returns></returns>
        protected static int Multiplo10(int soma)
        {
            //Variaveis
            int result = 0;
            //Encontrando multiplo de 10
            while (result < soma)
            {
                result = result + 10;
            }
            //Subtraindo
            result = result - soma;
            //Retornando
            return result;
        }
        #endregion Mod
    }
}
