using ENS.Application.Authentification.Dto;
using ENS.Domain.Entities;
using System.Text.Json;

namespace ENS.Application.Authentification;

public class AuthService(ApplicationContext applicationContext, CurrentUserService currentUserService) : IAuthService
{
    /// <summary>
    /// Авторизация пользователя 
    /// </summary>
    /// <param name="loginRequest">Данные для авторизации</param>
    /// <returns></returns>
    /// <exception cref="Exception">Пользователя нет</exception>
    public LoginResponceDTO LogIn(LoginRequestDTO loginRequest)
    {
        var loginResponce = new LoginResponceDTO();
        if (!string.IsNullOrEmpty(loginRequest.Name) && !string.IsNullOrEmpty(loginRequest.Password))
        {
            var user = applicationContext.Users
                .SingleOrDefault(x => x.UserName == loginRequest.Name && x.Password == loginRequest.Password)
                ?? throw new Exception("Пользователь не найден");

            loginResponce.Token = CreateToken(user);
        }
        return loginResponce;
    }

    /// <summary>
    /// Регистрация пользователя 
    /// </summary>
    /// <param name="registrationRequest">Данные для регистрации</param>
    public void SignIn(RegistrationRequestDTO registrationRequest)
    {
        var currentUser = applicationContext.Users.FirstOrDefault(x => x.UserName == registrationRequest.Name);
        
        if (currentUser != null) throw new Exception("Ошибка, пользователь с таким логином уже существует");
       
        var user = new User
        {
            UserName = registrationRequest.Name,
            Password = registrationRequest.Password,
            Role = registrationRequest.Role,
        };
        applicationContext.Users.Add(user);
        applicationContext.SaveChanges();
    }

    /// <summary>
    /// Создание токена пользователя 
    /// </summary>
    /// <param name="user">Получаемый пользователь</param>
    /// <returns></returns>
    public string CreateToken(User user)
    {
        var payload = new TokenPayload()
        {
            Name = user.UserName,
            Id = user.Id
        };

        var secret = "my-secret-token";
        var jsonPayload = JsonSerializer.Serialize(payload);
        var jwtToken = $"{jsonPayload}.{secret}";

        return jwtToken;
    }

    /// <summary>
    /// Проверка валидности токена 
    /// </summary>
    /// <param name="token">Получаемый токен</param>
    public void ValideToken(string token)
    {
        var exception = new Exception("Ошибка аутенфикации");

        if (string.IsNullOrEmpty(token)) throw exception;

        if (token.Split('.') is not [var payload,var secret]) throw exception;
        
        if (secret != "my-secret-token") throw exception;

        var tokenPayload = JsonSerializer.Deserialize<TokenPayload>(payload);

        var user = applicationContext.Users
            .FirstOrDefault(x => x.UserName == tokenPayload.Name && x.Id == tokenPayload.Id) ?? throw exception;

        currentUserService.Id = user.Id;
    }

}
