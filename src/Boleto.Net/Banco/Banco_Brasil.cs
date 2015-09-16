
using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using Microsoft.VisualBasic;
using BoletoNet.EDI.Banco;

[assembly: WebResource("BoletoNet.Imagens.001.jpg", "image/jpg")]
namespace BoletoNet
{
    /// <summary>
    /// Classe referente ao Banco do Brasil
    /// </summary>
    internal class Banco_Brasil : AbstractBanco, IBanco
    {

        #region Vari�veis

        private string _dacNossoNumero = string.Empty;
        private int _dacBoleto = 0;

        #endregion

        #region Construtores

        internal Banco_Brasil()
        {
            try
            {
                this.Codigo = 1;
                this.Digito = "9";
                this.Nome = "Banco do Brasil";
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao instanciar objeto.", ex);
            }
        }
        #endregion

        #region M�todos de Inst�ncia

        /// <summary>
        /// Valida��es particulares do Banco do Brasil
        /// </summary>
        public override void ValidaBoleto(Boleto boleto)
        {
            if (string.IsNullOrEmpty(boleto.Carteira))
                throw new NotImplementedException("Carteira n�o informada. Utilize a carteira 11, 16, 17, 17-019, 17-027, 18, 18-019, 18-027, 18-035, 18-140 ou 31.");

            //Verifica as carteiras implementadas
            if (!boleto.Carteira.Equals("11") &
                !boleto.Carteira.Equals("16") &
                !boleto.Carteira.Equals("17") &
                !boleto.Carteira.Equals("17-019") &
                !boleto.Carteira.Equals("17-027") &
                !boleto.Carteira.Equals("18") &
                !boleto.Carteira.Equals("18-019") &
                !boleto.Carteira.Equals("18-027") &
                !boleto.Carteira.Equals("18-035") &
                !boleto.Carteira.Equals("18-140") &
                !boleto.Carteira.Equals("31"))

                throw new NotImplementedException("Carteira n�o informada. Utilize a carteira 11, 16, 17, 17-019, 17-027, 18, 18-019, 18-027, 18-035, 18-140 ou 31.");

            //Verifica se o nosso n�mero � v�lido
            if (Utils.ToString(boleto.NossoNumero) == string.Empty)
                throw new NotImplementedException("Nosso n�mero inv�lido");


            #region Carteira 11
            //Carteira 18 com nosso n�mero de 11 posi��es
            if (boleto.Carteira.Equals("11"))
            {
                if (!boleto.TipoModalidade.Equals("21"))
                {
                    if (boleto.NossoNumero.Length > 11)
                        throw new NotImplementedException(string.Format("Para a carteira {0}, a quantidade m�xima s�o de 11 de posi��es para o nosso n�mero", boleto.Carteira));

                    if (boleto.Cedente.Convenio.ToString().Length == 6)
                        boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 11));
                    else
                        boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 11);
                }
                else
                {
                    if (boleto.Cedente.Convenio.ToString().Length != 6)
                        throw new NotImplementedException(string.Format("Para a carteira {0} e o tipo da modalidade 21, o n�mero do conv�nio s�o de 6 posi��es", boleto.Carteira));

                    boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 17);
                }
            }
            #endregion Carteira 11

            #region Carteira 16
            //Carteira 18 com nosso n�mero de 11 posi��es
            if (boleto.Carteira.Equals("16"))
            {
                if (!boleto.TipoModalidade.Equals("21"))
                {
                    if (boleto.NossoNumero.Length > 11)
                        throw new NotImplementedException(string.Format("Para a carteira {0}, a quantidade m�xima s�o de 11 de posi��es para o nosso n�mero", boleto.Carteira));

                    if (boleto.Cedente.Convenio.ToString().Length == 6)
                        boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 11));
                    else
                        boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 11);
                }
                else
                {
                    if (boleto.Cedente.Convenio.ToString().Length != 6)
                        throw new NotImplementedException(string.Format("Para a carteira {0} e o tipo da modalidade 21, o n�mero do conv�nio s�o de 6 posi��es", boleto.Carteira));

                    boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 17);
                }
            }
            #endregion Carteira 16

            #region Carteira 17
            //Carteira 17
            if (boleto.Carteira.Equals("17"))
            {
                switch (boleto.Cedente.Convenio.ToString().Length)
                {
                    //O BB manda como padr�o 7 posi��es, mas � poss�vel solicitar um conv�nio com 6 posi��es na carteira 17
                    case 6:
                        if (boleto.NossoNumero.Length > 12)
                            throw new NotImplementedException(string.Format("Para a carteira {0}, a quantidade m�xima s�o de 12 de posi��es para o nosso n�mero", boleto.Carteira));
                        boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 12);
                        break;
                    case 7:
                        if (boleto.NossoNumero.Length > 17)
                            throw new NotImplementedException(string.Format("Para a carteira {0}, a quantidade m�xima s�o de 10 de posi��es para o nosso n�mero", boleto.Carteira));
                        boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 10));
                        break;
                    default:
                        throw new NotImplementedException(string.Format("Para a carteira {0}, o n�mero do conv�nio deve ter 6 ou 7 posi��es", boleto.Carteira));
                }
            }
            #endregion Carteira 17

            #region Carteira 17-019
            //Carteira 17, com varia��o 019
            if (boleto.Carteira.Equals("17-019"))
            {
                /*
                 * Conv�nio de 7 posi��es
                 * Nosso N�mero com 17 posi��es
                 */
                if (boleto.Cedente.Convenio.ToString().Length == 7)
                {
                    if (boleto.NossoNumero.Length > 10)
                        throw new NotImplementedException(string.Format("Para a carteira {0}, a quantidade m�xima s�o de 10 de posi��es para o nosso n�mero", boleto.Carteira));

                    boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 10));
                }
                /*
                 * Conv�nio de 6 posi��es
                 * Nosso N�mero com 11 posi��es
                 */
                else if (boleto.Cedente.Convenio.ToString().Length == 6)
                {
                    //Nosso N�mero com 17 posi��es
                    if ((boleto.Cedente.Codigo.ToString().Length + boleto.NossoNumero.Length) > 11)
                        throw new NotImplementedException(string.Format("Para a carteira {0}, a quantidade m�xima s�o de 11 de posi��es para o nosso n�mero. Onde o nosso n�mero � formado por CCCCCCNNNNN-X: C -> n�mero do conv�nio fornecido pelo Banco, N -> seq�encial atribu�do pelo cliente e X -> d�gito verificador do �Nosso-N�mero�.", boleto.Carteira));

                    boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 5));
                }
                /*
                  * Conv�nio de 4 posi��es
                  * Nosso N�mero com 11 posi��es
                  */
                else if (boleto.Cedente.Convenio.ToString().Length == 4)
                {
                    if (boleto.NossoNumero.Length > 7)
                        throw new NotImplementedException(string.Format("Para a carteira {0}, a quantidade m�xima s�o de 7 de posi��es para o nosso n�mero [{1}]", boleto.Carteira, boleto.NossoNumero));

                    boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 7));
                }
                else
                    boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 11);
            }
            #endregion Carteira 17-019

            #region Carteira 17-027
            //Carteira 17, com varia��o 027
            if (boleto.Carteira.Equals("17-027"))
            {
                /*
                 * Conv�nio de 7 posi��es
                 * Nosso N�mero com 17 posi��es
                 */
                if (boleto.Cedente.Convenio.ToString().Length == 7)
                {
                    if (boleto.NossoNumero.Length > 10)
                        throw new NotImplementedException(string.Format("Para a carteira {0}, a quantidade m�xima s�o de 10 de posi��es para o nosso n�mero", boleto.Carteira));

                    boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 10));
                }
                /*
                 * Conv�nio de 6 posi��es
                 * Nosso N�mero com 11 posi��es
                 */
                else if (boleto.Cedente.Convenio.ToString().Length == 6)
                {
                    //Nosso N�mero com 17 posi��es
                    if ((boleto.Cedente.Codigo.ToString().Length + boleto.NossoNumero.Length) > 11)
                        throw new NotImplementedException(string.Format("Para a carteira {0}, a quantidade m�xima s�o de 11 de posi��es para o nosso n�mero. Onde o nosso n�mero � formado por CCCCCCNNNNN-X: C -> n�mero do conv�nio fornecido pelo Banco, N -> seq�encial atribu�do pelo cliente e X -> d�gito verificador do �Nosso-N�mero�.", boleto.Carteira));

                    boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 5));
                }
                /*
                  * Conv�nio de 4 posi��es
                  * Nosso N�mero com 11 posi��es
                  */
                else if (boleto.Cedente.Convenio.ToString().Length == 4)
                {
                    if (boleto.NossoNumero.Length > 7)
                        throw new NotImplementedException(string.Format("Para a carteira {0}, a quantidade m�xima s�o de 7 de posi��es para o nosso n�mero [{1}]", boleto.Carteira, boleto.NossoNumero));

                    boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 7));
                }
                else
                    boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 11);
            }
            #endregion Carteira 17-027

            #region Carteira 18
            //Carteira 18 com nosso n�mero de 11 posi��es
            if (boleto.Carteira.Equals("18"))
            {
                boleto.BancoCarteira.ValidaBoleto(boleto);
                boleto.BancoCarteira.FormataNossoNumero(boleto);
            }
            #endregion Carteira 18

            #region Carteira 18-019
            //Carteira 18, com varia��o 019
            if (boleto.Carteira.Equals("18-019"))
            {
                /*
                 * Conv�nio de 7 posi��es
                 * Nosso N�mero com 17 posi��es
                 */
                if (boleto.Cedente.Convenio.ToString().Length == 7)
                {
                    if (boleto.NossoNumero.Length > 10)
                        throw new NotImplementedException(string.Format("Para a carteira {0}, a quantidade m�xima s�o de 10 de posi��es para o nosso n�mero", boleto.Carteira));

                    boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 10));
                }
                /*
                 * Conv�nio de 6 posi��es
                 * Nosso N�mero com 11 posi��es
                 */
                else if (boleto.Cedente.Convenio.ToString().Length == 6)
                {
                    //Modalidades de Cobran�a Sem Registro � Carteira 16 e 18
                    //Nosso N�mero com 17 posi��es
                    if (!boleto.TipoModalidade.Equals("21"))
                    {
                        if ((boleto.Cedente.Codigo.ToString().Length + boleto.NossoNumero.Length) > 11)
                            throw new NotImplementedException(string.Format("Para a carteira {0}, a quantidade m�xima s�o de 11 de posi��es para o nosso n�mero. Onde o nosso n�mero � formado por CCCCCCNNNNN-X: C -> n�mero do conv�nio fornecido pelo Banco, N -> seq�encial atribu�do pelo cliente e X -> d�gito verificador do �Nosso-N�mero�.", boleto.Carteira));

                        boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 5));
                    }
                    else
                    {
                        if (boleto.Cedente.Convenio.ToString().Length != 6)
                            throw new NotImplementedException(string.Format("Para a carteira {0} e o tipo da modalidade 21, o n�mero do conv�nio s�o de 6 posi��es", boleto.Carteira));

                        boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 17);
                    }
                }
                /*
                  * Conv�nio de 4 posi��es
                  * Nosso N�mero com 11 posi��es
                  */
                else if (boleto.Cedente.Convenio.ToString().Length == 4)
                {
                    if (boleto.NossoNumero.Length > 7)
                        throw new NotImplementedException(string.Format("Para a carteira {0}, a quantidade m�xima s�o de 7 de posi��es para o nosso n�mero [{1}]", boleto.Carteira, boleto.NossoNumero));

                    boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 7));
                }
                else
                    boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 11);
            }
            #endregion Carteira 18-019


            //Para atender o cliente Fiemg foi adaptado no c�digo na varia��o 18-027 as varia��es 18-035 e 18-140
            #region Carteira 18-027
            //Carteira 18, com varia��o 019
            if (boleto.Carteira.Equals("18-027"))
            {
                /*
                 * Conv�nio de 7 posi��es
                 * Nosso N�mero com 17 posi��es
                 */
                if (boleto.Cedente.Convenio.ToString().Length == 7)
                {
                    if (boleto.NossoNumero.Length > 10)
                        throw new NotImplementedException(string.Format("Para a carteira {0}, a quantidade m�xima s�o de 10 de posi��es para o nosso n�mero", boleto.Carteira));

                    boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 10));
                }
                /*
                 * Conv�nio de 6 posi��es
                 * Nosso N�mero com 11 posi��es
                 */
                else if (boleto.Cedente.Convenio.ToString().Length == 6)
                {
                    //Modalidades de Cobran�a Sem Registro � Carteira 16 e 18
                    //Nosso N�mero com 17 posi��es
                    if (!boleto.TipoModalidade.Equals("21"))
                    {
                        if ((boleto.Cedente.Codigo.ToString().Length + boleto.NossoNumero.Length) > 11)
                            throw new NotImplementedException(string.Format("Para a carteira {0}, a quantidade m�xima s�o de 11 de posi��es para o nosso n�mero. Onde o nosso n�mero � formado por CCCCCCNNNNN-X: C -> n�mero do conv�nio fornecido pelo Banco, N -> seq�encial atribu�do pelo cliente e X -> d�gito verificador do �Nosso-N�mero�.", boleto.Carteira));

                        boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 5));
                    }
                    else
                    {
                        if (boleto.Cedente.Convenio.ToString().Length != 6)
                            throw new NotImplementedException(string.Format("Para a carteira {0} e o tipo da modalidade 21, o n�mero do conv�nio s�o de 6 posi��es", boleto.Carteira));

                        boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 17);
                    }
                }
                /*
                  * Conv�nio de 4 posi��es
                  * Nosso N�mero com 11 posi��es
                  */
                else if (boleto.Cedente.Convenio.ToString().Length == 4)
                {
                    if (boleto.NossoNumero.Length > 7)
                        throw new NotImplementedException(string.Format("Para a carteira {0}, a quantidade m�xima s�o de 7 de posi��es para o nosso n�mero [{1}]", boleto.Carteira, boleto.NossoNumero));

                    boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 7));
                }
                else
                    boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 11);
            }
            #endregion Carteira 18-027

            #region Carteira 18-035
            //Carteira 18, com varia��o 019
            if (boleto.Carteira.Equals("18-035"))
            {
                /*
                 * Conv�nio de 7 posi��es
                 * Nosso N�mero com 17 posi��es
                 */
                if (boleto.Cedente.Convenio.ToString().Length == 7)
                {
                    if (boleto.NossoNumero.Length > 10)
                        throw new NotImplementedException(string.Format("Para a carteira {0}, a quantidade m�xima s�o de 10 de posi��es para o nosso n�mero", boleto.Carteira));

                    boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 10));
                }
                /*
                 * Conv�nio de 6 posi��es
                 * Nosso N�mero com 11 posi��es
                 */
                else if (boleto.Cedente.Convenio.ToString().Length == 6)
                {
                    //Modalidades de Cobran�a Sem Registro � Carteira 16 e 18
                    //Nosso N�mero com 17 posi��es
                    if (!boleto.TipoModalidade.Equals("21"))
                    {
                        if ((boleto.Cedente.Codigo.ToString().Length + boleto.NossoNumero.Length) > 11)
                            throw new NotImplementedException(string.Format("Para a carteira {0}, a quantidade m�xima s�o de 11 de posi��es para o nosso n�mero. Onde o nosso n�mero � formado por CCCCCCNNNNN-X: C -> n�mero do conv�nio fornecido pelo Banco, N -> seq�encial atribu�do pelo cliente e X -> d�gito verificador do �Nosso-N�mero�.", boleto.Carteira));

                        boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 5));
                    }
                    else
                    {
                        if (boleto.Cedente.Convenio.ToString().Length != 6)
                            throw new NotImplementedException(string.Format("Para a carteira {0} e o tipo da modalidade 21, o n�mero do conv�nio s�o de 6 posi��es", boleto.Carteira));

                        boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 17);
                    }
                }
                /*
                  * Conv�nio de 4 posi��es
                  * Nosso N�mero com 11 posi��es
                  */
                else if (boleto.Cedente.Convenio.ToString().Length == 4)
                {
                    if (boleto.NossoNumero.Length > 7)
                        throw new NotImplementedException(string.Format("Para a carteira {0}, a quantidade m�xima s�o de 7 de posi��es para o nosso n�mero [{1}]", boleto.Carteira, boleto.NossoNumero));

                    boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 7));
                }
                else
                    boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 11);
            }
            #endregion Carteira 18-035

            #region Carteira 18-140
            //Carteira 18, com varia��o 019
            if (boleto.Carteira.Equals("18-140"))
            {
                /*
                 * Conv�nio de 7 posi��es
                 * Nosso N�mero com 17 posi��es
                 */
                if (boleto.Cedente.Convenio.ToString().Length == 7)
                {
                    if (boleto.NossoNumero.Length > 10)
                        throw new NotImplementedException(string.Format("Para a carteira {0}, a quantidade m�xima s�o de 10 de posi��es para o nosso n�mero", boleto.Carteira));

                    boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 10));
                }
                /*
                 * Conv�nio de 6 posi��es
                 * Nosso N�mero com 11 posi��es
                 */
                else if (boleto.Cedente.Convenio.ToString().Length == 6)
                {
                    //Modalidades de Cobran�a Sem Registro � Carteira 16 e 18
                    //Nosso N�mero com 17 posi��es
                    if (!boleto.TipoModalidade.Equals("21"))
                    {
                        if ((boleto.Cedente.Codigo.ToString().Length + boleto.NossoNumero.Length) > 11)
                            throw new NotImplementedException(string.Format("Para a carteira {0}, a quantidade m�xima s�o de 11 de posi��es para o nosso n�mero. Onde o nosso n�mero � formado por CCCCCCNNNNN-X: C -> n�mero do conv�nio fornecido pelo Banco, N -> seq�encial atribu�do pelo cliente e X -> d�gito verificador do �Nosso-N�mero�.", boleto.Carteira));

                        boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 5));
                    }
                    else
                    {
                        if (boleto.Cedente.Convenio.ToString().Length != 6)
                            throw new NotImplementedException(string.Format("Para a carteira {0} e o tipo da modalidade 21, o n�mero do conv�nio s�o de 6 posi��es", boleto.Carteira));

                        boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 17);
                    }
                }
                /*
                  * Conv�nio de 4 posi��es
                  * Nosso N�mero com 11 posi��es
                  */
                else if (boleto.Cedente.Convenio.ToString().Length == 4)
                {
                    if (boleto.NossoNumero.Length > 7)
                        throw new NotImplementedException(string.Format("Para a carteira {0}, a quantidade m�xima s�o de 7 de posi��es para o nosso n�mero [{1}]", boleto.Carteira, boleto.NossoNumero));

                    boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 7));
                }
                else
                    boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 11);
            }
            #endregion Carteira 18-140

            #region Carteira 31
            //Carteira 31
            if (boleto.Carteira.Equals("31"))
            {
                switch (boleto.Cedente.Convenio.ToString().Length)
                {
                    //O BB manda como padr�o 7 posi��es, mas � poss�vel solicitar um conv�nio com 6 posi��es na carteira 31
                    case 5:
                        if (boleto.NossoNumero.Length > 10)
                            throw new NotImplementedException(string.Format("Para a carteira {0}, a quantidade m�xima s�o de 12 de posi��es para o nosso n�mero", boleto.Carteira));
                        boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 10);
                        break;
                    case 6:
                        if (boleto.NossoNumero.Length > 10)
                            throw new NotImplementedException(string.Format("Para a carteira {0}, a quantidade m�xima s�o de 12 de posi��es para o nosso n�mero", boleto.Carteira));
                        boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 10);
                        break;
                    case 7:
                        if (boleto.NossoNumero.Length > 17)
                            throw new NotImplementedException(string.Format("Para a carteira {0}, a quantidade m�xima s�o de 10 de posi��es para o nosso n�mero", boleto.Carteira));
                        boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 10));
                        break;
                    default:
                        throw new NotImplementedException(string.Format("Para a carteira {0}, o n�mero do conv�nio deve ter 6 ou 7 posi��es", boleto.Carteira));
                }
            }
            #endregion Carteira 31


            #region Ag�ncia e Conta Corrente
            //Verificar se a Agencia esta correta
            if (boleto.Cedente.ContaBancaria.Agencia.Length > 4)
                throw new NotImplementedException("A quantidade de d�gitos da Ag�ncia " + boleto.Cedente.ContaBancaria.Agencia + ", s�o de 4 n�meros.");
            else if (boleto.Cedente.ContaBancaria.Agencia.Length < 4)
                boleto.Cedente.ContaBancaria.Agencia = Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4);

            //Verificar se a Conta esta correta
            if (boleto.Cedente.ContaBancaria.Conta.Length > 8)
                throw new NotImplementedException("A quantidade de d�gitos da Conta " + boleto.Cedente.ContaBancaria.Conta + ", s�o de 8 n�meros.");
            else if (boleto.Cedente.ContaBancaria.Conta.Length < 8)
                boleto.Cedente.ContaBancaria.Conta = Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 8);
            #endregion Ag�ncia e Conta Corrente

            //Atribui o nome do banco ao local de pagamento
            if (boleto.LocalPagamento == "At� o vencimento, preferencialmente no ")
                boleto.LocalPagamento += Nome;

            //Verifica se data do processamento � valida
            //if (boleto.DataProcessamento.ToString("dd/MM/yyyy") == "01/01/0001")
            if (boleto.DataProcessamento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataProcessamento = DateTime.Now;

            //Verifica se data do documento � valida
            //if (boleto.DataDocumento.ToString("dd/MM/yyyy") == "01/01/0001")
            if (boleto.DataDocumento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataDocumento = DateTime.Now;

            boleto.QuantidadeMoeda = 0;

            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);
            FormataNossoNumero(boleto);
        }

        # endregion

        private string LimparCarteira(string carteira)
        {
            return carteira.Split('-')[0];
        }

        #region M�todos de formata��o do boleto

        public override void FormataCodigoBarra(Boleto boleto)
        {
            string valorBoleto = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            valorBoleto = Utils.FormatCode(valorBoleto, 10);

            //Criada por AFK
            #region Carteira 11
            if (boleto.Carteira.Equals("11"))
            {
                if (boleto.Cedente.Convenio.ToString().Length == 6)
                {
                    if (boleto.TipoModalidade.Equals("21"))
                        boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                            Utils.FormatCode(Codigo.ToString(), 3),
                            boleto.Moeda,
                            FatorVencimento(boleto),
                            valorBoleto,
                            boleto.Cedente.Convenio,
                            boleto.NossoNumero,
                            "21");
                }
                else
                {
                    boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                        Utils.FormatCode(Codigo.ToString(), 3),
                        boleto.Moeda,
                        FatorVencimento(boleto),
                        valorBoleto,
                        boleto.NossoNumero,
                        boleto.Cedente.ContaBancaria.Agencia,
                        boleto.Cedente.ContaBancaria.Conta,
                        boleto.Carteira);
                }
            }
            #endregion Carteira 11

            #region Carteira 16
            if (boleto.Carteira.Equals("16"))
            {
                if (boleto.Cedente.Convenio.ToString().Length == 6)
                {
                    if (boleto.TipoModalidade.Equals("21"))
                        boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                            Utils.FormatCode(Codigo.ToString(), 3),
                            boleto.Moeda,
                            FatorVencimento(boleto),
                            valorBoleto,
                            boleto.Cedente.Convenio,
                            boleto.NossoNumero,
                            "21");
                }
                else
                {
                    boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                        Utils.FormatCode(Codigo.ToString(), 3),
                        boleto.Moeda,
                        FatorVencimento(boleto),
                        valorBoleto,
                        boleto.NossoNumero,
                        boleto.Cedente.ContaBancaria.Agencia,
                        boleto.Cedente.ContaBancaria.Conta,
                        boleto.Carteira);
                }
            }
            #endregion Carteira 16

            #region Carteira 17
            if (boleto.Carteira.Equals("17"))
            {
                if (boleto.Cedente.Convenio.ToString().Length == 7)
                {
                    boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                        Utils.FormatCode(Codigo.ToString(), 3),
                        boleto.Moeda,
                        FatorVencimento(boleto),
                        valorBoleto,
                        "000000",
                        boleto.NossoNumero,
                        Utils.FormatCode(LimparCarteira(boleto.Carteira), 2));
                }
                else if (boleto.Cedente.Convenio.ToString().Length == 6)
                {
                    boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                        Utils.FormatCode(Codigo.ToString(), 3),
                        boleto.Moeda,
                        FatorVencimento(boleto),
                        valorBoleto,
                        Strings.Mid(boleto.NossoNumero, 1, 11),
                        boleto.Cedente.ContaBancaria.Agencia,
                        boleto.Cedente.ContaBancaria.Conta,
                        boleto.Carteira);
                }
                else
                {
                    boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                        Utils.FormatCode(Codigo.ToString(), 3),
                        boleto.Moeda,
                        FatorVencimento(boleto),
                        valorBoleto,
                        boleto.NossoNumero,
                        boleto.Cedente.ContaBancaria.Agencia,
                        boleto.Cedente.ContaBancaria.Conta,
                        boleto.Carteira);
                }
            }
            #endregion Carteira 17

            #region Carteira 17-019
            if (boleto.Carteira.Equals("17-019"))
            {
                if (boleto.Cedente.Convenio.ToString().Length == 7)
                {
                    #region Especifica��o Conv�nio 7 posi��es
                    /*
                    Posi��o     Tamanho     Picture     Conte�do
                    01 a 03         03      9(3)            C�digo do Banco na C�mara de Compensa��o = �001�
                    04 a 04         01      9(1)            C�digo da Moeda = '9'
                    05 a 05         01      9(1)            DV do C�digo de Barras (Anexo 10)
                    06 a 09         04      9(04)           Fator de Vencimento (Anexo 8)
                    10 a 19         10      9(08)           V(2) Valor
                    20 a 25         06      9(6)            Zeros
                    26 a 42         17      9(17)           Nosso-N�mero, sem o DV
                    26 a 32         9       (7)             N�mero do Conv�nio fornecido pelo Banco (CCCCCCC)
                    33 a 42         9       (10)            Complemento do Nosso-N�mero, sem DV (NNNNNNNNNN)
                    43 a 44         02      9(2)            Tipo de Carteira/Modalidade de Cobran�a
                     */
                    #endregion Especifica��o Conv�nio 7 posi��es

                    boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                        Utils.FormatCode(Codigo.ToString(), 3),
                        boleto.Moeda,
                        FatorVencimento(boleto),
                        valorBoleto,
                        "000000",
                        boleto.NossoNumero,
                        Utils.FormatCode(LimparCarteira(boleto.Carteira), 2));
                }
                else if (boleto.Cedente.Convenio.ToString().Length == 6)
                {
                    boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                            Utils.FormatCode(Codigo.ToString(), 3),
                            boleto.Moeda,
                            FatorVencimento(boleto),
                            valorBoleto,
                            boleto.NossoNumero,
                            boleto.Cedente.ContaBancaria.Agencia,
                            boleto.Cedente.ContaBancaria.Conta,
                            LimparCarteira(boleto.Carteira));
                }
                else if (boleto.Cedente.Convenio.ToString().Length == 4)
                {
                    boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                        Utils.FormatCode(Codigo.ToString(), 3),
                        boleto.Moeda,
                        FatorVencimento(boleto),
                        valorBoleto,
                        boleto.NossoNumero,
                        boleto.Cedente.ContaBancaria.Agencia,
                        boleto.Cedente.ContaBancaria.Conta,
                        LimparCarteira(boleto.Carteira));
                }
            }
            #endregion Carteira 17-019

            #region Carteira 17-027
            if (boleto.Carteira.Equals("17-027"))
            {
                if (boleto.Cedente.Convenio.ToString().Length == 7)
                {
                    #region Especifica��o Conv�nio 7 posi��es
                    /*
                    Posi��o     Tamanho     Picture     Conte�do
                    01 a 03         03      9(3)            C�digo do Banco na C�mara de Compensa��o = �001�
                    04 a 04         01      9(1)            C�digo da Moeda = '9'
                    05 a 05         01      9(1)            DV do C�digo de Barras (Anexo 10)
                    06 a 09         04      9(04)           Fator de Vencimento (Anexo 8)
                    10 a 19         10      9(08)           V(2) Valor
                    20 a 25         06      9(6)            Zeros
                    26 a 42         17      9(17)           Nosso-N�mero, sem o DV
                    26 a 32         9       (7)             N�mero do Conv�nio fornecido pelo Banco (CCCCCCC)
                    33 a 42         9       (10)            Complemento do Nosso-N�mero, sem DV (NNNNNNNNNN)
                    43 a 44         02      9(2)            Tipo de Carteira/Modalidade de Cobran�a
                     */
                    #endregion Especifica��o Conv�nio 7 posi��es

                    boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                        Utils.FormatCode(Codigo.ToString(), 3),
                        boleto.Moeda,
                        FatorVencimento(boleto),
                        valorBoleto,
                        "000000",
                        boleto.NossoNumero,
                        Utils.FormatCode(LimparCarteira(boleto.Carteira), 2));
                }
                else if (boleto.Cedente.Convenio.ToString().Length == 6)
                {
                    boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                            Utils.FormatCode(Codigo.ToString(), 3),
                            boleto.Moeda,
                            FatorVencimento(boleto),
                            valorBoleto,
                            boleto.NossoNumero,
                            boleto.Cedente.ContaBancaria.Agencia,
                            boleto.Cedente.ContaBancaria.Conta,
                            LimparCarteira(boleto.Carteira));
                }
                else if (boleto.Cedente.Convenio.ToString().Length == 4)
                {
                    boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                        Utils.FormatCode(Codigo.ToString(), 3),
                        boleto.Moeda,
                        FatorVencimento(boleto),
                        valorBoleto,
                        boleto.NossoNumero,
                        boleto.Cedente.ContaBancaria.Agencia,
                        boleto.Cedente.ContaBancaria.Conta,
                        LimparCarteira(boleto.Carteira));
                }
            }
            #endregion Carteira 17-027

            #region Carteira 18
            if (boleto.Carteira.Equals("18"))
            {
                boleto.BancoCarteira.FormataCodigoBarra(boleto);
            }
            #endregion Carteira 18

            #region Carteira 18-019
            if (boleto.Carteira.Equals("18-019"))
            {
                if (boleto.Cedente.Convenio.ToString().Length == 7)
                {
                    #region Especifica��o Conv�nio 7 posi��es
                    /*
                    Posi��o     Tamanho     Picture     Conte�do
                    01 a 03         03      9(3)            C�digo do Banco na C�mara de Compensa��o = �001�
                    04 a 04         01      9(1)            C�digo da Moeda = '9'
                    05 a 05         01      9(1)            DV do C�digo de Barras (Anexo 10)
                    06 a 09         04      9(04)           Fator de Vencimento (Anexo 8)
                    10 a 19         10      9(08)           V(2) Valor
                    20 a 25         06      9(6)            Zeros
                    26 a 42         17      9(17)           Nosso-N�mero, sem o DV
                    26 a 32         9       (7)             N�mero do Conv�nio fornecido pelo Banco (CCCCCCC)
                    33 a 42         9       (10)            Complemento do Nosso-N�mero, sem DV (NNNNNNNNNN)
                    43 a 44         02      9(2)            Tipo de Carteira/Modalidade de Cobran�a
                     */
                    #endregion Especifica��o Conv�nio 7 posi��es

                    boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                        Utils.FormatCode(Codigo.ToString(), 3),
                        boleto.Moeda,
                        FatorVencimento(boleto),
                        valorBoleto,
                        "000000",
                        boleto.NossoNumero,
                        Utils.FormatCode(LimparCarteira(boleto.Carteira), 2));
                }
                else if (boleto.Cedente.Convenio.ToString().Length == 6)
                {
                    if (boleto.TipoModalidade.Equals("21"))
                        boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                            Utils.FormatCode(Codigo.ToString(), 3),
                            boleto.Moeda,
                            FatorVencimento(boleto),
                            valorBoleto,
                            boleto.Cedente.Convenio,
                            boleto.NossoNumero,
                            "21");
                    else
                        boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                            Utils.FormatCode(Codigo.ToString(), 3),
                            boleto.Moeda,
                            FatorVencimento(boleto),
                            valorBoleto,
                            boleto.NossoNumero,
                            boleto.Cedente.ContaBancaria.Agencia,
                            boleto.Cedente.ContaBancaria.Conta,
                            LimparCarteira(boleto.Carteira));
                }
                else if (boleto.Cedente.Convenio.ToString().Length == 4)
                {
                    boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                        Utils.FormatCode(Codigo.ToString(), 3),
                        boleto.Moeda,
                        FatorVencimento(boleto),
                        valorBoleto,
                        boleto.NossoNumero,
                        boleto.Cedente.ContaBancaria.Agencia,
                        boleto.Cedente.ContaBancaria.Conta,
                        LimparCarteira(boleto.Carteira));
                }
            }
            #endregion Carteira 18-019

            //Para atender o cliente Fiemg foi adptado no c�digo na varia��o 18-027 as varia��es 18-035 e 18-140
            #region Carteira 18-027
            if (boleto.Carteira.Equals("18-027"))
            {
                if (boleto.Cedente.Convenio.ToString().Length == 7)
                {
                    #region Especifica��o Conv�nio 7 posi��es
                    /*
                    Posi��o     Tamanho     Picture     Conte�do
                    01 a 03         03      9(3)            C�digo do Banco na C�mara de Compensa��o = �001�
                    04 a 04         01      9(1)            C�digo da Moeda = '9'
                    05 a 05         01      9(1)            DV do C�digo de Barras (Anexo 10)
                    06 a 09         04      9(04)           Fator de Vencimento (Anexo 8)
                    10 a 19         10      9(08)           V(2) Valor
                    20 a 25         06      9(6)            Zeros
                    26 a 42         17      9(17)           Nosso-N�mero, sem o DV
                    26 a 32         9       (7)             N�mero do Conv�nio fornecido pelo Banco (CCCCCCC)
                    33 a 42         9       (10)            Complemento do Nosso-N�mero, sem DV (NNNNNNNNNN)
                    43 a 44         02      9(2)            Tipo de Carteira/Modalidade de Cobran�a
                     */
                    #endregion Especifica��o Conv�nio 7 posi��es

                    boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                        Utils.FormatCode(Codigo.ToString(), 3),
                        boleto.Moeda,
                        FatorVencimento(boleto).ToString("0000"),
                        valorBoleto,
                        "000000",
                        boleto.NossoNumero,
                        Utils.FormatCode(LimparCarteira(boleto.Carteira), 2));
                }
                else if (boleto.Cedente.Convenio.ToString().Length == 6)
                {
                    if (boleto.TipoModalidade.Equals("21"))
                        boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                            Utils.FormatCode(Codigo.ToString(), 3),
                            boleto.Moeda,
                            FatorVencimento(boleto),
                            valorBoleto,
                            boleto.Cedente.Convenio,
                            boleto.NossoNumero,
                            "21");
                    else
                        boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                            Utils.FormatCode(Codigo.ToString(), 3),
                            boleto.Moeda,
                            FatorVencimento(boleto),
                            valorBoleto,
                            boleto.NossoNumero,
                            boleto.Cedente.ContaBancaria.Agencia,
                            boleto.Cedente.ContaBancaria.Conta,
                            LimparCarteira(boleto.Carteira));
                }
                else if (boleto.Cedente.Convenio.ToString().Length == 4)
                {
                    boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                        Utils.FormatCode(Codigo.ToString(), 3),
                        boleto.Moeda,
                        FatorVencimento(boleto),
                        valorBoleto,
                        boleto.NossoNumero,
                        boleto.Cedente.ContaBancaria.Agencia,
                        boleto.Cedente.ContaBancaria.Conta,
                        LimparCarteira(boleto.Carteira));
                }
            }
            #endregion Carteira 18-027

            #region Carteira 18-035
            if (boleto.Carteira.Equals("18-035"))
            {
                if (boleto.Cedente.Convenio.ToString().Length == 7)
                {
                    #region Especifica��o Conv�nio 7 posi��es
                    /*
                    Posi��o     Tamanho     Picture     Conte�do
                    01 a 03         03      9(3)            C�digo do Banco na C�mara de Compensa��o = �001�
                    04 a 04         01      9(1)            C�digo da Moeda = '9'
                    05 a 05         01      9(1)            DV do C�digo de Barras (Anexo 10)
                    06 a 09         04      9(04)           Fator de Vencimento (Anexo 8)
                    10 a 19         10      9(08)           V(2) Valor
                    20 a 25         06      9(6)            Zeros
                    26 a 42         17      9(17)           Nosso-N�mero, sem o DV
                    26 a 32         9       (7)             N�mero do Conv�nio fornecido pelo Banco (CCCCCCC)
                    33 a 42         9       (10)            Complemento do Nosso-N�mero, sem DV (NNNNNNNNNN)
                    43 a 44         02      9(2)            Tipo de Carteira/Modalidade de Cobran�a
                     */
                    #endregion Especifica��o Conv�nio 7 posi��es

                    boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                        Utils.FormatCode(Codigo.ToString(), 3),
                        boleto.Moeda,
                        FatorVencimento(boleto).ToString("0000"),
                        valorBoleto,
                        "000000",
                        boleto.NossoNumero,
                        Utils.FormatCode(LimparCarteira(boleto.Carteira), 2));
                }
                else if (boleto.Cedente.Convenio.ToString().Length == 6)
                {
                    if (boleto.TipoModalidade.Equals("21"))
                        boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                            Utils.FormatCode(Codigo.ToString(), 3),
                            boleto.Moeda,
                            FatorVencimento(boleto),
                            valorBoleto,
                            boleto.Cedente.Convenio,
                            boleto.NossoNumero,
                            "21");
                    else
                        boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                            Utils.FormatCode(Codigo.ToString(), 3),
                            boleto.Moeda,
                            FatorVencimento(boleto),
                            valorBoleto,
                            boleto.NossoNumero,
                            boleto.Cedente.ContaBancaria.Agencia,
                            boleto.Cedente.ContaBancaria.Conta,
                            LimparCarteira(boleto.Carteira));
                }
                else if (boleto.Cedente.Convenio.ToString().Length == 4)
                {
                    boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                        Utils.FormatCode(Codigo.ToString(), 3),
                        boleto.Moeda,
                        FatorVencimento(boleto),
                        valorBoleto,
                        boleto.NossoNumero,
                        boleto.Cedente.ContaBancaria.Agencia,
                        boleto.Cedente.ContaBancaria.Conta,
                        LimparCarteira(boleto.Carteira));
                }
            }
            #endregion Carteira 18-035

            #region Carteira 18-140
            if (boleto.Carteira.Equals("18-140"))
            {
                if (boleto.Cedente.Convenio.ToString().Length == 7)
                {
                    #region Especifica��o Conv�nio 7 posi��es
                    /*
                    Posi��o     Tamanho     Picture     Conte�do
                    01 a 03         03      9(3)            C�digo do Banco na C�mara de Compensa��o = �001�
                    04 a 04         01      9(1)            C�digo da Moeda = '9'
                    05 a 05         01      9(1)            DV do C�digo de Barras (Anexo 10)
                    06 a 09         04      9(04)           Fator de Vencimento (Anexo 8)
                    10 a 19         10      9(08)           V(2) Valor
                    20 a 25         06      9(6)            Zeros
                    26 a 42         17      9(17)           Nosso-N�mero, sem o DV
                    26 a 32         9       (7)             N�mero do Conv�nio fornecido pelo Banco (CCCCCCC)
                    33 a 42         9       (10)            Complemento do Nosso-N�mero, sem DV (NNNNNNNNNN)
                    43 a 44         02      9(2)            Tipo de Carteira/Modalidade de Cobran�a
                     */
                    #endregion Especifica��o Conv�nio 7 posi��es

                    boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                        Utils.FormatCode(Codigo.ToString(), 3),
                        boleto.Moeda,
                        FatorVencimento(boleto).ToString("0000"),
                        valorBoleto,
                        "000000",
                        boleto.NossoNumero,
                        Utils.FormatCode(LimparCarteira(boleto.Carteira), 2));
                }
                else if (boleto.Cedente.Convenio.ToString().Length == 6)
                {
                    if (boleto.TipoModalidade.Equals("21"))
                        boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                            Utils.FormatCode(Codigo.ToString(), 3),
                            boleto.Moeda,
                            FatorVencimento(boleto),
                            valorBoleto,
                            boleto.Cedente.Convenio,
                            boleto.NossoNumero,
                            "21");
                    else
                        boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                            Utils.FormatCode(Codigo.ToString(), 3),
                            boleto.Moeda,
                            FatorVencimento(boleto),
                            valorBoleto,
                            boleto.NossoNumero,
                            boleto.Cedente.ContaBancaria.Agencia,
                            boleto.Cedente.ContaBancaria.Conta,
                            LimparCarteira(boleto.Carteira));
                }
                else if (boleto.Cedente.Convenio.ToString().Length == 4)
                {
                    boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                        Utils.FormatCode(Codigo.ToString(), 3),
                        boleto.Moeda,
                        FatorVencimento(boleto),
                        valorBoleto,
                        boleto.NossoNumero,
                        boleto.Cedente.ContaBancaria.Agencia,
                        boleto.Cedente.ContaBancaria.Conta,
                        LimparCarteira(boleto.Carteira));
                }
            }
            #endregion Carteira 18-140

            #region Carteira 31
            if (boleto.Carteira.Equals("31"))
            {
                boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                    Utils.FormatCode(Codigo.ToString(), 3),
                    boleto.Moeda,
                    FatorVencimento(boleto),
                    valorBoleto,
                    boleto.NossoNumero,
                    boleto.Cedente.ContaBancaria.Agencia,
                    boleto.Cedente.ContaBancaria.Conta,
                    boleto.Carteira);
            }
            #endregion Carteira 31

            _dacBoleto = Mod11(boleto.CodigoBarra.Codigo, 9);

            boleto.CodigoBarra.Codigo = Strings.Left(boleto.CodigoBarra.Codigo, 4) + _dacBoleto + Strings.Right(boleto.CodigoBarra.Codigo, 39);
        }

        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            string cmplivre = string.Empty;
            string campo1 = string.Empty;
            string campo2 = string.Empty;
            string campo3 = string.Empty;
            string campo4 = string.Empty;
            string campo5 = string.Empty;
            long icampo5 = 0;
            int digitoMod = 0;

            /*
            Campos 1 (AAABC.CCCCX):
            A = C�digo do Banco na C�mara de Compensa��o �001�
            B = C�digo da moeda "9" (*)
            C = Posi��o 20 a 24 do c�digo de barras
            X = DV que amarra o campo 1 (M�dulo 10, contido no Anexo 7)
             */

            cmplivre = Strings.Mid(boleto.CodigoBarra.Codigo, 20, 25);

            campo1 = Strings.Left(boleto.CodigoBarra.Codigo, 4) + Strings.Mid(cmplivre, 1, 5);
            digitoMod = Mod10(campo1);
            campo1 = campo1 + digitoMod.ToString();
            campo1 = Strings.Mid(campo1, 1, 5) + "." + Strings.Mid(campo1, 6, 5);
            /*
            Campo 2 (DDDDD.DDDDDY)
            D = Posi��o 25 a 34 do c�digo de barras
            Y = DV que amarra o campo 2 (M�dulo 10, contido no Anexo 7)
             */
            campo2 = Strings.Mid(cmplivre, 6, 10);
            digitoMod = Mod10(campo2);
            campo2 = campo2 + digitoMod.ToString();
            campo2 = Strings.Mid(campo2, 1, 5) + "." + Strings.Mid(campo2, 6, 6);


            /*
            Campo 3 (EEEEE.EEEEEZ)
            E = Posi��o 35 a 44 do c�digo de barras
            Z = DV que amarra o campo 3 (M�dulo 10, contido no Anexo 7)
             */
            campo3 = Strings.Mid(cmplivre, 16, 10);
            digitoMod = Mod10(campo3);
            campo3 = campo3 + digitoMod;
            campo3 = Strings.Mid(campo3, 1, 5) + "." + Strings.Mid(campo3, 6, 6);

            /*
            Campo 4 (K)
            K = DV do C�digo de Barras (M�dulo 11, contido no Anexo 10)
             */
            campo4 = Strings.Mid(boleto.CodigoBarra.Codigo, 5, 1);

            /*
            Campo 5 (UUUUVVVVVVVVVV)
            U = Fator de Vencimento ( Anexo 10)
            V = Valor do T�tulo (*)
             */
            icampo5 = Convert.ToInt64(Strings.Mid(boleto.CodigoBarra.Codigo, 6, 14));

            if (icampo5 == 0)
                campo5 = "000";
            else
                campo5 = icampo5.ToString();

            boleto.CodigoBarra.LinhaDigitavel = campo1 + " " + campo2 + " " + campo3 + " " + campo4 + " " + campo5;
        }

        /// <summary>
        /// Formata o nosso n�mero para ser mostrado no boleto.
        /// </summary>
        /// <remarks>
        /// �ltima a atualiza��o por Transis em 26/09/2011
        /// </remarks>
        /// <param name="boleto"></param>
        public override void FormataNossoNumero(Boleto boleto)
        {
            if (boleto.Cedente.Convenio.ToString().Length == 6) //somente monta o digito verificador no nosso numero se o convenio tiver 6 posi��es
            {
                switch (boleto.Carteira)
                {
                    case "18-019":
                        boleto.NossoNumero = string.Format("{0}/{1}-{2}", LimparCarteira(boleto.Carteira), boleto.NossoNumero, Mod11BancoBrasil(boleto.NossoNumero));
                        return;
                }
            }

            switch (boleto.Carteira)
            {
                case "17-019":
                case "17-027":
                case "18-019":
                    boleto.NossoNumero = string.Format("{0}/{1}", LimparCarteira(boleto.Carteira), boleto.NossoNumero);
                    return;
                case "31":
                    boleto.NossoNumero = string.Format("{0}{1}", Utils.FormatCode(boleto.Cedente.Convenio.ToString(), 7), boleto.NossoNumero);
                    return;
            }

            if (boleto.BancoCarteira != null)
                boleto.BancoCarteira.FormataNossoNumero(boleto);
            else
                boleto.NossoNumero = string.Format("{0}", boleto.NossoNumero);
        }


        public override void FormataNumeroDocumento(Boleto boleto)
        {
        }

        # endregion

        #region M�todos de gera��o do arquivo remessa - Gen�ricos
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
                        header = "";
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return header;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do HEADER DO LOTE do arquivo de REMESSA.", ex);
            }
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

                base.GerarHeaderRemessa(numeroConvenio, cedente, tipoArquivo, numeroArquivoRemessa);

                switch (tipoArquivo)
                {
                    case TipoArquivo.CNAB240:
                        _header = GerarHeaderRemessaCNAB240(cedente, numeroArquivoRemessa);
                        break;
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
                throw new Exception("Erro durante a gera��o do HEADER do arquivo de REMESSA.", ex);
            }
        }
        /// <summary>
        /// Efetua as Valida��es dentro da classe Boleto, para garantir a gera��o da remessa
        /// </summary>
        public override bool ValidarRemessa(TipoArquivo tipoArquivo, string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            bool vRetorno = true;
            string vMsg = string.Empty;
            //            
            switch (tipoArquivo)
            {
                case TipoArquivo.CNAB240:
                    vRetorno = ValidarRemessaCNAB240(numeroConvenio, banco, cedente, boletos, numeroArquivoRemessa, out vMsg);
                    break;
                case TipoArquivo.CNAB400:
                    vRetorno = ValidarRemessaCNAB400(numeroConvenio, banco, cedente, boletos, numeroArquivoRemessa, out vMsg);
                    break;
                case TipoArquivo.Outro:
                    throw new Exception("Tipo de arquivo inexistente.");
            }
            //
            mensagem = vMsg;
            return vRetorno;
        }
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
                        _detalhe = GerarDetalheRemessaCNAB240(boleto, numeroRegistro, tipoArquivo);
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
                throw new Exception("Erro durante a gera��o do DETALHE arquivo de REMESSA.", ex);
            }
        }
        public override string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio)
        {
            try
            {
                string _segmentoP;
                string _nossoNumero;

                _segmentoP = "00100013";
                _segmentoP += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);
                _segmentoP += "P 01";
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true);
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoAgencia, 1, 1, '0', 0, true, true, true);
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 12, 12, '0', 0, true, true, true);
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true);
                // D�gito verificador  da Ag�ncia/Conta.  Campo n�o tratado pelo BB.  Informar ESPA�O ou ZERO.
                _segmentoP += " "; // jefhtavares O banco n�o aceita mais esse campo como 0 (zero), o campo dever� ser enviado em branco

                //=====================================================================================================
                //Ajustes efetuados de acordo com manual Julho/2011 - Retirado por jsoda - em 11/05/2012
                //
                //int totalCaracteres = numeroConvenio.Length - 9;
                //_segmentoP += numeroConvenio.Substring(0, totalCaracteres);

                //_nossoNumero = Utils.FitStringLength(boleto.NumeroDocumento, 10, 10, '0', 0, true, true, true);
                //int _total = numeroConvenio.Substring(0, totalCaracteres).Length + _nossoNumero.Length;
                //int subtotal = 0;
                //subtotal = 20 - _total;
                //string _comnplemento = new string(' ', subtotal);
                //_segmentoP += _nossoNumero;
                //_segmentoP += _comnplemento;
                //=====================================================================================================

                switch (boleto.Cedente.Convenio.ToString().Length)
                {
                    case 4:
                        // Se conv�nio de 4 posi��es - normalmente carteira 17 - (0001 � 9999), informar NossoNumero com 11 caracteres, com DV, sendo:
                        // 4 posi��es do n� do conv�nio e 7 posi��es do n� de controle (n� do documento) e DV.
                        _nossoNumero = string.Format("{0}{1}{2}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 7), Mod11BancoBrasil(boleto.NossoNumero));
                        break;
                    case 6:
                        // Se conv�nio de 6 posi��es (acima de 10.000 � 999.999), informar NossoNumero com 11 caracteres + DV, sendo:
                        // 6 posi��es do n� do conv�nio e 5 posi��es do n� de controle (n� do documento) e DV do nosso numero.
                        if (boleto.NossoNumero.Length != 11)
                            _nossoNumero = string.Format("{0}{1}{2}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 5), Mod11BancoBrasil(boleto.NossoNumero));
                        else
                            _nossoNumero = boleto.NossoNumero + Mod11BancoBrasil(boleto.NossoNumero);
                        break;
                    case 7:
                        // Se conv�nio de 7 posi��es (acima de 1.000.000 � 9.999.999), informar NossoNumero com 17 caracteres, sem DV, sendo:
                        // 7 posi��es do n� do conv�nio e 10 posi��es do n� de controle (n� do documento)
                        //_nossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 7));
                        //ALTERADO POR MARCELHSOUZA EM 28/03/2013
                        if (boleto.NossoNumero.Length != 17)
                            _nossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 10)); //sao 10 digitos pro no. sequencial
                        else
                            _nossoNumero = boleto.NossoNumero;
                        break;
                    default:
                        throw new Exception("Posi��es do n� de conv�nio deve ser 4, 6 ou 7.");
                }

                // Importante: Nosso n�mero, alinhar � esquerda com brancos � direita (conforme manual)
                _segmentoP += Utils.FitStringLength(_nossoNumero, 20, 20, ' ', 0, true, true, false);

                // Informar 1 � para carteira 11/12 na modalidade Simples; 
                // 2 ou 3 � para carteira 11/17 modalidade Vinculada/Caucionada e carteira 31; 
                // 4 � para carteira 11/17 modalidade Descontada e carteira 51; 
                // 7 � para carteira 17 modalidade Simples.
                if (boleto.Carteira.Equals("17-019") || boleto.Carteira.Equals("17-027"))
                    _segmentoP += "7";
                else
                    _segmentoP += "0";

                // Campo n�o tratado pelo BB. Forma de cadastramento do t�tulo no banco. Pode ser branco/espa�o, 0, 1=cobran�a registrada, 2=sem registro.
                _segmentoP += "1";
                // Campo n�o tratado pelo BB. Tipo de documento. Pode ser branco, 0, 1=tradicional, 2=escritural.
                _segmentoP += "1";
                // Campo n�o tratado pelo BB. Identifica��o de emiss�o do boleto. Pode ser branco/espa�o, 0, ou:
                // No caso de carteira 11/12/31/51, utilizar c�digo 1 � Banco emite, 4 � Banco reemite, 5 � Banco n�o reemite, por�m nestes dois �ltimos casos, 
                // o c�digo de Movimento Remessa (posi��es 16 a 17) deve ser c�digo '31'.
                // Altera��o de outros dados (para t�tulos que j� est�o registrados no Banco do Brasil). 
                // No caso de carteira 17, podem ser usados os c�digos: 1 � Banco emite, 2 � Cliente emite, 3 � Banco pre-emite e cliente complementa, 6 � Cobran�a sem papel. 
                // Permite ainda, c�digos 4 � Banco reemite e 5 � Banco n�o reemite, por�m o c�digo de Movimento Remessa (posi��es 16 a 17) deve ser c�digo '31' 
                // Altera��o de outros dados (para t�tulos que j� est�o registrados no Banco do Brasil). 
                // Obs.: Quando utilizar c�digo, informar de acordo com o que foi cadastrado para a carteira junto ao Banco do Brasil, consulte seu gerente de relacionamento.
                _segmentoP += "2";
                // Campo n�o tratado pelo BB. Informar 'branco' (espa�o) OU zero ou de acordo com a carteira e quem far� a distribui��o dos bloquetos. 
                // Para carteira 11/12/31/51 utilizar c�digo 1� Banco distribui. 
                // Para carteira 17, pode ser utilizado c�digo 1 � Banco distribui, 2 � Cliente distribui ou 3 � Banco envia e-mail (nesse caso complementar com registro S), 
                // de acordo com o que foi cadastrado para a carteira junto ao Banco do Brasil, consulte seu gerente de relacionamento.
                _segmentoP += "2";
                _segmentoP += Utils.FitStringLength(boleto.NumeroDocumento, 15, 15, ' ', 0, true, true, false);
                _segmentoP += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, ' ', 0, true, true, false);
                _segmentoP += Utils.FitStringLength(boleto.ValorBoleto.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true);
                _segmentoP += "00000 ";
                _segmentoP += Utils.FitStringLength(boleto.EspecieDocumento.Codigo.ToString(), 2, 2, '0', 0, true, true, true);
                _segmentoP += "N";
                _segmentoP += Utils.FitStringLength(boleto.DataDocumento.ToString("ddMMyyyy"), 8, 8, ' ', 0, true, true, false);

                if (boleto.JurosMora > 0)
                {
                    _segmentoP += "1";
                    _segmentoP += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false);
                    _segmentoP += Utils.FitStringLength(boleto.JurosMora.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true);
                }
                else if (boleto.JurosPermanente)
                {
                    _segmentoP += "1";
                    _segmentoP += "00000000";
                    _segmentoP += "000000000000000";
                }
                else
                {
                    _segmentoP += "3";
                    _segmentoP += "00000000";
                    _segmentoP += "000000000000000";
                }

                if (boleto.ValorDesconto > 0)
                {
                    _segmentoP += "1";
                    _segmentoP += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false);
                    _segmentoP += Utils.FitStringLength(boleto.ValorDesconto.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true);
                }
                else
                    _segmentoP += "000000000000000000000000";

                _segmentoP += "000000000000000";
                _segmentoP += "000000000000000";
                _segmentoP += Utils.FitStringLength(boleto.NumeroDocumento, 25, 25, ' ', 0, true, true, false);

                //alterado por marcelhsouza em 28/03/2013
                //O Banco do Brasil trata somente os c�digos '1' � Protestar dias corridos, '2' � Protestar dias �teis, e '3' � N�o protestar.
                string codigo_protesto = "3";
                string dias_protesto = "00";

                foreach (var instrucao in boleto.Instrucoes)
                {
                    switch ((EnumInstrucoes_BancoBrasil)instrucao.Codigo)
                    {
                        case EnumInstrucoes_BancoBrasil.ProtestarAposNDiasCorridos:
                            codigo_protesto = "1";
                            dias_protesto = Utils.FitStringLength(instrucao.QuantidadeDias.ToString(), 2, 2, '0', 0, true, true, true); //Para c�digo '1' � � poss�vel, de 6 a 29 dias
                            break;
                        case EnumInstrucoes_BancoBrasil.ProtestarAposNDiasUteis:
                            codigo_protesto = "2";
                            dias_protesto = Utils.FitStringLength(instrucao.QuantidadeDias.ToString(), 2, 2, '0', 0, true, true, true); //Para c�digo '2' � � poss�vel, 3�, 4� ou 5� dia �til
                            break;
                        case EnumInstrucoes_BancoBrasil.NaoProtestar:
                            codigo_protesto = "3";
                            dias_protesto = "00";
                            break;
                        default:
                            /*codigo_protesto = "3"; 
                            dias_protesto = "00";*/
                            break;
                        /*
                         * Bloco do "default" comentado por J�ferson, jefhtavares em 26/11 se fossem adicionadas mais de duas instru��es e a instru��o de protesto fosse a �ltima a mesma n�o iria aparecer no arquivo
                         */
                    }
                }

                _segmentoP += codigo_protesto;
                _segmentoP += dias_protesto;

                /*if (boleto.Instrucoes.Count > 1 && boleto.Instrucoes[0].QuantidadeDias > 0)
                {
                    _segmentoP += "2";
                    _segmentoP += Utils.FitStringLength(boleto.Instrucoes[0].QuantidadeDias.ToString(), 2, 2, '0', 0, true, true, true);
                }
                else
                    _segmentoP += "300";*/

                //alterado por marcelhsouza em 28/03/2013
                //38.3P C�digo para Baixa/Devolu��o 224 224 1 - Num�rico C028 Campo n�o tratado pelo sistema. Informar 'zeros'. O sistema considera a informa��o que foi cadastrada na sua carteira junto ao Banco do Brasil.
                _segmentoP += "0000090000000000 ";

                //_segmentoP += "2000090000000000 ";

                _segmentoP = Utils.SubstituiCaracteresEspeciais(_segmentoP);

                return _segmentoP;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do SEGMENTO P DO DETALHE do arquivo de REMESSA.", ex);
            }
        }
        public override string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _zeros16 = new string('0', 16);
                string _brancos28 = new string(' ', 28);
                string _brancos40 = new string(' ', 40);

                string _segmentoQ;

                _segmentoQ = "00100013";
                _segmentoQ += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);
                _segmentoQ += "Q 01";

                if (boleto.Sacado.CPFCNPJ.Length <= 11)
                    _segmentoQ += "1";
                else
                    _segmentoQ += "2";

                _segmentoQ += Utils.FitStringLength(boleto.Sacado.CPFCNPJ, 15, 15, '0', 0, true, true, true);
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Nome.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.End.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.Bairro.TrimStart(' '), 15, 15, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.CEP, 8, 8, ' ', 0, true, true, false).ToUpper(); ;
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.Cidade.TrimStart(' '), 15, 15, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.UF, 2, 2, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += _zeros16;
                _segmentoQ += _brancos40;
                _segmentoQ += "000";
                _segmentoQ += _brancos28;

                _segmentoQ = Utils.SubstituiCaracteresEspeciais(_segmentoQ);

                return _segmentoQ;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do SEGMENTO Q DO DETALHE do arquivo de REMESSA.", ex);
            }
        }
        public override string GerarDetalheSegmentoRRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _brancos110 = new string(' ', 110);
                string _brancos9 = new string(' ', 9);

                string _segmentoR;

                _segmentoR = "00100013";
                _segmentoR += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);
                _segmentoR += "R 01";
                // Desconto 2
                _segmentoR += "000000000000000000000000"; //24 zeros
                // Desconto 3
                _segmentoR += "000000000000000000000000"; //24 zeros

                if (boleto.PercMulta > 0)
                {
                    // C�digo da multa 2 - percentual
                    _segmentoR += "2";
                }
                else if (boleto.ValorMulta > 0)
                {
                    // C�digo da multa 1 - valor fixo
                    _segmentoR += "1";
                }
                else
                {
                    // C�digo da multa 0 - sem multa
                    _segmentoR += "0";
                }

                _segmentoR += Utils.FitStringLength(boleto.DataMulta.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false);
                _segmentoR += Utils.FitStringLength(boleto.ValorMulta.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true);
                _segmentoR += _brancos110;
                _segmentoR += "0000000000000000"; //16 zeros
                _segmentoR += " "; //1 branco
                _segmentoR += "000000000000"; //12 zeros
                _segmentoR += "  "; //2 brancos
                _segmentoR += "0"; //1 zero
                _segmentoR += _brancos9;

                _segmentoR = Utils.SubstituiCaracteresEspeciais(_segmentoR);

                return _segmentoR;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do SEGMENTO R DO DETALHE do arquivo de REMESSA.", ex);
            }
        }
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
                        _trailer = GerarTrailerRemessa400(numeroRegistro, 0);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return _trailer;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do TRAILER do arquivo de REMESSA.", ex);
            }
        }
        public override string GerarTrailerLoteRemessa(int numeroRegistro)
        {
            try
            {
                //string _brancos217 = new string(' ', 217);
                string _zeros = new string('0', 92);
                string _brancos = new string(' ', 125);

                string _trailerLote;

                _trailerLote = "00100015         ";
                _trailerLote += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);
                _trailerLote += _zeros;
                _trailerLote += _brancos;

                //_trailerLote += _brancos217;

                _trailerLote = Utils.SubstituiCaracteresEspeciais(_trailerLote);

                return _trailerLote;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do TRAILER de lote do arquivo de REMESSA.", ex);
            }
        }
        public override string GerarTrailerArquivoRemessa(int numeroRegistro)
        {
            try
            {
                string _brancos205 = new string(' ', 205);

                string _trailerArquivo;

                _trailerArquivo = "00199999         000001";
                _trailerArquivo += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);
                _trailerArquivo += "000000";
                _trailerArquivo += _brancos205;

                _trailerArquivo = Utils.SubstituiCaracteresEspeciais(_trailerArquivo);

                return _trailerArquivo;
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            throw new NotImplementedException("Fun��o n�o implementada.");
        }
        #endregion

        #region CNAB240 - Espec�ficos
        public bool ValidarRemessaCNAB240(string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            string vMsg = string.Empty;
            mensagem = vMsg;
            return true;
            //throw new NotImplementedException("Fun��o n�o implementada.");
        }
        private string GerarHeaderLoteRemessaCNAB240(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                string _brancos40 = new string(' ', 40);
                string _brancos33 = new string(' ', 33);
                string _headerLote;

                //alterado por marcelhsouza em 28/03/2013
                //_headerLote = "00100011R0100";
                _headerLote = "00100011R01  "; //posi��es 12-13 s�o espa�os e n�o zeros
                // Campo n�o criticado pelo sistema, informar ZEROS ou n� da vers�o do layout do arquivo que foi utilizado
                // para a formata��o dos campos.
                // Como n�o sei onde pegar esse n�, deixei como padr�o.
                //alterado por marcelhsouza em 28/03/2013
                //_headerLote += "020";
                _headerLote += "000"; //no header do arquivo esta 000, entao aqui deve-se por 000, poderia ser 020 SE no header do arquivo tivesse 030
                _headerLote += " ";

                if (cedente.CPFCNPJ.Length <= 11)
                    _headerLote += "1";
                else
                    _headerLote += "2";

                _headerLote += Utils.FitStringLength(cedente.CPFCNPJ, 15, 15, '0', 0, true, true, true);
                _headerLote += Utils.FitStringLength(numeroConvenio, 9, 9, '0', 0, true, true, true);
                _headerLote += "0014";
                // O C�digo da carteira � dividida em 2 partes:
                // - n� da carteira 9(02)
                // - varia��o (se houver) 9(03)
                if (cedente.Carteira.Length == 2)
                    _headerLote += cedente.Carteira.ToString() + "000  ";
                else
                    _headerLote += cedente.Carteira.Replace("-", "") + "  ";

                _headerLote += Utils.FitStringLength(cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true);
                _headerLote += Utils.FitStringLength(cedente.ContaBancaria.DigitoAgencia, 1, 1, '0', 0, true, true, true);
                _headerLote += Utils.FitStringLength(cedente.ContaBancaria.Conta, 12, 12, '0', 0, true, true, true);
                _headerLote += Utils.FitStringLength(cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true);
                // D�gito verificador  da Ag�ncia/Conta.  Campo n�o tratado pelo BB.  Informar ESPA�O ou ZERO.
                _headerLote += " "; // jefhtavares O banco n�o aceita mais esse campo como 0 (zero), o campo dever� ser enviado em branco
                _headerLote += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false);
                _headerLote += _brancos40;
                _headerLote += _brancos40;
                // Campo n�o tratado pelo BB. Sugerem utilizar n� sequencial para controle da empresa.  N�o especifica se � controle de arquivo.
                // Em todo caso, coloquei o n� sequencial do arquivo remessa.
                _headerLote += Utils.FitStringLength(numeroArquivoRemessa.ToString(), 8, 8, '0', 0, true, true, true);
                _headerLote += DateTime.Now.ToString("ddMMyyyy");
                _headerLote += "00000000";
                _headerLote += _brancos33;

                _headerLote = Utils.SubstituiCaracteresEspeciais(_headerLote);

                return _headerLote;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do HEADER DE LOTE do arquivo de REMESSA.", ex);
            }
        }
        public string GerarHeaderRemessaCNAB240(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                string _brancos20 = new string(' ', 20);
                string _brancos10 = new string(' ', 10);
                string _header;

                _header = "00100000         ";
                if (cedente.CPFCNPJ.Length <= 11)
                    _header += "1";
                else
                    _header += "2";
                _header += Utils.FitStringLength(cedente.CPFCNPJ, 14, 14, '0', 0, true, true, true);
                _header += _brancos20;
                _header += Utils.FitStringLength(cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true);
                _header += Utils.FitStringLength(cedente.ContaBancaria.DigitoAgencia, 1, 1, ' ', 0, true, true, false);
                _header += Utils.FitStringLength(cedente.ContaBancaria.Conta, 12, 12, '0', 0, true, true, true);
                _header += Utils.FitStringLength(cedente.ContaBancaria.DigitoConta, 1, 1, ' ', 0, true, true, false);
                _header += " "; // D�GITO VERIFICADOR DA AG./CONTA
                _header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false);
                _header += Utils.FitStringLength("BANCO DO BRASIL S.A.", 30, 30, ' ', 0, true, true, false);
                _header += _brancos10;
                _header += "1";
                _header += DateTime.Now.ToString("ddMMyyyy");
                _header += DateTime.Now.ToString("hhMMss");
                // N�MERO SEQUENCIAL DO ARQUIVO *EVOLUIR UM N�MERO A CADA HEADER DE ARQUIVO
                //_header += "000001";
                //alterado por MarcelHenrique 13/04/2013 deve-se ser sequencia a numeracao do arquivo incrementando a cada envio
                _header += Utils.FitStringLength(numeroArquivoRemessa.ToString(), 6, 6, '0', 0, true, true, true);
                // Campo n�o criticado pelo sistema, informar ZEROS ou n� da vers�o do layout do arquivo que foi utilizado
                // para a formata��o dos campos.
                // Como n�o sei onde pegar esse n�, deixei como padr�o.
                _header += "000";
                _header += "00000";
                _header += _brancos20;
                _header += _brancos20;
                _header += _brancos10;
                _header += "    ";
                _header += "000  ";
                _header += _brancos10;

                _header = Utils.SubstituiCaracteresEspeciais(_header);

                return _header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do HEADER DE ARQUIVO do arquivo de REMESSA.", ex);
            }
        }
        public string GerarDetalheRemessaCNAB240(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException("Fun��o n�o implementada.");
        }
        public string GerarTrailerRemessa240()
        {
            throw new NotImplementedException("Fun��o n�o implementada.");
        }
        public override DetalheSegmentoTRetornoCNAB240 LerDetalheSegmentoTRetornoCNAB240(string registro)
        {

            string _Controle_numBanco = registro.Substring(0, 3); //01
            string _Controle_lote = registro.Substring(3, 7); //02
            string _Controle_regis = registro.Substring(7, 1); //03
            string _Servico_numRegis = registro.Substring(8, 5); //04
            string _Servico_segmento = registro.Substring(13, 1); //05
            string _cnab06 = registro.Substring(14, 1); //06
            string _Servico_codMov = registro.Substring(15, 2); //07
            string _ccAgenciaCodigo = registro.Substring(17, 5); //08
            string _ccAgenciaDv = registro.Substring(22, 1); //09
            string _ccContaNumero = registro.Substring(23, 12); //10
            string _ccContaDv = registro.Substring(35, 1); //11
            string _ccDv = registro.Substring(36, 1); //12
            string _outUsoExclusivo = registro.Substring(37, 9); //13
            string _outNossoNumero = registro.Substring(37, 20); //14
            string _outCarteira = registro.Substring(57, 1); //15
            string _outNumeroDocumento = registro.Substring(58, 15); //16
            string _outVencimento = registro.Substring(73, 8); //17
            string _outValor = registro.Substring(81, 15); //18
            string _outUf = registro.Substring(96, 2); //19
            string _outBanco = registro.Substring(98, 1); //20
            string _outCodCedente = registro.Substring(99, 5); //21
            string _outDvCedente = registro.Substring(104, 1); //22
            string _outNomeCedente = registro.Substring(105, 25); //23
            string _outCodMoeda = registro.Substring(130, 2); //24
            string _sacadoInscricaoTipo = registro.Substring(132, 1); //25
            string _sacadoInscricaoNumero = registro.Substring(133, 15); //26
            string _sacadoNome = registro.Substring(148, 40); //27
            string _cnab28 = registro.Substring(188, 10); //28
            string _valorTarifasCustas = registro.Substring(198, 13); //29
            string _motivoCobraca = registro.Substring(213, 10); //30
            string _cnab31 = registro.Substring(223, 17); //31

            try
            {
                /* 05 */
                if (!registro.Substring(13, 1).Equals(@"T"))
                {
                    throw new Exception("Registro inv�lida. O detalhe n�o possu� as caracter�sticas do segmento T.");
                }
                DetalheSegmentoTRetornoCNAB240 segmentoT = new DetalheSegmentoTRetornoCNAB240(registro);
                segmentoT.CodigoBanco = Convert.ToInt32(registro.Substring(0, 3)); //01
                segmentoT.idCodigoMovimento = Convert.ToInt32(registro.Substring(15, 2)); //07
                segmentoT.Agencia = Convert.ToInt32(registro.Substring(17, 5)); //08
                segmentoT.DigitoAgencia = registro.Substring(22, 1); //09
                segmentoT.Conta = Convert.ToInt64(registro.Substring(23, 12)); //10
                segmentoT.DigitoConta = registro.Substring(35, 1); //11
                segmentoT.DACAgenciaConta = (String.IsNullOrEmpty(registro.Substring(36, 1).Trim())) ? 0 : Convert.ToInt32(registro.Substring(36, 1)); //12
                segmentoT.NossoNumero = registro.Substring(37, 20); //14
                segmentoT.CodigoCarteira = Convert.ToInt32(registro.Substring(57, 1)); //15
                segmentoT.NumeroDocumento = registro.Substring(58, 15); //16
                segmentoT.DataVencimento = registro.Substring(73, 8).ToString() == "00000000" ? DateTime.Now : DateTime.ParseExact(registro.Substring(73, 8), "ddMMyyyy", CultureInfo.InvariantCulture); //17
                segmentoT.ValorTitulo = Convert.ToDecimal(registro.Substring(81, 15)) / 100; //18
                segmentoT.IdentificacaoTituloEmpresa = registro.Substring(105, 25); //23
                segmentoT.TipoInscricao = Convert.ToInt32(registro.Substring(132, 1)); //25
                segmentoT.NumeroInscricao = registro.Substring(133, 15); //26
                segmentoT.NomeSacado = registro.Substring(148, 40); //27
                segmentoT.ValorTarifas = Convert.ToDecimal(registro.Substring(198, 15)) / 100; //29
                //J�ferson (jefhtavares) em 12/12/2013 - O campo Valor Tarifas � composto de 15 posi��es (199-213) e n�o de 13
                segmentoT.CodigoRejeicao = registro.Substring(213, 1) == "A" ? registro.Substring(214, 9) : registro.Substring(213, 10); //30
                segmentoT.UsoFebraban = _cnab31;

                return segmentoT;
            }
            catch (Exception ex)
            {
                //TrataErros.Tratar(ex);
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO T.", ex);
            }
        }
        public override DetalheSegmentoURetornoCNAB240 LerDetalheSegmentoURetornoCNAB240(string registro)
        {
            string _Controle_numBanco = registro.Substring(0, 3); //01
            string _Controle_lote = registro.Substring(3, 7); //02
            string _Controle_regis = registro.Substring(7, 1); //03
            string _Servico_numRegis = registro.Substring(8, 5); //04
            string _Servico_segmento = registro.Substring(13, 1); //05
            string _cnab06 = registro.Substring(14, 1); //06
            string _Servico_codMov = registro.Substring(15, 2); //07
            string _dadosTituloAcrescimo = registro.Substring(17, 15); //08
            string _dadosTituloValorDesconto = registro.Substring(32, 15); //09
            string _dadosTituloValorAbatimento = registro.Substring(47, 15); //10
            string _dadosTituloValorIof = registro.Substring(62, 15); //11
            string _dadosTituloValorPago = registro.Substring(76, 15); //12
            string _dadosTituloValorCreditoBruto = registro.Substring(92, 15); //13
            string _outDespesas = registro.Substring(107, 15); //14
            string _outPerCredEntRecb = registro.Substring(122, 5); //15
            string _outBanco = registro.Substring(127, 10); //16
            string _outDataOcorrencia = registro.Substring(137, 8); //17
            string _outDataCredito = registro.Substring(145, 8); //18
            string _cnab19 = registro.Substring(153, 87); //19


            try
            {

                if (!registro.Substring(13, 1).Equals(@"U"))
                {
                    throw new Exception("Registro inv�lida. O detalhe n�o possu� as caracter�sticas do segmento U.");
                }

                var segmentoU = new DetalheSegmentoURetornoCNAB240(registro);
                segmentoU.Servico_Codigo_Movimento_Retorno = Convert.ToDecimal(registro.Substring(15, 2)); //07.3U|Servi�o|C�d. Mov.|C�digo de Movimento Retorno
                segmentoU.JurosMultaEncargos = Convert.ToDecimal(registro.Substring(17, 15)) / 100;
                segmentoU.ValorDescontoConcedido = Convert.ToDecimal(registro.Substring(32, 15)) / 100;
                segmentoU.ValorAbatimentoConcedido = Convert.ToDecimal(registro.Substring(47, 15)) / 100;
                segmentoU.ValorIOFRecolhido = Convert.ToDecimal(registro.Substring(62, 15)) / 100;
                segmentoU.ValorOcorrenciaSacado = segmentoU.ValorPagoPeloSacado = Convert.ToDecimal(registro.Substring(77, 15)) / 100;
                segmentoU.ValorLiquidoASerCreditado = Convert.ToDecimal(registro.Substring(92, 15)) / 100;
                segmentoU.ValorOutrasDespesas = Convert.ToDecimal(registro.Substring(107, 15)) / 100;
                segmentoU.ValorOutrosCreditos = Convert.ToDecimal(registro.Substring(122, 15)) / 100;
                segmentoU.DataOcorrencia = segmentoU.DataOcorrencia = DateTime.ParseExact(registro.Substring(137, 8), "ddMMyyyy", CultureInfo.InvariantCulture);
                segmentoU.DataCredito = registro.Substring(145, 8).ToString() == "00000000" ? segmentoU.DataOcorrencia : DateTime.ParseExact(registro.Substring(145, 8), "ddMMyyyy", CultureInfo.InvariantCulture);
                segmentoU.CodigoOcorrenciaSacado = registro.Substring(153, 4);

                return segmentoU;
            }
            catch (Exception ex)
            {
                //TrataErros.Tratar(ex);
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO U.", ex);
            }
        }
        #endregion

        internal static string Mod11BancoBrasil(string value)
        {
            #region Trecho do manual DVMD11.doc
            /* 
            Multiplicar cada algarismo que comp�e o n�mero pelo seu respectivo multiplicador (PESO).
            Os multiplicadores(PESOS) variam de 9 a 2.
            O primeiro d�gito da direita para a esquerda dever� ser multiplicado por 9, o segundo por 8 e assim sucessivamente.
            O resultados das multiplica��es devem ser somados:
            72+35+24+27+4+9+8=179
            O total da soma dever� ser dividido por 11:
            179 / 11=16
            RESTO=3

            Se o resto da divis�o for igual a 10 o D.V. ser� igual a X. 
            Se o resto da divis�o for igual a 0 o D.V. ser� igual a 0.
            Se o resto for menor que 10, o D.V.  ser� igual ao resto.

            No exemplo acima, o d�gito verificador ser� igual a 3
            */
            #endregion

            /* d - D�gito
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */

            string d;
            int s = 0, p = 9, b = 2;

            for (int i = value.Length - 1; i >= 0; i--)
            {
                s += (int.Parse(value[i].ToString()) * p);
                if (p == b)
                    p = 9;
                else
                    p--;
            }

            int r = (s % 11);
            if (r == 10)
                d = "X";
            else if (r == 0)
                d = "0";
            else
                d = r.ToString();

            return d;
        }

        #region M�todos de processamento do arquivo retorno CNAB400


        #endregion

        #region CNAB 400 - Espec�ficos sidneiklein
        public bool ValidarRemessaCNAB400(string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            bool vRetorno = true;
            string vMsg = string.Empty;
            //
            #region Pr� Valida��es
            if (banco == null)
            {
                vMsg += String.Concat("Remessa: O Banco � Obrigat�rio!", Environment.NewLine);
                vRetorno = false;
            }
            if (cedente == null)
            {
                vMsg += String.Concat("Remessa: O Cedente/Benefici�rio � Obrigat�rio!", Environment.NewLine);
                vRetorno = false;
            }
            if (boletos == null || boletos.Count.Equals(0))
            {
                vMsg += String.Concat("Remessa: Dever� existir ao menos 1 boleto para gera��o da remessa!", Environment.NewLine);
                vRetorno = false;
            }
            #endregion
            //
            foreach (Boleto boleto in boletos)
            {
                #region Valida��o de cada boleto
                if (boleto.Remessa == null)
                {
                    vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe as diretrizes de remessa!", Environment.NewLine);
                    vRetorno = false;
                }
                else
                {
                    #region Valida��es da Remessa que dever�o estar preenchidas quando BANCO DO BRASIL
                    if (String.IsNullOrEmpty(boleto.Remessa.TipoDocumento))
                    {
                        vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe o Tipo Documento!", Environment.NewLine);
                        vRetorno = false;
                    }
                    #endregion
                }
                #endregion
            }
            //
            mensagem = vMsg;
            return vRetorno;
        }
        public string GerarHeaderRemessaCNAB400(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0001, 001, 0, "0", '0'));                                   //001-001
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0002, 001, 0, "1", '0'));                                   //002-002
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0003, 007, 0, "REMESSA", ' '));                             //003-009 "TESTE"
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0010, 002, 0, "01", '0'));                                  //010-011
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0012, 008, 0, "COBRANCA", ' '));                            //012-019
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0020, 007, 0, string.Empty, ' '));                          //020-026
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0027, 004, 0, cedente.ContaBancaria.Agencia, '0'));         //027-030
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0031, 001, 0, cedente.ContaBancaria.DigitoAgencia, ' '));   //031-031
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0032, 008, 0, cedente.ContaBancaria.Conta, '0'));           //032-039
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0040, 001, 0, cedente.ContaBancaria.DigitoConta, ' '));     //040-040
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0041, 006, 0, "000000", '0'));                              //041-046
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0047, 030, 0, cedente.Nome.ToUpper(), ' '));                //047-076
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0077, 018, 0, "001BANCODOBRASIL", ' '));                    //077-094
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0095, 006, 0, DateTime.Now, ' '));                          //095-100
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0101, 007, 0, numeroArquivoRemessa, '0'));                  //101-107
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0108, 022, 0, string.Empty, ' '));                          //108-129
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0130, 007, 0, "0", '0'));                                   //130-136
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0137, 258, 0, string.Empty, ' '));                          //137-394
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0395, 006, 0, "000001", ' '));                              //395-400
                //
                reg.CodificarLinha();
                //
                string vLinha = reg.LinhaRegistro;
                string _header = Utils.SubstituiCaracteresEspeciais(vLinha);
                //
                return _header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
        }
        public string GerarDetalheRemessaCNAB400(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                //Vari�veis Locais a serem Implementadas em n�vel de Config do Boleto...
                boleto.Remessa.CodigoOcorrencia = "01"; //remessa p/ BANCO DO BRASIL
                //
                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);
                //
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0001, 001, 0, "7", '0'));                                       //001-001
                #region Regra Tipo de Inscri��o Cedente
                string vCpfCnpjEmi = "00";
                if (boleto.Cedente.CPFCNPJ.Length.Equals(11)) vCpfCnpjEmi = "01"; //Cpf � sempre 11;
                else if (boleto.Cedente.CPFCNPJ.Length.Equals(14)) vCpfCnpjEmi = "02"; //Cnpj � sempre 14;
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0002, 002, 0, vCpfCnpjEmi, '0'));                               //002-003
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0004, 014, 0, boleto.Cedente.CPFCNPJ, '0'));                    //004-017
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0018, 004, 0, boleto.Cedente.ContaBancaria.Agencia, '0'));      //018-021
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0022, 001, 0, boleto.Cedente.ContaBancaria.DigitoAgencia, ' '));//022-022
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0023, 008, 0, boleto.Cedente.ContaBancaria.Conta, '0'));        //023-030
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0031, 001, 0, boleto.Cedente.ContaBancaria.DigitoConta, ' '));  //031-031
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0032, 007, 0, boleto.Cedente.Convenio, ' '));                   //032-038
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0039, 025, 0, boleto.NumeroDocumento, ' '));                    //039-063
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0064, 017, 0, boleto.NossoNumero, '0'));                        //064-080
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0081, 002, 0, "00", '0'));                                      //081-082
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0083, 002, 0, "00", '0'));                                      //083-084
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0085, 003, 0, string.Empty, ' '));                              //085-087
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0088, 001, 0, string.Empty, ' '));                              //088-088
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0089, 003, 0, string.Empty, ' '));                              //089-091
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0092, 003, 0, boleto.VariacaoCarteira, '0'));                   //092-094
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0095, 001, 0, "0", '0'));                                       //095-095
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0096, 006, 0, "0", '0'));                                       //096-101
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0102, 005, 0, string.Empty, ' '));                              //102-106
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0107, 002, 0, boleto.Cedente.Carteira, '0'));                   //107-108
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0109, 002, 0, boleto.Remessa.CodigoOcorrencia, ' '));           //109-110
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0111, 010, 0, boleto.NumeroDocumento, '0'));                    //111-120
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0121, 006, 0, boleto.DataVencimento, ' '));                     //121-126
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0127, 013, 2, boleto.ValorBoleto, '0'));                        //127-139
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0140, 003, 0, "001", '0'));                                     //140-142   
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0143, 004, 0, "0000", '0'));                                    //143-146
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0147, 001, 0, string.Empty, ' '));                              //147-147 
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0148, 002, 0, boleto.Especie, '0'));                            //148-149
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0150, 001, 0, boleto.Aceite, ' '));                             //150-150
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0151, 006, 0, boleto.DataProcessamento, ' '));                  //151-156
                //
                #region Instru��es
                string vInstrucao1 = "0";
                string vInstrucao2 = "0";
                //string vInstrucao3 = "0";
                switch (boleto.Instrucoes.Count)
                {
                    case 1:
                        vInstrucao1 = boleto.Instrucoes[0].Codigo.ToString();
                        vInstrucao2 = "0";
                        //vInstrucao3 = "0";
                        break;
                    case 2:
                        vInstrucao1 = boleto.Instrucoes[0].Codigo.ToString();
                        vInstrucao2 = boleto.Instrucoes[1].Codigo.ToString();
                        //vInstrucao3 = "0";
                        break;
                    case 3:
                        vInstrucao1 = boleto.Instrucoes[0].Codigo.ToString();
                        vInstrucao2 = boleto.Instrucoes[1].Codigo.ToString();
                        //vInstrucao3 = boleto.Instrucoes[2].Codigo.ToString();
                        break;
                }
                #endregion
                //
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0157, 002, 0, vInstrucao1, '0'));                               //157-158
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0159, 002, 0, vInstrucao2, '0'));                               //159-160
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0161, 013, 2, boleto.JurosMora, '0'));                          //161-173

                #region Instru��es Conforme C�digo de Ocorr�ncia...
                if (boleto.Remessa.CodigoOcorrencia.Equals("35") || boleto.Remessa.CodigoOcorrencia.Equals("36"))   //�35� � Cobrar Multa � ou �36� - Dispensar Multa 
                {
                    #region C�digo de Multa e Valor/Percentual Multa
                    string vCodigoMulta = "9"; //�9� = Dispensar Multa
                    Decimal vMulta = 0;

                    if (boleto.ValorMulta > 0)
                    {
                        vCodigoMulta = "1";    //�1� = Valor
                        vMulta = boleto.ValorMulta;
                    }
                    else if (boleto.PercMulta > 0)
                    {
                        vCodigoMulta = "2";   //�2� = Percentual
                        vMulta = boleto.PercMulta;
                    }
                    #endregion

                    #region DataVencimento
                    string vDataVencimento = "000000";
                    if (!boleto.DataVencimento.Equals(DateTime.MinValue))
                        vDataVencimento = boleto.DataVencimento.ToString("ddMMyy");
                    #endregion

                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0174, 001, 0, vCodigoMulta, '0'));                          //174 a 174      C�digo da Multa 1=Valor 
                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0175, 006, 0, vDataVencimento, ' '));                       //175 a 180      Data de inicio para Cobran�a da Multa 
                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0180, 013, 2, vMulta, '0'));                                //181 a 192      Valor de Multa 
                }
                else
                {
                    #region DataDesconto
                    string vDataDesconto = "000000";
                    if (!boleto.DataDesconto.Equals(DateTime.MinValue))
                        vDataDesconto = boleto.DataDesconto.ToString("ddMMyy");
                    #endregion
                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0174, 006, 0, vDataDesconto, '0'));                             //174-179                
                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0180, 013, 2, boleto.ValorDesconto, '0'));                      //180-192                
                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0193, 013, 2, boleto.IOF, '0'));                                //193-205
                }
                #endregion
                //

                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0206, 013, 2, boleto.Abatimento, '0'));                         //206-218
                #region Regra Tipo de Inscri��o Sacado
                string vCpfCnpjSac = "00";
                if (boleto.Sacado.CPFCNPJ.Length.Equals(11)) vCpfCnpjSac = "01"; //Cpf � sempre 11;
                else if (boleto.Sacado.CPFCNPJ.Length.Equals(14)) vCpfCnpjSac = "02"; //Cnpj � sempre 14;
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0219, 002, 0, vCpfCnpjSac, '0'));                               //219-220
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0221, 014, 0, boleto.Sacado.CPFCNPJ, '0'));                     //221-234
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0235, 037, 0, boleto.Sacado.Nome.ToUpper(), ' '));              //235-271
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0272, 003, 0, string.Empty, ' '));                              //272-274
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0275, 040, 0, boleto.Sacado.Endereco.End.ToUpper(), ' '));      //275-314
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0315, 012, 0, boleto.Sacado.Endereco.Bairro.ToUpper(), ' '));   //315-326
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0327, 008, 0, boleto.Sacado.Endereco.CEP, '0'));                //327-334
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0335, 015, 0, boleto.Sacado.Endereco.Cidade.ToUpper(), ' '));   //335-349
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0350, 002, 0, boleto.Sacado.Endereco.UF.ToUpper(), ' '));       //350-351
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0352, 040, 0, string.Empty, ' '));                              //352-391
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0392, 002, 0, string.Empty, ' '));                              //392-393
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0394, 001, 0, string.Empty, ' '));                              //394-394                
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0395, 006, 0, numeroRegistro, '0'));                            //395-400
                //
                reg.CodificarLinha();
                //
                string _detalhe = Utils.SubstituiCaracteresEspeciais(reg.LinhaRegistro);
                //
                return _detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB400.", ex);
            }
        }
        public string GerarTrailerRemessa400(int numeroRegistro, decimal vltitulostotal)
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
                throw new Exception("Erro durante a gera��o do registro TRAILER do arquivo de REMESSA.", ex);
            }
        }
        //

        public override DetalheRetorno LerDetalheRetornoCNAB400(string registro)
        {
            try
            {
                TRegistroEDI_BancoBrasil_Retorno reg = new TRegistroEDI_BancoBrasil_Retorno();
                //
                reg.LinhaRegistro = registro;
                reg.DecodificarLinha();

                //Passa para o detalhe as propriedades de reg;
                DetalheRetorno detalhe = new DetalheRetorno(registro);
                //
                //detalhe. = reg.Identificacao;
                //detalhe. = reg.Zeros1;
                //detalhe. = reg.Zeros2;
                detalhe.Agencia = Utils.ToInt32(String.Concat(reg.PrefixoAgencia, reg.DVPrefixoAgencia));
                detalhe.Conta = Utils.ToInt32(reg.ContaCorrente);
                detalhe.DACConta = Utils.ToInt32(reg.DVContaCorrente);
                //detalhe. = reg.NumeroConvenioCobranca;
                //detalhe. = reg.NumeroControleParticipante;
                //
                detalhe.NossoNumeroComDV = reg.NossoNumero;
                detalhe.NossoNumero = reg.NossoNumero.Substring(0, reg.NossoNumero.Length - 1); //Nosso N�mero sem o DV!
                detalhe.DACNossoNumero = reg.NossoNumero.Substring(reg.NossoNumero.Length - 1); //DV
                //
                //detalhe. = reg.TipoCobranca;
                //detalhe. = reg.TipoCobrancaEspecifico;
                //detalhe. = reg.DiasCalculo;
                //detalhe. = reg.NaturezaRecebimento;
                //detalhe. = reg.PrefixoTitulo;
                //detalhe. = reg.VariacaoCarteira;
                //detalhe. = reg.ContaCaucao;
                //detalhe. = reg.TaxaDesconto;
                //detalhe. = reg.TaxaIOF;
                //detalhe. = reg.Brancos1;
                detalhe.Carteira = reg.Carteira;
                detalhe.CodigoOcorrencia = Utils.ToInt32(reg.Comando);
                //
                int dataLiquidacao = Utils.ToInt32(reg.DataLiquidacao);
                detalhe.DataLiquidacao = Utils.ToDateTime(dataLiquidacao.ToString("##-##-##"));
                //
                detalhe.NumeroDocumento = reg.NumeroTituloCedente;
                //detalhe. = reg.Brancos2;
                //
                int dataVencimento = Utils.ToInt32(reg.DataVencimento);
                detalhe.DataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));
                //
                detalhe.ValorTitulo = (Convert.ToInt64(reg.ValorTitulo) / 100);
                detalhe.CodigoBanco = Utils.ToInt32(reg.CodigoBancoRecebedor);
                detalhe.AgenciaCobradora = Utils.ToInt32(reg.PrefixoAgenciaRecebedora);
                //detalhe. = reg.DVPrefixoRecebedora;
                detalhe.Especie = Utils.ToInt32(reg.EspecieTitulo);
                //
                int dataCredito = Utils.ToInt32(reg.DataCredito);
                detalhe.DataOcorrencia = Utils.ToDateTime(dataCredito.ToString("##-##-##"));
                //
                detalhe.TarifaCobranca = (Convert.ToInt64(reg.ValorTarifa) / 100);
                detalhe.OutrasDespesas = (Convert.ToInt64(reg.OutrasDespesas) / 100);
                detalhe.ValorOutrasDespesas = (Convert.ToInt64(reg.JurosDesconto) / 100);
                detalhe.IOF = (Convert.ToInt64(reg.IOFDesconto) / 100);
                detalhe.Abatimentos = (Convert.ToInt64(reg.ValorAbatimento) / 100);
                detalhe.Descontos = (Convert.ToInt64(reg.DescontoConcedido) / 100);
                detalhe.ValorPrincipal = (Convert.ToInt64(reg.ValorRecebido) / 100);
                detalhe.JurosMora = (Convert.ToInt64(reg.JurosMora) / 100);
                detalhe.OutrosCreditos = (Convert.ToInt64(reg.OutrosRecebimentos) / 100);
                //detalhe. = reg.AbatimentoNaoAproveitado;
                detalhe.ValorPago = (Convert.ToInt64(reg.ValorLancamento) / 100);
                //detalhe. = reg.IndicativoDebitoCredito;
                //detalhe. = reg.IndicadorValor;
                //detalhe. = reg.ValorAjuste;
                //detalhe. = reg.Brancos3;
                //detalhe. = reg.Brancos4;
                //detalhe. = reg.Zeros3;
                //detalhe. = reg.Zeros4;
                //detalhe. = reg.Zeros5;
                //detalhe. = reg.Zeros6;
                //detalhe. = reg.Zeros7;
                //detalhe. = reg.Zeros8;
                //detalhe. = reg.Brancos5;
                //detalhe. = reg.CanalPagamento;
                //detalhe. = reg.NumeroSequenciaRegistro;
                #region NAO RETORNADOS PELO BANCO
                detalhe.MotivoCodigoOcorrencia = string.Empty;
                detalhe.MotivosRejeicao = string.Empty;
                detalhe.NumeroCartorio = 0;
                detalhe.NumeroProtocolo = string.Empty;
                detalhe.NomeSacado = string.Empty;
                #endregion

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        #endregion
    }
}