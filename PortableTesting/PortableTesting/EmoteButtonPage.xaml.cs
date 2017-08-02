using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using Xamarin.Forms;

namespace PortableTesting
{
    public partial class EmoteButtonPage : ContentPage
    {
        Data rawData = Data.Instance();
        Slider ManicSlider = new Slider();
        Slider DepressedSlider = new Slider();
        Label ManiaL = new Label();
        Label DepresionL = new Label();

        PCLMechanics pclMech = new PCLMechanics();

        public EmoteButtonPage()
        {
            ManicSlider.Minimum = 0;
            ManicSlider.Maximum = 100;

            DepressedSlider.Minimum = 0;
            DepressedSlider.Maximum = 100;

            NavigationPage.SetHasNavigationBar(this, true);
            
            InitializeComponent();

            Title = "Emotional Menu";

            StackLayout stackL2 = new StackLayout() { Padding = new Thickness(20) };


            #region Controls

            #region sliders

            ManiaL.Text = "Mania = " + rawData.maniaVal;
            ManiaL.FontSize = 20;
            ManicSlider.Value = rawData.maniaVal;

            ManicSlider.ValueChanged += (sender, e) => OnSliderValueChanged(sender, e, 0, ManiaL);

            DepresionL.Text = "Depression = " + rawData.depressionVal;
            DepresionL.FontSize = 20;
            DepressedSlider.Value = rawData.depressionVal;

            DepressedSlider.ValueChanged += (sender, e) => OnSliderValueChanged(sender,e,1,DepresionL);

            #endregion sliders


            #region EntryCells

            EntryCell sleepHours = new EntryCell()
            {
                Label = "Hours of Sleep?",
            };

            Binding sleepBind = new Binding("sleepHours");
            sleepBind.Source = rawData;
            sleepBind.Mode = BindingMode.TwoWay;
            sleepHours.SetBinding(EntryCell.TextProperty, sleepBind);


            #endregion EntryCells

            #endregion Controls

            var section = new TableSection
            {
                sleepHours,
            };

            var root = new TableRoot { section };

            var table = new MenuTableView()
            {
                Intent = TableIntent.Menu,
                Root = root,
            };

            stackL2.Children.Add(ManiaL);
            stackL2.Children.Add(ManicSlider);
            stackL2.Children.Add(DepresionL);
            stackL2.Children.Add(DepressedSlider);
            stackL2.Children.Add(table);

            ScrollView scrollV = new ScrollView();
            scrollV.Content = stackL2;

            Content = scrollV;
        }


        void OnSliderValueChanged(object sender, ValueChangedEventArgs e, int Choice, Label textL)
        {
            switch (Choice)
            {
                case 0:
                    textL.Text = "Mania = " + (int)ManicSlider.Value;
                    rawData.maniaVal = (int)ManicSlider.Value;
                    break;
                case 1:
                    textL.Text = "Depression = " + (int)DepressedSlider.Value;
                    rawData.depressionVal = (int)DepressedSlider.Value;
                    break;
                default: break;
            }
        }

        protected override void OnDisappearing()
        {

            settingsDataBase settings = new settingsDataBase();

            #region SleepHours

            settingsItem sleepItem = settings.GetSetting(PCLMechanics.SQLNames.SleepHours.ToString());

            if (sleepItem != null)
            {
                sleepItem.Value = rawData.sleepHours;

                settings.AddSetting(sleepItem);
            }
            else settings.AddSetting(new settingsItem { Name = PCLMechanics.SQLNames.SleepHours.ToString(), Value = rawData.sleepHours});

            #endregion SleepHours

            #region Mania

            settingsItem maniaItem = settings.GetSetting(PCLMechanics.SQLNames.Mania.ToString());
            if (maniaItem != null)
            {
                maniaItem.Value = rawData.maniaVal.ToString();

                settings.AddSetting(maniaItem);
            }
            else settings.AddSetting(new settingsItem { Name = PCLMechanics.SQLNames.Mania.ToString(), Value = rawData.maniaVal.ToString()});

            #endregion Mania

            #region Depression

            settingsItem depressItem = settings.GetSetting(PCLMechanics.SQLNames.Depression.ToString());
            if (depressItem != null)
            {
                depressItem.Value = rawData.depressionVal.ToString();

                settings.AddSetting(depressItem);
            }
            else settings.AddSetting(new settingsItem { Name = PCLMechanics.SQLNames.Depression.ToString(), Value = rawData.depressionVal.ToString() });

            #endregion Depression

            base.OnDisappearing();
        }
    }

    /// <summary>
    /// Required for Custom Renderer to target just this Type
    /// </summary>
    public class MenuTableView : TableView
    {
    }
}
