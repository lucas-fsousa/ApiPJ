using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPJ.Models.ImagePath {
  public class ImagePathOutputModel {
    public ImagePathOutputModel() {
      this.Extension = "image/jpeg";
    }
    public int Id { get; set; }
    public int ApartmentId { get; set; }
    public string Path { get; set; }
    public string Extension { get; }

  }
}
