using Lr5.Models;
using Microsoft.AspNetCore.Mvc;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public HomeController(ILogger<HomeController> logger){_logger = logger;}
    public IActionResult Index(){return View();}

    [HttpPost]
    public IActionResult SubmitCookie(CookieInputModel model)
    {
        if (ModelState.IsValid)
        {
            _logger.LogInformation("Встановлення куки. Значення: {Value}, Дата закінчення: {ExpirationDate}", model.Value, model.ExpirationDate);
            Response.Cookies.Append("CustomCookie", model.Value, new CookieOptions
            {
                Expires = model.ExpirationDate,
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax
            });
            return RedirectToAction("CheckCookie");
        }
        return View("Index", model);
    }
    public IActionResult CheckCookie()
    {
        var cookieValue = Request.Cookies["CustomCookie"];
        _logger.LogInformation("Отримане значення куки: {CookieValue}", cookieValue);
        ViewBag.CookieMessage = string.IsNullOrEmpty(cookieValue)
            ? "Значення куки не знайдено."
            : $"Значення куки: {cookieValue}";
        return View();
    }
}
