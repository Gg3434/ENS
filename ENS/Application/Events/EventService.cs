using ENS.Application.Authentification;
using ENS.Application.Events.Dto;
using ENS.Application.Senders;
using ENS.Domain.Entities;
using ENS.Domain.Entities.Common;
using System.Linq.Expressions;

namespace ENS.Application.Events;

public static class CustomWhereIf
{
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> values, bool condition, Expression<Func<T, bool>> expression) where T : EntityBase
    {
        if (condition)
        {
            return values.Where(expression);
        }
        return values;
    }
}

/// <summary>
/// Сервис работы с ивентами
/// </summary>
/// <param name="applicationContext">Dbcontext для работы с БД</param>
/// <param name="currentUserService">Текущий авторизованный пользователь</param>
/// <param name="senderFactory">Сервис создающий обьект отправляемого ивента</param>
public class EventService(ApplicationContext applicationContext,
    CurrentUserService currentUserService, ISenderFactory senderFactory) : IEventService
{
    /// <summary>
    /// Создание ивента
    /// </summary>
    /// <param name="eventRequest"> Данные для создания</param>
    public void Create(CreateEventRequestDTO eventRequest)
    {
        var @event = new Event
        {
            Text = eventRequest.Text,
            Type = eventRequest.Type,
            Contacts = eventRequest.Contacts,
            PostedDate = eventRequest.PostedDate,
            Subject = eventRequest.Subject,
            UserId = currentUserService.Id
        };

        applicationContext.Events.Add(@event);
        applicationContext.SaveChanges();
    }

    /// <summary>
    /// Получить отфильтрованные ивенты
    /// </summary>
    /// <param name="filter">Тип фильтра</param>
    /// <returns></returns>
    public List<GetAllEventDTO> GetAll(EventFilterDTO filter)
    {
        return applicationContext.Events
            .Where(x => x.UserId == currentUserService.Id)
            .WhereIf(filter.EventType != null, x => x.Type == filter.EventType)
            .WhereIf(filter.PostedDate != null, x => x.PostedDate == filter.PostedDate)
            .WhereIf(filter.IsPosted != null, x => x.IsPosted == filter.IsPosted)
            .Select(x => new GetAllEventDTO
            {
                Contacts = x.Contacts,
                PostedDate = x.PostedDate,
                Subject = x.Subject,
                CreatedDate = x.CreatedDate,
                IsPosted = x.IsPosted,
                Text = x.Text,
                Type = x.Type
            }
            ).ToList(); ;

    }

    /// <summary>
    /// Отправка ивента
    /// </summary>
    /// <param name="id">Id отправляемого ивента</param>
    /// <exception cref="Exception">Если ивент пустой</exception>
    public void Send(Guid id)
    {
        var @event = applicationContext.Events
            .FirstOrDefault(e => e.Id == id) ?? throw new Exception();

        senderFactory.Create(@event.Type).Send(@event);
    }

}

