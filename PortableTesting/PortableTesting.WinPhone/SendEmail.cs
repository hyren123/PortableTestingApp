using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.ApplicationModel.Email;

using PortableTesting.WinPhone;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(SendEmail))]
namespace PortableTesting.WinPhone
{
    class SendEmail : ISendEmail
    {
        public SendEmail() { }

        public async void sendEmails(string To, string Subject, string Body, Page page)
        {
            EmailMessage msg = new EmailMessage();
            msg.Body = Body;
            msg.Subject = Subject;
            EmailRecipient msgTo = new EmailRecipient();
            msgTo.Address = To;
            msg.To.Add(msgTo);
            await EmailManager.ShowComposeNewEmailAsync(msg);
        }
    }
}
