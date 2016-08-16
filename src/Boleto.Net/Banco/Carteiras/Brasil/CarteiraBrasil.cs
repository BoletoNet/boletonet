namespace BoletoNet
{
    public abstract class CarteiraBrasil : IBancoCarteira
    {
        protected string LimparCarteira(string carteira)
        {
            return carteira.Split('-')[0];
        }
        public abstract void FormataCodigoBarra(Boleto boleto);
        public abstract void FormataLinhaDigitavel(Boleto boleto);
        public abstract void FormataNossoNumero(Boleto boleto);
        public abstract void ValidaBoleto(Boleto boleto);
    }
}