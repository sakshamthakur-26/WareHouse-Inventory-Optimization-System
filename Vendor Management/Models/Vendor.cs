using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace VendorManagement.Models
{
    // Basic Vendor Model as per your requirement
    public class Vendor
    {
        public int VendorId { get; set; }
        public string Name { get; set; }
        public string ContactDetails { get; set; }
        public string GoodsSupllied { get; set; }
    }
}
