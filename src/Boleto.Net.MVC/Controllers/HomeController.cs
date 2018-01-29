using System;
using System.Linq;
using System.Web.Mvc;
using Boleto.Net.MVC.Models;
using BoletoNet;

namespace Boleto.Net.MVC.Controllers
{
    /// <Author>
    /// Sandro Ribeiro - CODTEC SISTEMAS
    /// </Author>
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Boleto.Net.MVC";
            var bancos = from Bancos s in Enum.GetValues(typeof(Bancos))
                         select new
                         {
                             ID = Convert.ChangeType(s, typeof(int)),
                             Name = s.ToString()
                         };
            ViewData["bancos"] = new SelectList(bancos, "ID", "Name", Bancos.BancodoBrasil);
            return View();
        }

        public ActionResult VisualizarBoleto(int Id)
        {
            var boleto = ObterBoletoBancario(Id);
            ViewBag.Boleto = boleto.MontaHtmlEmbedded();
            return View();
        }

        public ActionResult GerarBoletoPDF(int Id)
        {
            var boleto = ObterBoletoBancario(Id);
            var pdf = boleto.MontaBytesPDF();
            return File(pdf, "application/pdf");
        }

        public BoletoBancario ObterBoletoBancario(int Id)
        {
            var exemplos = new Exemplos(Id);

            switch ((Bancos)Id)
            {
                case Bancos.BancodoBrasil:
                    return exemplos.BancodoBrasil();
                case Bancos.Banrisul:
                    return exemplos.Banrisul();
                case Bancos.Basa:
                    return exemplos.Basa();
                case Bancos.Bradesco:
                    return exemplos.Bradesco();
                case Bancos.BRB:
                    return exemplos.BRB();
                case Bancos.Caixa:
                    return exemplos.Caixa();
                case Bancos.HSBC:
                    return exemplos.HSBC();
                case Bancos.Itau:
                    return exemplos.Itau();
                case Bancos.Real:
                    return exemplos.Real();
                case Bancos.Safra:
                    return exemplos.Safra();
                case Bancos.Santander:
                    return exemplos.Santander();
                case Bancos.Sicoob:
                    return exemplos.Sicoob();
                case Bancos.Sicred:
                    return exemplos.Sicred();
                case Bancos.Sudameris:
                    return exemplos.Sudameris();
                case Bancos.Unibanco:
                    return exemplos.Unibanco();
                case Bancos.Semear:
                    return exemplos.Semear();
                default:
                    throw new ArgumentException("Banco não implementado");
            }
        }
    }
}
