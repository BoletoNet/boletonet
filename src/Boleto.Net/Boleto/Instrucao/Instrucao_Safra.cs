using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_Safra
    {

    }

    #endregion

    public class Instrucao_Safra : AbstractInstrucao, IInstrucao
    {
        #region Construtores

        public Instrucao_Safra()
        {
            try
            {
                this.Banco = new Banco(422);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public Instrucao_Safra(int codigo)
        {
            this.carregar(codigo, 0);
        }

        public Instrucao_Safra(int codigo, int nrDias)
        {
            this.carregar(codigo, nrDias);
        }
        #endregion Construtores

        #region Metodos Privados

        private void carregar(int idInstrucao, int nrDias)
        {
            throw new Exception("Não implementado");
        }

        public override void Valida()
        {
            //base.Valida();
        }

        #endregion

    }
}
