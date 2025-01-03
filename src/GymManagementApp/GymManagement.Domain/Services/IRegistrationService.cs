using GymManagement.Domain.Enums;
using GymManagement.Domain.Models;
using GymManagement.Domain.ViewModel;
using System.Threading.Tasks;

namespace GymManagement.Domain.Services
{
    public interface IRegistrationService
    {
        Task<RegistrationResult> RegisterAsync(
                                    string email,
                                    string password,
                                    string fullname,
                                    string username,
                                    string phonenumber,
                                    Role role,
                                    float? weight = null,
                                    float? height = null,
                                    Membership membership = null,
                                    int remainingWorkouts = 0);


    }
}
