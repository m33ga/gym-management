using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GymManagement.Domain.Models;
using GymManagement.Domain.SeedWork;

namespace GymManagement.Domain.Repository
{
    internal interface ITrainerRepository : IRepository<Trainer> 
    {
        Task<Trainer> GetByUsernameAsync(string username);

        // get all trainers with their scheduled classes
        Task<IList<Trainer>> GetTrainersWithClassesAsync();

        // check if a trainer exists by email
        Task<bool> TrainerExistsByEmailAsync(string email);

        Task AddTrainerAsync(Trainer trainer);

        Task RemoveTrainerAsync(Trainer trainer);

        // this can also be handled by Unit of Work
        Task SaveChangesAsync();
    }
}
