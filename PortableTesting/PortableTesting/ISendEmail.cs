using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PortableTesting
{
    public interface ISendEmail
    {
        void sendEmails(string To, string subject, string Body, Page page);
    }
}
