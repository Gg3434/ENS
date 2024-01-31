namespace ENS.Application.Authentification.Dto;

/// <summary>
/// Запрос данных для автормизации
/// </summary>
public class LoginRequestDTO
{
    public string Name { get; set; }
    public string Password { get; set; }
}
