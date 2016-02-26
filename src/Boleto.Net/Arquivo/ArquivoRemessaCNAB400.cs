using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    internal class ArquivoRemessaCNAB400 : AbstractArquivoRemessa, IArquivoRemessa
    {

        #region Construtores

        public ArquivoRemessaCNAB400()
        {
            this.TipoArquivo = TipoArquivo.CNAB400;
        }

        #endregion

        #region Métodos de instância
        /// <summary>
        /// Método que fará a verificação se a classe está devidamente implementada para a geração da Remessa
        /// </summary>
        public override bool ValidarArquivoRemessa(string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            try
            {
                bool vRetorno = true;
                string vMsg = string.Empty;
                //
                foreach (Boleto boleto in boletos)
                {
                    string vMsgBol = string.Empty;
                    bool vRetBol = boleto.Banco.ValidarRemessa(this.TipoArquivo, numeroConvenio, banco, cedente, boletos, numeroArquivoRemessa, out vMsgBol);
                    if (!vRetBol && !String.IsNullOrEmpty(vMsgBol))
                    {
                        vMsg += vMsgBol;
                        vRetorno = vRetBol;
                    }
                }
                //
                mensagem = vMsg;
                return vRetorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override string GerarArquivoRemessa(string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa)
        {
            try
            {
                int numeroRegistro = 2;
                string strline;
                decimal vltitulostotal = 0;                 //Uso apenas no registro TRAILER do banco Santander - jsoda em 09/05/2012 - Add no registro TRAILER do banco Banrisul - sidneiklein em 08/08/2013

                var arquivoBuilder = new StringBuilder();

                strline = banco.GerarHeaderRemessa(numeroConvenio, cedente, TipoArquivo.CNAB400, numeroArquivoRemessa);
                arquivoBuilder.AppendLine(strline);

                foreach (Boleto boleto in boletos)
                {
                    boleto.Banco = banco;
                    strline = boleto.Banco.GerarDetalheRemessa(boleto, numeroRegistro, TipoArquivo.CNAB400);
                    arquivoBuilder.AppendLine(strline);
                    vltitulostotal += boleto.ValorBoleto;   //Uso apenas no registro TRAILER do banco Santander - jsoda em 09/05/2012 - Add no registro TRAILER do banco Banrisul - sidneiklein em 08/08/2013
                    numeroRegistro++;

                    // 85 - CECRED
                    if (banco.Codigo == 85) {
                        if (boleto.PercMulta > 0 || boleto.ValorMulta > 0) {
                            Banco_Cecred _banco = new Banco_Cecred();
                            string linhaCECREDRegistroDetalhe5 = _banco.GerarRegistroDetalhe5(boleto, numeroRegistro, TipoArquivo.CNAB400);
                            arquivoBuilder.AppendLine(linhaCECREDRegistroDetalhe5);
                            numeroRegistro++;
                        }
                    }

                }

                strline = banco.GerarTrailerRemessa(numeroRegistro, TipoArquivo.CNAB400, cedente, vltitulostotal);

                arquivoBuilder.AppendLine(strline);

                return arquivoBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar arquivo remessa.", ex);
            }
        }

        #endregion

    }
}
