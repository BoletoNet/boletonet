using System;

namespace BoletoNet
{
    #region Enumerador

    public enum EnumInstrucoes_C6
    {
    }

    #endregion Enumerador

    public class Instrucao_C6 : AbstractInstrucao, IInstrucao
    {
        #region Construtores

        public Instrucao_C6()
        {
            try
            {
                this.Banco = new Banco((int)Enums.Bancos.C6Bank);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Erro ao carregar objeto", ex);
            }
        }

        public Instrucao_C6(int codigo) : this(codigo, 0)
        {
        }

        public Instrucao_C6(int codigo, int nrDias)
        {
            throw new NotImplementedException();
        }

        #endregion Construtores

        #region Metodos Privados

        public override void Valida()
        {
            throw new NotImplementedException();
        }

        #endregion Metodos Privados
    }
}