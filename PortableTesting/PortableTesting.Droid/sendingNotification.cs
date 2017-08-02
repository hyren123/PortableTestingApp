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
using Android.Support.V4.App;

using Xamarin.Forms;
using Android.Media;
using PortableTesting.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(sendingNotification))]
namespace PortableTesting.Droid
{
    class sendingNotification : IsendNotification
    {
        public sendingNotification() { }

        public void CancelingNotification(string Title, string Subtitle, string Body, string requestID, DateTime inputDate, Page page)
        {
            Intent alarmIntent = new Intent(Forms.Context, typeof(AlarmReceiver));
            alarmIntent.PutExtra("message", Body);
            alarmIntent.PutExtra("title", Title);
            alarmIntent.PutExtra("id", requestID);
            alarmIntent.PutExtra("date", inputDate.ToString());

            PendingIntent pendingIntent = PendingIntent.GetBroadcast(Forms.Context, int.Parse(requestID), alarmIntent, PendingIntentFlags.UpdateCurrent);
            AlarmManager alarmManager = (AlarmManager)Forms.Context.GetSystemService(Context.AlarmService);

            alarmManager.Cancel(pendingIntent);
        }

        public void SchedulingNotifications(string Title, string Subtitle, string Body, string requestID, DateTime inputDate, Page page)
        {
            Intent alarmIntent = new Intent(Forms.Context, typeof(AlarmReceiver));
            alarmIntent.PutExtra("message", Body);
            alarmIntent.PutExtra("title", Title);
            alarmIntent.PutExtra("id", requestID);
            alarmIntent.PutExtra("date", inputDate.ToString());

            Remind(inputDate, int.Parse(requestID), alarmIntent);
        }

        public void Remind(DateTime dateTime, int ID, Intent alarmIntent)
        {
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(Forms.Context, ID, alarmIntent, PendingIntentFlags.UpdateCurrent);
            AlarmManager alarmManager = (AlarmManager)Forms.Context.GetSystemService(Context.AlarmService);

            Java.Util.Calendar cal = Java.Util.Calendar.Instance;
            cal.Set(Java.Util.CalendarField.Month, dateTime.Month);
            cal.Set(Java.Util.CalendarField.DayOfMonth, dateTime.Day);
            cal.Set(Java.Util.CalendarField.HourOfDay, dateTime.Hour);
            cal.Set(Java.Util.CalendarField.Minute, dateTime.Minute);
            cal.Set(Java.Util.CalendarField.Second, dateTime.Second);

            alarmManager.Set(AlarmType.RtcWakeup, cal.TimeInMillis, pendingIntent);
        }
    }

    [BroadcastReceiver]
    public class AlarmReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            string message = intent.GetStringExtra("message");
            string title = intent.GetStringExtra("title");
            int id = int.Parse(intent.GetStringExtra("id"));
            DateTime time = DateTime.Parse(intent.GetStringExtra("date"));
            //Magic Page for now
            Page page = new SettingsPage();

            /**
            var notIntent = new Intent(context, typeof(MainActivity));
            PendingIntent contentIntent = PendingIntent.GetActivity(context, 0, notIntent, PendingIntentFlags.UpdateCurrent);
            **/

            NotificationCompat.Builder builder = new NotificationCompat.Builder(Forms.Context)
                .SetAutoCancel(true)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetSmallIcon(Resource.Drawable.icon);

            Notification notification = builder.Build();

            NotificationManager notificationManager =
                   Forms.Context.GetSystemService(Context.NotificationService) as NotificationManager;

            notificationManager.Notify(id, notification);

            //sendingNotification sn = new sendingNotification();
            //time.AddDays(1);
            //sn.SchedulingNotifications(title,"",message, id.ToString(), time, page);
        }
    }
}