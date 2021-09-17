using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Business.Methods {
  public static class Functions {
    public static string EncodePassword(this string str) {
      byte[] salt = new byte[128 / 8];
      byte[] codify = KeyDerivation.Pbkdf2(str, salt, KeyDerivationPrf.HMACSHA1, 10000, 256 / 8);
      return Convert.ToBase64String(codify);
    }


    public static bool ValidateCPF(this string cpf) {
      // Remove invalid spaces
      cpf = cpf.Trim().Replace(".", "").Replace("-", "");

      // Check if the CPF entered has 11 digits
      if(cpf.Count() != 11) {
        return false;
      } else {
        // Check if the CPF entered is a "KNOWN" CPF. Example: 22222222222
        for(int i = 0; i < 10; i++) {
          if((i * 11111111111).ToString().Equals(cpf)) {
            return false;
          }
        }
      }

      // Finding the first check digit
      long soma = 0;
      var _base = 100000000;
      var _cpf = Int32.Parse(cpf.Substring(0, 9));
      for(int i = 0; i < 9; i++) {
        soma += (_cpf / _base) * (i + 1);
        _cpf %= _base;
        _base /= 10;
      }
      var d1 = soma % 11;
      d1 = d1 > 9 ? 0 : d1;

      // Finding the second check digit
      _base = 100000000;
      soma = 0;
      _cpf = Int32.Parse(cpf.Substring(0, 9));
      for(int i = 10; i > 1; i--) {
        soma += (_cpf / _base) * (i + 1);
        _cpf %= _base;
        _base /= 10;
      }
      soma += d1 * 2;
      var d2 = (soma * 10) % 11;
      d2 = d2 > 9 ? 0 : d2;
      var confirmCPF = String.Concat(cpf.Substring(0, 9), d1, d2);
      return confirmCPF.Equals(cpf) ? true : false;
    }

  }
}
