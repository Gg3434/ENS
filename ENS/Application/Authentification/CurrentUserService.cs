namespace ENS.Application.Authentification;

/// <summary>
/// Текущий авторизованнный пользователь
/// </summary>
public class CurrentUserService
{
    private Guid _id;
    public Guid Id
    {
        get
        {

            return _id;
        }
        set
        {
            if (_id == Guid.Empty)
            {
                _id = value;
            }
        }
    }
}
