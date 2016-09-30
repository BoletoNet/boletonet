using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public class DetalheSegmentoYRetornoCNAB240
    {

        #region Variáveis

        int _CodigoMovimento;
        int _IdentificacaoRegistro;
        string _IdentificacaoCheque1;
        string _IdentificacaoCheque2;
        string _IdentificacaoCheque3;
        string _IdentificacaoCheque4;
        string _IdentificacaoCheque5;
        string _IdentificacaoCheque6;
        string _registro;

        private List<DetalheSegmentoYRetornoCNAB240> _listaDetalhe = new List<DetalheSegmentoYRetornoCNAB240>();

        #endregion

        #region Construtores

        public DetalheSegmentoYRetornoCNAB240(string registro)
        {
            _registro = registro;
        }

        public DetalheSegmentoYRetornoCNAB240()
        {
        }

        #endregion

        #region Propriedades
        public int CodigoMovimento
        {
            get { return _CodigoMovimento; }
            set { _CodigoMovimento = value; }
        }

        public int IdentificacaoRegistro
        {
            get { return _IdentificacaoRegistro; }
            set { _IdentificacaoRegistro = value; }
        }

        public string IdentificacaoCheque1
        {
            get { return _IdentificacaoCheque1; }
            set { _IdentificacaoCheque1 = value; }
        }

        public string IdentificacaoCheque2
        {
            get { return _IdentificacaoCheque2; }
            set { _IdentificacaoCheque2 = value; }
        }

        public string IdentificacaoCheque3
        {
            get { return _IdentificacaoCheque3; }
            set { _IdentificacaoCheque3 = value; }
        }

        public string IdentificacaoCheque4
        {
            get { return _IdentificacaoCheque4; }
            set { _IdentificacaoCheque4 = value; }
        }

        public string IdentificacaoCheque5
        {
            get { return _IdentificacaoCheque5; }
            set { _IdentificacaoCheque5 = value; }
        }

        public string IdentificacaoCheque6
        {
            get { return _IdentificacaoCheque6; }
            set { _IdentificacaoCheque6 = value; }
        }

        public string Registro
        {
            get { return _registro; }
        }

        public string TipoInscricao { get; set; }
        public string NumeroInscricao { get; set; }
        public string NomeSacador { get; set; }
        public string EnderecoSacador { get; set; }
        public string BairroSacador { get; set; }
        public string CEPSacador { get; set; }
        public string CidadeSacador { get; set; }
        public string UFSacador { get; set; }

        #endregion

        #region Métodos de Instância

        public void LerDetalheSegmentoYRetornoCNAB240(string registro)
        {
            try
            {
                _registro = Registro;

                if (registro.Substring(13, 1) != "Y")
                    throw new Exception("Registro inválido. O detalhe não possuí as características do segmento Y.");

                CodigoMovimento = Convert.ToInt32(registro.Substring(15, 2));
                IdentificacaoRegistro = Convert.ToInt32(registro.Substring(17, 4));
                IdentificacaoCheque1 = registro.Substring(19, 34);
                IdentificacaoCheque2 = registro.Substring(43, 34);
                IdentificacaoCheque3 = registro.Substring(87, 34);
                IdentificacaoCheque4 = registro.Substring(121, 34);
                IdentificacaoCheque5 = registro.Substring(155, 34);
                IdentificacaoCheque6 = registro.Substring(189, 34);

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO Y.", ex);
            }
        }

        #endregion
    }
}
