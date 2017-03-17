using System;

namespace BoletoNet.Excecoes
{
    public class BoletoNetException : Exception
    {
        public BoletoNetException(string message) : base(message){}
        public BoletoNetException(string message, Exception ex) : base(message, ex){}
    }
}
