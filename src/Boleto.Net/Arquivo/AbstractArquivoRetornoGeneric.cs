using System;
using System.Collections.Generic;

namespace BoletoNet
{
    public abstract class AbstractArquivoRetorno<T> : AbstractArquivoRetorno
    {
        protected AbstractArquivoRetorno()
        {
            Linhas = new List<T>();
        }
        public event Action<T> LinhaLida;

        public List<T> Linhas { get; set; }

        protected virtual void OnLinhaLida(T obj)
        {
            Linhas.Add(obj);
            var handler = LinhaLida;
            if (handler != null) handler(obj);
        }
    }
}
