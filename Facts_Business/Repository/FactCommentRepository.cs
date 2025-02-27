using Facts_Business.Repository.IRepository;
using Facts_DataAccess;
using Facts_Domain.FactsDB;

namespace Facts_Business.Repository
{

    /// <summary>
    /// Репозиторий для работы с сущностью БД Fact
    /// </summary>
    public class FactCommentRepository : Repository<FactComment>, IFactCommentRepository
    {
        public FactCommentRepository(ApplicationDbContext _db) : base(_db)
        {

        }
    }
}
