using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskStatusTransitionValidation.RazorMock.Services;

public class LoginModel : PageModel
{
    private readonly IApiClient _api;

    public LoginModel(IApiClient api)
    {
        _api = api;
    }

    [BindProperty]
    public LoginRequest Input { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var result = await _api.LoginAsync(Input);

            if (result == null)
            {
                ErrorMessage = "ログインに失敗しました。入力内容をご確認いただくか、時間をおいて再度お試しください。";
                return Page();
            }

            Response.Cookies.Append("auth_token", result.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax
            });

            return RedirectToPage("/Projects/Index");
        }
        catch
        {
            ErrorMessage = "ログイン処理中にエラーが発生しました。時間をおいて再度お試しください。";
            return Page();
        }
    }
}