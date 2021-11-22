using PaySystemsApi.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

//Esta capa se encargará de ejecutar los Crud pero teniendo acceso directo a la BD (DbContext)
//Su lógica no está relacionada con la lógica de negocios de la aplicación si no solo con la lógica Crud
namespace PaySystemsAPI.Repository
{
    public interface IRepository<T>:ICrud<T>
    {

    }
    public class Repository<T> : IRepository<T> where T : IEntity
    {
        IDbContext<T> _ctx;
        public Repository(IDbContext<T> ctx)
        {
            _ctx = ctx;
        }
        public void Delete(int id)
        {
            _ctx.Delete(id);
        }

        //public void DeleteAsync(int id)
        //{
        //    _ctx.DeleteAsync(id);
        //}

        public IList<T> GetAll()
        {
            return _ctx.GetAll();
        }

        //public async Task<IList<T>> GetAllAsync()
        //{
        //    return await _ctx.GetAllAsync();
        //}

        public T GetById(int id)
        {
            return _ctx.GetById(id);
        }

        //public async Task<T> GetByIdAsync(int id)
        //{
        //    return await _ctx.GetByIdAsync(id);
        //}

        public T Save(T entity)
        {
            return _ctx.Save(entity);
        }

        //public Task<T> SaveAsync(T entity)
        //{
        //    return _ctx.SaveAsync(entity);
        //}
    }
}
