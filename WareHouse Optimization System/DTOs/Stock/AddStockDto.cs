using WareHouse_Optimization_System.Models;

namespace WareHouse_Optimization_System.DTOs.Stock
{
    public class AddStockDto
    {
        public string ItemName { get; set; }
        public string CategoryName { get; set; }
       
        public int Quantity { get; set; }

        public string VendorName { get; set; }
        //public Vendor Vendor { get; set; }/

    }
}
