using reservation_system_be.Models;

namespace reservation_system_be.Services.EmployeeServices
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllEmployees();
        Task<Employee> GetEmployee(int id);
        Task<Employee> AddEmployee(Employee employee);
        Task<Employee> UpdateEmployee(int id, Employee employee);
        Task DeleteEmployee(int id);
        Task<Employee> GetEmployeeById(int id);
    }
}
