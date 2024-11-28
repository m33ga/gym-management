using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymManagement.Domain.Models;
using GymManagement.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repository
{
    public class MemberRepository : GenericRepository<Member>, IMemberRepository
    {
        private readonly GymManagementDbContext _dbContext;

        public MemberRepository(GymManagementDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddMemberAsync(Member member)
        {
            await _dbContext.Members.AddAsync(member);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Member> GetByEmailAsync(string email)
        {
            return await _dbContext.Members.FirstOrDefaultAsync(m => m.Email == email);
        }

        public async Task<IList<Member>> GetAllMembersAsync()
        {
            return await _dbContext.Members.ToListAsync();
        }

        public async Task<Member> GetMemberWithBookingsAsync(int memberId)
        {
            return await _dbContext.Members
                .Include(m => m.Bookings)
                .FirstOrDefaultAsync(m => m.Id == memberId);
        }

        public async Task<bool> MemberExistsByEmailAsync(string email)
        {
            return await _dbContext.Members.AnyAsync(m => m.Email == email);
        }

        public async Task RemoveMemberAsync(Member member)
        {
            _dbContext.Members.Remove(member);
            await _dbContext.SaveChangesAsync();
        }


        public override async Task<Member> FindOrCreateAsync(Member entity)
        {
            var existing = await _dbContext.Members.FirstOrDefaultAsync(m => m.Id == entity.Id);
            if (existing != null)
                return existing;

            await _dbContext.Members.AddAsync(entity);
            return entity;
        }

    }
}
