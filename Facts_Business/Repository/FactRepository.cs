using Facts_Business.Repository.IRepository;
using Facts_DataAccess;
using Facts_Domain.FactsDB;
using Microsoft.EntityFrameworkCore;

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

        /// <summary>
        /// Получить текущий статус экземпляра книги
        /// </summary>
        /// <param name="bookInstanceId">ИД экземпляра книги</param>
        /// <returns>Текущий статус экземпляра книги</returns>
        public async Task<State> GetCurrentBookInstanceStateAsync(int bookInstanceId)
        {
            var foundFact = await _db.Facts
                    .Include(u => u.StateIn)
                    .Where(u => u.BookInstanceId == bookInstanceId && u.StateIdIn != null)
                    .OrderBy(u => u.DateOfReturn)
                    .LastOrDefaultAsync();
            if (foundFact != null)
                return foundFact.StateIn;
            else
            {
                return await _db.States.FirstOrDefaultAsync(u => u.IsInitialState);
            }
        }

        /// <summary>
        /// Получить факт по ИД
        /// </summary>
        /// <param name="id">ИД факта</param>
        /// <returns>Найденый факт</returns>
        public new async Task<Fact> GetByIdAsync(int id)
        {
            var foundFact = await _db.Facts
                .Include(u => u.StateOut)
                .Include(u => u.StateIn)
                .Include(u => u.FactComment)
                .FirstOrDefaultAsync(u => u.Id == id);
            return foundFact;
        }

        /// <summary>
        /// Получить информацию о факте выдачи экземпляра книги читателю
        /// </summary>
        /// <param name="bookInstanceId">ИД экземпляра книги</param>
        /// <param name="memberId">ИД читателя</param>
        /// <returns>Факт выдачи экземпляра книги читателю</returns>
        public async Task<Fact> GetCheckOutInfoByBookInstanceIsAndMemberIdAsync(int bookInstanceId, int memberId)
        {
            var foundFact = await _db.Facts
                .Include(u => u.StateIn)
                .Where(u => u.BookInstanceId == bookInstanceId && u.MemberId == memberId)
                    .OrderBy(u => u.FromDate)
                    .LastOrDefaultAsync();
            return foundFact;
        }
    }
}
