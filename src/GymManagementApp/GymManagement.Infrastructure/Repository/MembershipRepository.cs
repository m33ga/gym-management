using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymManagement.Domain.Models;
using GymManagement.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repository
{
    public class MembershipRepository : GenericRepository<Membership>, IMembershipRepository
    {
        private readonly GymManagementDbContext _dbContext;

        public MembershipRepository(GymManagementDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddMembershipAsync(Membership membership)
        {
            await _dbContext.Memberships.AddAsync(membership);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Membership> GetMembershipByIdAsync(int membershipId)
        {
            return await _dbContext.Memberships.FirstOrDefaultAsync(m => m.Id == membershipId);
        }

        public async Task<Membership> GetMembershipByMemberIdAsync(int memberId)
        {
            return await _dbContext.Members
                .Where(mb => mb.Id == memberId)
                .Select(mb => mb.Membership)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateMembershipAsync(Membership membership)
        {
            _dbContext.Memberships.Update(membership);
            await _dbContext.SaveChangesAsync();
        }

        public override async Task<Membership> FindOrCreateAsync(Membership entity)
        {
            var existing = await _dbContext.Memberships.FirstOrDefaultAsync(m => m.Id == entity.Id);
            if (existing != null)
                return existing;

            await _dbContext.Memberships.AddAsync(entity);
            return entity;
        }
    }
}
