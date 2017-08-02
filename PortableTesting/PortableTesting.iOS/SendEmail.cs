using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using MessageUI;

using Xamarin.Forms;

namespace PortableTesting.iOS
{
    class SendEmail : ISendEmail
    {
        public SendEmail() { }

        public void sendEmails(string To, string Subject, string Body, Page page)
        {
            MFMailComposeViewController mailController;

            if (MFMailComposeViewController.CanSendMail)
            {
                mailController = new MFMailComposeViewController();

                mailController.SetToRecipients(new string[] { To });
                mailController.SetSubject(Subject);
                mailController.SetMessageBody(Body, false);

                mailController.Finished += (object s, MFComposeResultEventArgs args) =>
                {
                    Console.WriteLine(args.Result.ToString());
                    args.Controller.DismissViewController(true, null);
                };

                UIViewController uivc = new UIViewController();
                uivc.PresentViewController(mailController, false, null);
            }
        }
    }
}