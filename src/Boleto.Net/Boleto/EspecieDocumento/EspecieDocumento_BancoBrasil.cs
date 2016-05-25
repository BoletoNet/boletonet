using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumEspecieDocumento_BancoBrasil
    {
        DuplicataMercantil = 1,
        NotaPromissoria = 2,
        NotaSeguro = 3,
        Recibo = 5,
        LetraCambio = 8,
        Warrant = 9,
        Cheque = 10,
        DuplicataServico = 12,
        NotaDebito = 13,
        ApoliceSeguro = 15,
        DividaAtivaUniao = 25,
        DividaAtivaEstado = 26,
        DividaAtivaMunicipio = 27
    }

    #endregion

    public class EspecieDocumento_BancoBrasil : AbstractEspecieDocumento, IEspecieDocumento
    {
        public EnumEspecieDocumento_BancoBrasil EspecieDocumento { get; set; }

        #region Construtores

        public EspecieDocumento_BancoBrasil()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumento_BancoBrasil(string codigo)
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

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil especie)
        {
            return especie.ToString("D").PadLeft(2, '0');
        }

        public EnumEspecieDocumento_BancoBrasil getEnumEspecieByCodigo(string codigo)
        {
            int codigoValor;

            int.TryParse(codigo, out codigoValor);

            if(Enum.IsDefined(typeof(EnumEspecieDocumento_BancoBrasil), codigoValor)) {
                return (EnumEspecieDocumento_BancoBrasil)codigoValor;
            }

            return EnumEspecieDocumento_BancoBrasil.DuplicataMercantil;
        }

        private void carregar(string idCodigo)
        {
            try
            {
                this.Banco = new Banco_Brasil();

                EspecieDocumento = getEnumEspecieByCodigo(idCodigo);

                switch (EspecieDocumento)
                {
                    case EnumEspecieDocumento_BancoBrasil.Cheque:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.Cheque);
                        this.Especie = "CHEQUE";
                        this.Sigla = "CH";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.DuplicataMercantil:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.DuplicataMercantil);
                        this.Especie = "DUPLICATA MERCANTIL";
                        this.Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.DuplicataServico:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.DuplicataServico);
                        this.Especie = "DUPLICATA DE SERVIÇO";
                        this.Sigla = "DS";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.LetraCambio:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.LetraCambio);
                        this.Especie = "LETRA DE CAMBIO";
                        this.Sigla = "LC";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.NotaPromissoria:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaPromissoria);
                        this.Especie = "NOTA PROMISSÓRIA";
                        this.Sigla = "NP";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.NotaSeguro:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaSeguro);
                        this.Especie = "NOTA DE SEGURO";
                        this.Sigla = "NS";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.Recibo:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.Recibo);
                        this.Especie = "RECIBO";
                        this.Sigla = "RC";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.NotaDebito:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaDebito);
                        this.Especie = "NOTA DE DÉBITO";
                        this.Sigla = "ND";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.ApoliceSeguro:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.ApoliceSeguro);
                        this.Especie = "APÓLICE DE SEGURO";
                        this.Sigla = "AP";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.Warrant:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.Warrant);
                        this.Especie = "WARRANT";
                        this.Sigla = "WA";
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
            try
            {
                var alEspeciesDocumento = new EspeciesDocumento();
                var ed = new EspecieDocumento_BancoBrasil();

                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.DuplicataMercantil)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaPromissoria)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaSeguro)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.Recibo)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.LetraCambio)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.Warrant)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.Cheque)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.DuplicataServico)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaDebito)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.ApoliceSeguro)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.DividaAtivaUniao)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.DividaAtivaEstado)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.DividaAtivaMunicipio)));

                return alEspeciesDocumento;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar objetos", ex);
            }
        }

        #endregion

        public override string ObterCodigo(Boleto boleto, TipoArquivo tipoArquivo) {
            if (tipoArquivo == TipoArquivo.CNAB240) {
                switch (EspecieDocumento) {
                    case EnumEspecieDocumento_BancoBrasil.DuplicataMercantil:
                        return "02";
                    case EnumEspecieDocumento_BancoBrasil.NotaPromissoria:
                        return "12";
                    case EnumEspecieDocumento_BancoBrasil.Recibo:
                        return "17";
                    case EnumEspecieDocumento_BancoBrasil.LetraCambio:
                        return "07";
                    case EnumEspecieDocumento_BancoBrasil.Warrant:
                        return "26";
                    case EnumEspecieDocumento_BancoBrasil.Cheque:
                        return "01";
                    case EnumEspecieDocumento_BancoBrasil.DuplicataServico:
                        return "04";
                    case EnumEspecieDocumento_BancoBrasil.NotaDebito:
                        return "19";
                    case EnumEspecieDocumento_BancoBrasil.DividaAtivaUniao:
                        return "29";
                    case EnumEspecieDocumento_BancoBrasil.DividaAtivaEstado:
                        return "27";
                    case EnumEspecieDocumento_BancoBrasil.DividaAtivaMunicipio:
                        return "28";
                    case EnumEspecieDocumento_BancoBrasil.NotaSeguro:
                        return "16";
                    case EnumEspecieDocumento_BancoBrasil.ApoliceSeguro:
                        return "20";
                    default:
                        throw new ApplicationException($"Espécie '{Codigo}' inválida para a carteira '{boleto.Carteira}'.");
                }
            }

            return Codigo;
        }
    }
}
