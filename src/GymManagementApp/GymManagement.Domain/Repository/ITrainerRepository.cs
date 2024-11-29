using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GymManagement.Domain.Models;
using GymManagement.Domain.SeedWork;

namespace GymManagement.Domain.Repository
{
    public interface ITrainerRepository : IRepository<Trainer> 
    {
        Task<Trainer> GetByUsernameAsync(string username);

        // get all trainers with their scheduled classes
        Task<IList<Trainer>> GetTrainersWithClassesAsync();

        // Get trainer by email
        Task<Trainer> GetTrainerByEmailAsync(string email);

        Task AddTrainerAsync(Trainer trainer);

        Task RemoveTrainerAsync(Trainer trainer);

        // this can also be handled by Unit of Work
        Task SaveChangesAsync();
    }
}
