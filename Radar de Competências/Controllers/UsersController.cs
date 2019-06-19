using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RadarCompetencias.Data;
using RadarCompetencias.Models;
using RadarCompetencias.Models.UsersViewModels;

namespace RadarCompetencias.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserContext _userContext;

        public UsersController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            ILogger<UsersController> logger, 
            UserContext userContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _userContext = userContext;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        #region Users "CRUD"


        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Details(int id)
        {
            return View();
        }
        #region Create

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, ApplicationRole role, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Name = model.Name };
                //Criação do usuário
                var result = await _userManager.CreateAsync(user, model.Password);

                //Adicionando usuário criado a role de usuário
                var result2 = await _userManager.AddToRoleAsync(user, "user");
                role.RoleID = 1;

                if (result.Succeeded && result2.Succeeded)
                {
                    _logger.LogInformation("Usuário criou uma nova conta.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("Usuário criou uma nova conta.");
                    return RedirectToLocal(returnUrl);
                }
            }

            return View(model);
        }
        #endregion

        #region Read
        [Authorize]
        public async Task<IActionResult> List()
        {
            var users = (await _userContext.GetAllAsync());
            var viewModel = new List<ListViewModel>();

            foreach (var user in users)
            {
                var role = await _userManager.GetRolesAsync(user);

                await Task.Run(() => viewModel.Add(new ListViewModel
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    Name = user.Name,
                    Role = role.SingleOrDefault(),
                    Id = user.Id
                }));
            }

            return View(viewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Usuário logado.");
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Tentativa de login inválida.");
                    return View(model);
                }
            }

            return View(model);
        }

        
        public ActionResult Logout()
        {
             _signInManager.SignOutAsync();
            _logger.LogInformation("Usuário desconectado.");
            return RedirectToAction(nameof(UsersController.Login), "Users");
        }
        #endregion

        #region Update
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            return View(
                new EditViewModel
                {
                    Id = id,
                    Email = user.Email,
                    Name = user.Name
                });
        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditViewModel userEdited, IFormCollection collection)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userEdited.Id);

                user.Email = userEdited.Email;
                user.Name = userEdited.Name;
                user.NormalizedEmail = _userManager.NormalizeKey(userEdited.Email);
                user.NormalizedUserName = _userManager.NormalizeKey(userEdited.Name);

                await _userManager.UpdateAsync(user);

                return RedirectToAction(nameof(List));
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region Delete

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            return View(
                new DeleteViewModel
                {
                    Email = user.Email,
                    Name = user.Name
                });
        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                await _userManager.DeleteAsync(user);

                return RedirectToAction(nameof(List));
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #endregion

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(UsersController.List), "Users");
            }
        }
    }
}