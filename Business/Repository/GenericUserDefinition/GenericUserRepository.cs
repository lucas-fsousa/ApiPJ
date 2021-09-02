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

    public void Commit() {
       _context.SaveChanges();
    }

    public Task Delete(string cpf) {
      throw new NotImplementedException();
    }

    // this method performs a search in the database looking for the CPF informed
    public async Task<GenericUser> GetUser(string cpf) {
      return await _context.GenericUserContext.FirstOrDefaultAsync(x => x.Cpf == cpf);
    }

    public async Task Register(GenericUser genericUser) {
      await _context.GenericUserContext.AddAsync(genericUser);
    }

    public Task Update(string cpf, GenericUser genericUser) {
      throw new NotImplementedException();
    }
  }
}
