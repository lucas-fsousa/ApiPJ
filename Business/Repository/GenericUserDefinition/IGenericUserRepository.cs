using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiPJ.Entities;

namespace ApiPJ.Business.Repository.GenericUserDefinition {
  public interface IGenericUserRepository {
    public Task<GenericUser> GetUser(string cpf);
    public Task Update(string cpf, GenericUser genericUser);
    public Task Register(GenericUser genericUser);
    public void Delete(GenericUser genericUser);
    public Task Commit();
  }
}
