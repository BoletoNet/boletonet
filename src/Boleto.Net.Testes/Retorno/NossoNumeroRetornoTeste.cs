using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BoletoNet;

namespace Boleto.Net.Testes.Retorno
{
    [TestClass]
    public class NossoNumeroRetornoTeste
    {
        [TestMethod, TestCategory("Tratar nosso número")]
        public void Recuperar_nosso_numero_retorno_banco_do_brasil_convenio_4_posicoes()
        {
            //123412345671
            //CCCCSSSSSSSD
            //Onde: C = Convênio S = Sequencial D = dígito verificador
            var convenio = 1234;
            var nossoNumero = 1234567;
            var dv = 1;
            var nossoNumeroRetorno = string.Format("{0}{1}{2}", convenio, nossoNumero, dv);

            var banco = new Banco(1);
            var actual = banco.ObterNossoNumeroSemConvenioOuDigitoVerificador(convenio, nossoNumeroRetorno);
            Assert.AreEqual(nossoNumero, actual);
        }

        [TestMethod, TestCategory("Tratar nosso número")]
        public void Recuperar_nosso_numero_retorno_banco_do_brasil_convenio_6_posicoes()
        {
            //123456123451
            //CCCCCCSSSSSD
            //Onde: C = Convênio S = Sequencial D = dígito verificador
            var convenio = 123456;
            var nossoNumero = 12345;
            var dv = 1;
            var nossoNumeroRetorno = string.Format("{0}{1}{2}", convenio, nossoNumero, dv);

            var banco = new Banco(1);
            var actual = banco.ObterNossoNumeroSemConvenioOuDigitoVerificador(convenio, nossoNumeroRetorno);
            Assert.AreEqual(nossoNumero, actual);
        }

        [TestMethod, TestCategory("Tratar nosso número")]
        public void Recuperar_nosso_numero_retorno_banco_do_brasil_convenio_7_posicoes()
        {
            //12345671234567890
            //CCCCCCCSSSSSSSSSS
            //Onde: C = Convênio S = Sequencial
            var convenio = 1234567;
            var nossoNumero = 1234567890;
            var nossoNumeroRetorno = string.Format("{0}{1}", convenio, nossoNumero);

            var banco = new Banco(1);
            var actual = banco.ObterNossoNumeroSemConvenioOuDigitoVerificador(convenio, nossoNumeroRetorno);
            Assert.AreEqual(nossoNumero, actual);
        }

        [TestMethod, TestCategory("Tratar nosso número")]
        public void Recuperar_nosso_numero_retorno_banco_Itau()
        {
            var convenio = 1234567;
            var nossoNumero = 1234567890;

            var banco = new Banco(341);
            var actual = banco.ObterNossoNumeroSemConvenioOuDigitoVerificador(convenio, nossoNumero.ToString());
            Assert.AreEqual(nossoNumero, actual);
        }

        [TestMethod, TestCategory("Tratar nosso número")]
        public void Recuperar_nosso_numero_retorno_banco_Santander()
        {
            var convenio = 1234567;
            var nossoNumero = 100003147578;
            var dv = 6;

            var nossoNumeroRetorno = string.Format("{0}{1}", nossoNumero, dv);

            var banco = new Banco(353);            

            var atual = banco.ObterNossoNumeroSemConvenioOuDigitoVerificador(convenio, nossoNumeroRetorno);
            Assert.AreEqual(nossoNumero, atual);
        }
    }
}
