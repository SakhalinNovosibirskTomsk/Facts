using Facts_Domain.FactsDB;

namespace Facts_Business.Repository.IRepository
{
    /// <summary>
    /// Репозиторий для работы с информацией о выдаче, бронировании и списании экземпляров книг
    /// </summary>
    public interface IBookInstanceRepository : IRepository<BookInstance>
    {

    }
}
