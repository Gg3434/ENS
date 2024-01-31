using ENS.Application.Authentification;
using Serilog;

namespace ENS.MiddleWare;

public class TokenMiddleWare(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IAuthService _authService, CurrentUserService userService)
    {
        var token = context.Request.Headers["token"];
        if (!string.IsNullOrEmpty(token))
        {
            _authService.ValideToken(token);
            Log.Logger.Information("Пользователь с {userId} прошел авторизацию", userService.Id);

            await next.Invoke(context);
        }
        else
        {
            await next.Invoke(context);
        }
    }
}
