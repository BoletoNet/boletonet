using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerador

    public enum EnumEspecieDocumento_Caixa
    {
        DuplicataMercantil,
        NotaPromissoria,
        DuplicataServico,
        NotaSeguro,
        LetraCambio,
        Outros
    }

    #endregion

    public class EspecieDocumento_Caixa : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_Caixa()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumento_Caixa(string codigo)
        {
            try
            {
                this.carregar(codigo);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        #endregion

        #region Metodos Privados

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_Caixa.DuplicataMercantil: return "1";
                case EnumEspecieDocumento_Caixa.NotaPromissoria : return "2";
                case EnumEspecieDocumento_Caixa.DuplicataServico: return "3";
                case EnumEspecieDocumento_Caixa.NotaSeguro : return "5";
                case EnumEspecieDocumento_Caixa.LetraCambio : return "6";
                case EnumEspecieDocumento_Caixa.Outros: return "9";
                default: return "23";
            }
        }

        public EnumEspecieDocumento_Caixa getEnumEspecieByCodigo(string codigo)
        {
            switch (codigo)
            {
                case "1": return EnumEspecieDocumento_Caixa.DuplicataMercantil;
                case "2": return EnumEspecieDocumento_Caixa.NotaPromissoria;
                case "3": return EnumEspecieDocumento_Caixa.DuplicataServico;
                case "5": return EnumEspecieDocumento_Caixa.NotaSeguro;
                case "6": return EnumEspecieDocumento_Caixa.LetraCambio;
                case "9": return EnumEspecieDocumento_Caixa.Outros;
                default: return EnumEspecieDocumento_Caixa.DuplicataMercantil;
            }
        }

        private void carregar(string idCodigo)
        {
            try
            {
                this.Banco = new Banco_Caixa();

                switch (getEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_Caixa.DuplicataMercantil:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa.DuplicataMercantil);
                        this.Especie = "DUPLICATA MERCANTIL";
                        this.Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_Caixa.NotaPromissoria:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa.NotaPromissoria);
                        this.Especie = "NOTA PROMISSORIA";
                        this.Sigla = "NP";
                        break;
                    case EnumEspecieDocumento_Caixa.DuplicataServico:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa.DuplicataServico);
                        this.Especie = "DUPLICATA DE PRESTACAO DE SERVICOS";
                        this.Sigla = "DS";
                        break;
                    case EnumEspecieDocumento_Caixa.NotaSeguro:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa.NotaSeguro);
                        this.Especie = "NOTA DE SEGURO";
                        this.Sigla = "NS";
                        break;
                    case EnumEspecieDocumento_Caixa.LetraCambio:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa.LetraCambio);
                        this.Especie = "LETRA DE CAMBIO";
                        this.Sigla = "LC";
                        break;
                    case EnumEspecieDocumento_Caixa.Outros:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa.Outros);
                        this.Especie = "OUTROS";
                        this.Sigla = "OU";
                        break;
                    default:
                        this.Codigo = "0";
                        this.Especie = "( Selecione )";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public static EspeciesDocumento CarregaTodas()
        {
            EspeciesDocumento especiesDocumento = new EspeciesDocumento();
            EspecieDocumento_Caixa ed = new EspecieDocumento_Caixa();

            foreach (EnumEspecieDocumento_Caixa item in Enum.GetValues(typeof(EnumEspecieDocumento_Caixa)))
                especiesDocumento.Add(new EspecieDocumento_Caixa(ed.getCodigoEspecieByEnum(item)));

            return especiesDocumento;
        }

        #endregion
    }
}
