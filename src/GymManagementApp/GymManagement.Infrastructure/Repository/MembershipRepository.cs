using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymManagement.Domain.Models;
using GymManagement.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repository
{
    public class MembershipRepository : IMembershipRepository
    {
        private readonly GymManagementDbContext _dbContext;

        public MembershipRepository(GymManagementDbContext dbContext)
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

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Create(Membership entity)
        {
            _dbContext.Memberships.Add(entity);
        }

        public void Update(Membership entity)
        {
            _dbContext.Memberships.Update(entity);
        }

        public void Delete(Membership entity)
        {
            _dbContext.Memberships.Remove(entity);
        }

        public async Task<Membership> FindByIdAsync(int id)
        {
            return await _dbContext.Memberships.FindAsync(id);
        }

        public async Task<Membership> FindOrCreateAsync(Membership entity)
        {
            var existing = await _dbContext.Memberships.FirstOrDefaultAsync(m => m.Id == entity.Id);
            if (existing != null)
                return existing;

            await _dbContext.Memberships.AddAsync(entity);
            return entity;
        }

        public async Task<List<Membership>> FindAllAsync()
        {
            return await _dbContext.Memberships.ToListAsync();
        }
    }
}
