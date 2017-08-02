using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Syncfusion.SfCalendar.XForms;

namespace PortableTesting
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmailPage : ContentPage
    {
        Data rawData = Data.Instance();

        public EmailPage()
        {
            Title = "Select Dates";

            SfCalendar calendar = new SfCalendar();
            calendar.SelectionMode = SelectionMode.MultiSelection;

            List<DateTime> userchosenDates = new List<DateTime>();

            calendar.OnCalendarTapped += (sender, e) => { addDatesToList(calendar.SelectedDates, userchosenDates); };

            Button sendMail = new Button()
            {
                Text = "Send Email",
                HorizontalOptions = LayoutOptions.Center,
            };

            sendMail.Clicked += (sender, e) => 
            {
                DependencyService.Get<ISendEmail>().sendEmails(rawData.therapistEmailAd, "Test Message", "Successful Test??", this);
            };
            
            StackLayout mainStack = new StackLayout
            {
                Spacing = 0,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    calendar,
                    //sendMail,
                }
            };

            this.Content = mainStack;

            InitializeComponent();
        }



        void addDatesToList(List<DateTime> DateList, List<DateTime> DateStorage)
        {
            DateList = DateStorage;
        }
    }
}
