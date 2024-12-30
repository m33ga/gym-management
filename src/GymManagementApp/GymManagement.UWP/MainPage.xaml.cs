using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using GymManagement.UWP.Views.Profile;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GymManagement.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            frmMain.Navigate(typeof(ProfilePage));
        }
        private void NvMain_OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var selectedItem = args.InvokedItemContainer as NavigationViewItem;
            if (selectedItem != null)
            {
                switch (selectedItem.Tag)
                {
                    case "profile":
                        frmMain.Navigate(typeof(ProfilePage));
                        break;
                    case "notifications":

                        break;
                    case "dashboard":
                        
                        break;
                    case "schedule":

                        break;
                    case "meal_plan":

                        break;

                }
            }
        }
        private void NvLogout_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }



    }
}
