using System;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.Controllers
{
    public partial class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
