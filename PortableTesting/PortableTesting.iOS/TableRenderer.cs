using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(PortableTesting.MenuTableView), typeof(PortableTesting.iOS.CustomTableViewRenderer))]
namespace PortableTesting.iOS
{
    public class CustomTableViewRenderer : TableViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.TableView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
                return;

            Control.SeparatorColor = UIKit.UIColor.Clear;
            Control.SeparatorStyle = UITableViewCellSeparatorStyle.None;
        }

    }
}
