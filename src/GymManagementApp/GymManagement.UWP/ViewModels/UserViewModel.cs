using GymManagement.Domain.Enums;
using GymManagement.Domain.Models;
using GymManagement.Domain.Services;
using GymManagement.Infrastructure;
using System;
using System.Threading.Tasks;
using Windows.System;

namespace GymManagement.UWP.ViewModels
{
    public class UserViewModel : BindableBase
    {
        private readonly IAuthentificationService _authentificationService;


        private object _loggedUser; // Can hold Admin, Trainer, or Member
        private bool _showError;
        public Role Role1;
        public string Email { get; set; }
        public string Password { get; set; }

        public object LoggedUser
        {
            get => _loggedUser;
            set
            {
                _loggedUser = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsLogged));
                OnPropertyChanged(nameof(IsMember));
                OnPropertyChanged(nameof(IsAdmin));
                OnPropertyChanged(nameof(IsTrainer));
            }

        }

        public bool IsLogged => LoggedUser != null;
        public bool IsAdmin => LoggedUser is Admin;
        public bool IsTrainer => LoggedUser is Trainer;
        public bool IsMember => LoggedUser is Member;

        public bool ShowError
        {
            get => _showError;
            set => Set(ref _showError, value);
        }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public Membership MemberShip { get; set; }
        public int RemainingWorkouts { get; set; }

        public UserViewModel(IAuthentificationService authentificationService)
        {
            _authentificationService = authentificationService;
        }

        public async Task<bool> DoLoginAsync()
        {
            try
            {
                var result = await _authentificationService.Authentificate(Email, Password);
                using (var uow = new UnitOfWork())
                {
                    if (result.IsAuthentificated)
                    {
                        switch (result.UserRole)
                        {
                            case Role.Admin:
                                LoggedUser = await uow.Admins.GetAdminByEmailAsync(Email);
                                break;
                            case Role.Trainer:
                                LoggedUser = await uow.Trainers.GetTrainerByEmailAsync(Email);
                                break;
                            case Role.Member:
                                LoggedUser = await uow.Members.GetMemberByEmailAsync(Email);
                                break;
                            default:
                                LoggedUser = null;
                                break;
                        }
                    }

                    ShowError = LoggedUser == null;
                    return IsLogged;
                }
            }
            catch (Exception ex)
            {
                ShowError = true;
                Console.WriteLine($"Error during login: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> DoRegisterAsync(Role role)
        {
            try
            {
                using (var uow = new UnitOfWork())
                {
                    // Check if the user already exists
                    if (await uow.Members.GetMemberByEmailAsync(Email) != null)
                    {
                        ShowError = true;
                        return false;
                    }
                    if (await uow.Trainers.GetTrainerByEmailAsync(Email) != null)
                    {
                        ShowError = true;
                        return false;
                    }

                    // Create the appropriate user based on the UserType
                    switch (role)
                    {
                        case Role.Member:
                            var membership = await uow.Memberships.GetMembershipByIdAsync(1);
                            var member = new Member(FullName, Email, Password, Username, PhoneNumber, Weight, Height, 1, membership);             
                            await uow.Members.AddMemberAsync(member);
                            await uow.SaveChangesAsync();
                            LoggedUser = member;
                            break;

                        case Role.Trainer:
                            var trainer = new Trainer(FullName, Password, Email, Username, PhoneNumber);
                            await uow.Trainers.AddTrainerAsync(trainer);
                            await uow.SaveChangesAsync();
                            LoggedUser = trainer;
                            break;

                        default:
                            ShowError = true;
                            return false; // Invalid UserType
                    }

                    await uow.SaveChangesAsync();
                    ShowError = LoggedUser == null;
                    return IsLogged; // Registration succeeded
                }
            }
            catch (Exception ex)
            {
                ShowError = true;
                Console.WriteLine($"Error during registration: {ex.Message}");
                return false; // Registration failed due to an exception
            }
        }

        public void DoLogout()
        {
            LoggedUser = null;
            Email = string.Empty;
            Password = string.Empty;
        }
    }
}
