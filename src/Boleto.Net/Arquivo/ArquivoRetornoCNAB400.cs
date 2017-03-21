using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public class ArquivoRetornoCNAB400 : AbstractArquivoRetorno, IArquivoRetorno
    {

        private HeaderRetorno _headerRetorno = new HeaderRetorno();
        private List<DetalheRetorno> _listaDetalhe = new List<DetalheRetorno>();

        public List<DetalheRetorno> ListaDetalhe
        {
            get { return _listaDetalhe; }
            set { _listaDetalhe = value; }
        }

        public HeaderRetorno HeaderRetorno
        {
            get { return _headerRetorno; }
            set { _headerRetorno = value; }
        }

        #region Construtores

        public ArquivoRetornoCNAB400()
		{
            this.TipoArquivo = TipoArquivo.CNAB400;
        }

        #endregion

        #region Métodos de instância

        public override void LerArquivoRetorno(IBanco banco, Stream arquivo)
        {
            try
            {
                StreamReader stream = new StreamReader(arquivo, System.Text.Encoding.UTF8);
                string linha = "";
                // Identificação do registro detalhe
                List<string> IdsRegistroDetalhe = new List<string>();

                // Lendo o arquivo
                linha = stream.ReadLine();
                this.HeaderRetorno = banco.LerHeaderRetornoCNAB400(linha);

                // Próxima linha (DETALHE)
                linha = stream.ReadLine();

                // 85 - CECRED - Código de registro detalhe 7 para CECRED
                // 1 - Banco do Brasil- Código de registro detalhe 7 para convênios com 7 posições, e detalhe 1 para convênios com 6 posições(colocado as duas, pois não interferem em cada tipo de arquivo)
                if (banco.Codigo == 85)
                {
                    IdsRegistroDetalhe.Add("7");
                }
                else if (banco.Codigo == 1)
                {
                    IdsRegistroDetalhe.Add("1");//Para convênios de 6 posições
                    IdsRegistroDetalhe.Add("7");//Para convênios de 7 posições
                }
                else
                {
                    IdsRegistroDetalhe.Add("1");
                }

                while (IdsRegistroDetalhe.Contains(DetalheRetorno.PrimeiroCaracter(linha)))
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
