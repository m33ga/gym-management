using GymManagement.Domain.Models;
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

            if (adminRepository == null) throw new ArgumentNullException(nameof(adminRepository));
            if (memberRepository == null) throw new ArgumentNullException(nameof(memberRepository));
            if (trainerRepository == null) throw new ArgumentNullException(nameof(trainerRepository));

            this.adminRepository = adminRepository;
            this.memberRepository = memberRepository;
            this.trainerRepository = trainerRepository;
        }
        public async Task<AuthentificationResult> Authentificate(string email, string password)
        {
            using (var uow = new UnitOfWork())
            {
                var result = new AuthentificationResult();
                result.IsAuthentificated = true;
                if (await uow.Admins.GetAdminByEmailAsync(email) != null)
                {
                    Admin admin = await uow.Admins.GetAdminByEmailAsync(email);
                    password = PasswordUtils.PasswordUtils.HashPassword(password);
                    if (admin.Password != password) 
                    {
                        result.IsAuthentificated = false;
                        return result;
                    }
                    result.IsAuthentificated = true;
                    result.UserRole = Domain.Enums.Role.Admin;
                    return result;
                }
                else if (await uow.Members.GetMemberByEmailAsync(email) != null)
                {
                    Member member = await uow.Members.GetMemberByEmailAsync(email);
                    password = PasswordUtils.PasswordUtils.HashPassword(password);
                    if (member.Password != password) 
                    {
                        result.IsAuthentificated= false;
                        return result;
                    }
                    result.IsAuthentificated = true;
                    result.UserRole = Domain.Enums.Role.Member;
                    return result;
                }
                else if (await uow.Trainers.GetTrainerByEmailAsync(email) != null)
                {
                    Trainer trainer = await uow.Trainers.GetTrainerByEmailAsync(email);
                    password = PasswordUtils.PasswordUtils.HashPassword(password);
                    if (trainer.Password != password)
                    {
                        result.IsAuthentificated = false;
                        return result;
                    }
                    result.IsAuthentificated = true;
                    result.UserRole = Domain.Enums.Role.Trainer;
                    return result;
                }
                result.IsAuthentificated = false;
                return result;
            }
        }
    }
}
