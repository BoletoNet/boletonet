using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public interface IInstrucao
    {
        /// <summary>
        /// Valida os dados referentes � instru��o
        /// </summary>
        void Valida();

        /// <summary>
        /// Carrega a instru��o espec�fica do banco referente a valor.
        /// </summary>
        /// <param name="idInstrucao">C�digo da instru��o.</param>
        /// <param name="valor">Valor apresentado na instru��o.</param>
        bool Carregar(int idInstrucao, decimal valor);

        /// <summary>
        /// Carrega a instru��o espec�fica do banco referente a valor.
        /// </summary>
        /// <param name="idInstrucao">C�digo da instru��o.</param>
        /// <param name="nrDias">N� de dias apresentado na instru��o.</param>
        bool Carregar(int idInstrucao, int nrDias);

        IBanco Banco { get; set; }
        int Codigo { get; set; }
        string Descricao { get; set; }
        int QuantidadeDias { get; set; }
    }
}
