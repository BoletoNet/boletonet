using System;
using System.Collections;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_Uniprime
    {
        Protestar = 9,
        NaoProtestar = 10,
        ProtestoFinsFalimentares = 42,
        ProtestarAposNDiasCorridos = 81,
        ProtestarAposNDiasUteis = 82,
        NaoReceberAposNDias = 91,
        DevolverAposNDias = 92,
        ComDesconto = 93,
        BoletoOriginal = 94,

        OutrasInstrucoes_ExibeMensagem_MoraDiaria = 900,
        OutrasInstrucoes_ExibeMensagem_MultaVencimento = 901
    }

    #endregion 

    public class Instrucao_Uniprime : AbstractInstrucao, IInstrucao
    {

        #region Construtores 

		public Instrucao_Uniprime()
		{
			try
			{
                this.Banco = new Banco(237);
			}
			catch (Exception ex)
			{
                throw new Exception("Erro ao carregar objeto", ex);
			}
		}

        public Instrucao_Uniprime(int codigo)
        {
            this.carregar(codigo, 0);
        }

        public Instrucao_Uniprime(int codigo, int nrDias)
        {
            this.carregar(codigo, nrDias);
        }
        public Instrucao_Uniprime(int codigo, double valor)
        {
            this.carregar(codigo, valor);
        }

        public Instrucao_Uniprime(int codigo, double valor, EnumTipoValor tipoValor)
        {
            this.carregar(codigo, valor, tipoValor);
        }

        public Instrucao_Uniprime(int codigo, double valor, DateTime data, EnumTipoValor tipoValor)
        {
            this.carregar(codigo, valor, data, tipoValor);
        }

        #endregion Construtores

        #region Metodos Privados

        private void carregar(int idInstrucao, double valor, EnumTipoValor tipoValor = EnumTipoValor.Percentual)
        {
            try
            {
                this.Banco = new Banco_Bradesco();
                this.Valida();

                switch ((EnumInstrucoes_Uniprime)idInstrucao)
                {
                    case EnumInstrucoes_Uniprime.OutrasInstrucoes_ExibeMensagem_MoraDiaria:
                        this.Codigo = 0;
                        this.Descricao = String.Format("Após vencimento cobrar juros de {0} {1} por dia de atraso",
                            (tipoValor.Equals(EnumTipoValor.Reais) ? "R$ " : valor.ToString("F2")),
                            (tipoValor.Equals(EnumTipoValor.Percentual) ? "%" : valor.ToString("F2")));
                        break;
                    case EnumInstrucoes_Uniprime.OutrasInstrucoes_ExibeMensagem_MultaVencimento:
                        this.Codigo = 0;
                        this.Descricao = String.Format("Após vencimento cobrar multa de {0} {1}",
                            (tipoValor.Equals(EnumTipoValor.Reais) ? "R$ " : valor.ToString("F2")),
                            (tipoValor.Equals(EnumTipoValor.Percentual) ? "%" : valor.ToString("F2")));
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = " (Selecione) ";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        private void carregar(int idInstrucao, int nrDias)
        {
            try
            {
                this.Banco = new Banco_Uniprime();
                this.Valida();

                switch ((EnumInstrucoes_Uniprime)idInstrucao)
                {
                    case EnumInstrucoes_Uniprime.Protestar:
                        this.Codigo = (int)EnumInstrucoes_Uniprime.Protestar;
                        this.Descricao = "Protestar";
                        break;
                    case EnumInstrucoes_Uniprime.NaoProtestar:
                        this.Codigo = (int)EnumInstrucoes_Uniprime.NaoProtestar;
                        this.Descricao = "Não protestar";
                        break;
                    case EnumInstrucoes_Uniprime.ProtestoFinsFalimentares:
                        this.Codigo = (int)EnumInstrucoes_Uniprime.ProtestoFinsFalimentares;
                        this.Descricao = "Protesto para fins falimentares";
                        break;
                    case EnumInstrucoes_Uniprime.ProtestarAposNDiasCorridos:
                        this.Codigo = (int)EnumInstrucoes_Uniprime.ProtestarAposNDiasCorridos;
                        this.Descricao = "Protestar após " + nrDias + " dias corridos do vencimento";
                        break;
                    case EnumInstrucoes_Uniprime.ProtestarAposNDiasUteis:
                        this.Codigo = (int)EnumInstrucoes_Uniprime.ProtestarAposNDiasUteis;
                        this.Descricao = "Protestar após " + nrDias + " dias úteis do vencimento";
                        break;
                    case EnumInstrucoes_Uniprime.NaoReceberAposNDias:
                        this.Codigo = (int)EnumInstrucoes_Uniprime.NaoReceberAposNDias;
                        this.Descricao = "Não receber após " + nrDias + " dias do vencimento";
                        break;
                    case EnumInstrucoes_Uniprime.DevolverAposNDias:
                        this.Codigo = (int)EnumInstrucoes_Uniprime.DevolverAposNDias;
                        this.Descricao = "Devolver após " + nrDias + " dias do vencimento";
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

        private void carregar(int idInstrucao, double valor, DateTime data, EnumTipoValor tipoValor = EnumTipoValor.Reais)
        {
            try
            {
                this.Banco = new Banco_Uniprime();
                this.Valida();

                switch ((EnumInstrucoes_Uniprime)idInstrucao)
                {
                    case EnumInstrucoes_Uniprime.ComDesconto:
                        this.Codigo = (int)EnumInstrucoes_Uniprime.ComDesconto;
                        this.Descricao = String.Format("Desconto de pontualidade no valor de {0} {1} se pago até " + data.ToShortDateString(),
                            (tipoValor.Equals(EnumTipoValor.Reais) ? "R$ " : valor.ToString("C")),
                            (tipoValor.Equals(EnumTipoValor.Percentual) ? "%" : valor.ToString("F2")));
                        break;
                    case EnumInstrucoes_Uniprime.BoletoOriginal:
                        this.Codigo = (int)EnumInstrucoes_Uniprime.BoletoOriginal;
                        this.Descricao = "Vencimento " + data.ToShortDateString() + ", no valor de " + valor.ToString("C") + "";
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = " (Selecione) ";
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
