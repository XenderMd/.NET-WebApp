using DutchTreat.Data;
using DutchTreat.Services;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IDutchRepository _repository;
       
        public AppController(IMailService mailService, IDutchRepository repository)
        {
            _mailService = mailService;
            _repository = repository;
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

        [Authorize]
        public IActionResult Shop()
        {
            var results = _repository.GetAllProducts();
            return View(results);
        }

        public IActionResult About()
        {
            ViewBag.Title = "About";
            return View();
        }
    }
}
