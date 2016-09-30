using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public enum EnumTipodeLinha
    {
        HeaderDeArquivo = 1,
        HeaderDeLote = 2,
        DetalheSegmentoP = 3,
        DetalheSegmentoQ = 4,
        DetalheSegmentoR = 5,
        TraillerDeLote = 6,
        TraillerDeArquivo = 7,
        DetalheSegmentoS = 8
    }
    
    public class LinhaDeArquivoGeradaArgs : EventArgs
    {
        private string _linha;
        private Boleto _boleto;
        private EnumTipodeLinha _tipoLinha;

        public LinhaDeArquivoGeradaArgs(Boleto boleto, string linha, EnumTipodeLinha tipoLinha)
        {
            try
            {
                _boleto = boleto;
                _linha = linha;
                _tipoLinha = tipoLinha;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao instanciar objeto", ex);
            }
        }       

        public string Linha
        {
            get { return _linha; }
        }

        public Boleto Boleto
        {
            get { return _boleto; }
        }

        public EnumTipodeLinha TipoLinha
        {
            get { return _tipoLinha; }
        }
    }
}
