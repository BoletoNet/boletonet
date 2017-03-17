namespace BoletoNet.Arquivo
{
    public class TrailerCbr643 : LinhaCbr643
    {
        public TrailerCbr643()
        {
            Id = 9;
        }

        /// <summary>
        ///     Cobrança Simples - quantidade de títulos
        /// </summary>
        [TextPos(017, 008)]
        public int CobrancaSimplesQuantidadeDeTitulos { get; set; }

        /// <summary>
        ///     Cobrança Simples - valor total
        /// </summary>
        [TextPos(025, 013)]
        public decimal CobrancaSimplesValorTotal { get; set; }

        /// <summary>
        ///     Cobrança Simples - Número do aviso
        /// </summary>
        [TextPos(039, 008)]
        public int CobrancaSimplesNumeroDoAviso { get; set; }

        /// <summary>
        ///     Cobrança Vinculada - quantidade de títulos
        /// </summary>
        [TextPos(057, 008)]
        public int CobrançaVinculadaQuantidadeDeTítulos { get; set; }

        /// <summary>
        ///     v99 Cobrança Vinculada - valor total
        /// </summary>
        [TextPos(065, 013)]
        public decimal CobrançaVinculadaValorTotal { get; set; }

        /// <summary>
        ///     Cobrança Vinculada - Número do aviso
        /// </summary>
        [TextPos(079, 008)]
        public int CobrançaVinculadaNúmeroDoAviso { get; set; }

        /// <summary>
        ///     Cobrança Caucionada - quantidade de títulos
        /// </summary>
        [TextPos(097, 008)]
        public int CobrançaCaucionadaQuantidadeDeTítulos { get; set; }

        /// <summary>
        ///     Cobrança Caucionada - valor total
        /// </summary>
        [TextPos(105, 013)]
        public decimal CobrançaCaucionadaValorTotal { get; set; }

        /// <summary>
        ///     Cobrança Caucionada - Número do aviso
        /// </summary>
        [TextPos(119, 008)]
        public int CobrançaCaucionadaNúmeroDoAviso { get; set; }

        /// <summary>
        ///     Cobrança Descontada - quantidade de títulos
        /// </summary>
        [TextPos(137, 008)]
        public int CobrançaDescontadaQuantidadeDeTítulos { get; set; }

        /// <summary>
        ///     Cobrança Descontada - valor total
        /// </summary>
        [TextPos(145, 013)]
        public decimal CobrançaDescontadaValorTotal { get; set; }

        /// <summary>
        ///     Cobrança Descontada - Número do aviso
        /// </summary>
        [TextPos(159, 008)]
        public int CobrançaDescontadaNúmeroDoAviso { get; set; }
        
        /// <summary>
        ///     Cobrança Vendor - quantidade de títulos
        /// </summary>
        [TextPos(217, 008)]
        public int CobrançaVendorQuantidadeDeTítulos { get; set; }

        /// <summary>
        ///     Cobrança Vendor - valor total
        /// </summary>
        [TextPos(225, 013)]
        public decimal CobrançaVendorValorTotal { get; set; }

        /// <summary>
        ///     Cobrança Vendor - Número do aviso
        /// </summary>
        [TextPos(239, 008)]
        public int CobrançaVendorNúmeroDoAviso { get; set; }
    }
}
