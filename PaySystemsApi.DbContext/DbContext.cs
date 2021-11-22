//using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PaySystemsApi.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySystemsApi.DataAccess
{
    //Esta clase ejecutará los crud que interactúan directamente con la base de datos
    //Inyectando como dependencia la conexión con la misma (ApiDbContext)
    public class DbContext<T> : IDbContext<T> where T : class, IEntity
    {
        DbSet<T> _items;
        protected ApiDbContext _ctx;

        //protected ApiDbContext _ctx;
        public DbContext(ApiDbContext ctx)
        {
            _ctx = ctx;
            _items = ctx.Set<T>();
        }
        public void Delete(int id)
        {
            
        }

        public void DeleteAsync(int id)
        {
            //var tmp = _data.Find(id);
            //if (tmp != null)
            //{
            //    _data.Remove(tmp);
            //    _ctx.SaveChangesAsync();
            //}
        }

        public IList<T> GetAll()
        {
            return _items.ToList();
        }

        //public async Task<IList<T>> GetAllAsync()
        //{
        //    return  await _data.ToList();
        //}

        public T GetById(int id)
        {
            return _items.Find(id);
        }

        //public async Task<T> GetByIdAsync(int id)
        //{
        //    //return await _data.FindAsync(id);
        //}

        public T Save(T entity)
        {
            if (entity.Id.Equals(0))
            {
                _items.Add(entity);
            }
            else
            {
                _items.Update(entity);
            }

            _ctx.SaveChanges();
            return entity;
        }

        //public async Task<T> SaveAsync(T entity)
        //{
        //    if (entity.Id.Equals(0))
        //    {
        //        await _data.AddAsync(entity);
        //    }
        //    else
        //    {
        //        _data.Update(entity);
        //    }

        //    await _ctx.SaveChangesAsync();
        //    return entity;
        //}
    }
}
