using System.Diagnostics;
using System.Security.Claims;
using AspNetCoreGeneratedDocument;
using JekShop.Core.Domain;
using JekShop.Core.Dto;
using JekShop.Core.ServiceInterface;
using JekShop.Models;
using JekShop.Models.Accounts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JekShop.Controllers
{
    public class AccountsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailServices _emailService;

        public AccountsController
            (
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailServices emailService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser 
                { UserName = vm.Email,
                    Name = vm.Name,
                    Email = vm.Email,
                    City = vm.City,
                };

                var result = await _userManager.CreateAsync(user, vm.Password);
                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action("ConfirmEmail", "Accounts",
                        new { userId = user.Id, token = token }, Request.Scheme);

                    EmailTokenDto newsignup = new();
                    newsignup.Token = token;
                    newsignup.Body = $"Please register your account by: <a href='{confirmationLink}'>clicking here</a>";
                    newsignup.Subject = "CRUID registration";
                    newsignup.To = user.Email;

                    if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Administration");
                    }

                    _emailService.SendEmailToken(newsignup, token);
                    List <string> errordatas =
                        [
                        "Area", "Accounts",
                        "Issue", "Success",
                        "StatusMessage", "Registration successful",
                        "ActedOn", $"{vm.Email}",
                        "CreatedAccountData", $"{vm.Email}\n{vm.Name}\n{vm.City}\n[password hidden]\n[password hidden]",
                        ];
                    ViewBag.errordatas = errordatas;
                    ViewBag.ErrorTitle = "You Have successfully registered";
                    ViewBag.ErrorMessage = "Before you can login, please confirm your " +
                        "\nemail, by clicking on the confirmation link we have emailed you" +
                        "\nwe have send you email to your email box";

                    return View("ConfirmEmailMassage");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }            
            return View();
        }

        // для ConfirmEmail метод
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userid, string token)
        {
            if (userid == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var user = await _userManager.FindByIdAsync(userid);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"The user with id of {userid} is not valid";
                return View("NotFound");
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            List<string> errordatas =
                        [
                        "Area", "Accounts",
                        "Issue", "Success",
                        "StatusMessage", "Registration successful",
                        "ActedOn", $"{user.Email}",
                        "CreatedAccountData", $"{user.Email}\n{user.Name}\n{user.City}\n[password hidden]\n[password hidden]",
                        ];
            if (result.Succeeded)
            {
                errordatas =
                        [
                        "Area", "Accounts",
                        "Issue", "Success",
                        "StatusMessage", "Registration successful",
                        "ActedOn", $"{user.Email}",
                        "CreatedAccountData", $"{user.Email}\n{user.Name}\n{user.City}\n[password hidden]\n[password hidden]",
                        ];
                ViewBag.ErrorDatas = errordatas;
                return View();
            }
            ViewBag.ErrorDatas = errordatas;
            ViewBag.ErrorTitle = "Email cannot be confirmed";
            ViewBag.ErrorMessege = $"The user email, with userid of {userid}, cannot be confirmed.";
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id?? HttpContext.TraceIdentifier});
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var result = await _signInManager.PasswordSignInAsync(
                vm.Email,
                vm.Password,
                vm.RememberMe,
                lockoutOnFailure: true); // важно для блокировки

            if (result.Succeeded)
            {
                // вместо Redirect — показываем новое окно
                // Так как не отдельную страницу хочу а обратно на home
                ViewBag.Username = vm.Email;
                return View("LoginSuccess");
                // return RedirectToAction("Index", "Home");
            }

            // если аккаунт заблокирован
            if (result.IsLockedOut)
            {
                return RedirectToAction(
                    nameof(AccountLocked),
                    new { email = vm.Email }
                );
            }

            // неверный логин или пароль
            ModelState.AddModelError("", "Invalid login attempt.");
            return View(vm);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> AccountLocked(string? email = null)
        {
            if (string.IsNullOrEmpty(email))
                return View();

            var user = await _userManager.FindByEmailAsync(email);
            if (user?.LockoutEnd != null)
            {
                var remaining = user.LockoutEnd.Value - DateTimeOffset.UtcNow;

                if (remaining.TotalSeconds > 0)
                {
                    ViewBag.RemainingMinutes = Math.Ceiling(remaining.TotalMinutes);
                }
            }

            return View();
        }


        // В классе login
        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userManager.FindByEmailAsync(model.Email);

        //        if (user != null && !user.EmailConfirmed &&
        //            (await _userManager.CheckPasswordAsync(user, model.Password)))
        //        {
        //            ModelState.AddModelError(string.Empty, "Email not confirmed yet");
        //            return View(model);
        //        }
        //        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);

        // 
        // if (result.Succeeded)
        // {
        //     if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        //     {
        //         return Redirect(returnUrl);
        //      }
        //     else
        //     {
        //         return RedirectToAction("Index", "Home");
        //     }
        // }
        // if (result.IsLockedOut)
        // {
        //     return View("AccountLocked");
        // }
        // ModelState.AddModelError("", "Invalid login attempt.");
        // }
        // return View(model);
        // }


        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user = await _userManager.FindByEmailAsync(vm.Email);

            if (user == null)
            {
                // Чтобы не подсказать злоумышленнику, что пользователя нет:
                return View("ForgotPasswordConfirmation");
            }

            // Создаём токен для сброса
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var link = Url.Action("ResetPassword", "Accounts",
                new { email = vm.Email, token = token }, Request.Scheme);

            // Отправляем письмо
            EmailTokenDto resetPasswordEmail = new()
            {
                To = vm.Email,
                Subject = "Reset your password",
                Body = $"Please reset your password by clicking <a href='{link}'>here</a>",
                Token = token
            };

            _emailService.SendEmailToken(resetPasswordEmail, token);

            return View("ForgotPasswordConfirmation");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                // можно показать ошибку или NotFound
                return View("NotFound");
            }

            var model = new ResetPasswordViewModel
            {
                Email = email,
                Token = token
            };

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user = await _userManager.FindByEmailAsync(vm.Email);

            if (user == null)
            {
                // Чтобы не раскрывать, существует ли пользователь
                return View("ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, vm.Token, vm.Password);

            if (result.Succeeded)
            {
                if (await _userManager.IsLockedOutAsync(user))
                {
                    await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                }
                return View("ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(vm);
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordViewModel());
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePasswordConfirmation()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Accounts");

            var result = await _userManager.ChangePasswordAsync(user, vm.CurrentPassword!, vm.NewPassword!);

            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                // TempData["SuccessMessage"] = "Password changed successfully.";
                return RedirectToAction(nameof(ChangePasswordConfirmation));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(vm);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // ----------------------------------------------  Google/Facebook login --------------------------------------------
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string? returnUrl = null)
        {
            // если returnUrl не передали — по умолчанию на главную
            returnUrl ??= Url.Content("~/");

            // после успешного логина у провайдера (Google/Facebook) вернёмся сюда
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Accounts", new { returnUrl });

            // эти properties содержат returnUrl и данные для external cookie
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            // отправляем пользователя на Google/Facebook
            return Challenge(properties, provider);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
        {
            returnUrl ??= Url.Content("~/");

            if (!string.IsNullOrEmpty(remoteError))
            {
                TempData["ExternalError"] = $"External provider error: {remoteError}";
                return RedirectToAction(nameof(Login));
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                TempData["ExternalError"] = "External login info is missing (info == null). Usually cookies/SameSite issue.";
                return RedirectToAction(nameof(Login));
            }

            // 1) если логин уже привязан — просто входим
            var signInResult = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider,
                info.ProviderKey,
                isPersistent: false,
                bypassTwoFactor: true);

            if (signInResult.IsLockedOut)
            {
                var emailLocked = info.Principal.FindFirstValue(ClaimTypes.Email);
                return RedirectToAction(nameof(AccountLocked), new { email = emailLocked });
            }

            if (signInResult.Succeeded)
            {
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme); // очистить внешнюю куку
                ViewBag.Username =
                    info.Principal.FindFirstValue(ClaimTypes.Email)
                    ?? info.Principal.Identity?.Name
                    ?? "External user";
                return View("LoginSuccess");
            }

            // 2) достаём email
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
            {
                TempData["ExternalError"] = "Provider did not return email.";
                return RedirectToAction(nameof(Login));
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    Name = email,
                    EmailConfirmed = true
                };

                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    TempData["ExternalError"] = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    return RedirectToAction(nameof(Login));
                }
            }

            // 3) снять блокировку, если была
            if (await _userManager.IsLockedOutAsync(user))
            {
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                await _userManager.ResetAccessFailedCountAsync(user);
            }

            // 4) привязать логин
            var addLoginResult = await _userManager.AddLoginAsync(user, info);
            if (!addLoginResult.Succeeded)
            {
                // если уже привязан — просто продолжаем
                // если хочешь увидеть причину — раскомментируй:
                // TempData["ExternalError"] = string.Join(", ", addLoginResult.Errors.Select(e => e.Description));
                // return RedirectToAction(nameof(Login));
            }

            // 5) теперь повторно пробуем ExternalLoginSignInAsync (надёжнее)
            var finalSignIn = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider,
                info.ProviderKey,
                isPersistent: false,
                bypassTwoFactor: true);

            if (!finalSignIn.Succeeded)
            {
                // fallback
                await _signInManager.SignInAsync(user, isPersistent: false);
            }

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme); // очистить внешнюю куку

            ViewBag.Username = user.Email ?? user.UserName ?? "External user";
            return View("LoginSuccess");
        }






    }
}
