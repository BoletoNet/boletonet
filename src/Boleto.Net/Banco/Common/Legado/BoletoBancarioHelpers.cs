using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoletoNet
{
    public static class BoletoBancarioHelpers
    {
        /// <summary>
        /// [Deprecated] Atalho para a chamada Boleto.Montar() na nova API
        /// <author>Olavo Rocha Neto</author>
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string MontaHtml(this BoletoBancario b)
        {
            return b.Boleto.Montar();
        }
    }

    /// <summary>
    /// Parte da classe Boleto Bancário detinada a guardar os legados para o menor impacto possível nas versões.
    /// </summary>
    public partial class BoletoBancario
    {
        /// <summary>
        /// [deprecated] Definne se o endereço do cedente será ou não exibido na renderização do Boleto
        /// </summary>
        /// <remarks>
        /// Usar Boleto.Opcoes.MostrarEnderecoCedente
        /// </remarks>
        public bool MostrarEnderecoCedente {
            get
            {
                return this.Boleto.Opcoes.MostrarEnderecoCedente;
            }
            set
            {
                if (this.Boleto != null)
                    if (this.Boleto.Opcoes != null)
                        this.Boleto.Opcoes.MostrarEnderecoCedente = value;
            }
        }
    }

    
}
