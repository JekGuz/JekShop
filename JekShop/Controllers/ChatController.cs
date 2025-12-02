using JekShop.Models.Chat;
using Microsoft.AspNetCore.Mvc;

namespace JekShop.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View(new ChatViewModel());
        }
    }
}