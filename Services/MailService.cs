
using MailKit;
using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;
using StackExchange.Redis;

namespace marketplace_api.Services;

public class MailService
{
    public async Task SendEmailAsync(string messages, string emailUser)
    {
        string smtpServer = "smtp.gmail.com"; 
        int smtpPort = 587; 
        string senderEmail = "rippergods@gmail.com";
        string senderPassword = "";

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Отправитель", senderEmail));
        message.To.Add(new MailboxAddress("Получатель", emailUser));
        message.Subject = "Оповещение";

        var bodyBuilder = new BodyBuilder();
        bodyBuilder.TextBody = messages;

        message.Body = bodyBuilder.ToMessageBody();

        using (var client = new SmtpClient())
        {
            try
            {
                await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(senderEmail, senderPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при отправке оповещения");
            }
        }

    }
}
