using GymManagement.Infrastructure;
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
    public sealed partial class ScheduleManagementPage : Page
    {
        public ScheduleManagementPage()
        {
            this.InitializeComponent();
            var uow = new UnitOfWork();
            var viewModel = new ScheduleViewModel(uow);
            DataContext = viewModel;
        }
        private void CalendarView_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            try
            {
                if (args.AddedDates.Count > 0)
                {
                    var selectedDate = args.AddedDates[0];

                    if (DataContext is ScheduleViewModel viewModel)
                    {
                        viewModel.ScheduledDate = selectedDate;
                    }
                }
                else
                {
                    if (DataContext is ScheduleViewModel viewModel)
                    {
                        viewModel.ScheduledDate = null;
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
