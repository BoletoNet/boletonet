using System;
using System.Web.UI;
using Microsoft.VisualBasic;
using System.Text;

[assembly: WebResource("BoletoNet.Imagens.003.jpg", "image/jpg")]

namespace BoletoNet
{
    /// <author>  
    /// Ermilson
    /// </author>    
    internal class Banco_Basa : AbstractBanco, IBanco
    {
        private string _dacNossoNumero = string.Empty;
        private int _dacBoleto = 0;

        /// <summary>
        /// Classe responsavel em criar os campos do Banco Banco_Bradesco.
        /// </summary>
        internal Banco_Basa()
        {
            this.Codigo = 3;
            this.Digito = "5";
            this.Nome = "Basa";
        }

        #region IBanco Members

        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            string nossonumero = boleto.NossoNumero + "000000";

            // grupo 1:

            string banco = Utils.FormatCode(Codigo.ToString(), 3);
            string moeda = boleto.Moeda.ToString("0");
            string agencia = Strings.Right(boleto.Cedente.ContaBancaria.Agencia, 3) + boleto.Cedente.ContaBancaria.DigitoAgencia;
            string convenio_parte1 = boleto.Cedente.Codigo.ToString().Substring(0, 1);

            string dv1 = Mod10_LinhaDigitavel(banco + moeda + agencia + convenio_parte1).ToString("0");

            string grupo1 = banco + moeda + agencia + convenio_parte1 + dv1;

            // grupo 2:

            string convenio_parte2 = boleto.Cedente.Codigo.ToString().Substring(1, 3);
            string nossonumero_parte1 = nossonumero.Substring(0, 7);

            string dv2 = Mod10_LinhaDigitavel(convenio_parte2 + nossonumero_parte1).ToString("0");

            string grupo2 = convenio_parte2 + nossonumero_parte1 + dv2;

            // grupo 3:

            string nossonumero_parte2 = nossonumero.Substring(7, 9);
            string identificadorsistema = "8";

            string dv3 = Mod10_LinhaDigitavel(nossonumero_parte2 + identificadorsistema).ToString("0");

            string grupo3 = nossonumero_parte2 + identificadorsistema + dv3;

            // grupo 4:

            string dv_codigobarra = boleto.CodigoBarra.Codigo.Substring(4, 1);

            string grupo4 = dv_codigobarra;

            // grupo 5:

            string fatorvencimento = FatorVencimento(boleto).ToString();
            string valordocumento = Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10);

            string grupo5 = fatorvencimento + valordocumento;

            string L = grupo1 + grupo2 + grupo3 + grupo4 + grupo5;

            boleto.CodigoBarra.LinhaDigitavel =
                L.Substring(0, 5) + "." +
                L.Substring(5, 5) + " " +
                L.Substring(10, 5) + "." +
                L.Substring(15, 6) + "  " +
                L.Substring(21, 5) + "." +
                L.Substring(26, 6) + " " +
                L.Substring(32, 1) + " " +
                L.Substring(33, 14);
        }

        public override void FormataCodigoBarra(Boleto boleto)
        {
            // Código de Barras
            //banco & moeda & fator & valor & carteira & nossonumero & dac_nossonumero & agencia & conta & dac_conta & "000"

            string banco = Utils.FormatCode(Codigo.ToString(), 3);
            int moeda = boleto.Moeda;
            //string digito = "";
            string valorBoleto = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            valorBoleto = Utils.FormatCode(valorBoleto, 10);

            string fatorvencimento = FatorVencimento(boleto).ToString();

            string agencia = Strings.Right(boleto.Cedente.ContaBancaria.Agencia, 3) + boleto.Cedente.ContaBancaria.DigitoAgencia;
            string convenio = boleto.Cedente.Codigo.PadLeft(4, '0'); //ToString("0000");
            string nossonumero = boleto.NossoNumero + "000000";

            string sistemaarrecadacao = "8";  // Este numero foi fornecido pelo BASA para o convenio testado.. nao sei se muda.

            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}",
                banco, moeda, fatorvencimento, valorBoleto);

            boleto.CodigoBarra.Codigo += string.Format("{0}{1}{2}{3}",
                agencia, convenio, nossonumero, sistemaarrecadacao);

            _dacBoleto = Mod11_CodigoBarra(boleto.CodigoBarra.Codigo, 9);

            boleto.CodigoBarra.Codigo = Strings.Left(boleto.CodigoBarra.Codigo, 4) + _dacBoleto + Strings.Right(boleto.CodigoBarra.Codigo, 39);

        }
        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public string FormataCampoLivre(Boleto boleto)
        {
            throw new NotImplementedException();
        }


        public override void FormataNumeroDocumento(Boleto boleto)
        {
            boleto.NumeroDocumento = string.Format("{0}", boleto.NumeroDocumento);
        }


        public override void FormataNossoNumero(Boleto boleto)
        {
            boleto.NossoNumero = string.Format("{0:0000}{1}", boleto.Cedente.Codigo, boleto.NumeroDocumento);
        }


        public override void ValidaBoleto(Boleto boleto)
        {

            //Verifica se o nosso número é válido
            if (Utils.ToInt64(boleto.NossoNumero) == 0)
                throw new NotImplementedException("Nosso número inválido");


            //Verifica se o tamanho para o NossoNumero são 10 dígitos
            if (boleto.NossoNumero.Length > 10)
                throw new NotImplementedException("A quantidade de dígitos do nosso número para a carteira " + boleto.Carteira + ", são 10 números.");
            else if (boleto.NossoNumero.Length < 10)
                boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 10);

            if (boleto.Carteira != "CNR")
                throw new NotImplementedException("Carteira não implementada. Utilize a carteira CNR.");

            //Atribui o nome do banco ao local de pagamento
            boleto.LocalPagamento += Nome + "";

            //Verifica se data do processamento é valida
			//if (boleto.DataProcessamento.ToString("dd/MM/yyyy") == "01/01/0001")
			if (boleto.DataProcessamento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataProcessamento = DateTime.Now;


            //Verifica se data do documento é valida
			//if (boleto.DataDocumento.ToString("dd/MM/yyyy") == "01/01/0001")
			if (boleto.DataDocumento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataDocumento = DateTime.Now;


            FormataNossoNumero(boleto);
            FormataNumeroDocumento(boleto);
            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);
        }
        #endregion IBanco Members

        #region Métodos privados

        private int Mod11_CodigoBarra(string value, int Base)
        {
            int Digito, Soma = 0, Peso = 2;
            for (int i = value.Length; i > 0; i--)
            {
                Soma = Soma + (Convert.ToInt32(Strings.Mid(value, i, 1)) * Peso);
                if (Peso == Base)
                    Peso = 2;
                else
                    Peso = Peso + 1;
            }
            if (((Soma % 11) == 0) || ((Soma % 11) == 10) || ((Soma % 11) == 1))
            {
                Digito = 1;
            }
            else
            {
                Digito = 11 - (Soma % 11);
            }
            return Digito;
        }

        private int Mod10_LinhaDigitavel(string seq)
        {
            int Digito, Soma = 0, Peso = 2, m1;
            string m2;
            for (int i = seq.Length; i > 0; i--)
            {
                m1 = (Convert.ToInt32(Strings.Mid(seq, i, 1)) * Peso);
                m2 = m1.ToString();

                for (int j = 1; j <= m2.Length; j++)
                {
                    Soma += Convert.ToInt32(Strings.Mid(m2, j, 1));
                }

                if (Peso == 2)
                    Peso = 1;
                else
                    Peso = Peso + 1;
            }
            Digito = ((10 - (Soma % 10)) % 10);
            return Digito;
        }

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
