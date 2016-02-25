using System;
using System.Collections;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_BancoBrasil
    {
        Protestar = 9,                      // Emite aviso ao sacado ap�s N dias do vencto, e envia ao cart�rio ap�s 5 dias �teis
        NaoProtestar = 10,                  // Inibe protesto, quando houver instru��o permanente na conta corrente
        ImportanciaporDiaDesconto = 30,
        Percentual_Multa = 35,
        ProtestoFinsFalimentares = 42,
        ProtestarAposNDiasCorridos = 81,
        ProtestarAposNDiasUteis = 82,
        NaoReceberAposNDias = 91,
        DevolverAposNDias = 92,
        JurosdeMora = 998,
        DescontoporDia = 999,
    }

    #endregion

    public class Instrucao_BancoBrasil : AbstractInstrucao, IInstrucao
    {

        #region Construtores

        public Instrucao_BancoBrasil()
        {
            try
            {
                this.Banco = new Banco(001);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public Instrucao_BancoBrasil(int codigo)
        {
            this.carregar(codigo, 0);
        }

        public Instrucao_BancoBrasil(int codigo, int nrDias)
        {
            this.carregar(codigo, nrDias);
        }

        public Instrucao_BancoBrasil(int codigo, double valor)
        {
            this.carregar(codigo, valor);
        }
        #endregion

        #region Metodos Privados

        private void carregar(int idInstrucao, double valor)
        {
            try
            {
                this.Banco = new Banco_Brasil();
                this.Valida();

                switch ((EnumInstrucoes_BancoBrasil)idInstrucao)
                {
                    case EnumInstrucoes_BancoBrasil.Percentual_Multa:
                        this.Codigo = (int)EnumInstrucoes_BancoBrasil.Percentual_Multa;
                        this.Descricao = "Ap�s vencimento cobrar multa de " + valor + " %";
                        break;
                    case EnumInstrucoes_BancoBrasil.JurosdeMora:
                        this.Codigo = (int)EnumInstrucoes_BancoBrasil.JurosdeMora;
                        this.Descricao = "Ap�s vencimento cobrar R$ " + valor + " por dia de atraso";
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
                this.Banco = new Banco_Brasil();
                this.Valida();

                switch ((EnumInstrucoes_BancoBrasil)idInstrucao)
                {
                    case EnumInstrucoes_BancoBrasil.Protestar:
                        this.Codigo = (int)EnumInstrucoes_BancoBrasil.Protestar;
                        this.Descricao = "Protestar ap�s " + nrDias + " dias �teis.";
                        break;
                    case EnumInstrucoes_BancoBrasil.NaoProtestar:
                        this.Codigo = (int)EnumInstrucoes_BancoBrasil.NaoProtestar;
                        this.Descricao = "N�o protestar";
                        break;
                    case EnumInstrucoes_BancoBrasil.ImportanciaporDiaDesconto:
                        this.Codigo = (int)EnumInstrucoes_BancoBrasil.ImportanciaporDiaDesconto;
                        this.Descricao = "Import�ncia por dia de desconto.";
                        break;
                    case EnumInstrucoes_BancoBrasil.ProtestoFinsFalimentares:
                        this.Codigo = (int)EnumInstrucoes_BancoBrasil.ProtestoFinsFalimentares;
                        this.Descricao = "Protesto para fins falimentares";
                        break;
                    case EnumInstrucoes_BancoBrasil.ProtestarAposNDiasCorridos:
                        this.Codigo = (int)EnumInstrucoes_BancoBrasil.ProtestarAposNDiasCorridos;
                        this.Descricao = "Protestar no " + nrDias + "� dia corrido ap�s vencimento";
                        break;
                    case EnumInstrucoes_BancoBrasil.ProtestarAposNDiasUteis:
                        this.Codigo = (int)EnumInstrucoes_BancoBrasil.ProtestarAposNDiasUteis;
                        this.Descricao = "Protestar no " + nrDias + "� dia �til ap�s vencimento";/*J�ferson (jefhtavares) em 02/12/2013 a pedido do setor de homologa��o do BB*/
                        break;
                    case EnumInstrucoes_BancoBrasil.NaoReceberAposNDias:
                        this.Codigo = (int)EnumInstrucoes_BancoBrasil.NaoReceberAposNDias;
                        this.Descricao = "N�o receber ap�s " + nrDias + " dias do vencimento";
                        break;
                    case EnumInstrucoes_BancoBrasil.DevolverAposNDias:
                        this.Codigo = (int)EnumInstrucoes_BancoBrasil.DevolverAposNDias;
                        this.Descricao = "Devolver ap�s " + nrDias + " dias do vencimento";
                        break;
                    case EnumInstrucoes_BancoBrasil.JurosdeMora:
                        this.Codigo = (int)EnumInstrucoes_BancoBrasil.JurosdeMora;
                        this.Descricao = "Ap�s vencimento cobrar R$ "; // por dia de atraso
                        break;
                    case EnumInstrucoes_BancoBrasil.DescontoporDia:
                        this.Codigo = (int)EnumInstrucoes_BancoBrasil.DescontoporDia;
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

        public override void Valida()
        {
            //base.Valida();
        }

        #endregion

    }
}
