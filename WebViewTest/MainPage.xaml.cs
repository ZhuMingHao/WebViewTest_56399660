using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WebViewTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            MyWebView.LoadCompleted += MyWebView_LoadCompleted;
        }

        private async void MyWebView_LoadCompleted(object sender, NavigationEventArgs e)
        {
            string functionString = @"var anchors = document.querySelectorAll('a');      
        for (var i = 0; i < anchors.length; i += 1) {
                anchors[i].oncontextmenu = function (e) {
                    var e = e||window.event;  
                    var oX = e.clientX;
                    var oY = e.clientY;
                    var href = this.getAttribute('href');
                    window.external.notify([oX.toString(), oY.toString(), href].toString());
                };
            }";
            await MyWebView.InvokeScriptAsync("eval", new string[] { functionString });
        }

        private void MyWebView_ScriptNotify(object sender, NotifyEventArgs e)
        {
            var items = e.Value.Split(',');
            MenuFlyout myFlyout = new MenuFlyout();
            MenuFlyoutItem firstItem = new MenuFlyoutItem { Text = "Open new tab" };
            myFlyout.Items.Add(firstItem);
            myFlyout.ShowAt(sender as UIElement, new Point(System.Convert.ToDouble(items[0]) , System.Convert.ToDouble(items[1])));
        }
    }
}
