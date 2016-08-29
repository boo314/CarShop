using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AKarbowski.Infrastructure.Models
{
    public class ImageModel
    {
        public int ImageId { get; set; }
        public string ImagePath { get; set; }
        public string ImageName { get; set; }
        public string MimeType { get; set; }
        public string ImageCode { get; set; }
        public ImageTypeModel Type { get; set; }
    }
}