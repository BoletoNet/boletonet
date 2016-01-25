using System;
using System.Web.UI;
using BoletoNet;
using Microsoft.VisualBasic;

[assembly: WebResource("BoletoNet.Imagens.422.jpg", "image/jpg")]

namespace BoletoNet
{
    /// <author>  
    /// Eduardo Frare
    /// Stiven 
    /// Diogo
    /// Miamoto
    /// </author>    

    ///<summary>
    /// Classe referente ao Banco Banco_Safra
    ///</summary>
    internal class Banco_Safra : AbstractBanco, IBanco
    {
        private string _dacNossoNumero = string.Empty;
        private int _dacContaCorrente = 0;
        private int _dacBoleto = 0;

        /// <summary>
        /// Classe responsavel em criar os campos do Banco Banco_Safra.
        /// </summary>
        internal Banco_Safra()
        {
            this.Codigo = 422;
            this.Digito = "7";
            this.Nome = "Banco_Safra";
        }

        /// <summary>
        /// Calcula o digito do Nosso Numero
        /// </summary>
        public string CalcularDigitoNossoNumero(Boleto boleto)
        {
            string sfCarteira = boleto.Carteira.ToString();


            if (boleto.NossoNumero.Length < 9)
            {
                throw new IndexOutOfRangeException("Erro. O campo 'Nosso N�mero' deve ter mais de 9 digitos. Voc� digitou " + boleto.NossoNumero);
            }
            string sfNossoNumero = boleto.NossoNumero.Substring(0, 8);
            int sfDigitoNossoNumero = Mod11(sfNossoNumero, 9, 0);
            string sfDigito = "";

            if (sfDigitoNossoNumero == 0)
                sfDigito = "1";
            else if (sfDigitoNossoNumero > 1)
                sfDigito = Convert.ToString(11 - sfDigitoNossoNumero);

            return sfDigito;

        }


        /// <summary>       
        /// SISTEMA	        020	020	7	FIXO
        /// CLIENTE	        021	034	C�DIGO DO CLIENTE	C�DIGO/AG�NCIA CEDENTE
        /// N/N�MERO	    035	043	NOSSO N�MERO	NOSSO N�MERO DO T�TULO
        /// TIPO COBRAN�A	044	044	2	FIXO
        /// </summary>
        public string CampoLivre(Boleto boleto)
        {

            string campolivre = "7" + boleto.Cedente.ContaBancaria.Conta.ToString() + boleto.Cedente.ContaBancaria.Agencia.ToString() +
                                boleto.NossoNumero.Substring(0, 9) + "2";
            return campolivre;
        }

        #region IBanco Members
        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            throw new NotImplementedException("Fun��o n�o implementada.");
        }

        public override void ValidaBoleto(Boleto boleto)
        {

            // Calcula o DAC do Nosso N�mero
            _dacNossoNumero = CalcularDigitoNossoNumero(boleto);

            // Calcula o DAC da Conta Corrente
            _dacContaCorrente = Mod10(boleto.Cedente.ContaBancaria.Agencia + boleto.Cedente.ContaBancaria.Conta);

            //Verifica se o nosso n�mero � v�lido
            if (Utils.ToInt64(boleto.NossoNumero) == 0)
                throw new NotImplementedException("Nosso n�mero inv�lido");

            //Verifica se data do processamento � valida
			//if (boleto.DataProcessamento.ToString("dd/MM/yyyy") == "01/01/0001")
			if (boleto.DataProcessamento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataProcessamento = DateTime.Now;

            //Verifica se data do documento � valida
			//if (boleto.DataDocumento.ToString("dd/MM/yyyy") == "01/01/0001")
			if (boleto.DataDocumento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataDocumento = DateTime.Now;

            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);
            FormataNossoNumero(boleto);
        }

        public override void FormataNumeroDocumento(Boleto boleto)
        {
            throw new NotImplementedException("Fun��o n�o implementada.");
        }

        public override void FormataNossoNumero(Boleto boleto)
        {
            //throw new NotImplementedException("Fun��o n�o implementada.");
        }

        /// <summary>
        ///	O c�digo de barra para cobran�a cont�m 44 posi��es dispostas da seguinte forma:
        ///    01 a 03 - 3 - Identifica��o  do  Banco
        ///    04 a 04 - 1 - C�digo da Moeda
        ///    05 a 05 � 1 - D�gito verificador do C�digo de Barras
        ///    06 a 19 - 14 - Valor
        ///    20 a 44 � 25 - Campo Livre
        /// </summary>
        public override void FormataCodigoBarra(Boleto boleto)
        {
            string valorBoleto = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            valorBoleto = Utils.FormatCode(valorBoleto, 14);

            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}", Codigo, boleto.Moeda,
                    FatorVencimento(boleto), valorBoleto, CampoLivre(boleto));

            _dacBoleto = 0;
            //Mod11(Boleto.CodigoBarra.Codigo.Substring(0, 3) + Boleto.CodigoBarra.Codigo.Substring(5, 43), 9, 0);

            boleto.CodigoBarra.Codigo = Strings.Left(boleto.CodigoBarra.Codigo, 4) + _dacBoleto + Strings.Right(boleto.CodigoBarra.Codigo, 39);
        }

        /// <summary>
        /// A linha digit�vel ser� composta por cinco campos:
        ///    1� CAMPO - Composto pelo c�digo do banco ( sem o d�gito verificador = 422 ), 
        ///       c�digo da moeda, as cinco primeiras posi��es do campo livre ou seja, da 
        ///       posi��o 20 � 24 do c�digo de barras, e mais um d�gito verificador deste campo. 
        ///       Ap�s os 5 primeiros d�gitos deste campo separar o conte�do por um ponto ( . ). 
        ///    2� CAMPO - Composto pelas posi��es 6 � 15 do campo livre ou seja, da 
        ///       posi��o 25 � 34 do c�digo de barras e mais um d�gito verificador deste campo. 
        ///       Ap�s os 5 primeiros d�gitos deste campo separar o conte�do por um ponto ( . ).
        ///    3� CAMPO - Composto pelas posi��es 16 � 25 do campo livre ou seja, da 
        ///       posi��o 35 � 44 do c�digo de barras, e mais um d�gito verificador deste campo. 
        ///       Ap�s os 5 primeiros d�gitos deste campo separar o conte�do por um ponto ( . ).
        ///    4� CAMPO  - Composto pelo d�gito de autoconfer�ncia do c�digo de barras.
        ///    5� CAMPO - Composto pelo valor nominal do documento ou seja, pelas 
        ///       posi��es 06 � 19 do c�digo de barras, com supress�o de zeros a esquerda e 
        ///       sem edi��o ( sem ponto e v�rgula ). Quando se tratar de valor zerado, a 
        ///       representa��o dever� ser 000 ( tr�s zeros ).
        /// </summary>
        public override void FormataLinhaDigitavel(Boleto boleto)
        {

            //AAABC.CCCCX DDDDD.DDDDDY EEEEE.EEEEEZ K VVVVVVVVVVVVVV

            string LD = string.Empty; //Linha Digit�vel

            #region Campo 1

            //Campo 1
            string AAA = Utils.FormatCode(Codigo.ToString(), 3);
            string B = boleto.Moeda.ToString();
            string CCCCC = CampoLivre(boleto).Substring(0, 4);
            string X = Mod10(AAA + B + CCCCC).ToString();

            LD = string.Format("{0}{1}{2}.", AAA, B, CCCCC.Substring(0, 1));
            LD += string.Format("{0}{1}", CCCCC.Substring(0, 4), X);

            #endregion Campo 1

            #region Campo 2

            string DDDDDD = CampoLivre(boleto).Substring(6, 15);
            string Y = Mod10(DDDDDD).ToString();

            LD += string.Format("{0}.", DDDDDD.Substring(0, 5));
            LD += string.Format("{0}{1} ", DDDDDD.Substring(5, 10), Y);

            #endregion Campo 2

            #region Campo 3

            string EEEEE = CampoLivre(boleto).Substring(12, 10);
            string Z = Mod10(EEEEE).ToString();

            LD += string.Format("{0}.", EEEEE.Substring(0, 5));
            LD += string.Format("{0}{1} ", EEEEE.Substring(5, 5), Z);

            #endregion Campo 3

            #region Campo 4

            string K = _dacBoleto.ToString();

            LD += string.Format(" {0} ", K);

            #endregion Campo 4

            #region Campo 5
            string VVVVVVVVVVVVVV;
            if (boleto.ValorBoleto != 0)
            {
                VVVVVVVVVVVVVV = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
                VVVVVVVVVVVVVV = Utils.FormatCode(VVVVVVVVVVVVVV, 14);
            }
            else
                VVVVVVVVVVVVVV = "000";

            LD += VVVVVVVVVVVVVV;

            #endregion Campo 5

            boleto.CodigoBarra.LinhaDigitavel = LD;

        }
        #endregion IBanco Members


        /// <summary>
        /// Efetua as Valida��es dentro da classe Boleto, para garantir a gera��o da remessa
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
