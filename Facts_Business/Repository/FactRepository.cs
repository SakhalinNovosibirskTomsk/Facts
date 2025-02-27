using Facts_Business.Repository.IRepository;
using Facts_DataAccess;
using Facts_Domain.FactsDB;

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
    }
}
