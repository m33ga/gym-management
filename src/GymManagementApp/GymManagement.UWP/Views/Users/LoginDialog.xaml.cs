using GymManagement.UWP.ViewModels;
using GymManagement.UWP.Views.Profile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GymManagement.UWP.Views.Users
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginDialog : Page
    {
        public UserViewModel UserViewModel { get; set; }
        public LoginDialog()
        {
            this.InitializeComponent();
            UserViewModel = App.UserViewModel;
        }
        private async void LoginButtonTapped(object sender, TappedRoutedEventArgs e)
        {
            var loginButton = sender as Button;
            loginButton.IsEnabled = false;

            try
            {
                bool loginSuccessful = await UserViewModel.DoLoginAsync();
                if (loginSuccessful)
                {
                    Frame.Navigate(typeof(ProfilePage));
                }
                else
                {
                    ContentDialog loginFailedDialog = new ContentDialog
                    {
                        Title = "Login Failed",
                        Content = "Invalid email or password. Please try again.",
                        CloseButtonText = "OK"
                    };
                    await loginFailedDialog.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = $"An error occurred during login: {ex.Message}",
                    CloseButtonText = "OK"
                };
                await errorDialog.ShowAsync();
            }
            finally
            {
                loginButton.IsEnabled = true;
            }
        }
        private void OnRegisterClick(object sender, RoutedEventArgs e) 
        {
            Frame.Navigate(typeof(RegisterDialog));
        }
    }
}
    

