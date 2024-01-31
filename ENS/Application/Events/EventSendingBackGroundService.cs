using ENS.Application.Senders;
using ENS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ENS.Application.Events;

/// <summary>
/// Сервис обработки задач в фоновом режиме
/// </summary>
/// <param name="serviceProvider">сервис провайдер для вытягивания зависимостей</param>
public class EventSendingBackGroundService(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();

        var eventService = scope.ServiceProvider.GetService<IEventService>();

        var dbContext = scope.ServiceProvider.GetService<ApplicationContext>()

        var senderFactory = scope.ServiceProvider.GetService<ISenderFactory>();

        while (!cancellationToken.IsCancellationRequested)
        {
            var currentDate = DateTime.UtcNow;
            var events = await dbContext.Events
                .Where(x =>
                    x.PostedDate.Year == currentDate.Year &&
                    x.PostedDate.Month == currentDate.Month &&
                    x.PostedDate.Minute == currentDate.Minute &&
                    x.PostedDate.Hour == currentDate.Hour &&
                    x.PostedDate.Day == currentDate.Day &&
                    x.IsPosted == false)
                .ToListAsync();

            foreach (var Event in events)
            {
                senderFactory.Create(Event.Type).Send(Event);
                Event.IsPosted = true;
            }
            dbContext.SaveChanges();

            await Task.Delay(5000, cancellationToken);
        }
    }
}