using ApiPJ.Database;
using ApiPJ.Entities;
using ApiPJ.Models.Login;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPJ.Business.Repository.EmployeeDefinition {
  public class EmployeeRepository : IEmployeeRepository {
    private readonly Context _context;
    public EmployeeRepository(Context context) {
      _context = context;
    }
    public async Task Commit() {
      await _context.SaveChangesAsync();
    }

    public void Delete(Employee employee) {
      _context.FullAdressesContext.Remove(employee.Adress);
      _context.EmployeeContext.Remove(employee);
    }

    public async Task<List<Employee>> GetAllUser(int currentPage) {
      int itemsPerPage = 10;

      // Context variables
      var adressContext = _context.FullAdressesContext;
      var employeerContext = _context.EmployeeContext;

      // Sql query to make a select on two tables and return a list with 10 results.
      var generic = await adressContext.Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage)
        .Select(x => x).Join(employeerContext, user => user.Id, adress => adress.Id, (adress, user) => new { adress, user })
        .Where(x => x.adress.Id == x.user.Id).ToListAsync();

      // Filling in the objects related to the executed sql query
      var result = new List<Employee>();
      foreach(var user in generic) {
        Employee employee = user.user;
        result.Add(employee);
      }
      return result;
    }

    // this method performs a search in the database looking for the CPF informed
    public async Task<Employee> GetUser(string cpf) {
      cpf = cpf.Trim();
      var employee = _context.EmployeeContext;
      var adress = _context.FullAdressesContext;

      var generic = await employee.Select(x => x).Where(x => x.Cpf == cpf).Join(adress, employeeTable => employeeTable.Id, adressTable => adressTable.Id, (employeeTable, adressTable) => new { employeeTable, adressTable }).ToArrayAsync();

      // It guarantees that the search result will only be delivered if all entities are filled.
      if(generic.Length == 0) {
        return null;
      }

      Employee returnedEmployee = generic[0].employeeTable;
      return returnedEmployee;
    }

    // method to confirm the existence of the user, returns the user.
    public async Task<Employee> LogIn(LoginInputViewModel loginInput) {
      return await _context.EmployeeContext.FirstOrDefaultAsync(x => x.Password == loginInput.Password && x.Cpf == loginInput.Cpf);
    }

    // method responsible for registering user
    public async Task Register(Employee employee) {
      await _context.EmployeeContext.AddAsync(employee);
    }

    // this method is able to update the data contained in the database according to user input.
    public async Task Update(Employee employee) {
      await _context.EmployeeContext.Where(x => x.Cpf == employee.Cpf).ForEachAsync(x => {
        x.Adress.PublicPlace = employee.Adress.PublicPlace;
        x.Adress.Reference = employee.Adress.Reference;
        x.Adress.Street = employee.Adress.Street;
        x.MaritalStatus = employee.MaritalStatus;
        x.PhoneNumber = employee.PhoneNumber;
        x.BirthDate = employee.BirthDate;
        x.Password = employee.Password;
        x.Email = employee.Email;
        x.Name = employee.Name;
        x.Sex = employee.Sex;
        x.Cpf = employee.Cpf;
        x.Rg = employee.Rg;

        x.ContractualSalary = employee.ContractualSalary;
        x.DemissionDate = employee.DemissionDate;
        x.AdmissionDate = employee.AdmissionDate;
        x.WalletWorkId = employee.WalletWorkId;
        x.FunctionName = employee.FunctionName;
        x.AcessLevel = employee.AcessLevel;
      });
    }

  }
}
