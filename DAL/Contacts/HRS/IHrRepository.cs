using DAL.Models;

namespace DAL.Contacts.HRS
{
    public interface IHrRepository : IRepository<Employee, int>
    {
        IQueryable<Employee> GetAllEmployeesAsQuerableAsync();

        
    }
}
