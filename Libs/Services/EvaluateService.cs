﻿namespace Libs.Services
{
    public class EvaluateService
    {
        private ApplicationDbContext dbContext;
        private IEvaluateRepository EvaluateRepository;

        public EvaluateService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.EvaluateRepository = new EvaluateRepository(dbContext);
        }

        public void Save()
        {
            this.dbContext.SaveChanges();
        }

        public List<Evaluate> getEvaluatesList()
        {
            return EvaluateRepository.getEvaluatesList();
        }
        public Evaluate getEvaluateById(int id)
        {
            return this.EvaluateRepository.getEvaluateById(id);
        }
        public void insertEvaluate(Evaluate evaluate)
        {
            dbContext.Evaluates.Add(evaluate);
            Save();
        }
        public void updateEvaluate(Evaluate evaluate)
        {
            EvaluateRepository.Update(evaluate);
            Save();
        }
        public void deleteEvaluate(int id)
        {
            Evaluate evaluate = EvaluateRepository.getEvaluateById(id);
            if (evaluate != null)
            {
                EvaluateRepository.Delete(evaluate);
                Save();
            }
        }
    }
}
