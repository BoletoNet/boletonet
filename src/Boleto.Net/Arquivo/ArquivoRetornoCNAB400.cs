using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public class ArquivoRetornoCNAB400 : AbstractArquivoRetorno, IArquivoRetorno
    {

        private List<DetalheRetorno> _listaDetalhe = new List<DetalheRetorno>();

        public List<DetalheRetorno> ListaDetalhe
        {
            get { return _listaDetalhe; }
            set { _listaDetalhe = value; }
        }

        #region Construtores

        public ArquivoRetornoCNAB400()
		{
            this.TipoArquivo = TipoArquivo.CNAB400;
        }

        #endregion

        #region M�todos de inst�ncia

        public override void LerArquivoRetorno(IBanco banco, Stream arquivo)
        {
            try
            {
                StreamReader stream = new StreamReader(arquivo, System.Text.Encoding.UTF8);
                string linha = "";
                // Identifica��o do registro detalhe
                string IdRegistroDetalhe = string.Empty;

                // Lendo o arquivo
                linha = stream.ReadLine();

                // Pr�xima linha (DETALHE)
                linha = stream.ReadLine();

                // 85 - CECRED - C�digo de registro detalhe 7 para CECRED
                if (banco.Codigo == 85) {
                    IdRegistroDetalhe = "7";
                } else {
                    IdRegistroDetalhe = "1";
                }

                while (DetalheRetorno.PrimeiroCaracter(linha) == IdRegistroDetalhe)
                {
                    DetalheRetorno detalhe = banco.LerDetalheRetornoCNAB400(linha);
                    ListaDetalhe.Add(detalhe);
                    OnLinhaLida(detalhe, linha);
                    linha = stream.ReadLine();
                }

                stream.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler arquivo.", ex);
            }
        }

        #endregion
    }
}
