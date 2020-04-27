using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;
using Torch;
using Torch.API.Plugins;

namespace SensorBlockFix
{
    public class SensorBlockFixPlugin : TorchPluginBase, IWpfPlugin
    {
        private readonly Lazy<UserControl> _userControl = new Lazy<UserControl>(InitUserControl);

        private static UserControl InitUserControl()
        {
            var hyperLink = new Hyperlink
            {
                Inlines = {"Here"},
                NavigateUri = new Uri("https://torchapi.net/plugins/item/d4948c7d-5363-469c-a24a-48d64ed02073")
            };
            hyperLink.RequestNavigate += (sender, e) =>
            {
                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
                e.Handled = true;
            };

            return new UserControl
            {
                Content = new TextBlock
                {
                    Inlines =
                    {
                        "This plugin is deprecated, please use another plugin ",
                        hyperLink
                    }
                }
            };
        }

        public UserControl GetControl() => _userControl.Value;
    }
}