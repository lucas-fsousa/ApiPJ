using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPJ.Error {
  public static class GenericError {
    public static IList<string> List { get; }

    internal static void AddError(string message) {
      List.Add(message);
      
    }
  }
}
