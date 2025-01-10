using GymManagement.Domain.Services;
using GymManagement.Infrastructure;
using GymManagement.Infrastructure.Repository;
using GymManagement.UWP.ViewModels;
using System;
using Windows.UI.Xaml.Controls;

namespace GymManagement.UWP.Views.Notifications

{
    public sealed partial class NotificationsPage : Page
    {
        public NotificationsViewModel ViewModel { get; }

        public NotificationsPage()
        {
            this.InitializeComponent();
            UnitOfWork unitOfWork = new UnitOfWork(); 
            ViewModel = new NotificationsViewModel(unitOfWork);
            this.DataContext = ViewModel;
        }
    }
}
