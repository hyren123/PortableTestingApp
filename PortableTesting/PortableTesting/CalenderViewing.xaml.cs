using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using Syncfusion.SfCalendar.XForms;

namespace PortableTesting
{
    public partial class CalenderViewing : ContentPage
    {
        Data rawData = Data.Instance();
        PCLMechanics pclMech = new PCLMechanics();
        DatePicker datePicker;
        List<Label> labL = new List<Label>();

        public CalenderViewing()
        {
            InitializeComponent();

            Title = "View History";

            datePicker = new DatePicker
            {
                Format = "d",
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            Label HeaderText = new Label { Text = "Choose Date", HorizontalOptions = LayoutOptions.Center, FontSize = 20, };

            StackLayout stack = new StackLayout()
            {
                Padding = new Thickness(20),
            };

            LoadDataToView(stack, DateTime.Today);

            datePicker.DateSelected += (object sender, DateChangedEventArgs e) =>
            {
                LoadDataToView(stack, datePicker.Date);
            };

            Button email = new Button()
            {
                Text = "Email History",
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            email.Clicked += emailToBeSent;

            ScrollView scroll = new ScrollView
            {
                Content = stack,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            //All UI Elements must be added here
            StackLayout MainStack = new StackLayout
            {
                Spacing = 0,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children =
                {
                    HeaderText,
                    datePicker,
                    email,
                    scroll,
                }
            };
            
            this.Content = MainStack;
        }


        //Takes in a stackLayout, to build labels of each of the Emotional Log items
        void LoadDataToView(StackLayout stack, DateTime userChosenDate)
        {
            //Load data to view from SQL Database or Data class

            //else stuff below
            //the list may not be present, display a warning showing that fact
            {
                DisplayAlert("File Missing", "No log for " + userChosenDate.Month + "/" + userChosenDate.Day + "/" + userChosenDate.Year, "Ok");
                return;
            }
        }



        //Email to be built and sent
        async void emailToBeSent(object sender, EventArgs e)
        {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                await Navigation.PushAsync(new EmailPage(), true);
            }
            else
            {
                await DisplayAlert("Email Error", "Cannot Send email from this device. Check your internet Connection and try again.", "Ok");
                return;
            }
        }
    }
}    
