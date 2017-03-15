namespace BoletoNet.Excecoes
{
    public class NossoNumeroInvalidoException : BoletoNetException
    {
        public NossoNumeroInvalidoException() : this("Nosso número é inválido")
        {
        }
        public NossoNumeroInvalidoException(string message) : base(message)
        {
        }
    }
}
