using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Boleto.Net.MVC.Models;

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
                         select new { ID = Convert.ChangeType(s, typeof(int)), Name = s.ToString() };
            ViewData["bancos"] = new SelectList(bancos, "ID", "Name", Bancos.BancodoBrasil);

            return View();
        }

        public ActionResult VisualizarBoleto(int Id)
        {
            Exemplos exemplos = new Exemplos(Id);

            switch ((Bancos)Id)
            {
                case Bancos.BancodoBrasil:
                    ViewBag.Boleto = exemplos.BancodoBrasil();
                    break;
                case Bancos.Banrisul:
                    ViewBag.Boleto = exemplos.Banrisul();
                    break;
                case Bancos.Basa:
                    ViewBag.Boleto = exemplos.Basa();
                    break;
                case Bancos.Bradesco:
                    ViewBag.Boleto = exemplos.Bradesco();
                    break;
                case Bancos.BRB:
                    ViewBag.Boleto = exemplos.BRB();
                    break;
                case Bancos.Caixa:
                    ViewBag.Boleto = exemplos.Caixa();
                    break;
                case Bancos.HSBC:
                    ViewBag.Boleto = exemplos.HSBC();
                    break;
                case Bancos.Itau:
                    ViewBag.Boleto = exemplos.Itau();
                    break;
                case Bancos.Real:
                    ViewBag.Boleto = exemplos.Real();
                    break;
                case Bancos.Safra:
                    ViewBag.Boleto = exemplos.Safra();
                    break;
                case Bancos.Santander:
                    ViewBag.Boleto = exemplos.Santander();
                    break;
                case Bancos.Sicoob:
                    ViewBag.Boleto = exemplos.Sicoob();
                    break;
                case Bancos.Sicred:
                    ViewBag.Boleto = exemplos.Sicred();
                    break;
                case Bancos.Sudameris:
                    ViewBag.Boleto = exemplos.Sudameris();
                    break;
                case Bancos.Unibanco:
                    ViewBag.Boleto = exemplos.Unibanco();
                    break;
                default:
                    ViewBag.Boleto = "Banco não implementado";
                    break;
            }
            return View();
        }

        public FileResult GeraPDF()
        {
            Exemplos exemplos = new Exemplos(341);
            return File(exemplos.ItauPDF(), "application/pdf");
        }
    }
}
