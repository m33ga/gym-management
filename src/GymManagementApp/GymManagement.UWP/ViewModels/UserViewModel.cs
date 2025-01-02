using GymManagement.Domain.Enums;
using GymManagement.Domain.Models;
using GymManagement.Domain.Repository;
using GymManagement.Domain.Services;
using GymManagement.Domain.ViewModel;
using System;
using System.Threading.Tasks;

namespace GymManagement.UWP.ViewModels
{
    public class UserViewModel : BindableBase
    {
        private readonly IAuthentificationService _authentificationService;
        private readonly IAdminRepository _adminRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly ITrainerRepository _trainerRepository;

        private object _loggedUser; // Can hold Admin, Trainer, or Member
        private bool _showError;

        public string Email { get; set; }
        public string Password { get; set; }

        public object LoggedUser
        {
            get => _loggedUser;
            set => Set(ref _loggedUser, value);
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

        public UserViewModel(IAuthentificationService authentificationService, IAdminRepository adminRepository, IMemberRepository memberRepository, ITrainerRepository trainerRepository)
        {
            _authentificationService = authentificationService;
            _adminRepository = adminRepository;
            _memberRepository = memberRepository;
            _trainerRepository = trainerRepository;
        }

        public async Task<bool> DoLoginAsync()
        {
            try
            {
                var result = await _authentificationService.Authentificate(Email, Password);

                if (result.IsAuthentificated)
                {
                    switch (result.UserRole)
                    {
                        case Role.Admin:
                            LoggedUser = await _adminRepository.GetAdminByEmailAsync(Email);
                            break;
                        case Role.Trainer:
                            LoggedUser = await _trainerRepository.GetTrainerByEmailAsync(Email);
                            break;
                        case Role.Member:
                            LoggedUser = await _memberRepository.GetMemberByEmailAsync(Email);
                            break;
                        default:
                            LoggedUser = null;
                            break;
                    }
                }
                else
                {
                    LoggedUser = null;
                }

                ShowError = LoggedUser == null;
                return IsLogged;
            }
            catch (Exception ex)
            {
                ShowError = true;
                Console.WriteLine($"Error during login: {ex.Message}");
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
