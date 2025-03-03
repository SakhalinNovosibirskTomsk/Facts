﻿using Facts_Business.Repository.IRepository;
using Facts_DataAccess;
using Facts_Domain.FactsDB;
using Microsoft.EntityFrameworkCore;

namespace Facts_Business.Repository
{

    /// <summary>
    /// Репозиторий для работы с сущностью БД State
    /// </summary>
    public class StateRepository : Repository<State>, IStateRepository
    {
        public StateRepository(ApplicationDbContext _db) : base(_db)
        {

        }

        /// <summary>
        /// Получить статус по его наименованию
        /// </summary>
        /// <param name="name">Наименование статуса</param>
        /// <returns>Возвращает найденый по наименованию статус - объект State</returns>
        public async Task<State> GetStateByNameAsync(string name)
        {

            var state = await _db.States.FirstOrDefaultAsync(s => s.Name.Trim().ToUpper() == name.Trim().ToUpper());

            return state;
        }

        /// <summary>
        /// Получить статус, являющийся статусом по умолчанию для новых экземпляров книг
        /// </summary>
        /// <returns>Возвращает статус, являющийся статусом по умолчанию для новых экземпляров книг - обхект типа State</returns>
        public async Task<State> GetIsInitialStateAsync()
        {
            var state = await _db.States.FirstOrDefaultAsync(s => s.IsInitialState);
            return state;
        }

    }
}
