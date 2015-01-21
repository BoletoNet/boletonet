using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public class CodigoLiquidacao : AbstractCodigoLiquidacao, ICodigoLiquidacao
    {

        #region Variaveis

        private ICodigoLiquidacao _ICodigoLiquidacao = null;

        #endregion

        #region Propriedades da interface

        public override IBanco Banco
        {
            get
            {
                return _ICodigoLiquidacao.Banco;
            }
        }

        public override int Enumerado
        {
            get
            {
                return _ICodigoLiquidacao.Enumerado;
            }
        }

        public override string Codigo
        {
            get
            {
                return _ICodigoLiquidacao.Codigo;
            }
        }

        public override string Descricao
        {
            get
            {
                return _ICodigoLiquidacao.Descricao;
            }
        }

        public override string Recurso
        {
            get
            {
                return _ICodigoLiquidacao.Recurso;
            }
        }

        #endregion

    }
}
