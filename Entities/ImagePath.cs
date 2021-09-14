using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPJ.Entities {
  public class ImagePath {
    public int Id { get; set; }
    public int ApartmentId { get; set; }
    public string Path { get; set; }
  }
}
