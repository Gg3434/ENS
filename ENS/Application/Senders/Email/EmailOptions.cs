namespace ENS.Application.Senders.Email;

/// <summary>
/// Настройки для отправки ивента на почту
/// </summary>
public class EmailOptions
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string From { get; set; }
    public string Pass { get; set; }
    public string Name { get; set; }

}
