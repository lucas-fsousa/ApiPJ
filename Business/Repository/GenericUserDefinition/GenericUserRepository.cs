using ApiPJ.Database;
using ApiPJ.Entities;
using ApiPJ.Error;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPJ.Business.Repository.GenericUserDefinition {
  public class GenericUserRepository : IGenericUserRepository {
    private readonly Context _context;
    public GenericUserRepository(Context context) {
      _context = context;
    }

    // This method confirms the execution of the procedure and ends the transaction
    public async Task Commit() {
       await _context.SaveChangesAsync();
    }

    //This method is responsible for deleting the entity from the database and all the values ​​that correspond to it.
    public void Delete(GenericUser genericUser) {
      _context.fullAdresses.Remove(genericUser.Adress);
      _context.GenericUserContext.Remove(genericUser);
    }

    // this method performs a search in the database looking for the CPF informed
    public async Task<GenericUser> GetUser(string cpf) {
      cpf = cpf.Trim();
      var result = await _context.GenericUserContext.FirstOrDefaultAsync(x => x.Cpf == cpf);

      // It guarantees that the search result will only be delivered if all entities are filled.
      if(result != null) {
        result.Adress = await _context.fullAdresses.FirstOrDefaultAsync(x => x.Id == result.Id);
        result = result.Adress == null ? null : result; 
      }
      
      return result;
    }

    public async Task Register(GenericUser genericUser) {
      await _context.GenericUserContext.AddAsync(genericUser);
    }

    // replaces the items according to the information passed by the object and updates the items.
    public async Task Update(GenericUser genericUser) {
      await _context.GenericUserContext.Where(x => x.Cpf == genericUser.Cpf).ForEachAsync(x => {
        x.Adress.PublicPlace = genericUser.Adress.PublicPlace;
        x.Adress.Reference = genericUser.Adress.Reference;
        x.Adress.Street = genericUser.Adress.Street;
        x.MaritalStatus = genericUser.MaritalStatus;
        x.PhoneNumber = genericUser.PhoneNumber;
        x.BirthDate = genericUser.BirthDate;
        x.Password = genericUser.Password;
        x.Email = genericUser.Email;
        x.Name = genericUser.Name;
        x.Sex = genericUser.Sex;
        x.Cpf = genericUser.Cpf;
        x.Rg = genericUser.Rg;
      });

    }
  }
}
