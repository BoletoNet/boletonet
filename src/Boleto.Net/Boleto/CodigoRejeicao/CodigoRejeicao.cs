using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public class CodigoRejeicao : ICodigoRejeicao
    {

        #region Variaveis

        private ICodigoRejeicao _ICodigoRejeicao = null;

        #endregion

        #region Propriedades da interface

        public IBanco Banco
        {
            get { return _ICodigoRejeicao.Banco; }
        }

        public int Codigo
        {
            get { return _ICodigoRejeicao.Codigo; }
            set { _ICodigoRejeicao.Codigo = value; }
        }

        public string Descricao
        {
            get { return _ICodigoRejeicao.Descricao; }
        }

        #endregion

    }
}
