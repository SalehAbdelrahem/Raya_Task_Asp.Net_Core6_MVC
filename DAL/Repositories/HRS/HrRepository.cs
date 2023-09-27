
using DAL.Contacts.HRS;
using DAL.Data;
using DAL.Models;

namespace DAL.Repositories.HRS
{
    public class HrRepository : Repository<Employee, int>, IHrRepository
    {
        public HrRepository(AppDBContext context) : base(context)
        {

        }

        public IQueryable<Employee> GetAllEmployeesAsQuerableAsync()
        {
            return _context.Employees.AsQueryable();
        }
    }
}
