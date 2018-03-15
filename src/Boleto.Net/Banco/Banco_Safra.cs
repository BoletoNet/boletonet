using System;
using System.Web.UI;
using BoletoNet;
using BoletoNet.Util;

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
                throw new IndexOutOfRangeException("Erro. O campo 'Nosso Número' deve ter mais de 9 digitos. Você digitou " + boleto.NossoNumero);
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
        /// CLIENTE	        021	034	CÓDIGO DO CLIENTE	CÓDIGO/AGÊNCIA CEDENTE
        /// N/NÚMERO	    035	043	NOSSO NÚMERO	NOSSO NÚMERO DO TÍTULO
        /// TIPO COBRANÇA	044	044	2	FIXO
        /// </summary>
        public string CampoLivre(Boleto boleto)
        {

            string campolivre = "7" + boleto.Cedente.ContaBancaria.Agencia.ToString() + boleto.Cedente.ContaBancaria.Conta.ToString() + 
                                boleto.NossoNumero.Substring(0, 9) + "2";
            return campolivre;
        }

        #region IBanco Members
        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public override void ValidaBoleto(Boleto boleto)
        {

            // Calcula o DAC do Nosso Número
            _dacNossoNumero = CalcularDigitoNossoNumero(boleto);

            // Calcula o DAC da Conta Corrente
            _dacContaCorrente = Mod10(boleto.Cedente.ContaBancaria.Agencia + boleto.Cedente.ContaBancaria.Conta);

            //Verifica se o nosso número é válido
            if (Utils.ToInt64(boleto.NossoNumero) == 0)
                throw new NotImplementedException("Nosso número inválido");

            //Verifica se data do processamento é valida
			//if (boleto.DataProcessamento.ToString("dd/MM/yyyy") == "01/01/0001")
			if (boleto.DataProcessamento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataProcessamento = DateTime.Now;

            //Verifica se data do documento é valida
			//if (boleto.DataDocumento.ToString("dd/MM/yyyy") == "01/01/0001")
			if (boleto.DataDocumento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataDocumento = DateTime.Now;

            string cedente = boleto.Cedente.Codigo;

            boleto.Cedente.Codigo = string.Format("{0}/{1}-{2}", cedente.Substring(0, 5), cedente.Substring(5, 8), cedente.Substring(13));

            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);
            FormataNossoNumero(boleto);
        }

        public override void FormataNumeroDocumento(Boleto boleto)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public override void FormataNossoNumero(Boleto boleto)
        {
            string nossoNumero = boleto.NossoNumero;

            if (nossoNumero == null || nossoNumero.Length != 9)
            {
                throw new Exception("Erro ao tentar formatar nosso número, verifique o tamanho do campo");
            }

            try  
            {
                boleto.NossoNumero = string.Format("{0}-{1}", nossoNumero.Substring(0, 8), nossoNumero.Substring(8));
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao formatar nosso número", ex);
            }
        }

        /// <summary>
        ///	O código de barra para cobrança contém 44 posições dispostas da seguinte forma:
        ///    01 a 03 - 3 - Identificação  do  Banco
        ///    04 a 04 - 1 - Código da Moeda
        ///    05 a 05 – 1 - Dígito verificador do Código de Barras
        ///    06 a 19 - 14 - Valor
        ///    20 a 44 – 25 - Campo Livre
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
        /// A linha digitável será composta por cinco campos:
        ///    1º CAMPO - Composto pelo código do banco ( sem o dígito verificador = 422 ), 
        ///       código da moeda,digito(7), os quatros primeiros numeros da agência e mais
        ///       um dígito verificador deste campo. 
        ///       Após os 5 primeiros dígitos deste campo separar o conteúdo por um ponto ( . ). 
        ///    2º CAMPO - Composto pelo ultimo numero da agência, numero da conta bancária com 
        ///        o digito e mais um dígito verificador deste campo. 
        ///       Após os 5 primeiros dígitos deste campo separar o conteúdo por um ponto ( . ).
        ///    3º CAMPO - Composto pelos 9 numeros do nosso numero,digito fixo(2) e mais um dígito verificador deste campo. 
        ///       Após os 5 primeiros dígitos deste campo separar o conteúdo por um ponto ( . ).
        ///    4º CAMPO  - Composto pelo dígito de autoconferência do código de barras.
        ///    5º CAMPO - Composto pelo 4 numeros do fator vencimento e pelo valor nominal do documento, com supressão de 
        ///    zeros a esquerda e sem edição ( sem ponto e vírgula ). 
        ///    Quando se tratar de valor zerado, a representação deverá ser 000 ( três zeros ).
        /// </summary>
        /// Modificado por Carlos Rogerio - Aracaju/SE
        public override void FormataLinhaDigitavel(Boleto boleto)
        {

            //AAABC.CCCCX DDDDD.DDDDDY EEEEE.EEEEEZ K VVVVVVVVVVVVVV

            string LD = string.Empty; //Linha Digitável
            
            #region Campo 1

            //Campo 1
            string AAA = Utils.FormatCode(Codigo.ToString(), 3);
            string B = boleto.Moeda.ToString();
            string F = Digito;
            string CCCCC = boleto.Cedente.ContaBancaria.Agencia.Substring(0, 4); //CampoLivre(boleto).Substring(0, 5);
            string X = Mod10(AAA + B + F + CCCCC).ToString();

            LD = string.Format("{0}{1}{2}.", AAA, B, F);
            LD += string.Format("{0}{1} ", CCCCC, X);

            #endregion Campo 1

            #region Campo 2

            string C = boleto.Cedente.ContaBancaria.Agencia.Substring(4, 1);
            string DDDDDDDDD = boleto.Cedente.ContaBancaria.Conta.ToString().Trim() +  boleto.Cedente.ContaBancaria.DigitoConta.ToString().Trim(); //CampoLivre(boleto).Substring(6, 15);
            DDDDDDDDD = DDDDDDDDD.Replace(" ", "");
            DDDDDDDDD = Utils.FormatCode(DDDDDDDDD, 9);
            string Y = Mod10(C + DDDDDDDDD).ToString();

            LD += string.Format("{0}{1}.", C, DDDDDDDDD.Substring(0, 4));
            LD += string.Format("{0}{1} ", DDDDDDDDD.Substring(4, 5), Y);

            #endregion Campo 2

            #region Campo 3

            string EEEEE = boleto.NossoNumero.Substring(0, 9); //CampoLivre(boleto).Substring(11, 10);
            string FX = "2";
            string Z = Mod10(EEEEE + FX).ToString();

            LD += string.Format("{0}.", EEEEE.Substring(0, 5));
            LD += string.Format("{0}{1}{2}", EEEEE.Substring(5, 4), FX, Z);

            #endregion Campo 3

            #region Campo 4

            string K = _dacBoleto.ToString();

            LD += string.Format(" {0} ", K);

            #endregion Campo 4

            #region Campo 5
            string FVENC = FatorVencimento(boleto).ToString();
            string VVVVVVVVVV;

            if (boleto.ValorBoleto != 0)
            {
                VVVVVVVVVV = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
                VVVVVVVVVV = Utils.FormatCode(VVVVVVVVVV, 10);
            }
            else
                VVVVVVVVVV = "000";

            //LD += VVVVVVVVVV;
            LD += string.Format("{0}{1} ", FVENC.Substring(0,4), VVVVVVVVVV);

            #endregion Campo 5

            boleto.CodigoBarra.LinhaDigitavel = LD;

        }
        #endregion IBanco Members


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
