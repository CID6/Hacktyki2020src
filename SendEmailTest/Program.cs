using SendGrid;
using SendGrid.Helpers.Mail;
using System;

namespace SendEmailTest
{
    class Program
    {
        static void Main(string[] args)
        {
            SendGridClient client = new SendGridClient("1326H123123gg!");
            SendGridMessage msg = new SendGridMessage()
            {
                From = new EmailAddress("user@test.com"),
                Subject = "Hi",
                PlainTextContent = "test wiadomosci",
                HtmlContent = "test wiadomosci"
            };
            msg.AddTo(new EmailAddress(email));

            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}
