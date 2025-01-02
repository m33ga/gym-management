using GymManagement.Domain.Enums;
using GymManagement.Domain.Models;
using GymManagement.Domain.Repository;
using GymManagement.Domain.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.UWP.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
        private readonly IAuthentificationService _authentificationService;

        private object _loggedUser; // Can hold Admin, Trainer, or Member
        private readonly IAdminRepository adminRepository;
        private readonly IMemberRepository memberRepository;
        private readonly ITrainerRepository trainerRepository;
        public object LoggedUser
        {
            get => _loggedUser;
            set
            {
                _loggedUser = value;
                OnPropertyChanged(nameof(LoggedUser));
                OnPropertyChanged(nameof(IsLogged));
                OnPropertyChanged(nameof(IsAdmin));
                OnPropertyChanged(nameof(IsTrainer));
                OnPropertyChanged(nameof(IsMember));
            }
        }

        public bool IsLogged => LoggedUser != null;

        public bool IsAdmin => LoggedUser is Admin;

        public bool IsTrainer => LoggedUser is Trainer;

        public bool IsMember => LoggedUser is Member;

        private bool _showError;

        public bool ShowError
        {
            get => _showError;
            set
            {
                _showError = value;
                OnPropertyChanged(nameof(ShowError));
            }
        }

        public string Email { get; set; }
        public string Password { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public UserViewModel(IAuthentificationService authentificationService)
        {
            _authentificationService = authentificationService;
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
                            LoggedUser = await adminRepository.GetAdminByEmailAsync(Email);
                            break;
                        case Role.Trainer:
                            LoggedUser = await trainerRepository.GetTrainerByEmailAsync(Email);
                            break;
                        case Role.Member:
                            LoggedUser = await memberRepository.GetMemberByEmailAsync(Email);
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

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
