using ENS.Application.Senders.Common;
using ENS.Domain.Entities;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace ENS.Application.Senders.Email;

public class EmailSender : IEventSender
{
    private EmailOptions _options;
    public EmailSender(IOptions<EmailOptions> options)
    {
        _options = options.Value;
    }
    public void Send(Event @event)
    {
        if (@event.Type != EventType.Email)
            throw new Exception();

        var Body = $@"
        <!DOCTYPE html>
        <html lang=""en"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <title>{@event.Subject}</title>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f4;
                    margin: 0;
                    padding: 0;
                    display: flex;
                    justify-content: center;
                    align-items: center;
                    height: 100vh;
                }}

                .container {{
                    background-color: #fff;
                    padding: 20px;
                    border-radius: 8px;
                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                    width: 400px;
                    border: 2px solid #4caf50;
                }}

                h2 {{
                    color: #4caf50;
                    text-align: center;
                    margin-bottom: 20px;
                }}

                .content {{
                    border-top: 2px solid #4caf50;
                    padding-top: 20px;
                }}

                p {{
                    margin-bottom: 10px;
                }}
            </style>
        </head>
        <body>

        <div class=""container"">
            <h2>{@event.Subject}</h2>
            <div class=""content"">
                <p>Здравствуйте,</p>
                <p>Это текст вашего письма. Здесь может быть любой контент, который вы хотите передать получателю.</p>
                <p>С наилучшими пожеланиями,</p>
                <p></p>
            </div>
        </div>

        </body>
        </html>";

        var mail = CreateMesage(@event.Contacts, @event.Subject, Body);
        SendMail(mail);
    }
    private List<MailMessage> CreateMesage(List<string> contacts, string subject, string body)
    {
        var from = new MailAddress(_options.From, _options.Name);

        var mailsMessages = contacts
            .Select(adress =>
            {
                var adres = new MailAddress(adress);
                return adres;
            })
            .Select(adress => new MailMessage(from, adress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            }).ToList();

        return mailsMessages;
    }
    private void SendMail(List<MailMessage> mails)
    {
        var smtpClient = new SmtpClient(_options.Host, _options.Port)
        {
            Credentials = new NetworkCredential(_options.From, _options.Pass),
            EnableSsl = true
        };

        mails.ForEach(mail => smtpClient.Send(mail));
    }
}
