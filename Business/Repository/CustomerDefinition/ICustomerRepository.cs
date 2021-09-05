using ApiPJ.Entities;
using ApiPJ.Models.Login;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiPJ.Business.Repository.CustomerDefinition {
  public interface ICustomerRepository {
    public Task<Customer> GetUser(string cpf);
    public Task<List<Customer>> GetAllUser(int currentPage);
    public Task Update(Customer genericUser);
    public Task Register(Customer genericUser);
    public void Delete(Customer genericUser);
    public Task<Customer> LogIn(LoginInputViewModel loginInput);
    public Task Commit();
  }
}
