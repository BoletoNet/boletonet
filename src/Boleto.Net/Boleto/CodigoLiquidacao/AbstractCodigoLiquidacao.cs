using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public abstract class AbstractCodigoLiquidacao : ICodigoLiquidacao
    {

        #region Variaveis

        private IBanco _banco;
        private int _enum;
        private string _codigo;
        private string _descricao;
        private string _recurso;

        #endregion

        # region Propriedades

        public virtual IBanco Banco
        {
            get
            {
                return _banco;
            }
            set
            {
                _banco = value;
            }
        }

        public virtual int Enumerado
        {
            get
            {
                return _enum;
            }
            set
            {
                _enum = value;
            }
        }

        public virtual string Codigo
        {
            get
            {
                return _codigo;
            }
            set
            {
                _codigo = value;
            }
        }

        public virtual string Descricao
        {
            get
            {
                return _descricao;
            }
            set
            {
                _descricao = value;
            }
        }

        public virtual string Recurso
        {
            get
            {
                return _recurso;
            }
            set
            {
                _recurso = value;
            }
        }

        # endregion

    }
}
