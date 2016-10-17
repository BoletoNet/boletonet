using System;
using System.Collections;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_Itau
    {
        Protestar = 9,                      // Emite aviso ao sacado após N dias do vencto, e envia ao cartório após 5 dias úteis
        NaoProtestar = 10,                  // Inibe protesto, quando houver instrução permanente na conta corrente
        ImportanciaporDiaDesconto = 30,
        ProtestoFinsFalimentares = 42,
        ProtestarAposNDiasCorridos = 34, //Jéferson (jefhtavares) em 09/09/14 -- Segundo o manual que eu tenho (v. de maio de 2014) não é 81 este código
        ProtestarAposNDiasUteis = 35, //Jéferson (jefhtavares) em 09/09/14 -- Segundo o manual que eu tenho (v. de maio de 2014) não é 82 este código
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

        public Instrucao_Itau(int codigo, double valor, EnumTipoValor tipoValor)
        {
            this.carregar(codigo, valor, tipoValor);
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
                        this.Descricao = "Protestar após 5 dias úteis.";
                        break;
                    case EnumInstrucoes_Itau.NaoProtestar:
                        this.Codigo = (int)EnumInstrucoes_Itau.NaoProtestar;
                        this.Descricao = "Não protestar";
                        break;
                    case EnumInstrucoes_Itau.ImportanciaporDiaDesconto:
                        this.Codigo = (int)EnumInstrucoes_Itau.ImportanciaporDiaDesconto;
                        this.Descricao = "Importância por dia de desconto.";
                        break;
                    case EnumInstrucoes_Itau.ProtestoFinsFalimentares:
                        this.Codigo = (int)EnumInstrucoes_Itau.ProtestoFinsFalimentares;
                        this.Descricao = "Protesto para fins falimentares";
                        break;
                    case EnumInstrucoes_Itau.ProtestarAposNDiasCorridos:
                        this.Codigo = (int)EnumInstrucoes_Itau.ProtestarAposNDiasCorridos;
                        this.Descricao = "Protestar após " + nrDias + " dias corridos do vencimento";
                        break;
                    case EnumInstrucoes_Itau.ProtestarAposNDiasUteis:
                        this.Codigo = (int)EnumInstrucoes_Itau.ProtestarAposNDiasUteis;
                        this.Descricao = "Protestar após " + nrDias + " dias úteis do vencimento";
                        break;
                    case EnumInstrucoes_Itau.NaoReceberAposNDias:
                        this.Codigo = (int)EnumInstrucoes_Itau.NaoReceberAposNDias;
                        this.Descricao = "Não receber após N dias do vencimento";
                        break;
                    case EnumInstrucoes_Itau.DevolverAposNDias:
                        this.Codigo = (int)EnumInstrucoes_Itau.DevolverAposNDias;
                        this.Descricao = "Devolver após N dias do vencimento";
                        break;                  
                    case EnumInstrucoes_Itau.DescontoporDia:
                        this.Codigo = (int)EnumInstrucoes_Itau.DescontoporDia;
                        this.Descricao = "Conceder desconto de R$ "; // por dia de antecipação
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

        private void carregar(int idInstrucao, double valor, EnumTipoValor tipoValor = EnumTipoValor.Percentual)
        {
            try
            {
                this.Banco = new Banco_Itau();
                this.Valida();

                switch ((EnumInstrucoes_Itau)idInstrucao)
                {
                    case EnumInstrucoes_Itau.JurosdeMora:
                        this.Codigo = (int)EnumInstrucoes_Itau.JurosdeMora;   
                        this.Descricao = String.Format("Após vencimento cobrar juros de {0} {1} por dia de atraso",
                            (tipoValor.Equals(EnumTipoValor.Reais) ? "R$ " : valor.ToString("F2")),
                            (tipoValor.Equals(EnumTipoValor.Percentual) ? "%" : valor.ToString("F2")));
                        break;
                    case EnumInstrucoes_Itau.MultaVencimento:
                        this.Codigo = (int)EnumInstrucoes_Itau.MultaVencimento;
                        this.Descricao = String.Format("Após vencimento cobrar multa de {0} {1}",
                            (tipoValor.Equals(EnumTipoValor.Reais) ? "R$ " : valor.ToString("F2")),
                            (tipoValor.Equals(EnumTipoValor.Percentual) ? "%" : valor.ToString("F2")));
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
