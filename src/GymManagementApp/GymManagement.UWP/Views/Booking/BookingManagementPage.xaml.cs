using GymManagement.Domain.Services;
using GymManagement.Infrastructure;
using GymManagement.Infrastructure.Repository;
using GymManagement.UWP.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace GymManagement.UWP.Views.Booking
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BookingManagementPage : Page
    {
        public BookingViewModel ViewModel { get; }

        public BookingManagementPage()
        {
            this.InitializeComponent();
            UnitOfWork unitOfWork = new UnitOfWork();
            AuthentificationService auth = new AuthentificationService();
            ViewModel = new BookingViewModel(unitOfWork,auth);
            DataContext = ViewModel;
            
        }


        private void CalendarView_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            try
            {
                if (args.AddedDates.Count > 0)
                {
                    var selectedDate = args.AddedDates[0];

                    if (DataContext is BookingViewModel viewModel)
                    {
                        viewModel.SelectedDate = selectedDate;
                    }
                }
                else
                {
                    if (DataContext is BookingViewModel viewModel)
                    {
                        viewModel.SelectedDate = null;
                    }
                }
            }
            catch (System.Runtime.InteropServices.COMException comEx)
            {
                Debug.WriteLine($"COMException: {comEx.Message}");
                Debug.WriteLine($"Stack Trace: {comEx.StackTrace}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
        }
    }
}
