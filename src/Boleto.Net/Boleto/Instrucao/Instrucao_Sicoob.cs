namespace BoletoNet
{
    using System;
    #region EnumInstrucoes_Sicoob enum
    public enum EnumInstrucoes_Sicoob
    {
        AusenciaDeInstrucoes = 0,

        CobrarJuros = 1,

        Protestar3DiasUteis = 3,

        Protestar4DiasUteis = 4,

        Protestar5DiasUteis = 5,

        NaoProtestar = 7,

        Protestar10DiasUteis = 10,

        Protestar15DiasUteis = 15,

        Protestar20DiasUteis = 20,

        ConcederDescontoApenasAteDataEstipulada = 22,

        DevolverApos15DiasVencido = 42,

        DevolverApos30DiasVencido = 43
    }
    #endregion
    
    public class Instrucao_Sicoob : AbstractInstrucao, IInstrucao
	{
		#region Construtores

		public Instrucao_Sicoob()
		{
			try
			{
				this.Banco = new Banco(756);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao carregar objeto", ex);
			}
		}

		public Instrucao_Sicoob(int codigo)
		{
			this.carregar(codigo, 0, 0);
		}

		public Instrucao_Sicoob(int codigo, int nrDias)
		{
			this.carregar(codigo, nrDias, (double)0.0);
		}

		public Instrucao_Sicoob(int codigo, double percentualMultaDia)
		{
			this.carregar(codigo, 0, percentualMultaDia);
		}

		public Instrucao_Sicoob(int codigo, int nrDias, double percentualMultaDia)
		{
			this.carregar(codigo, nrDias, percentualMultaDia);
		}

		#endregion

		#region Metodos Privados

        private void carregar(int idInstrucao, int nrDias, double percentualMultaDia)
        {
            try
            {
                this.Banco = new Banco_Banrisul();
                this.Valida();

                switch ((EnumInstrucoes_Sicoob)idInstrucao)
                {
                    case EnumInstrucoes_Sicoob.AusenciaDeInstrucoes:
                        break;
                    case EnumInstrucoes_Sicoob.CobrarJuros:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.CobrarJuros;
                        this.Descricao = "Cobrar Juros";
                        break;
                    case EnumInstrucoes_Sicoob.Protestar3DiasUteis:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.Protestar3DiasUteis;
                        this.Descricao = "Protestar 3 dias úteis após vencimento";
                        break;
                    case EnumInstrucoes_Sicoob.Protestar4DiasUteis:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.Protestar4DiasUteis;
                        this.Descricao = "Protestar 4 dias úteis após vencimento";
                        break;
                    case EnumInstrucoes_Sicoob.Protestar5DiasUteis:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.Protestar5DiasUteis;
                        this.Descricao = "Protestar 5 dias úteis após vencimento";
                        break;
                    case EnumInstrucoes_Sicoob.NaoProtestar:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.NaoProtestar;
                        this.Descricao = "Não protestar";
                        break;
                    case EnumInstrucoes_Sicoob.Protestar10DiasUteis:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.Protestar10DiasUteis;
                        this.Descricao = "Protestar 10 dias úteis após vencimento";
                        break;
                    case EnumInstrucoes_Sicoob.Protestar15DiasUteis:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.Protestar15DiasUteis;
                        this.Descricao = "Protestar 15 dias úteis após vencimento";
                        break;
                    case EnumInstrucoes_Sicoob.Protestar20DiasUteis:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.Protestar20DiasUteis;
                        this.Descricao = "Protestar 20 dias úteis após vencimento";
                        break;
                    case EnumInstrucoes_Sicoob.ConcederDescontoApenasAteDataEstipulada:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.ConcederDescontoApenasAteDataEstipulada;
                        this.Descricao = "Conceder desconto só até a data estipulada";
                        break;
                    case EnumInstrucoes_Sicoob.DevolverApos15DiasVencido:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.DevolverApos15DiasVencido;
                        this.Descricao = "Devolver após 15 dias vencido";
                        break;
                    case EnumInstrucoes_Sicoob.DevolverApos30DiasVencido:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.DevolverApos30DiasVencido;
                        this.Descricao = "Devolver após 30 dias vencido";
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = " (Selecione) ";
                        break;

                }

                this.QuantidadeDias = nrDias;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

		public override void Valida()
		{
	        base.Valida();
		}

		#endregion
	}
}