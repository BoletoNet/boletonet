using System;
using System.Collections;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_Banrisul
    {
        NaoDispensarComissaoPermanencia = 1, //01 - Não dispensar comissão de permanência
        NaoCobrarComissaoPermanencia = 8,    //08 - Não cobrar comissão de permanência 
        Protestar = 9,                       //09 - Protestar caso impago NN dias após vencimento (posições 370-371 = NN). Obs.: O número de dias para protesto deverá ser igual ou maior do que '03'. 
        DevolverAposNDias = 15,              //15 - Devolver se impago após NN dias do vencimento (posições 370-371 = NN). Obs.: Para o número de dias igual a '00' será impresso no bloqueto: 'NÃO RECEBER APÓS O VENCIMENTO'.
        CobrarMultaAposNDias = 18,           //18 - Após NN dias do vencimento, cobrar xx,x% de multa. 
        CobrarMultaOuFracaoAposNDias = 20,   //20 - Após NN dias do vencimento, cobrar xx,x% de multa ao mês ou fração. 
        NaoProtestar = 23,                   //23 - Não protestar.
    }

    #endregion

    public class Instrucao_Banrisul : AbstractInstrucao, IInstrucao
    {

        #region Construtores

        public Instrucao_Banrisul()
        {
            try
            {
                this.Banco = new Banco(41);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public Instrucao_Banrisul(int codigo)
        {
            this.carregar(codigo, 0, 0);
        }

        public Instrucao_Banrisul(int codigo, int nrDias)
        {
            this.carregar(codigo, nrDias, (double)0.0);
        }

        public Instrucao_Banrisul(int codigo, double percentualMultaDia)
        {
            this.carregar(codigo, 0, percentualMultaDia);
        }

        public Instrucao_Banrisul(int codigo, int nrDias, double percentualMultaDia)
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

                switch ((EnumInstrucoes_Banrisul)idInstrucao)
                {
                    case EnumInstrucoes_Banrisul.NaoDispensarComissaoPermanencia:
                        this.Codigo = (int)EnumInstrucoes_Banrisul.NaoDispensarComissaoPermanencia;
                        this.Descricao = "Não dispensar comissão de permanência"; //01
                        break;
                    case EnumInstrucoes_Banrisul.NaoCobrarComissaoPermanencia:
                        this.Codigo = (int)EnumInstrucoes_Banrisul.NaoCobrarComissaoPermanencia;
                        this.Descricao = "Não cobrar comissão de permanência"; //08
                        break;
                    case EnumInstrucoes_Banrisul.Protestar:
                        this.Codigo = (int)EnumInstrucoes_Banrisul.Protestar;
                        this.Descricao = "Protestar caso impago " + nrDias + " dias após vencimento"; //09
                        break;
                    case EnumInstrucoes_Banrisul.DevolverAposNDias:
                        this.Codigo = (int)EnumInstrucoes_Banrisul.DevolverAposNDias;
                        this.Descricao = "Devolver se impago após " + nrDias + " dias do vencimento"; //15
                        break;
                    case EnumInstrucoes_Banrisul.CobrarMultaAposNDias:
                        this.Codigo = (int)EnumInstrucoes_Banrisul.CobrarMultaAposNDias;
                        this.Descricao = "Após " + nrDias + " dias do vencimento, cobrar " + percentualMultaDia + "% de multa"; //18
                        break;
                    case EnumInstrucoes_Banrisul.CobrarMultaOuFracaoAposNDias:
                        this.Codigo = (int)EnumInstrucoes_Banrisul.CobrarMultaOuFracaoAposNDias;
                        this.Descricao = "Após " + nrDias + " dias do vencimento, cobrar " + percentualMultaDia + "% de multa ao mês ou fração"; //20
                        break;
                    case EnumInstrucoes_Banrisul.NaoProtestar:
                        this.Codigo = (int)EnumInstrucoes_Banrisul.NaoProtestar;
                        this.Descricao = "Não protestar"; //23
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