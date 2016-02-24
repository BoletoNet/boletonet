using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public abstract class AbstractInstrucao : IInstrucao
    {

        #region Variaveis

        private IBanco _banco;
        private int _codigo;
        private string _descricao;
        private int _quantidadeDias;

        #endregion

        # region Propriedades

        public virtual IBanco Banco
        {
            get { return _banco; }
            set { _banco = value; }
        }

        public virtual int Codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }

        public virtual string Descricao
        {
            get { return _descricao; }
            set { _descricao = value; }
        }

        public virtual int QuantidadeDias
        {
            get { return _quantidadeDias; }
            set { _quantidadeDias = value; }
        }

        # endregion

        # region Metodos

        public virtual void Valida()
        {
            throw new NotImplementedException("Função não implementada");
        }

        #endregion
    }
}
