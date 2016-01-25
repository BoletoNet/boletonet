using System;
using System.Collections.Generic;
using System.Web.UI;
using BoletoNet;
using Microsoft.VisualBasic;

[assembly: WebResource("BoletoNet.Imagens.409.jpg", "image/jpg")]

namespace BoletoNet
{ 
    /// <author>  
    /// Marlon Oliveira (marlonoliveira@nextconsultoria.com.br)
    /// </author>    
    internal class Banco_Unibanco : AbstractBanco, IBanco
    {
        private string _dacNossoNumero = string.Empty;
        private int _dacBoleto = 0;

        /// <summary>
        /// Classe responsavel em criar os campos do banco Unibanco.
        /// </summary>
        internal Banco_Unibanco()
        {
            this.Codigo = 409;
            this.Digito = "2";
            this.Nome = "Unibanco";
        }
    
        #region IBanco Members

        /// <summary>
        /// A linha digit�vel ser� composta por cinco campos:
        ///      1� campo
        ///          composto pelo c�digo de Banco, c�digo da moeda, as cinco primeiras posi��es do campo 
        ///          livre e o d�gito verificador deste campo;
        ///      2� campo
        ///          composto pelas posi��es 6� a 15� do campo livre e o d�gito verificador deste campo;
        ///      3� campo
        ///          composto pelas posi��es 16� a 25� do campo livre e o d�gito verificador deste campo;
        ///      4� campo
        ///          composto pelo d�gito verificador do c�digo de barras, ou seja, a 5� posi��o do c�digo de 
        ///          barras;
        ///      5� campo
        ///          Composto pelo fator de vencimento com 4(quatro) caracteres e o valor do documento com 10(dez) caracteres, sem separadores e sem edi��o.
        /// 
        /// </summary>
        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            //BBBMC.CCCCD1 CCCCC.CCCCCD2 CCCCC.CCCCCD3 D4 FFFFVVVVVVVVVV

            #region Campo 1

                string Grupo1 = string.Empty;

                string BBB = boleto.CodigoBarra.Codigo.Substring(0, 3);
                string M = boleto.CodigoBarra.Codigo.Substring(3, 1);
                string CCCCC = boleto.CodigoBarra.Codigo.Substring(19, 5);
                string D1 = Banco_Unibanco.Mod10(BBB + M + CCCCC).ToString() ;

                Grupo1 = string.Format("{0}{1}{2}.{3}{4} ", BBB, M, CCCCC.Substring(0, 1), CCCCC.Substring(1, 4), D1);


            #endregion Campo 1

            #region Campo 2

                string Grupo2 = string.Empty;

                string CCCCCCCCCC2 = boleto.CodigoBarra.Codigo.Substring(24, 10);
                string D2 = Banco_Unibanco.Mod10(CCCCCCCCCC2).ToString();

                Grupo2 = string.Format("{0}.{1}{2} ", CCCCCCCCCC2.Substring(0, 5), CCCCCCCCCC2.Substring(5, 5), D2);

            #endregion Campo 2

            #region Campo 3

                string Grupo3 = string.Empty;

                string CCCCCCCCCC3 = boleto.CodigoBarra.Codigo.Substring(34, 10);
                string D3 = Banco_Unibanco.Mod10(CCCCCCCCCC3).ToString();

                Grupo3 = string.Format("{0}.{1}{2} ", CCCCCCCCCC3.Substring(0, 5), CCCCCCCCCC3.Substring(5, 5), D3);


            #endregion Campo 3

            #region Campo 4

                string Grupo4 = string.Empty;

                string D4 = _dacBoleto.ToString();

                Grupo4 = string.Format("{0} ", D4);

            #endregion Campo 4

            #region Campo 5

                string Grupo5 = string.Empty;

                long FFFF = FatorVencimento(boleto) ;

                string VVVVVVVVVV = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
                VVVVVVVVVV = Utils.FormatCode(VVVVVVVVVV, 10);

                if (Utils.ToInt64(VVVVVVVVVV) == 0)
                    VVVVVVVVVV = "000";

                Grupo5 = string.Format("{0}{1}", FFFF, VVVVVVVVVV);

            #endregion Campo 5

            boleto.CodigoBarra.LinhaDigitavel = Grupo1 + Grupo2 + Grupo3 + Grupo4 + Grupo5;

        }

        /// <summary>
        /// 
        ///   *******
        /// 
        ///	O c�digo de barra para cobran�a cont�m 44 posi��es dispostas da seguinte forma:
        ///    01 a 03 -  3 - 409 fixo - C�digo do banco
        ///    04 a 04 -  1 - 9 fixo - C�digo da moeda (R$)
        ///    05 a 05 �  1 - D�gito verificador do c�digo de barras
        ///    06 a 09 -  4 - Fator de vencimento
        ///    10 a 19 - 10 - Valor
        ///    20 a 44 � 25 - Campo livre
        /// 
        ///   *******
        /// 
        /// </summary>
        public override void FormataCodigoBarra(Boleto boleto)
        {
            string valorBoleto = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            valorBoleto = Utils.FormatCode(valorBoleto, 10);

            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}", Codigo.ToString(), boleto.Moeda,
                    FatorVencimento(boleto), valorBoleto, FormataCampoLivre(boleto));

            _dacBoleto = Banco_Unibanco.Mod11(boleto.CodigoBarra.Codigo);

            boleto.CodigoBarra.Codigo = Strings.Left(boleto.CodigoBarra.Codigo, 4) + _dacBoleto + Strings.Right(boleto.CodigoBarra.Codigo, 39);
        }

        ///<summary>
        /// Campo Livre
        ///    20 a 20 -  1 - 5 fixo - tipo de cobran�a (CVT cobran�a sem registro - 7744-5)
        ///    21 a 26 -  6 - C�digo do cedente
        ///    27 a 27 -  1 - D�gito verificador do c�digo do cedente
        ///    28 a 29 -  2 - 00 fixo - vago
        ///    30 a 43 - 14 - Nosso n�mero
        ///    44 a 44	- 1 - D�gito do nosso n�mero
        ///
        /// PS: C�lculo do d�gito do c�digo de barras: usar rotina de m�dulo 11
        ///</summary>
        public string FormataCampoLivre(Boleto boleto)
        {
            string codigoCedente = boleto.Cedente.Codigo.ToString();
            codigoCedente = Utils.FormatCode(codigoCedente, 6);

            string FormataCampoLivre = string.Format("{0}{1}{2}{3}{4}{5}",
                "5", codigoCedente, Banco_Unibanco.Mod11(codigoCedente),
                "00", boleto.NossoNumero, Banco_Unibanco.Mod11(boleto.NossoNumero, true));

            return FormataCampoLivre;
        }

        public override void FormataNumeroDocumento(Boleto boleto)
        {
            throw new NotImplementedException("Fun��o ainda n�o implementada.");
        }

        public override void FormataNossoNumero(Boleto boleto)
        {
            boleto.NossoNumero = string.Format("{0}-{1}", boleto.NossoNumero, Banco_Unibanco.Mod11(boleto.NossoNumero,true));
        }

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            throw new NotImplementedException("Fun��o n�o implementada.");
        }
        public override void ValidaBoleto(Boleto boleto)
        {
            //Verifica se o nosso n�mero � v�lido
            //Verifica se o tamanho para o NossoNumero s�o 12 d�gitos
            if (Utils.ToString(boleto.NossoNumero) == string.Empty)
                throw new NotImplementedException("Nosso n�mero inv�lido");
            else if (Convert.ToInt32(boleto.NossoNumero).ToString().Length > 14)
                throw new NotImplementedException("A quantidade de d�gitos do nosso n�mero s�o 14 n�meros.");
            else if (Convert.ToInt32(boleto.NossoNumero).ToString().Length < 14)
                boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 14);

            //Verificar se a Agencia esta correta
            if (boleto.Cedente.ContaBancaria.Agencia.Length > 4)
                throw new NotImplementedException("A quantidade de d�gitos da Ag�ncia " + boleto.Cedente.ContaBancaria.Agencia + ", s�o de 4 n�meros.");
            else if (boleto.Cedente.ContaBancaria.Agencia.Length < 4)
                boleto.Cedente.ContaBancaria.Agencia = Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4);

           //Verificar se a Conta esta correta
            if (boleto.Cedente.ContaBancaria.Conta.Length > 6)
                throw new NotImplementedException("A quantidade de d�gitos da Conta " + boleto.Cedente.ContaBancaria.Conta + ", s�o de 6 n�meros.");
            else if (boleto.Cedente.ContaBancaria.Conta.Length < 6)
                boleto.Cedente.ContaBancaria.Conta = Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 6);

            //Atribui o nome do banco ao local de pagamento
            boleto.LocalPagamento += Nome;

            //Verifica se data do processamento � valida
			//if (boleto.DataProcessamento.ToString("dd/MM/yyyy") == "01/01/0001")
			if (boleto.DataProcessamento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataProcessamento = DateTime.Now;

            //Verifica se data do documento � valida
			//if (boleto.DataDocumento.ToString("dd/MM/yyyy") == "01/01/0001")
			if (boleto.DataDocumento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataDocumento = DateTime.Now;

            if (boleto.Carteira != "20")
                throw new NotImplementedException("Carteira n�o implementada: " + boleto.Carteira + ". Utilize a carteira 20.");

            //Boleto.QuantidadeMoeda = 0;

            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);
            FormataNossoNumero(boleto);
        }
        #endregion IBanco Members

        internal new static int Mod10(string seq)
        {
            int Digito, Soma = 0, Peso = 2, res;

            for (int i = seq.Length; i > 0; i--)
            {
                res = (Convert.ToInt32(Strings.Mid(seq, i, 1)) * Peso);

                if (res > 9)
                    res = (res - 9);

                Soma += res;

                if (Peso == 2)
                    Peso = 1;
                else
                    Peso = Peso + 1;
            }

            Digito = ((10 - (Soma % 10)) % 10);

            return Digito;
        }

        internal new static int Mod11(string seq)
        {
            bool superDigito = false;

            return Mod11(seq, superDigito);
        }

        internal static int Mod11(string seq, bool superDigito)
        {
            int Digito, Soma = 0, Peso = 2;

            for (int i = seq.Length; i > 0; i--)
            {
                Soma += (Convert.ToInt32(Strings.Mid(seq, i, 1)) * Peso);

                if (Peso == 9)
                    Peso = 2;
                else
                    Peso = Peso + 1;
            }

            Digito = ((Soma * 10) % 11);

            if ((superDigito) && (Digito == 10))
                Digito = 0;
            else if ((Digito == 0) || (Digito == 10))
                Digito = 1;

            return Digito;
        }


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
