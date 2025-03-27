using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GymManagement.Domain.Models;
using GymManagement.Domain.SeedWork;

namespace GymManagement.Domain.Repository
{
    public interface IMembershipRepository : IRepository<Membership>
    {
        Task AddMembershipAsync(Membership membership);

        // get a membership by ID
        Task<Membership> GetMembershipByIdAsync(int membershipId);

        // get a member's current membership
        Task<Membership> GetMembershipByMemberIdAsync(int memberId);

        Task UpdateMembershipAsync(Membership membership);

        Task SaveChangesAsync();
    }
}
