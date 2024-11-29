using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GymManagement.Domain.Models;
using GymManagement.Domain.SeedWork;

namespace GymManagement.Domain.Repository
{
    public interface IMemberRepository : IRepository<Member>
    {
        // get a member by email
        Task<Member> GetMemberByEmailAsync(string email);

        Task<IList<Member>> GetAllMembersAsync();

        Task<Member> GetMemberWithBookingsAsync(int memberId);

        // check if a member exists by email
        Task<bool> MemberExistsByEmailAsync(string email);

        Task AddMemberAsync(Member member);

        Task RemoveMemberAsync(Member member);

        // this can also be handled by uow
        Task SaveChangesAsync();
    }
}
