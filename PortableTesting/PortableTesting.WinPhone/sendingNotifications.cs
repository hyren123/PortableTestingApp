using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Notifications;
using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.Data.Xml.Dom;

using Xamarin.Forms;
using PortableTesting.WinPhone;

[assembly: Xamarin.Forms.Dependency(typeof(sendingNotifications))]
namespace PortableTesting.WinPhone
{
    class sendingNotifications : IsendNotification
    {
        public sendingNotifications() { }

        public void SchedulingNotifications(string Title, string Subtitle, string Body, string RequestID, DateTime InputDate, Page page)
        {
            var toastXml = Windows.UI.Notifications.ToastNotificationManager.GetTemplateContent(Windows.UI.Notifications.ToastTemplateType.ToastText02);
            var strings = toastXml.GetElementsByTagName("text");
            strings[0].AppendChild(toastXml.CreateTextNode(Title));
            strings[1].AppendChild(toastXml.CreateTextNode(Body));

            if (InputDate > DateTime.Now.AddSeconds(5))
            {
                ScheduledToastNotification scheduledNotif = new ScheduledToastNotification(toastXml, InputDate);
                scheduledNotif.Id = RequestID;
                // And add it to the schedule
                ToastNotificationManager.CreateToastNotifier().AddToSchedule(scheduledNotif);
            }
        }

        public void CancelingNotification(string Title, string Subtitle, string Body, string requestID, DateTime inputDate, Page page)
        {
            var notifier = Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier();
            var scheduled = notifier.GetScheduledToastNotifications();

            for (int i = 0, length = scheduled.Count; i < length; i++)
            {
                if (scheduled[i].Id == requestID)
                {
                    notifier.RemoveFromSchedule(scheduled[i]);
                }
            }
        }
    }
}
