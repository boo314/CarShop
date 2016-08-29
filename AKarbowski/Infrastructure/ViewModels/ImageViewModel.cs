using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AKarbowski.Infrastructure.ViewModels
{
    public class ImageViewModel
    {
        public string Title { get; set; }
        public string Alt { get; set; }
        public string ImageBase64 { get; set; }
        public int ImageId { get; set; }

    }
}