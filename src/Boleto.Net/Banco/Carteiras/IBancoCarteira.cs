namespace BoletoNet
{
    public interface IBancoCarteira
    {
        void FormataCodigoBarra(Boleto boleto);
        void FormataLinhaDigitavel(Boleto boleto);
        void FormataNossoNumero(Boleto boleto);
        void ValidaBoleto(Boleto boleto);
    }
}