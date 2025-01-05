using GymManagement.UWP.ViewModels;
using GymManagement.Domain.Models;

namespace GymManagement.UWP.ViewModels
{
    public class ProfileViewModel : BindableBase
    {
        private readonly UserViewModel _userViewModel;

        // Public parameterless constructor
        public ProfileViewModel()
        {
            _userViewModel = App.UserViewModel;
        }

        public ProfileViewModel(UserViewModel userViewModel)
        {
            _userViewModel = userViewModel;
        }

        public string FullName => _userViewModel.FullName;
        public string Username => _userViewModel.Username;
        public float Weight => _userViewModel.Weight;
        public float Height => _userViewModel.Height;
        public Membership Membership => _userViewModel.MemberShip;
        public bool IsMember => _userViewModel.IsMember;
        public bool IsTrainer => _userViewModel.IsTrainer;
        public bool IsAdmin => _userViewModel.IsAdmin;

        // Profile completion validation logic
        public bool IsProfileComplete =>
            !string.IsNullOrWhiteSpace(FullName) &&
            !string.IsNullOrWhiteSpace(Username) &&
            Weight > 0 &&
            Height > 0;
    }
}
