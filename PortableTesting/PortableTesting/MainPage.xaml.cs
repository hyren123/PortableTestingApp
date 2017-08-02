using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using System.IO;

namespace PortableTesting
{
    public partial class MainPage : ContentPage
    {
        PCLMechanics pclMech = new PCLMechanics();
        Data rawData = Data.Instance();

        public MainPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            Button Settings = this.FindByName<Button>("Settings");
            Button CalendarPage = this.FindByName<Button>("CalendarPage"); 

            Settings.VerticalOptions = LayoutOptions.End;
            Settings.HorizontalOptions = LayoutOptions.Start;
            CalendarPage.VerticalOptions = LayoutOptions.End;
            CalendarPage.HorizontalOptions = LayoutOptions.End;

            startUp();
        }

        async void GoToPage2(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EmoteButtonPage(), true);
        }

        async void CalenderCharts(object sender, EventArgs e)
        {
            //This Opens up the Calender/barGraph Viewing page, Works
            await Navigation.PushAsync(new CalenderViewing(), true);
        }

        async void SettingsPageOpen(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage(), true);
        }

        //add to startup: query for every email address from SettingsDB
        public void startUp()
        {
            settingsDataBase settings = new settingsDataBase();


            settingsItem setItem = settings.GetSetting(PCLMechanics.SQLNames.TherapistEmail.ToString());

            if (setItem != null)
            {
                rawData.therapistEmailAd = setItem.Value;
            }

            #region AdditionalEmail

            setItem = settings.GetSetting(PCLMechanics.SQLNames.Contact1.ToString());

            if (setItem != null)
            {
                rawData.contact1 = setItem.Value;
            }

            setItem = settings.GetSetting(PCLMechanics.SQLNames.Contact2.ToString());

            if (setItem != null)
            {
                rawData.contact2 = setItem.Value;
            }

            setItem = settings.GetSetting(PCLMechanics.SQLNames.Contact3.ToString());

            if (setItem != null)
            {
                rawData.contact3 = setItem.Value;
            }

            setItem = settings.GetSetting(PCLMechanics.SQLNames.Contact4.ToString());

            if (setItem != null)
            {
                rawData.contact4 = setItem.Value;
            }

            setItem = settings.GetSetting(PCLMechanics.SQLNames.Contact5.ToString());

            if (setItem != null)
            {
                rawData.contact5 = setItem.Value;
            }

            #endregion AdditionalEmail

            #region Notifications

            setItem = settings.GetSetting(PCLMechanics.SQLNames.Notifications.ToString());

            if (setItem != null)
            {
                rawData.notificationstate = setItem.Value == "True" ? true : false;
            }


            setItem = settings.GetSetting(PCLMechanics.SQLNames.Medication.ToString());

            if (setItem != null)
            {
                rawData.Medication = setItem.Value == "True" ? true : false;
            }

            setItem = settings.GetSetting(PCLMechanics.SQLNames.Eating.ToString());

            if (setItem != null)
            {
                rawData.Eating = setItem.Value == "True" ? true : false;
            }

            setItem = settings.GetSetting(PCLMechanics.SQLNames.Sleeping.ToString());

            if (setItem != null)
            {
                rawData.Sleeping = setItem.Value == "True" ? true : false;
            }

            setItem = settings.GetSetting(PCLMechanics.SQLNames.Journal.ToString());

            if (setItem != null)
            {
                rawData.Journal = setItem.Value == "True" ? true : false;
            }

            #endregion Notifications


            #region Data

            setItem = settings.GetSetting(PCLMechanics.SQLNames.SleepHours.ToString());
            if (setItem != null)
            {
                rawData.sleepHours = setItem.Value;
            }


            setItem = settings.GetSetting(PCLMechanics.SQLNames.Mania.ToString());
            if (setItem != null)
            {
                rawData.maniaVal = int.Parse(setItem.Value);
            }


            setItem = settings.GetSetting(PCLMechanics.SQLNames.Depression.ToString());
            if (setItem != null)
            {
                rawData.depressionVal = int.Parse(setItem.Value);
            }


            setItem = settings.GetSetting(PCLMechanics.SQLNames.WakeTime.ToString());
            if (setItem != null)
            {
                rawData.WakeTimes = TimeSpan.Parse(setItem.Value);
            }


            setItem = settings.GetSetting(PCLMechanics.SQLNames.EatTime.ToString());
            if (setItem != null)
            {
                rawData.EatTime = TimeSpan.Parse(setItem.Value);
            }
            #endregion Data
        }
    }
}
