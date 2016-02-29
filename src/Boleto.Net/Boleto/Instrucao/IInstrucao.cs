using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public interface IInstrucao
    {
        /// <summary>
        /// Valida os dados referentes à instrução
        /// </summary>
        void Valida();

        /// <summary>
        /// Carrega a instrução específica do banco referente a valor.
        /// </summary>
        /// <param name="idInstrucao">Código da instrução.</param>
        /// <param name="valor">Valor apresentado na instrução.</param>
        bool Carregar(int idInstrucao, decimal valor);

        /// <summary>
        /// Carrega a instrução específica do banco referente a valor.
        /// </summary>
        /// <param name="idInstrucao">Código da instrução.</param>
        /// <param name="nrDias">Nº de dias apresentado na instrução.</param>
        bool Carregar(int idInstrucao, int nrDias);

        IBanco Banco { get; set; }
        int Codigo { get; set; }
        string Descricao { get; set; }
        int QuantidadeDias { get; set; }
    }
}
