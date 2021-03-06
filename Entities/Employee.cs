using ApiPJ.Entities.Base;
using System;

namespace ApiPJ.Entities {
  public class Employee: GenericUserBase {
    public DateTime AdmissionDate { get; set; }
    public DateTime DemissionDate { get; set; }
    public string WalletWorkId { get; set; }
    public string FunctionName { get; set; }
    public decimal ContractualSalary { get; set; }
    public int AcessLevel { get; set; }
    public bool Active { get; set; }
  }
}
