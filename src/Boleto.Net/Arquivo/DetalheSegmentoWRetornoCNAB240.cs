using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public class DetalheSegmentoWRetornoCNAB240
    {
        #region Variáveis

        int _codigoErro = 0;

        #endregion

        #region Construtores

        public DetalheSegmentoWRetornoCNAB240()
		{
        }

        #endregion

        #region Propriedades

        public int CodigoErro
        {
            get { return _codigoErro; }
            set { _codigoErro = value; }
        }

        #endregion

        #region Métodos de Instância

        public void LerDetalheSegmentoWRetornoCNAB240(string registro)
        {
            try
            {
                if (registro.Substring(13, 1) != "W")
                    throw new Exception("Registro inválido. O detalhe não possuí as características do segmento W.");

                if (!registro.Substring(28, 3).Trim().Equals(""))
                    _codigoErro = Convert.ToInt32(registro.Substring(28, 3));

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO W.", ex);
            }
        }

        #endregion
    }
}
