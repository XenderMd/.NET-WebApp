using DutchTreat.Data;
using DutchTreat.Services;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    public class AppController:Controller
    {
        private readonly IMailService _mailService;
        private readonly DutchContext _context;

        public AppController(IMailService mailService, DutchContext context)
        {
            _mailService = mailService;
            this._context = context;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Contact")] //specify a non-default route 
        public IActionResult Contact()
        {

            ViewBag.Title = "Contact Us";
            return View();
        }

        [HttpPost("Contact")]
        public IActionResult Contact(ContactViewModel model)
        {
            if(ModelState.IsValid)
            {
                //send email
                _mailService.SendMessage("denis.stavilamd@gmail.com", model.Subject, $"From: {model.Name} {model.Email}, Message:{model.Message}");
                ViewBag.UserMessage = "Mail Sent";
                ModelState.Clear();
            }
            ViewBag.Title = "Contact Us";
            return View();
        }

        public IActionResult Shop()
        {
            var results = _context.Products
                                    .OrderBy(p => p.Category)
                                    .ToList();
            return View(results);
        }

        public IActionResult About()
        {
            ViewBag.Title = "About";
            return View();
        }
    }
}
