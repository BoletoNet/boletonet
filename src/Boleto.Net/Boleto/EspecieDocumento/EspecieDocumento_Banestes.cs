using System;

namespace BoletoNet
{
    public enum EnumEspecieDocumento_Banestes
    {
        DuplicataMercantil,
        Outros,
    }

    public class EspecieDocumento_Banestes : AbstractEspecieDocumento, IEspecieDocumento
    {

        #region Construtores

        public EspecieDocumento_Banestes()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumento_Banestes(string codigo)
        {
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

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_Banestes especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_Banestes.DuplicataMercantil:
                    return "1";
                case EnumEspecieDocumento_Banestes.Outros:
                    return "99";
                default:
                    return "99";

            }
        }

        public EnumEspecieDocumento_Banestes getEnumEspecieByCodigo(string codigo)
        {
            switch (codigo)
            {
                case "1":
                    return EnumEspecieDocumento_Banestes.DuplicataMercantil;
                case "99":
                    return EnumEspecieDocumento_Banestes.Outros;
                default:
                    return EnumEspecieDocumento_Banestes.Outros;
            }
        }

        public override string getCodigoEspecieBySigla(string sigla)
        {
            switch (sigla)
            {
                case "DM": return "1";
                case "OU": return "99";
                default: return "1";
            }
        }

        private void carregar(string idCodigo)
        {
            try
            {
                Banco = new Banco_Banestes();

                switch (getEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_Banestes.DuplicataMercantil:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Banestes.DuplicataMercantil);
                        Especie = "Duplicata mercantil";
                        Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_Banestes.Outros:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Banestes.Outros);
                        Especie = "Outros";
                        break;
                    default:
                        Codigo = "0";
                        Especie = "( Selecione )";
                        Sigla = "";
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
            try
            {
                EspeciesDocumento alEspeciesDocumento = new EspeciesDocumento();

                EspecieDocumento_Banestes obj = new EspecieDocumento_Banestes();

                obj = new EspecieDocumento_Banestes(obj.getCodigoEspecieByEnum(EnumEspecieDocumento_Banestes.DuplicataMercantil));
                alEspeciesDocumento.Add(obj);

                obj = new EspecieDocumento_Banestes(obj.getCodigoEspecieByEnum(EnumEspecieDocumento_Banestes.Outros));
                alEspeciesDocumento.Add(obj);

                return alEspeciesDocumento;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar objetos", ex);
            }
        }

        public override IEspecieDocumento DuplicataMercantil()
        {
            return new EspecieDocumento_Banestes(getCodigoEspecieByEnum(EnumEspecieDocumento_Banestes.DuplicataMercantil));
        }

        #endregion
    }
}
