using ApiPJ.Models.UserBase;
using System;

namespace ApiPJ.Models.Employee {
  public class EmployeeUpdateInputModel : GenericUpdateModel {
    public bool Active { get; set; }
    public int AcessLevel { get; set; }
    public DateTime AdmissionDate { get; set; }
    public DateTime DemissionDate { get; set; }
    public decimal ContractualSalary { get; set; }
    public string FunctionName { get; set; }
    public string WalletWorkId { get; set; }
  }
}
