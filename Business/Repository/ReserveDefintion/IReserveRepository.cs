using ApiPJ.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPJ.Business.Repository.ReserveDefintion {
  public interface IReserveRepository {
    public Task Commit();
    public Task Insert(Reserve reserve);
    public Task<Reserve> GetReserve(int id);
    public Task<List<Reserve>> GetReserves(int CurrentPage, string cpf);
    public void Delete(Reserve reserve);
    public Task Update(Reserve reserve);
  }
}
