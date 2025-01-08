using GymManagement.Domain.Enums;
using GymManagement.UWP.ViewModels;
using GymManagement.UWP.Views.Profile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    
    public sealed partial class RegisterDialog : Page
    {
        public UserViewModel UserViewModel { get; set; }
        public RegisterDialog()
        {
            this.InitializeComponent();
            UserViewModel = App.UserViewModel;
        }
        private async void OnConfirmButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check if a role is selected
                var selectedRole = RoleComboBox.SelectedItem as ComboBoxItem;
                if (selectedRole == null)
                {
                    await ShowErrorDialog("Please select a role (Member or Trainer).");
                    return;
                }

                // Map role from ComboBox
                Role role = selectedRole.Content.ToString() == "Member" ? Role.Member : Role.Trainer;

                // Attempt registration
                bool registrationSuccessful = await UserViewModel.DoRegisterAsync(role);

                if (registrationSuccessful)
                {
                    // Navigate to profile page upon success
                    Frame.Navigate(typeof(ProfilePage));
                }
                else
                {
                    await ShowErrorDialog("An error occurred during registration. Please try again.");
                }
            }
            catch (Exception ex)
            {
                await ShowErrorDialog($"An unexpected error occurred: {ex.Message}");
            }
        }

        private async Task ShowErrorDialog(string message)
        {
            await new ContentDialog
            {
                Title = "Error",
                Content = message,
                CloseButtonText = "OK"
            }.ShowAsync();
        }
    }
}
