namespace ENS.Application.Authentification.Dto;

/// <summary>
/// Данные для регистрации пользователя
/// </summary>
public class RegistrationRequestDTO
{
    public string Name { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}
