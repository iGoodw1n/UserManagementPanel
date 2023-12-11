using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using UserManagementPanel.Data;

namespace UserManagementPanel.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public IndexModel(
            ILogger<IndexModel> logger,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public void OnGet()
        {
        }

        public async Task<RedirectToActionResult> OnPostDeleteAsync([FromForm] IEnumerable<string> usersId)
        {
            foreach (var id in usersId)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user is not null)
                {
                    if (user.Id == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                    {
                        await _signInManager.SignOutAsync();
                    }
                    await _userManager.DeleteAsync(user);
                }
            }
            return RedirectToAction("");
        }
        public async Task<RedirectToPageResult> OnPostBlockAsync([FromForm] IEnumerable<string> usersId)
        {
            foreach (var id in usersId)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user is not null)
                {
                    if (user.Id == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                    {
                        await _signInManager.SignOutAsync();
                    }
                    await _userManager.SetLockoutEnabledAsync(user, true);
                    await _userManager.SetLockoutEndDateAsync(user, DateTime.Now.AddYears(100));
                }
            }
            return RedirectToPage("Index");
        }

        public async Task<RedirectToPageResult> OnPostUnBlockAsync([FromForm] IEnumerable<string> usersId)
        {
            foreach (var id in usersId)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user is not null)
                {
                    await _userManager.SetLockoutEnabledAsync(user, false);
                    await _userManager.SetLockoutEndDateAsync(user, DateTime.Now.AddMinutes(-1));
                }
            }
            return RedirectToPage("Index");
        }
    }
}
