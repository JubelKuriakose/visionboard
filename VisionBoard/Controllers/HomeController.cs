using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using VisionBoard.DAL;
using VisionBoard.Models;

namespace VisionBoard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGoalRepository goalRepository;

        public HomeController(ILogger<HomeController> logger, IGoalRepository goalRepository)
        {
            _logger = logger;
            this.goalRepository = goalRepository;
        }

        public IActionResult Index()
        {
            Goal g = new Goal();
            g.Status = true;
            g.Name = "new";
            g.StartOn = DateTime.UtcNow;
            goalRepository.AddGoal(g);
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
