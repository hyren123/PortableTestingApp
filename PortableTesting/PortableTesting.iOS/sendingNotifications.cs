using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

using Xamarin.Forms;
using PortableTesting.iOS;
using UserNotifications;

[assembly: Xamarin.Forms.Dependency(typeof(sendingNotifications))]
namespace PortableTesting.iOS
{
    class sendingNotifications : IsendNotification
    {
        public sendingNotifications() { }

        public void CancelingNotification(string Title, string Subtitle, string Body, string requestID, DateTime inputDate, Page page)
        {

        }

        public void SchedulingNotifications(string Title, string Subtitle, string Body, string requestID, DateTime inputDate, Page page)
        {
            bool alertsAllowed = true;

            UNUserNotificationCenter.Current.GetNotificationSettings((settings) =>
            {
                alertsAllowed = (settings.AlertSetting == UNNotificationSetting.Enabled);
            });

            var content = new UNMutableNotificationContent();
            content.Title = Title;
            content.Subtitle = Subtitle;
            content.Body = Body;
            content.Badge = 1;

            var date = new NSDateComponents();
            date.Hour = inputDate.Hour;
            date.Day = inputDate.Day;
            date.Month = inputDate.Month;
            date.Year = inputDate.Year;

            var trigger = UNCalendarNotificationTrigger.CreateTrigger(date, true);

            var request = UNNotificationRequest.FromIdentifier(requestID, content, trigger);

            UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) => {
                if (err != null)
                {
                    // Do something with error...
                }
            });
        }
    }
}