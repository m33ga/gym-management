using System.Collections.Generic;
using System.Threading.Tasks;
using GymManagement.Domain.Models;
using GymManagement.Domain.SeedWork;

namespace GymManagement.Domain.Repository
{
    public interface IAdminRepository : IRepository<Admin>
    {
        // Get all admins
        Task<IList<Admin>> GetAllAdminsAsync();

        // Add a new admin
        Task AddAdminAsync(Admin admin);

        // Remove an admin
        Task RemoveAdminAsync(Admin admin);

        // Find an admin by ID (with notifications)
        //Task<Admin> FindByIdAsync(int id);

        // Get admin by username
        Task<Admin> GetAdminByUsernameAsync(string username);

        // Get Admin by Email
        Task<Admin> GetAdminByEmailAsync(string email);
    }
}
