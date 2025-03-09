using Facts_Business.Repository.IRepository;
using Facts_DataAccess;
using Facts_Domain.FactsDB;
using System.Data.Entity;

namespace Facts_Business.Repository
{

    /// <summary>
    /// Репозиторий для работы с сущностью БД Fact
    /// </summary>
    public class FactRepository : Repository<Fact>, IFactRepository
    {
        public FactRepository(ApplicationDbContext _db) : base(_db)
        {
        }

        /// <summary>
        /// Получить все факты выдачи/возврата экземпляров книги
        /// </summary>
        /// <returns>Все факты выдачи/возврата экземпляров книг читателями</returns>
        public async Task<IEnumerable<Fact>> GetAllFactsAsync()
        {
            var gotFacts = _db.Facts
                .Include(u => u.StateOut)
                .Include(u => u.StateIn)
                .Include(u => u.FactComment)
                .OrderBy(u => u.FromDate)
                .ToList();
            return gotFacts;
        }
    }
}
