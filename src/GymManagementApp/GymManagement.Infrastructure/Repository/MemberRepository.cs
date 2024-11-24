using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymManagement.Domain.Models;
using GymManagement.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repository
{
    public class MemberRepository : IMemberRepository
    {
        private readonly GymManagementDbContext _dbContext;

        public MemberRepository(GymManagementDbContext dbContext)
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

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Create(Member entity)
        {
            _dbContext.Members.Add(entity);
        }

        public void Update(Member entity)
        {
            _dbContext.Members.Update(entity);
        }

        public void Delete(Member entity)
        {
            _dbContext.Members.Remove(entity);
        }

        public async Task<Member> FindByIdAsync(int id)
        {
            return await _dbContext.Members.FindAsync(id);
        }

        public async Task<Member> FindOrCreateAsync(Member entity)
        {
            var existing = await _dbContext.Members.FirstOrDefaultAsync(m => m.Id == entity.Id);
            if (existing != null)
                return existing;

            await _dbContext.Members.AddAsync(entity);
            return entity;
        }

        public async Task<List<Member>> FindAllAsync()
        {
            return await _dbContext.Members.ToListAsync();
        }
    }
}
