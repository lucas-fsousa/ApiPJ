using ApiPJ.Entities;
using ApiPJ.Models.Login;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiPJ.Business.Repository.EmployeeDefinition {
  public interface IEmployeeRepository {
    public void Delete(Employee genericUser);
    public Task Commit();
    public Task<Employee> GetEmployee(string cpf);
    public Task<List<Employee>> GetEmployees(int currentPage);
    public Task Update(Employee genericUser);
    public Task Register(Employee genericUser);
    public Task<Employee> LogIn(LoginInputViewModel loginInput);
  }
}
