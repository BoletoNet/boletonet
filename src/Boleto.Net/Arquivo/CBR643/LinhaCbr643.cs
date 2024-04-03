
namespace BoletoNet.Arquivo
{
    public abstract class LinhaCbr643
    {
        /// <summary>
        /// Identificação do Registro Header: “0”   
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// Nr. Seqüencial do registro
        /// </summary>
        [TextPos(394, 6)]
        public int SequencialRegistro { get; set; }

        public bool LinhaCorresponde(string linha)
        {
            return linha.StartsWith(Id.ToString());
        }
    }
}