using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            mytestdbEntities DB = new mytestdbEntities();
            List<customer> c = DB.customers.ToList();
            return View(c);
        }
    }
}