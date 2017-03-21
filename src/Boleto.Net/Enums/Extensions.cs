namespace BoletoNet.Enums
{
    public static class TipoOcorrenciaRemessaExtension
    {
        public static string Format(this TipoOcorrenciaRemessa ocorrencia)
        {
            return Utils.FormatCode(((int)ocorrencia).ToString(), 2);
        }
    }
}
