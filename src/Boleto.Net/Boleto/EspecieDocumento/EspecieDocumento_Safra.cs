using System;

namespace BoletoNet
{
    /// <Author>
    /// Carlos Rogério - Aracaju/SE
    /// </Author>

    #region Enumerador

    public enum EnumEspecieDocumento_Safra
    {
        DuplicataMercantil,
        NotaPromissoria,
        DuplicataServico,
        NotaSeguro,
        LetraCambio,
        Fatura,
        Outros
    }

    #endregion

    public class EspecieDocumento_Safra : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_Safra()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumento_Safra(string codigo)
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

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_Safra especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_Safra.DuplicataMercantil: return "2";                
                case EnumEspecieDocumento_Safra.DuplicataServico: return "4";                
                case EnumEspecieDocumento_Safra.LetraCambio: return "7";
                case EnumEspecieDocumento_Safra.NotaPromissoria: return "12";
                case EnumEspecieDocumento_Safra.NotaSeguro: return "16";
                case EnumEspecieDocumento_Safra.Fatura: return "18";
                case EnumEspecieDocumento_Safra.Outros: return "99";
                default: return "2";
            }
        }

        public EnumEspecieDocumento_Safra getEnumEspecieByCodigo(string codigo)
        {
            switch (codigo)
            {
                case "2": return EnumEspecieDocumento_Safra.DuplicataMercantil;                
                case "4": return EnumEspecieDocumento_Safra.DuplicataServico;
                case "7": return EnumEspecieDocumento_Safra.LetraCambio;
                case "12": return EnumEspecieDocumento_Safra.NotaPromissoria;
                case "16": return EnumEspecieDocumento_Safra.NotaSeguro;
                case "18": return EnumEspecieDocumento_Safra.Fatura;
                case "99": return EnumEspecieDocumento_Safra.Outros;
                default: return EnumEspecieDocumento_Safra.DuplicataMercantil;
            }
        }

        private void carregar(string idCodigo)
        {
            try
            {
                this.Banco = new Banco_Safra();

                switch (getEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_Safra.DuplicataMercantil:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.DuplicataMercantil);
                        this.Especie = "DUPLICATA MERCANTIL";
                        this.Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_Safra.NotaPromissoria:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.NotaPromissoria);
                        this.Especie = "NOTA PROMISSORIA";
                        this.Sigla = "NP";
                        break;
                    case EnumEspecieDocumento_Safra.DuplicataServico:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.DuplicataServico);
                        this.Especie = "DUPLICATA DE SERVICOS";
                        this.Sigla = "DS";
                        break;
                    case EnumEspecieDocumento_Safra.NotaSeguro:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.NotaSeguro);
                        this.Especie = "NOTA DE SEGURO";
                        this.Sigla = "NS";
                        break;
                    case EnumEspecieDocumento_Safra.LetraCambio:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.LetraCambio);
                        this.Especie = "LETRA DE CAMBIO";
                        this.Sigla = "LC";
                        break;
                    case EnumEspecieDocumento_Safra.Fatura:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.Fatura);
                        this.Especie = "FATURA";
                        this.Sigla = "FAT";
                        break;
                    case EnumEspecieDocumento_Safra.Outros:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.Outros);
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
            EspecieDocumento_Safra ed = new EspecieDocumento_Safra();

            foreach (EnumEspecieDocumento_Safra item in Enum.GetValues(typeof(EnumEspecieDocumento_Safra)))
                especiesDocumento.Add(new EspecieDocumento_Safra(ed.getCodigoEspecieByEnum(item)));

            return especiesDocumento;
        }

        public override IEspecieDocumento DuplicataMercantil()
        {
            return new EspecieDocumento_Safra(getCodigoEspecieByEnum(EnumEspecieDocumento_Safra.DuplicataMercantil));
        }

        #endregion
    }
}
