using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PortableTesting
{
    public class App : Application
    {
        public App()
        {
            // The root page of your application
            MainPage = Init();
        }

        public static NavigationPage Init()
        {
            NavigationPage nav = new NavigationPage(new MainPage());
            return nav;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
