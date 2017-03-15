using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoletoNet
{
    public class EspecieDocumento_Mercantil : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Enumerador

        public enum EnumEspecieDocumento_Mercantil
        {
            DuplicataMercantil,
            Outros
        }

        #endregion

        #region Construtores

        public EspecieDocumento_Mercantil()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumento_Mercantil(string codigo)
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

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_Mercantil especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_Mercantil.DuplicataMercantil: return "1";
                case EnumEspecieDocumento_Mercantil.Outros: return "9";
                default: return "1";
            }
        }

        public EnumEspecieDocumento_Mercantil getEnumEspecieByCodigo(string codigo)
        {
            switch (codigo)
            {
                case "1": return EnumEspecieDocumento_Mercantil.DuplicataMercantil;
                case "9": return EnumEspecieDocumento_Mercantil.Outros;
                default: return EnumEspecieDocumento_Mercantil.DuplicataMercantil;
            }
        }

        private void carregar(string idCodigo)
        {
            try
            {
                this.Banco = new Banco_MercantilDoBrasil();

                switch (getEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_Mercantil.DuplicataMercantil:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Mercantil.DuplicataMercantil);
                        this.Especie = "DUPLICATA MERCANTIL";
                        this.Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_Mercantil.Outros:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Mercantil.Outros);
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
