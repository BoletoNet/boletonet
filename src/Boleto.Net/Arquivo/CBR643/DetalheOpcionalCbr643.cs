namespace BoletoNet.Arquivo.CBR643
{
    public class DetalheOpcionalCbr643 : LinhaCbr643
    {
        public DetalheOpcionalCbr643()
        {
            Id = 5;
        }

        /// <summary>
        /// Tipo de Serviço: “06”
        /// </summary>
        [TextPos(002, 002)]
        public string TipoServico { get; set; }

        /// <summary>
        /// Número do boleto do cedente com 15 posições.
        /// </summary>
        [TextPos(006, 015)]
        public string NumeroBoletoCedente { get; set; }
    }
}
