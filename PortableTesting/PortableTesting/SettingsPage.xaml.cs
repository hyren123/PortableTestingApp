using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using PCLStorage;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text.RegularExpressions;

namespace PortableTesting
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        Data rawData = Data.Instance();

        ObservableCollection<EntryCell> EmailCells = new ObservableCollection<EntryCell>();

        ObservableCollection<SwitchCell> NotificationCells = new ObservableCollection<SwitchCell>();

        ObservableCollection<TimePicker> TimeChoices = new ObservableCollection<TimePicker>();

        public SettingsPage()
        {
            NavigationPage.SetHasNavigationBar(this, true);

            Title = "Settings";

            //Nothing is programaticaly added
            //Entry Cells and Notifications Linked to Data.cs
            //

            #region EmailEntryCellandLogic
            EntryCell therapistEmail = new EntryCell
            {
                Label = "Therapist Email:",
                Placeholder = "Therapist@example.com",
                Keyboard = Keyboard.Email,
            };

            if (rawData.therapistEmailAd != "")
            {
                therapistEmail.Text = rawData.therapistEmailAd;
            }

            Binding bindingSetup = new Binding("therapistEmailAd");
            bindingSetup.Source = rawData;
            bindingSetup.Mode = BindingMode.TwoWay;
            therapistEmail.SetBinding(EntryCell.TextProperty, bindingSetup);

            EmailCells.Add(therapistEmail);

            #region AdditionalContacts

            string[] str = new string[] {rawData.contact1, rawData.contact2, rawData.contact3, rawData.contact4, rawData.contact5 };

            for (int i = 0; i < str.Length; i++)
            {
                emailCellCreation(i+1, str[i]);
            }

            str = null;

            #endregion AdditionalContacts

            #endregion EmailEntryCellandLogic

            #region DateTimePickers

            #region WakeTimer

            Label WakeUpLable = new Label
            {
                Text = "What time do you wake up?"
            };
            WakeUpLable.VerticalOptions = LayoutOptions.Center;

            TimeSpan wakeTimeSpan = new TimeSpan();

            TimePicker WakeUpTime = new TimePicker();
            WakeUpTime.HorizontalOptions = LayoutOptions.EndAndExpand;

            WakeUpTime.Time = rawData.WakeTimes != null ? rawData.WakeTimes : TimeSpan.Zero;

            WakeUpTime.PropertyChanging += (sender, e) =>
            {
                wakeTimeSpan = WakeUpTime.Time;
            };

            StackLayout WakeView = new StackLayout();
            WakeView.Orientation = StackOrientation.Horizontal;
            WakeView.Children.Add(WakeUpLable);
            WakeView.Children.Add(WakeUpTime);

            #endregion WakeTimer

            #region EatTimer

            Label EatLabel = new Label
            {
                Text = "What time do you eat? ",
            };
            EatLabel.VerticalOptions = LayoutOptions.Center;

            TimePicker EatTime = new TimePicker();
            EatTime.HorizontalOptions = LayoutOptions.EndAndExpand;

            EatTime.Time = rawData.EatTime != null ? rawData.EatTime : TimeSpan.Zero;

            TimeSpan eatSpan = new TimeSpan();
            EatTime.PropertyChanging += (sender, e) =>
            {
                eatSpan = EatTime.Time;
            };

            StackLayout EatView = new StackLayout();
            EatView.Orientation = StackOrientation.Horizontal;
            EatView.Children.Add(EatLabel);
            EatView.Children.Add(EatTime);

            #endregion EatTimer

            #region EmailReminderPicker

            Label EmailLabel = new Label()
            {
                Text = "When do you want a Reminder? ",
            };
            EmailLabel.VerticalOptions = LayoutOptions.Center;

            Picker EmailReminder = new Picker();
            EmailReminder.Items.Add("Never");
            EmailReminder.Items.Add("Once a Week");
            EmailReminder.Items.Add("Every Other Week");
            EmailReminder.Items.Add("Once a Month");

            EmailReminder.HorizontalOptions = LayoutOptions.FillAndExpand;

            EmailReminder.SelectedIndex = rawData.EmailReminder;

            EmailReminder.SelectedIndexChanged += (sender, e) =>
            { 
                rawData.EmailReminder = EmailReminder.SelectedIndex;

                DateTime tempTime = DateTime.Now;

                bool schedule = false;

                switch (EmailReminder.SelectedIndex)
                {
                    case 1:
                        schedule = true;
                        tempTime = tempTime.AddDays(7);
                        break;
                    case 2:
                        schedule = true;
                        tempTime = tempTime.AddDays(14);
                        break;
                    case 3:
                        schedule = true;
                        tempTime = tempTime.AddMonths(1);
                        break;
                    default: break;
                }
                NotificationChanged(schedule, "Send Email","","Time to Send Your Email!", "4", tempTime);
            };

            StackLayout EmailView = new StackLayout();
            EmailView.Orientation = StackOrientation.Horizontal;
            EmailView.Children.Add(EmailLabel);
            EmailView.Children.Add(EmailReminder);

            #endregion EmailReminderPicker

            #endregion DateTimePickers

            #region NotificationButtons

            MessagingCenter.Subscribe<SettingsPage, bool>(this, "Notifications", (sender, args) =>
            {
                foreach (SwitchCell sc in NotificationCells)
                {
                    sc.On = args;
                }
            });

            #region AllNotifications
            SwitchCell notification = new SwitchCell()
            {
                Text = "All Notifications",
            };

            notification.OnChanged += (sender, e) => MessagingCenter.Send<SettingsPage, bool>(this, "Notifications", notification.On);

            bindingSetup = new Binding("notificationstate");
            bindingSetup.Source = rawData;
            bindingSetup.Mode = BindingMode.TwoWay;
            notification.SetBinding(SwitchCell.OnProperty, bindingSetup);

            NotificationCells.Add(notification);
            #endregion AllNotifications


            #region Medication
            SwitchCell Medication = new SwitchCell()
            {
                Text = "Medication",
            };

            bindingSetup = new Binding("Medication");
            bindingSetup.Source = rawData;
            bindingSetup.Mode = BindingMode.TwoWay;
            Medication.SetBinding(SwitchCell.OnProperty, bindingSetup);

            Medication.OnChanged += (sender, e) =>
            {
                NotificationChanged(Medication.On, "Medication Reminder", "", "Time to take your Medications", "0", DateTime.Now.AddSeconds(15));
            };

            NotificationCells.Add(Medication);


            #endregion Medication


            #region Eating
            SwitchCell Eating = new SwitchCell()
            {
                Text = "Eating",
            };

            bindingSetup = new Binding("Eating");
            bindingSetup.Source = rawData;
            bindingSetup.Mode = BindingMode.TwoWay;
            Eating.SetBinding(SwitchCell.OnProperty, bindingSetup);

            Eating.OnChanged += (sender, e) =>
            {
                for (int i = 0; i < 3; i++)
                {
                    if (eatSpan == EatTime.Time)
                    {
                        NotificationChanged(Eating.On, "Time to Eat", "", "Time to find something tasty.", "1",
                            new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, EatTime.Time.Hours + (i * 4), EatTime.Time.Minutes, 0));
                    }
                    else
                        NotificationChanged(Eating.On, "Time to Eat", "", "Time to find something tasty.", "1",
                             new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, eatSpan.Hours + (i * 4), eatSpan.Minutes, 0));
                }
            };

            NotificationCells.Add(Eating);
            #endregion Eating


            #region Sleeping
            SwitchCell Sleeping = new SwitchCell()
            {
                Text = "Sleeping",
            };

            bindingSetup = new Binding("Sleeping");
            bindingSetup.Source = rawData;
            bindingSetup.Mode = BindingMode.TwoWay;
            Sleeping.SetBinding(SwitchCell.OnProperty, bindingSetup);

            Sleeping.OnChanged += (sender, e) =>
            {
                if (wakeTimeSpan == WakeUpTime.Time)
                {
                    NotificationChanged(Sleeping.On, "Sleep Input", "", "How much sleep did you get?", "2",
                        new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, wakeTimeSpan.Hours, wakeTimeSpan.Minutes, 0));
                }
                else 
                NotificationChanged(Sleeping.On, "Sleep Input", "", "How much sleep did you get?", "2",
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month,DateTime.Now.Day, WakeUpTime.Time.Hours, WakeUpTime.Time.Minutes, 0));
            };
            NotificationCells.Add(Sleeping);
            #endregion Sleeping


            #region Journal
            SwitchCell Journal = new SwitchCell()
            {
                Text = "Journal",
            };

            bindingSetup = new Binding("Journal");
            bindingSetup.Source = rawData;
            bindingSetup.Mode = BindingMode.TwoWay;
            Journal.SetBinding(SwitchCell.OnProperty, bindingSetup);

            Journal.OnChanged += (sender, e) =>
            {
                if (wakeTimeSpan == WakeUpTime.Time)
                {
                    DateTime temp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, WakeUpTime.Time.Hours, WakeUpTime.Time.Minutes, 0);
                    temp.AddMinutes(30);
                    temp.AddHours(14);

                    NotificationChanged(Journal.On, "Journal Entries", "", "Time to fill out a note about your day today", "3", temp);
                }
                else
                {
                    DateTime temp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, wakeTimeSpan.Hours, wakeTimeSpan.Minutes, 0);
                    temp.AddMinutes(30);
                    temp.AddHours(14);

                    NotificationChanged(Journal.On, "Journal Entries", "", "Time to fill out a note about your day today", "3", temp);
                }
            };

            NotificationCells.Add(Journal);
            #endregion Journal

            #endregion NotificationButtons

            //Add content to this
            WakeUpTime.PropertyChanged += (sender, e) =>
            {

                if (Sleeping.On)
                {
                    Sleeping.On = false;
                    Sleeping.On = true;
                }

                if (Journal.On)
                {
                    Journal.On = false;
                    Journal.On = true;
                }

                wakeTimeSpan = WakeUpTime.Time;

                rawData.WakeTimes = WakeUpTime.Time;
            };

            EatTime.PropertyChanged += (sender, e) =>
            {
                if (Eating.On)
                {
                    Eating.On = false;
                    Eating.On = true;
                }

                eatSpan = EatTime.Time;

                rawData.EatTime = EatTime.Time;
            };

            #region Views

            TableView notifTable = new TableView
            {
                Intent = TableIntent.Settings,
                Root = new TableRoot
                {
                    new TableSection("Notifications")
                    {
                        NotificationCells,
                    }
                }
            };

            TableView emailTable = new TableView
            {
                Intent = TableIntent.Settings,
                Root = new TableRoot
                {
                    new TableSection("Email")
                    {
                        EmailCells
                    }
                }
            };

            StackLayout MainStack = new StackLayout
            {
                Spacing = 0,
                Children =
                {
                    emailTable,
                    EatView,
                    WakeView,
                    notifTable,
                    EmailView,
                }
            };

            ScrollView scroll = new ScrollView
            {
                Content = MainStack,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            #endregion Views

            this.Content = scroll;

            InitializeComponent();
        }

        private void emailCellCreation(int cellNum, string Text)
        {
            string s = "Contact" + (cellNum);
            EntryCell ec = new EntryCell()
            {
                Label = s + ": ",
                Placeholder = s + "@" + s + ".com",
                Keyboard = Keyboard.Email,
                Text = Text,
            };

            Binding ecbind = new Binding("contact"+cellNum);
            ecbind.Source = rawData;
            ecbind.Mode = BindingMode.TwoWay;
            ec.SetBinding(EntryCell.TextProperty, ecbind);

            EmailCells.Add(ec);
        }

        private void NotificationChanged(bool On, string Title, string Subtitle, string Body, string RequestID, DateTime InputDate)
        {
            if (On)
            {
                if (InputDate > DateTime.Now)
                {
                    DependencyService.Get<IsendNotification>().SchedulingNotifications(Title, Subtitle, Body, RequestID, InputDate, this);
                }
                else
                {
                    InputDate = InputDate.AddDays(1);
                    DependencyService.Get<IsendNotification>().SchedulingNotifications(Title, Subtitle, Body, RequestID, InputDate, this);
                }
            }
            else
            {
                if (InputDate > DateTime.Now)
                {
                    DependencyService.Get<IsendNotification>().CancelingNotification(Title, Subtitle, Body, RequestID, InputDate, this);
                }
                else
                {
                    InputDate = InputDate.AddDays(1);
                    DependencyService.Get<IsendNotification>().CancelingNotification(Title, Subtitle, Body, RequestID, InputDate, this);
                }
            }
        }

        protected override void OnDisappearing()
        {
            settingsDataBase settings = new settingsDataBase();

            settingsItem setItem = new settingsItem();
            
            #region Email

            #region TherapistEmail
            setItem = settings.GetSetting((int)PCLMechanics.SQLNames.TherapistEmail);

            if (setItem != null)
            {
                setItem.Value = rawData.therapistEmailAd;

                settings.AddSetting(setItem);
            }
            else settings.AddSetting(new settingsItem {Name = PCLMechanics.SQLNames.TherapistEmail.ToString(), Value = rawData.therapistEmailAd});
            #endregion TherapistEmail

            #region ContactEmail

            setItem = settings.GetSetting(PCLMechanics.SQLNames.Contact1.ToString());

            if (setItem != null)
            {
                setItem.Value = rawData.contact1;

                settings.AddSetting(setItem);
            }
            else settings.AddSetting(new settingsItem { Name = PCLMechanics.SQLNames.Contact1.ToString(), Value = rawData.contact1 });

            setItem = settings.GetSetting(PCLMechanics.SQLNames.Contact2.ToString());

            if (setItem != null)
            {
                setItem.Value = rawData.contact2;

                settings.AddSetting(setItem);
            }
            else settings.AddSetting(new settingsItem { Name = PCLMechanics.SQLNames.Contact2.ToString(), Value = rawData.contact2 });

            setItem = settings.GetSetting(PCLMechanics.SQLNames.Contact3.ToString());

            if (setItem != null)
            {
                setItem.Value = rawData.contact3;

                settings.AddSetting(setItem);
            }
            else settings.AddSetting(new settingsItem { Name = PCLMechanics.SQLNames.Contact3.ToString(), Value = rawData.contact3 });

            setItem = settings.GetSetting(PCLMechanics.SQLNames.Contact4.ToString());

            if (setItem != null)
            {
                setItem.Value = rawData.contact4;

                settings.AddSetting(setItem);
            }
            else settings.AddSetting(new settingsItem { Name = PCLMechanics.SQLNames.Contact4.ToString(), Value = rawData.contact4 });

            setItem = settings.GetSetting(PCLMechanics.SQLNames.Contact5.ToString());

            if (setItem != null)
            {
                setItem.Value = rawData.contact5;

                settings.AddSetting(setItem);
            }
            else settings.AddSetting(new settingsItem { Name = PCLMechanics.SQLNames.Contact5.ToString(), Value = rawData.contact5 });



            #endregion ContactEmail

            #endregion Email


            #region savingNotifications

            //All settings
            setItem = settings.GetSetting(PCLMechanics.SQLNames.Notifications.ToString());

            if (setItem != null)
            {
                setItem.Value = rawData.notificationstate ? "True" : "False";

                settings.AddSetting(setItem);
            }
            else settings.AddSetting(new settingsItem { Name = PCLMechanics.SQLNames.Notifications.ToString(), Value = rawData.notificationstate ? "True" : "False" });

            //medication
            setItem = settings.GetSetting(PCLMechanics.SQLNames.Medication.ToString());

            if (setItem != null)
            {
                setItem.Value = rawData.Medication ? "True" : "False";

                settings.AddSetting(setItem);
            }
            else settings.AddSetting(new settingsItem { Name = PCLMechanics.SQLNames.Medication.ToString(), Value = rawData.Medication ? "True" : "False"});

            //Eating
            setItem = settings.GetSetting(PCLMechanics.SQLNames.Eating.ToString());

            if (setItem != null)
            {
                setItem.Value = rawData.Eating ? "True" : "False";

                settings.AddSetting(setItem);
            }
            else settings.AddSetting(new settingsItem { Name = PCLMechanics.SQLNames.Eating.ToString(), Value = rawData.Eating ? "True" : "False" });

            //Sleeping
            setItem = settings.GetSetting(PCLMechanics.SQLNames.Sleeping.ToString());

            if (setItem != null)
            {
                setItem.Value = rawData.Sleeping ? "True" : "False";

                settings.AddSetting(setItem);
            }
            else settings.AddSetting(new settingsItem { Name = PCLMechanics.SQLNames.Sleeping.ToString(), Value = rawData.Sleeping ? "True" : "False" });

            //Journal
            setItem = settings.GetSetting(PCLMechanics.SQLNames.Journal.ToString());

            if (setItem != null)
            {
                setItem.Value = rawData.Journal ? "True" : "False";

                settings.AddSetting(setItem);
            }
            else settings.AddSetting(new settingsItem { Name = PCLMechanics.SQLNames.Journal.ToString(), Value = rawData.Journal ? "True" : "False" });

            #endregion savingNotifications

            #region TimePickers

            //Wake up Time
            setItem = settings.GetSetting(PCLMechanics.SQLNames.WakeTime.ToString());

            if (setItem != null)
            {
                setItem.Value = rawData.WakeTimes.ToString();

                settings.AddSetting(setItem);
            }
            else settings.AddSetting(new settingsItem { Name = PCLMechanics.SQLNames.WakeTime.ToString(), Value = rawData.WakeTimes.ToString() });

            //Eating Time
            setItem = settings.GetSetting(PCLMechanics.SQLNames.EatTime.ToString());

            if (setItem != null)
            {
                setItem.Value = rawData.EatTime.ToString();

                settings.AddSetting(setItem);
            }
            else settings.AddSetting(new settingsItem { Name = PCLMechanics.SQLNames.EatTime.ToString(), Value = rawData.EatTime.ToString() });


            #endregion TimePickers

            setItem = settings.GetSetting(PCLMechanics.SQLNames.EmailReminder.ToString());

            if (setItem != null)
            {
                setItem.Value = rawData.EmailReminder.ToString();

                settings.AddSetting(setItem);
            }
            else settings.AddSetting(new settingsItem { Name = PCLMechanics.SQLNames.EmailReminder.ToString(), Value = rawData.EatTime.ToString() });


            NotificationCells.Clear();

            base.OnDisappearing();
        }
    }
}
