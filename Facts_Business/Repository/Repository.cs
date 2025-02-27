﻿using Facts_Business.Repository.IRepository;
using Facts_DataAccess;
using Facts_Domain.FactsDB;
using Microsoft.EntityFrameworkCore;

namespace Facts_Business.Repository
{
    public class Repository<T>
    : IRepository<T>
    where T : BaseEntity
    {
        protected readonly ApplicationDbContext _db;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var entities = await _db.Set<T>().ToListAsync();

            return entities;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _db.Set<T>().FirstOrDefaultAsync(x => x.Id == id);

            return entity;
        }

        public async Task<IEnumerable<T>> GetRangeByIdsAsync(List<int> ids)
        {
            var entities = await _db.Set<T>().Where(x => ids.Contains(x.Id)).ToListAsync();
            return entities;
        }

        public async Task<T> AddAsync(T entity, bool? saveChanges = true)
        {
            int id_var = 1;
            id_var = _db.Set<T>().AsNoTracking().Max(u => u.Id) + 1;
            entity.Id = id_var;
            var addedEntity = await _db.Set<T>().AddAsync(entity);
            if (saveChanges == true)
                await _db.SaveChangesAsync();
            return addedEntity.Entity;
        }

        public async Task<T> UpdateAsync(T entity, bool? saveChanges = true)
        {
            if (saveChanges == true)
                await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<int> DeleteAsync(T entity, bool? saveChanges = true)
        {
            _db.Set<T>().Remove(entity);
            if (saveChanges == true)
                return await _db.SaveChangesAsync();
            else
                return 0;
        }
    }
}
