using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using PortableTesting.Droid;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(SendEmail))]
namespace PortableTesting.Droid
{
    public class SendEmail : ISendEmail
    {
        public SendEmail() { }

        public void sendEmails(string To, string Subject, string Body, Page page)
        {
            if (To == "" || Subject == "" || Body == "")
            {
              page.DisplayAlert("Email Error", "Cannot Send email from this device. Check your internet Connection and try again.", "Ok");
                return;
            }
            Intent email = new Intent(Intent.ActionSend);

            email.PutExtra(Intent.ExtraEmail, new [] { To });

            email.PutExtra(Intent.ExtraSubject, Subject);

            email.PutExtra(Intent.ExtraText, Body);

            email.SetType("message/rfc822");

            Forms.Context.StartActivity(Intent.CreateChooser(email, "Send Email"));
        }
    }
}