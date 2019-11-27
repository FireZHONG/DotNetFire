using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RedisCoreDemo.Models;
using RedisExtension;

namespace RedisCoreDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        readonly IRedisClient _redisService;

        public HomeController(ILogger<HomeController> logger, IRedisClient redisService)
        {
            _logger = logger;
            _redisService = redisService;
        }

        public IActionResult Index()
        {
            ViewBag.test = _redisService.StringGet("newtest");
            return View();
        }

        public string Set(string value)
        {
            _redisService.StringSet("test", value);
            return "Success";
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
