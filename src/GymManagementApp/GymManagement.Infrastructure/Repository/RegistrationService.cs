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
        public RegistrationService() { }

        public async Task<RegistrationResult> RegisterAsync(string email, string password, string fullname, string username, string phonenumber, Role role, float? weight = null, float? height = null, Membership membership = null, int remainingWorkouts = 0)
        {
            using (var uow = new UnitOfWork())
            {
                // Check if the email already exists
                if (await uow.Members.GetMemberByEmailAsync(email) != null || await uow.Trainers.GetTrainerByEmailAsync(email) != null)
                {
                    throw new InvalidOperationException("A user with this email already exists.");
                }

                RegistrationResult result = new RegistrationResult();

                switch (role)
                {
                    case Role.Member:
                        // Assign default membership if not provided
                        Membership membership1 = new Membership("gold", 50,"falafel",50,50);

                        // Create a new member
                        var member = new Member(fullname, email, password, username, phonenumber, weight ?? 0.0f, height ?? 0.0f, 1, membership1);
                        

                        result.Member = member;
                        result.IsRegistered = true;
                        break;

                    case Role.Trainer:
                        // Create a new trainer
                        var trainer = new Trainer(fullname, password, email, username, phonenumber);
                        result.Trainer = trainer;
                        result.IsRegistered = true;
                        break;

                    default:
                        throw new InvalidOperationException("Invalid role specified.");
                }

                return result;
            }
        }

       
    }
}
            



