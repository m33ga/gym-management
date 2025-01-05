using System;
using System.Threading.Tasks;
using GymManagement.Domain;
using GymManagement.Domain.Models;
using GymManagement.Domain.Repository;

namespace GymManagement.UWP.ViewModels
{
    public class ProfileViewModel : BindableBase
    {
        private string _fullName;
        private string _username;
        private string _email;
        private string _phoneNumber;
        private float _weight;
        private float _height;
        private Membership _membership;
        private int _remainingWorkouts;

        private readonly IUnitOfWork _unitOfWork;
        private readonly UserViewModel _userViewModel;

        public string FullName
        {
            get => _fullName;
            set => Set(ref _fullName, value);
        }

        public string Username
        {
            get => _username;
            set => Set(ref _username, value);
        }

        public string Email
        {
            get => _email;
            set => Set(ref _email, value);
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set => Set(ref _phoneNumber, value);
        }

        public float Weight
        {
            get => _weight;
            set => Set(ref _weight, value);
        }

        public float Height
        {
            get => _height;
            set => Set(ref _height, value);
        }

        public Membership Membership
        {
            get => _membership;
            set => Set(ref _membership, value);
        }

        public int RemainingWorkouts
        {
            get => _remainingWorkouts;
            set => Set(ref _remainingWorkouts, value);
        }

        public bool IsProfileComplete =>
            !string.IsNullOrWhiteSpace(FullName) &&
            !string.IsNullOrWhiteSpace(Username) &&
            !string.IsNullOrWhiteSpace(Email) &&
            PhoneNumber.Length >= 10 &&
            Weight > 0 &&
            Height > 0;

        public ProfileViewModel(IUnitOfWork unitOfWork, UserViewModel userViewModel)
        {
            _unitOfWork = unitOfWork;
            _userViewModel = userViewModel;

            LoadProfileFromDatabase();
        }

        // Fetch user profile from the database
        private async void LoadProfileFromDatabase()
        {
            try
            {
                string userEmail = string.Empty;

                if (_userViewModel.LoggedUser is Member member)
                {
                    userEmail = member.Email;
                    var fetchedMember = await _unitOfWork.Members.GetMemberByEmailAsync(userEmail);
                    if (fetchedMember != null)
                    {
                        PopulateProfile(fetchedMember);
                    }
                }
                else if (_userViewModel.LoggedUser is Trainer trainer)
                {
                    userEmail = trainer.Email;
                    var fetchedTrainer = await _unitOfWork.Trainers.GetTrainerByEmailAsync(userEmail);
                    if (fetchedTrainer != null)
                    {
                        PopulateProfile(fetchedTrainer);
                    }
                }
                else if (_userViewModel.LoggedUser is Admin admin)
                {
                    userEmail = admin.Email;
                    // var fetchedAdmin = await _unitOfWork.Admins.GetAdminByEmailAsync(userEmail);
                    // PopulateProfile(fetchedAdmin);
                }
            }
            catch (Exception ex)
            {
                FullName = "Error Loading Profile";
                Email = "Error";
                // Log the exception if necessary
            }
        }

        // Populate ProfileViewModel with data from the database
        private void PopulateProfile(dynamic user)
        {
            FullName = user.FullName;
            Username = user.Username;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            Weight = user.Weight;
            Height = user.Height;
            Membership = user.Membership;
            RemainingWorkouts = user.RemainingWorkouts;

            OnPropertyChanged(nameof(FullName));
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(IsProfileComplete));
        }
    }
}
