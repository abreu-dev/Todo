using System.Threading.Tasks;

namespace Todo.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}