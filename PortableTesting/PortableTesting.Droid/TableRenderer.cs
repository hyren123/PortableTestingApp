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
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(PortableTesting.MenuTableView), typeof(PortableTesting.Droid.CustomTableViewRenderer))]
namespace PortableTesting.Droid
{
    public class CustomTableViewRenderer : TableViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<TableView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
                return;

            var listView = Control as Android.Widget.ListView;
            listView.DividerHeight = 3;
            listView.Divider = new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.Green);
        }
    }
}