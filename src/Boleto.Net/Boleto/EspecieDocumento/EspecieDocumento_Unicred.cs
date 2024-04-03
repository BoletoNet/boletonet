using System;

namespace BoletoNet {
    #region Enumerado

    public enum EnumEspecieDocumento_Unicred {
        DuplicataMercantil,
        NotaPromissoria,
        NotaSeguros,
        CobrancaSeriada,
        Recibo,
        LetraCambio,
        NotaDebito,
        DuplicataServico,
        Outros
    }

    #endregion

    public class EspecieDocumento_Unicred : AbstractEspecieDocumento, IEspecieDocumento {
        public string getCodigoEspecieByEnum(EnumEspecieDocumento_Unicred especie) {
            switch (especie)
            {
                case EnumEspecieDocumento_Unicred.DuplicataMercantil: return "DM";
                case EnumEspecieDocumento_Unicred.NotaPromissoria: return "NP";
                case EnumEspecieDocumento_Unicred.NotaSeguros: return "NS";
                case EnumEspecieDocumento_Unicred.Recibo: return "REC";
                case EnumEspecieDocumento_Unicred.LetraCambio: return "LC";
                case EnumEspecieDocumento_Unicred.NotaDebito: return "ND";
                case EnumEspecieDocumento_Unicred.CobrancaSeriada: return "CS";
                case EnumEspecieDocumento_Unicred.DuplicataServico: return "DS";
                case EnumEspecieDocumento_Unicred.Outros: return "O";
                default: return "K";
            }
        }

        public static EnumEspecieDocumento_Unicred getEnumEspecieByCodigo(string codigo) {
            switch (codigo)
            {
                case "DM": return EnumEspecieDocumento_Unicred.DuplicataMercantil;
                case "NP": return EnumEspecieDocumento_Unicred.NotaPromissoria;
                case "NS": return EnumEspecieDocumento_Unicred.NotaSeguros;
                case "REC": return EnumEspecieDocumento_Unicred.Recibo;
                case "LC": return EnumEspecieDocumento_Unicred.LetraCambio;
                case "ND": return EnumEspecieDocumento_Unicred.NotaDebito;
                case "CS": return EnumEspecieDocumento_Unicred.DuplicataServico;
                case "DS": return EnumEspecieDocumento_Unicred.NotaDebito;
                default: return EnumEspecieDocumento_Unicred.Outros;
            }
        }

        public override string getCodigoEspecieBySigla(string sigla) { return sigla; }

        #region Construtores

        public EspecieDocumento_Unicred() {
            try { }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumento_Unicred(string codigo) {
            try
            {
                carregar(codigo);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        #endregion

        #region Metodos Privados

        private void carregar(string idCodigo) {
            try
            {
                Banco = new Banco_Unicred();

                switch (getEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_Unicred.DuplicataMercantil:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Unicred.DuplicataMercantil);
                        Especie = "Duplicata Mercantil";
                        Sigla = idCodigo;
                        break;
                    case EnumEspecieDocumento_Unicred.NotaPromissoria:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Unicred.NotaPromissoria);
                        Especie = "Nota Promissória";
                        Sigla = idCodigo;
                        break;
                    case EnumEspecieDocumento_Unicred.NotaSeguros:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Unicred.NotaSeguros);
                        Especie = "Nota de Seguros";
                        Sigla = idCodigo;
                        break;
                    case EnumEspecieDocumento_Unicred.CobrancaSeriada:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Unicred.CobrancaSeriada);
                        Especie = "Cobrança Seriada,";
                        Sigla = idCodigo;
                        break;
                    case EnumEspecieDocumento_Unicred.Recibo:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Unicred.Recibo);
                        Especie = "Recibo";
                        Sigla = idCodigo;
                        break;
                    case EnumEspecieDocumento_Unicred.LetraCambio:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Unicred.LetraCambio);
                        Especie = "Letra de Câmbio";
                        Sigla = idCodigo;
                        break;
                    case EnumEspecieDocumento_Unicred.NotaDebito:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Unicred.NotaDebito);
                        Especie = "Nota de Débito";
                        Sigla = idCodigo;
                        break;
                    case EnumEspecieDocumento_Unicred.DuplicataServico:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Unicred.DuplicataServico);
                        Especie = "Duplicata de Serviço";
                        Sigla = idCodigo;
                        break;
                    case EnumEspecieDocumento_Unicred.Outros:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Unicred.Outros);
                        Especie = "Outros";
                        Sigla = idCodigo;
                        break;
                    default:
                        Codigo = "0";
                        Especie = "( Selecione )";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public static EspeciesDocumento CarregaTodas() {
            var especiesDocumento = new EspeciesDocumento();
            var ed = new EspecieDocumento_Unicred();

            foreach (EnumEspecieDocumento_Unicred item in Enum.GetValues(typeof(EnumEspecieDocumento_Unicred))) especiesDocumento.Add(new EspecieDocumento_Unicred(ed.getCodigoEspecieByEnum(item)));

            return especiesDocumento;
        }

        public override IEspecieDocumento DuplicataMercantil() { return new EspecieDocumento_Unicred(getCodigoEspecieByEnum(EnumEspecieDocumento_Unicred.DuplicataMercantil)); }

        #endregion
    }
}