using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BDIC_TAXATION_WEB.Pages;

public class App_SettingsModel : PageModel
{
    private readonly ILogger<App_SettingsModel> _logger;

    public App_SettingsModel(ILogger<App_SettingsModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}

