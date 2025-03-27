using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GymManagement.Domain.Models;
using GymManagement.Domain.SeedWork;

namespace GymManagement.Domain.Repository
{
    public interface IClassRepository : IRepository<Class>
    {
        // retrieve all available classes
        Task<IList<Class>> GetAvailableClassesAsync();


        // retrieve all classes by trainer ID
        Task<IList<Class>> GetClassesByTrainerAsync(int trainerId);

        // retrieve all booked classes by trainer ID
        Task<IList<Class>> GetUpcomingBookedClassesByTrainerAsync(int trainerId);

        Task<IList<Class>> GetUpcomingClassesByTrainerAsync(int trainerId);
        Task<IList<Class>> GetPastBookedClassesByTrainerAsync(int trainerId);

        Task<IList<Class>> GetPastBookedClassesByMemberAsync(int memberId);
        Task<IList<Class>> GetUpcomingBookedClassesByMemberAsync(int memberId);



        // check for overlapping schedules when adding a class
        Task<bool> HasScheduleConflictAsync(int trainerId, DateTime startTime, DateTime endTime);

        Task AddClassAsync(Class trainingClass);

        // get a class by its ID, including navigation properties
        Task<Class> GetByIdWithDetailsAsync(int id);

        // same as before (uow)
        Task SaveChangesAsync();
        Task<IEnumerable<object>> GetClassesByDateAsync(DateTime dateTime);
        Task<Class> GetClassByDateTimeAsync(DateTime dateTime);
    }
}
