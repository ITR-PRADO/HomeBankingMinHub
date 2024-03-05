using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Repositories
{
    public class LoanRepository:RepositoryBase<Loan>,ILoanRepository
    {
        public LoanRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }

        public Loan FindById(long id)
        {
            return FindByCondition(loan => loan.Id == id)
                .FirstOrDefault();
        }
        

        public IEnumerable<Loan> GetAllLoan()
        {
            return FindAll()
                .ToList();                
        }

        public void Save(Loan loan)
        {
            Create(loan);
            SaveChanges();
        }

    }
}
