using GymManagement.Domain.Enums;
using GymManagement.Domain.Models;
using GymManagement.Domain.Services;
using GymManagement.Infrastructure;
using GymManagement.Infrastructure.PasswordUtils;
using GymManagement.Infrastructure.Repository;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

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
                using (var uow = new UnitOfWork(new GymManagementDbContext()))
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
                var registrationService = new RegistrationService(new UnitOfWork(new GymManagementDbContext()));
                var pass = PasswordUtils.HashPassword(Password);
                var registrationResult = await registrationService.RegisterAsync(
                    email: Email,
                    password: pass,
                    fullname: FullName,
                    username: Username,
                    phonenumber: PhoneNumber,
                    role: role,
                    weight: Weight,
                    height: Height
                );

                if (!registrationResult.IsRegistered)
                {
                    ShowError = true;
                    Console.WriteLine("Registration failed.");
                    return false;
                }

                using (var uow = new UnitOfWork(new GymManagementDbContext()))
                {
                    if (role == Role.Member)
                    {
                        LoggedUser = registrationResult.Member;
                        uow.Members.Create(registrationResult.Member);
                    }
                    else if (role == Role.Trainer)
                    {
                        LoggedUser = registrationResult.Trainer;
                        uow.Trainers.Create(registrationResult.Trainer);
                    }
                    else
                    {
                        LoggedUser = null;
                    }

                    await uow.SaveChangesAsync();
                }

                ShowError = LoggedUser == null;
                return IsLogged;
            }
            catch (InvalidOperationException ex)
            {
                ShowError = true;
                Console.WriteLine($"Error during registration: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                ShowError = true;
                Console.WriteLine($"Unexpected error during registration: {ex.Message}");
                return false;
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
