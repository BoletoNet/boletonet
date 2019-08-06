using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public class Instrucao : IInstrucao
    {

        #region Variaveis

        private IInstrucao _IInstrucao;

        #endregion

        #region Construtores

        internal Instrucao()
        {
        }

        public Instrucao(int CodigoBanco)
        {
            try
            {
                InstanciaInstrucao(CodigoBanco);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao instanciar objeto.", ex);
            }
        }

        #endregion

        # region Métodos Privados

        private void InstanciaInstrucao(int codigoBanco)
        {
            try
            {
                switch (codigoBanco)
                {
                    //399 - HSBC
                    case 399:
                        _IInstrucao = new Instrucao_HSBC();
                        break;
                    //104 - Caixa
                    case 104:
                        _IInstrucao = new Instrucao_Caixa();
                        break;
                    //341 - Itaú
                    case 341:
                        _IInstrucao = new Instrucao_Itau();
                        break;
                    //1 - Banco do Brasil
                    case 1:
                        _IInstrucao = new Instrucao_BancoBrasil();
                        break;
                    //356 - Real
                    case 356:
                        _IInstrucao = new Instrucao_Real();
                        break;
                    //422 - Safra
                    case 422:
                        _IInstrucao = new Instrucao_Safra();
                        break;
                    //237 - Bradesco
                    //707 - Daycoval
                    case 237:
                    case 707:
                        _IInstrucao = new Instrucao_Bradesco();
                        break;
                    //347 - Sudameris
                    case 347:
                        _IInstrucao = new Instrucao_Sudameris();
                        break;
                    //353 - Santander
                    case 353:
                    case 33:
                    case 8:
                        //case 8:
                        _IInstrucao = new Instrucao_Santander();
                        break;
                    //070 - BRB
                    case 70:
                        _IInstrucao = new Instrucao_BRB();
                        break;
                    //479 - BankBoston
                    case 479:
                        _IInstrucao = new Instrucao_BankBoston();
                        break;
                    //41 - Banrisul
                    case 41:
                        _IInstrucao = new Instrucao_Banrisul();
                        break;
                    //756 - Sicoob
                    case 756:
                        _IInstrucao = new Instrucao_Sicoob();
                        break;
                    //97 - CredSis
                    case 97:
                        _IInstrucao = new Instrucao_CrediSIS();
                        break;
                    //85 - CECRED
                    case 85:
                        _IInstrucao = new Instrucao_Cecred();
                        break;
                    //748 - Sicredi
                    case 748:
                        _IInstrucao = new Instrucao_Sicredi();
                        break;
                    //655 - Votorantim
                    case 655:
                        _IInstrucao = new Instrucao_Votorantim();
                        break;
                    case 21:
                        _IInstrucao = new Instrucao_Banestes();
                        break;
                    case 4:
                        _IInstrucao = new Instrucao_BancoNordeste();
                        break;
                    case 84: 
                        _IInstrucao = new Instrucao_Uniprime();
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

        # endregion

        #region Propriedades da interface

        public IBanco Banco
        {
            get { return _IInstrucao.Banco; }
            set { _IInstrucao.Banco = value; }
        }

        public int Codigo
        {
            get { return _IInstrucao.Codigo; }
            set { _IInstrucao.Codigo = value; }
        }

        public string Descricao
        {
            get { return _IInstrucao.Descricao; }
            set { _IInstrucao.Descricao = value; }
        }

        public int QuantidadeDias
        {
            get { return _IInstrucao.QuantidadeDias; }
            set { _IInstrucao.QuantidadeDias = value; }
        }

        #endregion

        #region Métodos de interface

        public void Valida()
        {
            try
            {
                //_IInstrucao.Valida();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a validação dos campos.", ex);
            }
        }

        #endregion

    }
}
