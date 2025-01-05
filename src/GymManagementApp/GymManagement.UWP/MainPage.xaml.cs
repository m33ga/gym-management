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
using GymManagement.UWP.Views.Users;
using GymManagement.UWP.ViewModels;
using GymManagement.Infrastructure.Repository;
using GymManagement.Domain.Services;
using GymManagement.UWP.Views.Booking;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GymManagement.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {


        public UserViewModel UserViewModel { get; set; }
        public MainPage()
        {
            this.InitializeComponent();
       
            UserViewModel = App.UserViewModel;

            if (UserViewModel.IsLogged == true)
            {
                frmMain.Navigate(typeof(ProfilePage));
            }
            else
            {
                frmMain.Navigate(typeof(LoginDialog));
            }
        }
        private void NvMain_OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItemContainer is NavigationViewItem selectedItem)
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
                        
                        if(UserViewModel.IsMember)
                        {
                            frmMain.Navigate(typeof(BookingManagementPage));
                        }
                        if (UserViewModel.IsTrainer)
                        {
                            frmMain.Navigate(typeof(ScheduleManagementPage));
                        }
                        if (UserViewModel.IsAdmin)
                        {
                            frmMain.Navigate(typeof(ScheduleManagementPage));
                        }
                        break;
                    case "meal_plan":

                        break;
                    

                }
            }
        }
        private void NvLogout_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            UserViewModel.DoLogout();
            frmMain.Navigate(typeof(LoginDialog));
        }



    }
}
