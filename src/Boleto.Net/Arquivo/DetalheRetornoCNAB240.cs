using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public class DetalheRetornoCNAB240
    {

        #region Variáveis

        private DetalheSegmentoTRetornoCNAB240 _segmentoT = new DetalheSegmentoTRetornoCNAB240();
        private DetalheSegmentoURetornoCNAB240 _segmentoU = new DetalheSegmentoURetornoCNAB240();
        private DetalheSegmentoWRetornoCNAB240 _segmentoW = new DetalheSegmentoWRetornoCNAB240();
        private HeaderDeArquivoCNAB240 _headerArquivo = new HeaderDeArquivoCNAB240();

        #endregion

        #region Construtores
        public DetalheRetornoCNAB240()
        {
        }

        public DetalheRetornoCNAB240(DetalheSegmentoTRetornoCNAB240 segmentoT)
        {
            _segmentoT = segmentoT;
        }

        public DetalheRetornoCNAB240(DetalheSegmentoURetornoCNAB240 segmentoU)
        {
            _segmentoU = segmentoU;
        }

        public DetalheRetornoCNAB240(DetalheSegmentoWRetornoCNAB240 segmentoW)
        {
            _segmentoW = segmentoW;
        }

        public DetalheRetornoCNAB240(DetalheSegmentoERetornoCNAB240 segmentoE)
        {
            SegmentoE = segmentoE;
        }

        public DetalheRetornoCNAB240(DetalheSegmentoTRetornoCNAB240 segmentoT, DetalheSegmentoURetornoCNAB240 segmentoU)
		{
            _segmentoT = segmentoT;
            _segmentoU = segmentoU;
        }

        #endregion

        #region Propriedades

        public DetalheSegmentoTRetornoCNAB240 SegmentoT
        {
            get { return _segmentoT; }
            set { _segmentoT = value; }
        }

        public DetalheSegmentoURetornoCNAB240 SegmentoU
        {
            get { return _segmentoU; }
            set { _segmentoU = value; }
        }

        public DetalheSegmentoWRetornoCNAB240 SegmentoW
        {
            get { return _segmentoW; }
            set { _segmentoW = value; }
        }

        public DetalheSegmentoERetornoCNAB240 SegmentoE { get; set; }

        public HeaderDeArquivoCNAB240 HeaderArquivo
        {
            get { return _headerArquivo; }
            set { _headerArquivo = value; }
        }      

        #endregion

        #region Métodos de Instância

        #endregion

    }
}
