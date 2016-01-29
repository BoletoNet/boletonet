using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public class Instrucao : AbstractInstrucao, IInstrucao
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

        # region M�todos Privados

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
                    //341 - Ita�
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
                    case 237:
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
                    default:
                        throw new Exception("C�digo do banco n�o implementando: " + codigoBanco);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a execu��o da transa��o.", ex);
            }
        }

        # endregion

        #region Propriedades da interface

        public override IBanco Banco
        {
            get { return _IInstrucao.Banco; }
            set { _IInstrucao.Banco = value; }
        }

        public override int Codigo
        {
            get { return _IInstrucao.Codigo; }
            set { _IInstrucao.Codigo = value; }
        }

        public override string Descricao
        {
            get { return _IInstrucao.Descricao; }
            set { _IInstrucao.Descricao = value; }
        }

        public override int QuantidadeDias
        {
            get { return _IInstrucao.QuantidadeDias; }
            set { _IInstrucao.QuantidadeDias = value; }
        }

        #endregion

        #region M�todos de interface

        public override void Valida()
        {
            try
            {
                //_IInstrucao.Valida();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a valida��o dos campos.", ex);
            }
        }

        #endregion

    }
}
