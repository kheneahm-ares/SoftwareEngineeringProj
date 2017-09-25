using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CodingBlogDemo2.Models;
using Microsoft.AspNetCore.Identity;

namespace CodingBlogDemo2.Controllers
{
    public class HomeController : Controller
    {
        private IAccountRepository _accountRepository;
        private SignInManager<ApplicationUser> _manager;

        public HomeController(IAccountRepository repo, SignInManager<ApplicationUser> SignInManager)
        {
            _accountRepository = repo;
            _manager = SignInManager;
        }
        public IActionResult Index()
        {
           
                
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
