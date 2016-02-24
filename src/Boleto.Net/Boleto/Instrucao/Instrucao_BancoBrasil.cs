using System;
using System.Collections;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_BancoBrasil
    {
        Protestar = 9,                      // Emite aviso ao sacado após N dias do vencto, e envia ao cartório após 5 dias úteis
        NaoProtestar = 10,                  // Inibe protesto, quando houver instrução permanente na conta corrente
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
                        this.Descricao = "Após vencimento cobrar multa de " + valor + " %";
                        break;
                    case EnumInstrucoes_BancoBrasil.JurosdeMora:
                        this.Codigo = (int)EnumInstrucoes_BancoBrasil.JurosdeMora;
                        this.Descricao = "Após vencimento cobrar R$ " + valor + " por dia de atraso";
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
                        this.Descricao = "Protestar após " + nrDias + " dias úteis.";
                        break;
                    case EnumInstrucoes_BancoBrasil.NaoProtestar:
                        this.Codigo = (int)EnumInstrucoes_BancoBrasil.NaoProtestar;
                        this.Descricao = "Não protestar";
                        break;
                    case EnumInstrucoes_BancoBrasil.ImportanciaporDiaDesconto:
                        this.Codigo = (int)EnumInstrucoes_BancoBrasil.ImportanciaporDiaDesconto;
                        this.Descricao = "Importância por dia de desconto.";
                        break;
                    case EnumInstrucoes_BancoBrasil.ProtestoFinsFalimentares:
                        this.Codigo = (int)EnumInstrucoes_BancoBrasil.ProtestoFinsFalimentares;
                        this.Descricao = "Protesto para fins falimentares";
                        break;
                    case EnumInstrucoes_BancoBrasil.ProtestarAposNDiasCorridos:
                        this.Codigo = (int)EnumInstrucoes_BancoBrasil.ProtestarAposNDiasCorridos;
                        this.Descricao = "Protestar no " + nrDias + "º dia corrido após vencimento";
                        break;
                    case EnumInstrucoes_BancoBrasil.ProtestarAposNDiasUteis:
                        this.Codigo = (int)EnumInstrucoes_BancoBrasil.ProtestarAposNDiasUteis;
                        this.Descricao = "Protestar no " + nrDias + "º dia útil após vencimento";/*Jéferson (jefhtavares) em 02/12/2013 a pedido do setor de homologação do BB*/
                        break;
                    case EnumInstrucoes_BancoBrasil.NaoReceberAposNDias:
                        this.Codigo = (int)EnumInstrucoes_BancoBrasil.NaoReceberAposNDias;
                        this.Descricao = "Não receber após " + nrDias + " dias do vencimento";
                        break;
                    case EnumInstrucoes_BancoBrasil.DevolverAposNDias:
                        this.Codigo = (int)EnumInstrucoes_BancoBrasil.DevolverAposNDias;
                        this.Descricao = "Devolver após " + nrDias + " dias do vencimento";
                        break;
                    case EnumInstrucoes_BancoBrasil.JurosdeMora:
                        this.Codigo = (int)EnumInstrucoes_BancoBrasil.JurosdeMora;
                        this.Descricao = "Após vencimento cobrar R$ "; // por dia de atraso
                        break;
                    case EnumInstrucoes_BancoBrasil.DescontoporDia:
                        this.Codigo = (int)EnumInstrucoes_BancoBrasil.DescontoporDia;
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

        public override void Valida()
        {
            //base.Valida();
        }

        #endregion

    }
}
