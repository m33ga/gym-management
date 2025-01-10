using GymManagement.Domain;
using GymManagement.Domain.Enums;
using GymManagement.Domain.Models;
using GymManagement.Domain.Services;
using GymManagement.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.Infrastructure.Repository
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IUnitOfWork _unitOfWork;

        // Inject IUnitOfWork
        public RegistrationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<RegistrationResult> RegisterAsync(string email, string password, string fullname, string username, string phonenumber, Role role, float? weight = null, float? height = null, Membership membership = null, int remainingWorkouts = 0)
        {
            // Check if the email already exists
            if (await _unitOfWork.Members.GetMemberByEmailAsync(email) != null || await _unitOfWork.Trainers.GetTrainerByEmailAsync(email) != null)
            {
                throw new InvalidOperationException("A user with this email already exists.");
            }

            RegistrationResult result = new RegistrationResult();

            switch (role)
            {
                case Role.Member:
                    // Assign default membership if not provided
                    Membership membership1 = membership ?? new Membership("gold", 50, "falafel", 50, 50);

                    // Create a new member
                    var member = new Member(fullname, email, password, username, phonenumber, weight ?? 0.0f, height ?? 0.0f, remainingWorkouts, membership1);

                    // Add the member to the in-memory unit of work
                    result.Member = member;
                    result.IsRegistered = true;
                    break;

                case Role.Trainer:
                    // Create a new trainer
                    var trainer = new Trainer(fullname, password, email, username, phonenumber);

                    // Add the trainer to the in-memory unit of work
                    result.Trainer = trainer;
                    result.IsRegistered = true;
                    break;

                default:
                    throw new InvalidOperationException("Invalid role specified.");
            }

            // Save changes if necessary
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

    }
}
            



