using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AKarbowski.Infrastructure.ViewModels
{
    public class CarListItemViewModel
    {
        public string CarCode { get; set; }
        public int CarId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string ThumbnailBase64 { get; set; }
    }
}