using GymManagement.Domain.Enums;
using GymManagement.UWP.ViewModels;
using GymManagement.UWP.Views.Profile;
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
                // Retrieve the selected role from the ComboBox
                if (!(RoleComboBox.SelectedItem is ComboBoxItem selectedRole))
                {
                    // Show an error if no role is selected
                    await new ContentDialog
                    {
                        Title = "Error",
                        Content = "Please select a role (Member or Trainer).",
                        CloseButtonText = "OK"
                    }.ShowAsync();
                    return;
                }

                // Map the selected role to the Role enum
                var role = RoleComboBox.SelectedItem.ToString() == "Member"? Role.Member : Role.Trainer;

                // Call the DoRegisterAsync method in the ViewModel
                bool registrationSuccessful = await UserViewModel.DoRegisterAsync(role);

                if (registrationSuccessful)
                {
                    // Navigate to the next page (e.g., ProfilePage) upon successful registration
                    Frame.Navigate(typeof(ProfilePage));
                }
                else
                {
                    // Show an error dialog if registration fails
                    await new ContentDialog
                    {
                        Title = "Registration Failed",
                        Content = "An error occurred during registration. Please try again.",
                        CloseButtonText = "OK"
                    }.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                await new ContentDialog
                {
                    Title = "Error",
                    Content = $"An unexpected error occurred: {ex.Message}",
                    CloseButtonText = "OK"
                }.ShowAsync();
            }
        }
    }
}
