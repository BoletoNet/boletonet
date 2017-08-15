using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public class CodigoTarifas : ICodigoTarifas
    {

        #region Variaveis

        private ICodigoTarifas _ICodigoTarifas = null;

        #endregion

        #region Propriedades da interface

        public IBanco Banco
        {
            get { return _ICodigoTarifas.Banco; }
        }

        public int Codigo
        {
            get { return _ICodigoTarifas.Codigo; }
            set { _ICodigoTarifas.Codigo = value; }
        }

        public string Descricao
        {
            get { return _ICodigoTarifas.Descricao; }
        }

        #endregion

    }
}
