namespace DAL.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }
        public string? Status { get; set; }
        public virtual User User { get; set; }
    }
}
