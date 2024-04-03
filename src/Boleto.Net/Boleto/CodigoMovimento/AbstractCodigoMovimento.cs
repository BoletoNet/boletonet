using BoletoNet.Enums;
using BoletoNet.Excecoes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public abstract class AbstractCodigoMovimento : ICodigoMovimento
    {

        #region Variaveis

        private IBanco _banco;
        private int _codigo;
        private string _descricao;

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

        # endregion

        public abstract TipoOcorrenciaRetorno ObterCorrespondenteFebraban();

        protected TipoOcorrenciaRetorno ObterCorrespondenteFebraban<T>(Dictionary<T, TipoOcorrenciaRetorno> correspondentesFebraban, T ocorrencia) where T : struct, IConvertible
        {
            if (correspondentesFebraban.ContainsKey(ocorrencia))
                return correspondentesFebraban[ocorrencia];

            throw new BoletoNetException("Não há um movimento correspondente ao Febraban");
        }
    }
}
