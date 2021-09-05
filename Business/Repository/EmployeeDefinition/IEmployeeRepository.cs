using ApiPJ.Entities;
using ApiPJ.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPJ.Business.Repository.EmployeeDefinition {
  public interface IEmployeeRepository {
    public void Delete(Employee genericUser);
    public Task Commit();
    public Task<Employee> GetUser(string cpf);
    public Task<List<Employee>> GetAllUser(int currentPage);
    public Task Update(Employee genericUser);
    public Task Register(Employee genericUser);
    public Task<Employee> LogIn(LoginInputViewModel loginInput);
  }
}
