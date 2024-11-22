using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.Domain
{
    /// <summary>
    /// deal with database operations -> IDisposable
    /// bridge between repositories and rest of code
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        // TODO: add repos here
        Task SaveChangesAsync();
    }
}
