using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_Real
    {

    }

    #endregion 

    public class Instrucao_Real : AbstractInstrucao, IInstrucao
    {
        #region Construtores

        public Instrucao_Real()
        {
            try
            {
                this.Banco = new Banco(356);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public Instrucao_Real(int codigo)
        {
            this.carregar(codigo, 0);
        }

        public Instrucao_Real(int codigo, int nrDias)
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
