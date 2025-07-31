using System;

namespace BoletoNet
{
    #region Enumerador

    public enum EnumInstrucoes_Inter
    {
    }

    #endregion Enumerador

    public class Instrucao_Inter : AbstractInstrucao, IInstrucao
    {
        #region Construtores

        public Instrucao_Inter()
        {
            try
            {
                this.Banco = new Banco((int)Enums.Bancos.Inter);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Erro ao carregar objeto", ex);
            }
        }

        public Instrucao_Inter(int codigo) : this(codigo, 0)
        {
        }

        public Instrucao_Inter(int codigo, int nrDias)
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