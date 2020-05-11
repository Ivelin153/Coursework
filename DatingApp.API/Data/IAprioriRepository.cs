using System.Collections.Generic;
using System.Threading.Tasks;
using AprioriApp.API.Model;

namespace AprioriApp.API.Data
{
    public interface IAprioriRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<User> GetUser(int id);
        Task<IEnumerable<User>> GetUsers();
        Task<bool> SaveAll();
        Task<File> GetFile(int id);
    }
}