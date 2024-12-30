using GymManagement.Domain.Repository;
using GymManagement.Domain.Services;
using GymManagement.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.Infrastructure.Repository
{
    public class AuthentificationService : IAuthentificationService
    {
        private readonly IAdminRepository adminRepository;
        private readonly IMemberRepository memberRepository;
        private readonly ITrainerRepository trainerRepository;

        public AuthentificationService( IAdminRepository adminRepository, IMemberRepository memberRepository, ITrainerRepository trainerRepository)
        {
            this.adminRepository = adminRepository;
            this.memberRepository = memberRepository;
            this.trainerRepository = trainerRepository;
        }
        public async Task<AuthentificationResult> Authentificate(string email, string password)
        {
            var result = new AuthentificationResult();
            result.IsAuthentificated = true;
            if (await adminRepository.GetAdminByEmailAsync(email) != null)
            {
                result.UserRole = Domain.Enums.Role.Admin;
                return result;
            }
            if (await memberRepository.GetMemberByEmailAsync(email) != null)
            {
                result.UserRole = Domain.Enums.Role.Member;
                return result;
            }
            if (await trainerRepository.GetTrainerByEmailAsync(email) != null)
            {
                result.UserRole = Domain.Enums.Role.Member;
                return result;
            }
            result.IsAuthentificated = false;
            return result;
        }
    }
}
