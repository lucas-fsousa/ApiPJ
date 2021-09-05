using ApiPJ.Database;
using ApiPJ.Entities;
using ApiPJ.Models.Login;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPJ.Business.Repository.CustomerDefinition {
  public class CustomerRepository : ICustomerRepository {
    private readonly Context _context;
    public CustomerRepository(Context context) {
      _context = context;
    }

    // this method confirms the execution of the procedure and ends the transaction
    public async Task Commit() {
       await _context.SaveChangesAsync();
    }

    // this method is responsible for deleting the entity from the database and all the values ​​that correspond to it.
    public void Delete(Customer customer) {
      _context.FullAdressesContext.Remove(customer.Adress);
      _context.CustomerContext.Remove(customer);
    }

    // this method pagings the database and returns 10 users for each page
    public async Task<List<Customer>> GetAllUser(int currentPage) {
      int itemsPerPage = 10;

      // Context variables
      var adressContext = _context.FullAdressesContext;
      var customerContext = _context.CustomerContext;

      // Sql query to make a select on two tables and return a list with 10 results.
      var generic = await adressContext.Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage)
        .Select(x => x).Join(customerContext, user => user.Id, adress => adress.Id, (adress, user) => new { adress, user })
        .Where(x => x.adress.Id == x.user.Id).ToListAsync();

      // Filling in the objects related to the executed sql query
      var result = new List<Customer>();
      foreach(var item in generic) {
        Customer customer = item.user;
        customer.Adress = item.adress;
        result.Add(customer);
      }
      return result;
    }

    // this method performs a search in the database looking for the CPF informed
    public async Task<Customer> GetUser(string cpf) {
      cpf = cpf.Trim();
      var result = await _context.CustomerContext.FirstOrDefaultAsync(x => x.Cpf == cpf);

      // It guarantees that the search result will only be delivered if all entities are filled.
      if(result != null) {
        result.Adress = await _context.FullAdressesContext.FirstOrDefaultAsync(x => x.Id == result.Id);
        result = result.Adress == null ? null : result; 
      }
      return result;
    }

    // this method checks if the user exists and returns the login ok
    public async Task<Customer> LogIn(LoginInputViewModel loginInput) {
      return await _context.CustomerContext.FirstOrDefaultAsync(x => x.Password == loginInput.Password && x.Cpf == loginInput.Cpf);
    }

    // this method add a new user to the database
    public async Task Register(Customer customer) {
      await _context.CustomerContext.AddAsync(customer);
    }

    // this method replaces the items according to the information passed by the object and updates the items.
    public async Task Update(Customer customer) {
      await _context.CustomerContext.Where(x => x.Cpf == customer.Cpf).ForEachAsync(x => {
        x.Adress.PublicPlace = customer.Adress.PublicPlace;
        x.Adress.Reference = customer.Adress.Reference;
        x.Adress.Street = customer.Adress.Street;
        x.MaritalStatus = customer.MaritalStatus;
        x.PhoneNumber = customer.PhoneNumber;
        x.BirthDate = customer.BirthDate;
        x.Password = customer.Password;
        x.Email = customer.Email;
        x.Name = customer.Name;
        x.Sex = customer.Sex;
        x.Cpf = customer.Cpf;
        x.Rg = customer.Rg;
      });

    }
  }
}
