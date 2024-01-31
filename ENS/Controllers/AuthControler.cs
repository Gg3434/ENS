using ENS.Application.Authentification;
using ENS.Application.Authentification.Dto;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ENS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthControler(IAuthService authService) : ControllerBase
{
    [HttpPost("SignIn")]
    public void SignIn(RegistrationRequestDTO registrationRequest)
    {
        authService.SignIn(registrationRequest);

        Log.Logger.Information("Прошла регистрация пользователя {userName} роль {roleUser}"
            , registrationRequest.Name, registrationRequest.Role);
    }

    [HttpPost("LogIn")]
    public LoginResponceDTO Login(LoginRequestDTO loginRequest)
    {
        var loginResponce = authService.LogIn(loginRequest);

        Log.Logger.Information("Прошла авторизация пользователя {userName}", loginRequest.Name);

        return loginResponce;
    }
}
