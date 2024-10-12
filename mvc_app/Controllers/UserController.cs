using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class UserController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            return BadRequest("Email or password are important ...");
        }
        var user = new IdentityUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };
        var result = await _userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            return Ok("User is register");
        }
        foreach (var item in result.Errors)
        {
            Console.WriteLine(item);
        }
        return BadRequest(Json(result.Errors));
    }
    [HttpGet]
    public IActionResult Auth()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Auth(string email, string password)
    {
		if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
		{
			return BadRequest("Email or password are important ...");
        }
        var result = await _signInManager.PasswordSignInAsync(
            email, 
            password,
            isPersistent: false,
            lockoutOnFailure: false
            );
        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home");
			//return Ok("Auth Ok");
		}
		return BadRequest("Email or password are error ...");

	}
    [HttpPost]
	public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
		return RedirectToAction("Index", "Home");
	}
}
