using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public class Carteira : AbstractCarteira, ICarteira
    {

        #region Variaveis

        private ICarteira _ICarteira;

        #endregion

        #region Construtores

        internal Carteira() 
        { 
        }

        public Carteira(int CodigoBanco)
        {
            try
            {
                InstanciaCarteira(CodigoBanco, 0);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao instanciar objeto.", ex);
            }
        }

        public Carteira(int CodigoBanco, int codigoCarteira)
        {
            try
            {
                InstanciaCarteira(CodigoBanco, codigoCarteira);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao instanciar objeto.", ex);
            }
        }

        #endregion

        #region Propriedades da interface

        public override IBanco Banco
        {
            get { return _ICarteira.Banco; }
            set { _ICarteira.Banco = value; }
        }

        public override int NumeroCarteira
        {
            get { return _ICarteira.NumeroCarteira; }
            set { _ICarteira.NumeroCarteira = value; }
        }

        public override string Codigo
        {
            get { return _ICarteira.Codigo; }
            set { _ICarteira.Codigo = value; }
        }

        public override string Tipo
        {
            get { return _ICarteira.Tipo; }
            set { _ICarteira.Tipo = value; }
        }

        public override string Descricao
        {
            get { return _ICarteira.Descricao; }
            set { _ICarteira.Descricao = value; }
        }

        #endregion

        # region Métodos Privados

        private void InstanciaCarteira(int codigoBanco, int codigoCarteira)
        {
            try
            {
                switch (codigoBanco)
                {
                    //341 - Itaú
                    case 341:
                        _ICarteira = new Carteira_Itau(0);
                        break;
                    //356 - BankBoston
                    case 479:
                        _ICarteira = new Carteira_BankBoston(0);
                        break;
                    //422 - Safra
                    case 1:
                        _ICarteira = new Carteira_BancoBrasil(codigoCarteira);
                        break;
                    default:
                        throw new Exception("Código do banco não implementando: " + codigoBanco);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a execução da transação.", ex);
            }
        }

        public static Carteiras CarregaTodas(int codigoBanco)
        {
            try
            {
                switch (codigoBanco)
                {
                    case 1:

                        Carteiras alCarteiras = new Carteiras();

                        Carteira_BancoBrasil obj;

                        obj = new Carteira_BancoBrasil((int)EnumCarteiras_BancoBrasil.CobrancaSimples);
                        alCarteiras.Add(obj);

                        obj = new Carteira_BancoBrasil((int)EnumCarteiras_BancoBrasil.CobrancaVinculada);
                        alCarteiras.Add(obj);

                        obj = new Carteira_BancoBrasil((int)EnumCarteiras_BancoBrasil.CobrancaCaucionada);
                        alCarteiras.Add(obj);

                        obj = new Carteira_BancoBrasil((int)EnumCarteiras_BancoBrasil.CobrancaDescontada);
                        alCarteiras.Add(obj);

                        obj = new Carteira_BancoBrasil((int)EnumCarteiras_BancoBrasil.CobrançaDiretaEspecialCarteira17);
                        alCarteiras.Add(obj);

                        return alCarteiras;
                    default:
                        throw new Exception("Código do banco não implementando: " + codigoBanco);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar objetos", ex);
            }
        }

        # endregion

    }
}
