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
        
        public AuthentificationService()
        {         
        }
        public async Task<AuthentificationResult> Authentificate(string email, string password)
        {
            using (var uow = new UnitOfWork())
            {
                var result = new AuthentificationResult
                {
                    IsAuthentificated = true
                };
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
                if (await uow.Members.GetMemberByEmailAsync(email) != null)
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
                if (await uow.Trainers.GetTrainerByEmailAsync(email) != null)
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
