using JekShop.ApplicationServices.Services;
using JekShop.Core.Dto;
using JekShop.Core.ServiceInterface;
using JekShop.Models.Email;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JekShop.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailServices _emailServices;

        public EmailController
            (IEmailServices emailServices)
        {
            _emailServices = emailServices;
        }
        public IActionResult Index()
        {
            return View();
        }


        // teha meetod minega Send, mis võtab vastu EmailDto objekti
        // kasutab EmailServices, et saata email

        [HttpPost]
        public IActionResult SendEmail(EmailViewModel vm)
        {
            var files = Request.Form.Files.Any() ? Request.Form.Files.ToList() : new List<IFormFile>();

            var dto = new EmailDto
            {
                To = vm.To,
                Subject = vm.Subject,
                Body = vm.Body,
                Attchmen = files,
            };


            _emailServices.SendEmail(dto);
            return RedirectToAction(nameof(Index));
        }


    } 
}
