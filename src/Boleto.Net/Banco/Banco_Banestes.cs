using System;
using System.Web.UI;

[assembly: WebResource("BoletoNet.Imagens.021.jpg", "image/jpg")]
namespace BoletoNet
{
    /// <summary>
    /// Classe referente ao banco Itaú
    /// </summary>
    internal class Banco_Banestes : AbstractBanco, IBanco
    {

        #region Variáveis

        private int _dacBoleto = 0;
        private string _valorMoeda = string.Empty;

        #endregion

        #region Construtores

        internal Banco_Banestes()
        {
            Codigo = 21;
            Digito = "3";
            Nome = "Banestes";
        }

        #endregion

        #region Métodos de Instância

        /// <summary>
        /// Validações particulares do banco Banestes
        /// </summary>
        public override void ValidaBoleto(Boleto boleto)
        {
            try
            {
                //Carteiras válidas
                // 00 - Sem registro;

                //Atribui o nome do banco ao local de pagamento
                boleto.LocalPagamento += Nome + ". Após o vencimento, somente no BANESTES";

                //Verifica se o nosso número é válido
                if (Utils.ToInt64(boleto.NossoNumero) == 0)
                    throw new NotImplementedException("Nosso número inválido");

                //Verifica se data do processamento é valida

                if (boleto.DataProcessamento == DateTime.MinValue)
                    boleto.DataProcessamento = DateTime.Now;

                //Verifica se data do documento é valida
                if (boleto.DataDocumento == DateTime.MinValue)
                    boleto.DataDocumento = DateTime.Now;

                boleto.FormataCampos();
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao validar boletos.", e);
            }
        }

        # endregion

        # region Métodos de formatação do boleto
        /// <summary>
        /// Composição do Código de Barras.
        /// - Código do banco cedente 	 03 Posições (021)
        /// - Moeda 	                 01 Posição (9)
        /// - Dígito Verificador 	     01 Posição (descrito abaixo)
        /// - Fator de Vencimento....... 04 Posições  (descrito na página 19)
        /// - Valor do Título	10 Posições (8,2)
        /// - Chave ASBACE 	 25 Posições.
        /// No BANESTES ficou assim o Código de Barras.
        /// 0219DVVVVVVVVVVVVVVCCCCCCCCCCCCCCCCCCCCCCCCC
        /// Onde:
        /// 021	> Código do BANESTES
        /// 9 	> Moeda (Real)
        /// D 	> Dígito Verificador
        /// FFFF------------------------------------------------------------- > Fator de Vencimento
        /// VVVVVVVVVV-----	> Valor
        /// CCCCCCCCCCCCCCCCCCCCCCCCC 	> Chave ASBACE.
        /// </summary>
        /// <param name="boleto"></param>
        public override void FormataCodigoBarra(Boleto boleto)
        {
            try
            {
                //0219DFFFFVVVVVVVVVVCCCCCCCCCCCCCCCCCCCCCCCCC

                var FFFF = FatorVencimento(boleto);

                var VVVVVVVVVV = _valorMoeda = Utils.FormatCode(boleto.ValorBoleto.ToString("N").Replace(".", "").Replace(",", ""), 10);

                boleto.Banco.ChaveASBACE = GeraChaveASBACE(boleto.Carteira, boleto.Cedente.ContaBancaria.Conta, boleto.NossoNumero, 2);

                string chave = string.Format("0219{0}{1}{2}", FFFF, VVVVVVVVVV, boleto.Banco.ChaveASBACE);

                int peso = 9;

                int S = 0;
                int P = 0;
                int N = 0;

                for (int i = 0; i < chave.Length; i++)
                {

                    N = Convert.ToInt32(chave.Substring(i, 1));

                    if (i == 0)
                        peso = 4;

                    P = N * peso--;

                    S += P;

                    if (peso == 1)
                        peso = 9;
                }

                int R = S % 11;

                if (R == 0 || R == 1 || R == 10)
                    _dacBoleto = 1;
                else
                    _dacBoleto = 11 - R;




                boleto.CodigoBarra.Codigo = string.Format("0219{0}{1}{2}{3}", _dacBoleto, FFFF, VVVVVVVVVV, boleto.Banco.ChaveASBACE);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao formatar código de barras.", ex);
            }
        }

        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            try
            {
                int dv = 0;

                #region Parte 1

                var parte1 = string.Concat("0219", boleto.Banco.ChaveASBACE.Substring(0, 5));

                dv = CalculaDigitoVerificador(parte1);

                parte1 = string.Concat(parte1, dv);

                #endregion

                #region Parte 2

                var parte2 = boleto.Banco.ChaveASBACE.Substring(5, 10);

                dv = CalculaDigitoVerificador(parte2, 1);

                parte2 = string.Concat(parte2, dv);

                #endregion

                #region Parte 3

                var parte3 = boleto.Banco.ChaveASBACE.Substring(15, 10);

                dv = CalculaDigitoVerificador(parte3, 1);

                parte3 = string.Concat(parte3, dv);

                #endregion

                #region Parte 5

                var parte5 = string.Concat(FatorVencimento(boleto), _valorMoeda);

                #endregion

                //var linhaDigitavel = string.Concat("0219", _dacBoleto, FatorVencimento(boleto), _valorMoeda, _chaveASBACE);

                boleto.CodigoBarra.LinhaDigitavel = string.Format("{0}.{1} {2}.{3} {4}.{5} {6} {7}",
                    parte1.Substring(0, 5),
                    parte1.Substring(5, 5),
                    parte2.Substring(0, 5),
                    parte2.Substring(5, 6),
                    parte3.Substring(0, 5),
                    parte3.Substring(5, 6),
                    _dacBoleto,
                    parte5);
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
                boleto.NossoNumero = boleto.NossoNumero.Trim().Replace(".", "").Replace("-", "");

                if (string.IsNullOrEmpty(boleto.NossoNumero))
                    throw new Exception("Nosso Número não informado");

                if (boleto.NossoNumero.Length > 8)
                    throw new Exception("Tamanho máximo para o Nosso Número são de 8 caracteres");

                boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 8);

                int D1 = CalculaDVNossoNumero(boleto.NossoNumero);

                int D2 = CalculaDVNossoNumero(string.Concat(boleto.NossoNumero, D1), 10);
                boleto.NossoNumero = string.Format("{0}.{1}{2}", boleto.NossoNumero, D1, D2);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao formatar nosso número", ex);
            }
        }

        # endregion

        private int CalculaDVNossoNumero(string nossoNumero, short peso = 9)
        {
            int S = 0;
            int P = 0;
            int N = 0;
            int d = 0;

            for (int i = 0; i < nossoNumero.Length; i++)
            {
                N = Convert.ToInt32(nossoNumero.Substring(i, 1));

                P = N * peso--;

                S += P;
            }

            int R = S % 11;

            if (R == 0 || R == 1)
                d = 0;

            if (R > 1)
                d = 11 - R;

            return d;
        }

        private int CalculaDigitoVerificador(string chave, short peso = 2)
        {
            int D1 = 0;
            int K = 0;
            int S = 0;

            for (int i = 0; i < chave.Length; i++)
            {
                int N = Convert.ToInt32(chave.Substring(i, 1));

                int P = N * peso;

                if (P > 9)
                    K = P - 9;

                if (P < 10)
                    K = P;

                S += K;

                if (peso == 2)
                    peso = 1;
                else
                    peso = 2;
            }

            int resto = S % 10;

            if (resto == 0)
                D1 = 0;
            else if (resto > 0)
                D1 = 10 - resto;

            return D1;
        }

        private int CalculaD1(string chave)
        {
            int D1 = 0;
            short peso = 2;
            int K = 0;
            int S = 0;

            for (int i = 0; i < chave.Length; i++)
            {
                int N = Convert.ToInt32(chave.Substring(i, 1));

                int P = N * peso;

                if (P > 9)
                    K = P - 9;

                if (P < 10)
                    K = P;

                S += K;

                if (peso == 2)
                    peso = 1;
                else
                    peso = 2;
            }

            int resto = S % 10;

            if (resto == 0)
                D1 = 0;
            else if (resto > 0)
                D1 = 10 - resto;

            return D1;
        }

        private int CalculaD2(string chave, int D1)
        {
            int D2 = 0;
            short peso = 7;
            int P = 0;
            int S = 0;

            string chaveD1 = string.Concat(chave, D1);

            for (int i = 0; i < chaveD1.Length; i++)
            {
                int N = Convert.ToInt32(chaveD1.Substring(i, 1));

                P = N * peso--;

                S += P;

                if (peso == 1)
                    peso = 7;
            }

            int resto = S % 11;

            if (resto == 0)
                D2 = 0;

            if (resto == 1)
            {
                D1++;
                if (D1 == 10)
                    D1 = 0;
                return CalculaD2(chave, D1);
            }

            if (resto > 1)
                D2 = 11 - resto;

            return D2;
        }

        /// <summary>
        /// Composição da Chave ASBACE.
        /// - Campo livre 	 20 Posições
        /// (este campo é reservado para identificar a agência, cliente, número do título, etc. no banco cedente do título).
        /// - Código do banco cedente 	 03 Posições
        /// - Dígitos	 02 Posições.
        /// No BANESTES ficou assim a Chave ASBACE.
        /// NNNNNNNNCCCCCCCCCCCR021DD
        /// Onde:
        /// NNNNNNNN 	> Nosso Número (sem os dois dígitos).
        /// CCCCCCCCCCC 	> Conta Corrente.
        /// R 	> 2- Sem registro; 3- Caucionada; 4,5,6 e 7-Cobrança com registro.
        /// 021 	> Código do BANESTES .
        /// DD 	> Dígitos Verificadores.
        /// </summary>
        public string GeraChaveASBACE(string carteira, string conta, string nossoNumero, int tipoCobranca)
        {
            try
            {
                conta = conta.Replace(".", "").Replace("-", "");

                if (Utils.ToInt32(conta) < 1)
                    throw new Exception("Conta Corrente inválida");

                if (string.IsNullOrEmpty(carteira))
                    throw new Exception("Carteira não informada");

                string NNNNNNNN = Utils.FormatCode(nossoNumero.Substring(0, 8), 8);
                string CCCCCCCCCCC = Utils.FormatCode(conta, 11);
                string R = tipoCobranca.ToString();

                string NNNNNNNNCCCCCCCCCCCR021 = string.Concat(NNNNNNNN, CCCCCCCCCCC, R, "021");

                int D1 = CalculaD1(NNNNNNNNCCCCCCCCCCCR021);
                int D2 = CalculaD2(NNNNNNNNCCCCCCCCCCCR021, D1);

                return string.Concat(NNNNNNNNCCCCCCCCCCCR021, D1, D2);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Erro ao tentar gerar a chave ASBACE. {0}", e.Message));
            }
        }
    }
}
