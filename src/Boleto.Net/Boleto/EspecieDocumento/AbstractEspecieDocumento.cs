namespace BoletoNet
{
    public abstract class AbstractEspecieDocumento : IEspecieDocumento
    {
        public virtual IBanco Banco { get; set; }

        public virtual string Codigo { get; set; }

        public virtual string Sigla { get; set; }

        public virtual string Especie { get; set; }

        public virtual string ObterCodigo(Boleto boleto, TipoArquivo tipoArquivo) {
            return Codigo;
        }
    }
}