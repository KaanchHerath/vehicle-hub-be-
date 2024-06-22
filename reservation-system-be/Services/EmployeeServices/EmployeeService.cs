using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.Models;


namespace reservation_system_be.Services.EmployeeServices
{

    public class EmployeeService : IEmployeeService
    {
            private readonly DataContext _context;

            public EmployeeService(DataContext context)
            {
            _context = context;
            }

            public async Task<List<Employee>> GetAllEmployees()
            {
                return await _context.Employees.ToListAsync();
            }

            public async Task<Employee> GetEmployee(int id)
            {
                {
                    var employee = await _context.Employees.FindAsync(id);
                    if (employee == null)
                    {
                        throw new DataNotFoundException("Employee not found");
                    }
                    return employee;
                }
            }


            public async Task<Employee> AddEmployee(Employee employee)
            {
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                return employee;
            }




            public async Task<Employee> UpdateEmployee(int id, Employee employee)
            {
                var existingEmployee = await _context.Employees.FindAsync(id);
                if (existingEmployee == null)
                {
                    throw new DataNotFoundException("Employee not found");
                }

                // Update customer properties
                existingEmployee.Name = employee.Name;
                existingEmployee.NIC = employee.NIC;
                existingEmployee.Email = employee.Email;
                existingEmployee.Status = employee.Status;
                existingEmployee.Address = employee.Address;
                existingEmployee.Password = employee.Password;
                existingEmployee.Role = employee.Role;
                existingEmployee.ContactNo = employee.ContactNo;
                existingEmployee.Gender = employee.Gender;
                existingEmployee.DOB = employee.DOB;

            if (!string.IsNullOrEmpty(employee.Password))
            {
                // Hash the new password
                existingEmployee.Password = BCrypt.Net.BCrypt.HashPassword(employee.Password);
            }

            _context.Entry(existingEmployee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return existingEmployee;

        }

            public async Task DeleteEmployee(int id)
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee == null)
                {
                    throw new DataNotFoundException("Employee not found");
                }

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();

            }

            
        }
    }   

