using GymManagement.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.Domain.Services
{
    public interface IAuthentificationService
    {
        Task<AuthentificationResult> Authentificate (string email, string password);
    }
}
