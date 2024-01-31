using ENS.Application.Authentification;
using ENS.Application.Events;
using ENS.Application.Events.Dto;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ENS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventController(IEventService eventService, IAuthService _authService
    , CurrentUserService currentUser) : ControllerBase
{
    [HttpPost("Create")]
    public void Create([FromHeader(Name = "token")] string headerToken, CreateEventRequestDTO eventRequest)
    {
        Validate();
        eventService.Create(eventRequest);

        Log.Logger
            .Information("Было создано уведомление  пользователем с {UserId} " +
            " на  {PostedDate} времени на текущий контакт {ContactId}"
            , currentUser.Id, eventRequest.PostedDate, eventRequest.Contacts);
    }

    [HttpPost("GetAll")]
    public List<GetAllEventDTO> GetAll([FromHeader(Name = "token")] string headerToken, EventFilterDTO filter)
    {
        Validate();
        return eventService.GetAll(filter);
    }

    private void Validate()
    {
        var tokenExist = HttpContext.Request.Headers.TryGetValue("token", out var token);
        if (!tokenExist) throw new Exception("Токен не найден");

        _authService.ValideToken(token);
    }

}
