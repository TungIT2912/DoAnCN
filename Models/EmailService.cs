using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

public class EmailService
{
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var emailMessage = new MimeMessage();

        //emailMessage.From.Add(new MailboxAddress("Dental Management", "dentalldt2024@gmail.com"));
        emailMessage.From.Add(new MailboxAddress("Dental Management", "nhakhoaweb2024@gmail.com"));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message
        };

        using (var client = new SmtpClient())
        {
            //await client.ConnectAsync("smtp.example.com", 587, false);
            await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync("nhakhoaweb2024@gmail.com", "buxc ybdz wcwj grlg");
            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }
    }
    
}
