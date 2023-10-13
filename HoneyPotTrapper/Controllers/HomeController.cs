using HoneyPotTrapper.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using HoneyPotTrapper.Validations;
using Microsoft.Extensions.Logging;
using HoneyPotTrapper.Models.ViewModels;
namespace HoneyPotTrapper.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IValidators validators;
        private IAppModel appModel;

        public HomeController(ILogger<HomeController> _logger, IValidators _validators, IAppModel _appModel)
        {
            logger = _logger;
            validators = _validators;
            appModel = _appModel;
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