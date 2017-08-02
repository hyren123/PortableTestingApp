using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PortableTesting
{
    public interface IsendNotification
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Subtitle"></param>
        /// <param name="Body"></param>
        /// <param name="RequestID"></param>
        /// <param name="InputDate"></param>
        /// <returns></returns>
        void SchedulingNotifications(string Title, string Subtitle, string Body, string RequestID, DateTime InputDate, Page page);

        void CancelingNotification(string Title, string Subtitle, string Body, string requestID, DateTime inputDate, Page page);
    }
}
