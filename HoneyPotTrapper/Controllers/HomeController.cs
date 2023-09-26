using HoneyPotTrapper.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using HoneyPotTrapper.Validations;
using Microsoft.Extensions.Logging;
using HoneyPotTrapper.ViewModels;

namespace HoneyPotTrapper.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IValidators validators;
        private IPortsForListeningCollection portsForListeningCollection;

        public HomeController(ILogger<HomeController> _logger, IValidators _validators, IPortsForListeningCollection _portsForListeningCollection)
        {
            logger = _logger;
            validators = _validators;
            portsForListeningCollection = _portsForListeningCollection;
        }

        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}