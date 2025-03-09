using Facts_Business.Repository.IRepository;
using Facts_DataAccess;
using Facts_Domain.FactsDB;

namespace Facts_Business.Repository
{

    /// <summary>
    /// Репозиторий для работы с информацией о выдаче, бронировании и списании экземпляров книг
    /// </summary>
    public class BookInstanceRepository : Repository<BookInstance>, IBookInstanceRepository
    {
        public BookInstanceRepository(ApplicationDbContext _db) : base(_db)
        {

        }
    }
}
