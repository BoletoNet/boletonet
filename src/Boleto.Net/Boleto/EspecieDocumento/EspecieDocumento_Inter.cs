using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumEspecieDocumento_Inter
    {
        DuplicataMercantil
    }

    #endregion

    public class EspecieDocumento_Inter : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_Inter()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumento_Inter(string codigo)
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

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_Inter especie)
        {
            return "1";
        }

        public EnumEspecieDocumento_Inter getEnumEspecieByCodigo(string codigo)
        {
            return EnumEspecieDocumento_Inter.DuplicataMercantil;
        }

        public override string getCodigoEspecieBySigla(string sigla)
        {
            return "1";
        }

        private void carregar(string idCodigo)
        {
            try
            {
                this.Banco = new Banco_Inter();

                this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Inter.DuplicataMercantil);
                this.Especie = "Duplicata mercantil";
                this.Sigla = "DM";
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

                EspecieDocumento_Inter obj = new EspecieDocumento_Inter();

                obj = new EspecieDocumento_Inter(obj.getCodigoEspecieByEnum(EnumEspecieDocumento_Inter.DuplicataMercantil));
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
            return new EspecieDocumento_Inter(getCodigoEspecieByEnum(EnumEspecieDocumento_Inter.DuplicataMercantil));
        }

        #endregion
    }
}