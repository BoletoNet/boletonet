using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public abstract class AbstractCarteira : ICarteira
    {

        #region Variaveis

        private IBanco _banco;
        private int _carteira;
        private string _codigo;
        private string _tipo;
        private string _descricao;

        #endregion

        # region Propriedades

        public virtual IBanco Banco
        {
            get { return _banco; }
            set { _banco = value; }
        }

        public virtual int NumeroCarteira
        {
            get{ return _carteira; }
            set { _carteira = value; }
        }

        public virtual string Codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }

        public virtual string Tipo
        {
            get { return _tipo; }
            set { _tipo = value; }
        }

        public virtual string Descricao
        {
            get { return _descricao; }
            set { _descricao = value; }
        }

        # endregion

    }
}
