using GymManagement.Domain;
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

        private readonly IUnitOfWork _unitOfWork;

        public AuthentificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<AuthentificationResult> Authentificate(string email, string password)
        {
            var result = new AuthentificationResult
            {
                IsAuthentificated = true
            };

            if (await _unitOfWork.Admins.GetAdminByEmailAsync(email) != null)
            {
                Admin admin = await _unitOfWork.Admins.GetAdminByEmailAsync(email);
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
            if (await _unitOfWork.Members.GetMemberByEmailAsync(email) != null)
            {
                Member member = await _unitOfWork.Members.GetMemberByEmailAsync(email);
                password = PasswordUtils.PasswordUtils.HashPassword(password);
                if (member.Password != password)
                {
                    result.IsAuthentificated = false;
                    return result;
                }
                result.IsAuthentificated = true;
                result.UserRole = Domain.Enums.Role.Member;
                return result;
            }
            if (await _unitOfWork.Trainers.GetTrainerByEmailAsync(email) != null)
            {
                Trainer trainer = await _unitOfWork.Trainers.GetTrainerByEmailAsync(email);
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
