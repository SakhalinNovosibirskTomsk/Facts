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

        /// <summary>
        /// Получить текущий статус экземпляра книги
        /// </summary>
        /// <param name="bookInstanceId">ИД экземпляра книги</param>
        /// <returns>Текущий статус экземпляра книги</returns>
        public Task<State> GetCurrentBookInstanceStateAsync(int bookInstanceId);

        /// <summary>
        /// Получить факт по ИД
        /// </summary>
        /// <param name="id">ИД факта</param>
        /// <returns>Найденый факт</returns>
        public Task<Fact> GetByIdAsync(int id);

    }
}
