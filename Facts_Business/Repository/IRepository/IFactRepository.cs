using Facts_Domain.FactsDB;

namespace Facts_Business.Repository.IRepository
{
    /// <summary>
    /// Репозиторий для работы с сущностью БД Fact
    /// </summary>
    public interface IFactRepository : IRepository<Fact>
    {

        /// <summary>
        /// Получить все факты выдачи/возврата экземпляров книги
        /// </summary>
        /// <returns>Все факты выдачи/возврата экземпляров книг читателями</returns>
        public Task<IEnumerable<Fact>> GetAllFactsAsync();

    }
}
