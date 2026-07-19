using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMnagementBackend.Aplication.Abstraction.IRepositories
{
    public interface IWriteRepository<T> : IRepository<T> where T : class
    {
        public Task<bool> AddAsync(T t);
        public bool Update(T t);
        public Task<bool> AddRange(List<T> datas);
        public bool Delete(T model);
        public bool DeleteRange(List<T> datas);
        public Task<bool> DeleteAsync(int id);
        Task<int> SaveAsync();
    }
}
