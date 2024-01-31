using ENS;
using ENS.Application.Authentification;
using ENS.Application.Events;
using ENS.Application.Senders;
using ENS.Application.Senders.Common;
using ENS.Application.Senders.Email;
using ENS.Application.Senders.Whatsapp;
using ENS.MiddleWare;
using Microsoft.EntityFrameworkCore;
using Serilog;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateBootstrapLogger();



builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<CurrentUserService>();


builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("EmailOptions"));


builder.Services.AddScoped<IEventSender, WhatsappSender>();
builder.Services.AddScoped<IEventSender, EmailSender>();

builder.Services.AddScoped<ISenderFactory, SenderFactory>();


builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("D")));

builder.Services.AddHostedService<EventSendingBackGroundService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseMiddleware<TokenMiddleWare>();




app.UseAuthorization();

app.MapControllers();

app.Run();