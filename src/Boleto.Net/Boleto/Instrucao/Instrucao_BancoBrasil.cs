using System;
using System.Collections;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_BancoBrasil
    {
        Multa = 8,
        Protestar = 9,                      // Emite aviso ao sacado após N dias do vencto, e envia ao cartório após 5 dias úteis
        NaoProtestar = 10,                  // Inibe protesto, quando houver instrução permanente na conta corrente
        ImportanciaporDiaDesconto = 30,
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
                Banco = new Banco(001);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public Instrucao_BancoBrasil(int codigo)
        {
            Carregar(codigo, 0);
        }

        public Instrucao_BancoBrasil(int codigo, int nrDias)
        {
            Carregar(codigo, nrDias);
        }

        public Instrucao_BancoBrasil(int codigo, decimal valor)
        {
            Carregar(codigo, valor);
        }
        #endregion

        #region Metodos Privados

        public override bool Carregar(int idInstrucao, decimal valor)
        {
            try
            {
                this.Banco = new Banco_Brasil();
                this.Valida();

                switch ((EnumInstrucoes_BancoBrasil)idInstrucao)
                {
                    case EnumInstrucoes_BancoBrasil.Multa:
                        this.Codigo = (int)EnumInstrucoes_BancoBrasil.Multa;
                        this.Descricao = "Após vencimento cobrar Multa de " + valor + "%";
                        break;
                    case EnumInstrucoes_BancoBrasil.JurosdeMora:
                        this.Codigo = 1;
                        this.Descricao = "Após vencimento cobrar R$ " + valor + " por dia de atraso";
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = "";
                        break;
                }

                return true;
            } catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public override bool Carregar(int idInstrucao, int nrDias)
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
                        this.Codigo = 7;
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
                        if (nrDias >= 45) {
                            Codigo = 45;
                        } else if (nrDias >= 30) {
                            Codigo = 30;
                        } else if (nrDias >= 25) {
                            Codigo = 25;
                        } else if (nrDias >= 20) {
                            Codigo = 20;
                        } else if (nrDias >= 15) {
                            Codigo = 15;
                        } else if (nrDias <= 14) {
                            Codigo = 10;
                        }

                        Descricao = "Protestar no " + nrDias + "º dia corrido após vencimento";

                        break;
                    case EnumInstrucoes_BancoBrasil.ProtestarAposNDiasUteis:
                        if(nrDias == 3) {
                            Codigo = 3;
                        } else if(nrDias == 4) {
                            Codigo = 4;
                        } else if(nrDias >= 5) {
                            Codigo = 5;
                        }

                        Descricao = "Protestar no " + nrDias + "º dia útil após vencimento";

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
                        this.Codigo = 1;
                        this.Descricao = "Após vencimento cobrar R$ "; // por dia de atraso
                        break;
                    case EnumInstrucoes_BancoBrasil.DescontoporDia:
                        this.Codigo = 22;
                        this.Descricao = "Conceder desconto de R$ "; // por dia de antecipação
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = "";
                        break;
                }

                this.QuantidadeDias = nrDias;

                return true;
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
