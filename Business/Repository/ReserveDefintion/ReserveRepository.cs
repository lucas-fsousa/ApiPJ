using ApiPJ.Database;
using ApiPJ.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPJ.Business.Repository.ReserveDefintion {

  public class ReserveRepository : IReserveRepository {
    private readonly Context _context;
    public ReserveRepository(Context context) {
      _context = context;
    }

    public async Task Commit() {
      await _context.SaveChangesAsync();
    }

    public void Delete(Reserve reserve) {
      _context.Remove(reserve);
    }

    public Task<Reserve> GetReserve(int id) {
      return _context.ReserveContext.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Reserve>> GetReserves(int CurrentPage, string cpf) {
      int itemsPerPage = 10;

      // checks if the CPF was informed so that the search is based on a CPF
      if(!cpf.Equals("")) {
        var customerId = _context.CustomerContext.FirstOrDefault(x => x.Cpf == cpf).Id;
        return await _context.ReserveContext.Skip((CurrentPage - 1) * itemsPerPage).Take(itemsPerPage).Where(x => x.IdCustomer == customerId).ToListAsync();
      }
      return await _context.ReserveContext.Skip((CurrentPage - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync();
    }

    public async Task Insert(Reserve reserve) {
      await _context.AddAsync(reserve);
    }

    // this method is able to update the data contained in the database according to user input.
    public async Task Update(Reserve reserve) {
      await _context.ReserveContext.Where(x => x.Id == reserve.Id).ForEachAsync(x => {
        x.IdApartment = reserve.IdApartment;
        x.InitialDate = reserve.InitialDate;
        x.IdCustomer = reserve.IdCustomer;
        x.TotalPrice = reserve.TotalPrice;
        x.FinalDate = reserve.FinalDate;
      });
    }
  }
}
