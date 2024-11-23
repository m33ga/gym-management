using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GymManagement.Domain.Models;
using GymManagement.Domain.SeedWork;

namespace GymManagement.Domain.Repository
{
    public interface IAdminRepository : IRepository<Admin>
    {
        // admin doesn't have email
        Task<IList<Admin>> GetAllAdminsAsync();

        Task AddAdminAsync(Admin admin);

        // Remove an admin
        Task RemoveAdminAsync(Admin admin);

        // Save changes (optional if using Unit of Work)
        Task SaveChangesAsync();
    }
}
