using CodingBlogDemo2.Models;
using CodingBlogDemo2.Models.ProfileViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBlogDemo2.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private IAccountRepository _accountRepository;

        public ProfileController(IAccountRepository repo)
        {
            _accountRepository = repo;
        }

        public IActionResult Index()
        {
            ViewBag.FName = _accountRepository.getUserFName(User.Identity.Name);\

            return View();

        }
    }
}
