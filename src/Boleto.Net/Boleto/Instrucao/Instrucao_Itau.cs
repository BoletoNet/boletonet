using System;
using System.Collections;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_Itau
    {
        Protestar = 9,                      // Emite aviso ao sacado ap�s N dias do vencto, e envia ao cart�rio ap�s 5 dias �teis
        NaoProtestar = 10,                  // Inibe protesto, quando houver instru��o permanente na conta corrente
        ImportanciaporDiaDesconto = 30,
        ProtestoFinsFalimentares = 42,
        ProtestarAposNDiasCorridos = 34, //J�ferson (jefhtavares) em 09/09/14 -- Segundo o manual que eu tenho (v. de maio de 2014) n�o � 81 este c�digo
        ProtestarAposNDiasUteis = 35, //J�ferson (jefhtavares) em 09/09/14 -- Segundo o manual que eu tenho (v. de maio de 2014) n�o � 82 este c�digo
        NaoReceberAposNDias = 91,
        DevolverAposNDias = 92,
        MultaVencimento = 997,
        JurosdeMora = 998,
        DescontoporDia = 999,
    }

    #endregion 

    public class Instrucao_Itau: AbstractInstrucao, IInstrucao
    {

        #region Construtores 

		public Instrucao_Itau()
		{
			try
			{
                this.Banco = new Banco(341);
			}
			catch (Exception ex)
			{
                throw new Exception("Erro ao carregar objeto", ex);
			}
		}

        public Instrucao_Itau(int codigo, int nrDias)
        {
            this.carregar(codigo, nrDias);
        }

        public Instrucao_Itau(int codigo)
        {
            this.carregar(codigo, 0);
        }
        public Instrucao_Itau(int codigo, double valor)
        {
            this.carregar(codigo, valor);
        }
		#endregion 

        #region Metodos Privados

        private void carregar(int idInstrucao, int nrDias)
        {
            try
            {
                this.Banco = new Banco_Itau();
                this.Valida();

                switch ((EnumInstrucoes_Itau)idInstrucao)
                {
                    case EnumInstrucoes_Itau.Protestar:
                        this.Codigo = (int)EnumInstrucoes_Itau.Protestar;
                        this.Descricao = "Protestar ap�s 5 dias �teis.";
                        break;
                    case EnumInstrucoes_Itau.NaoProtestar:
                        this.Codigo = (int)EnumInstrucoes_Itau.NaoProtestar;
                        this.Descricao = "N�o protestar";
                        break;
                    case EnumInstrucoes_Itau.ImportanciaporDiaDesconto:
                        this.Codigo = (int)EnumInstrucoes_Itau.ImportanciaporDiaDesconto;
                        this.Descricao = "Import�ncia por dia de desconto.";
                        break;
                    case EnumInstrucoes_Itau.ProtestoFinsFalimentares:
                        this.Codigo = (int)EnumInstrucoes_Itau.ProtestoFinsFalimentares;
                        this.Descricao = "Protesto para fins falimentares";
                        break;
                    case EnumInstrucoes_Itau.ProtestarAposNDiasCorridos:
                        this.Codigo = (int)EnumInstrucoes_Itau.ProtestarAposNDiasCorridos;
                        this.Descricao = "Protestar ap�s " + nrDias + " dias corridos do vencimento";
                        break;
                    case EnumInstrucoes_Itau.ProtestarAposNDiasUteis:
                        this.Codigo = (int)EnumInstrucoes_Itau.ProtestarAposNDiasUteis;
                        this.Descricao = "Protestar ap�s " + nrDias + " dias �teis do vencimento";
                        break;
                    case EnumInstrucoes_Itau.NaoReceberAposNDias:
                        this.Codigo = (int)EnumInstrucoes_Itau.NaoReceberAposNDias;
                        this.Descricao = "N�o receber ap�s N dias do vencimento";
                        break;
                    case EnumInstrucoes_Itau.DevolverAposNDias:
                        this.Codigo = (int)EnumInstrucoes_Itau.DevolverAposNDias;
                        this.Descricao = "Devolver ap�s N dias do vencimento";
                        break;                  
                    case EnumInstrucoes_Itau.DescontoporDia:
                        this.Codigo = (int)EnumInstrucoes_Itau.DescontoporDia;
                        this.Descricao = "Conceder desconto de R$ "; // por dia de antecipa��o
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = "( Selecione )";
                        break;
                }

                this.QuantidadeDias = nrDias;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        private void carregar(int idInstrucao, double valor)
        {
            try
            {
                this.Banco = new Banco_Itau();
                this.Valida();

                switch ((EnumInstrucoes_Itau)idInstrucao)
                {
                    case EnumInstrucoes_Itau.JurosdeMora:
                        this.Codigo = (int)EnumInstrucoes_Itau.JurosdeMora;
                        this.Descricao = "Ap�s vencimento cobrar mora di�ria de " + valor + " %";
                        break;
                    case EnumInstrucoes_Itau.MultaVencimento:
                        this.Codigo = (int)EnumInstrucoes_Itau.MultaVencimento;
                        this.Descricao = "Ap�s vencimento cobrar multa de " + valor + " %";
                        break;
                }
            }
             catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public override void Valida()
        {
            //base.Valida();
        }

        #endregion

    }
}
