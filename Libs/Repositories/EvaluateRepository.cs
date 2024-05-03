namespace Libs.Repositories
{
    public interface IEvaluateRepository : IRepository<Evaluate>
    {
        public List<Evaluate> getEvaluatesList();
        public Evaluate getEvaluateById(int id);
    }
    public class EvaluateRepository : RepositoryBase<Evaluate>, IEvaluateRepository
    {
        public EvaluateRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
        public List<Evaluate> getEvaluatesList()
        {
            return _dbContext.Evaluates.ToList();
        }

        public Evaluate getEvaluateById(int id)
        {
            return _dbContext.Evaluates.FirstOrDefault(evaluate => evaluate.Id == id);
        }
    }
}
