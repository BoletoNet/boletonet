using System;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_Banestes
    {
        Protestar = 6,
        NaoProtestar = 7
    }

    #endregion 

    public class Instrucao_Banestes: AbstractInstrucao, IInstrucao
    {

        #region Construtores 

        public Instrucao_Banestes()
		{
			try
			{
                this.Banco = new Banco(21);
			}
			catch (Exception ex)
			{
                throw new Exception("Erro ao carregar objeto", ex);
			}
		}

		#endregion 


    }
}
