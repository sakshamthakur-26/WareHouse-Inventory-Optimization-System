namespace WareHouse_Optimization_System.Services;

public class DemoService
{

    public async Task<int> GetCategoryId(string category)
    {
        return 1;
    }

    public int GetZoneId(string zone)
    {
        return 1;
    }


    //function find the required zone capacity for the given category and quantity and return if available then return the zone id + zone name otherwise return null
    public async Task<int> CheckZoneCapacity(string category, int quantity)
    {
        return 1;
    }

    public async Task<string> GetZoneNameByCategory(string categoryName)
    {
        return "ZoneA";
    }

    public async Task<bool> CreateTransaction(int ItemId, string Name, int CategoryName, int quantity, int ZoneName,string type)
    {
        return true;
    }

    public async Task<bool> UpdateTransaction(int ItemId, string Name, string CategoryName, int quantity, string ZoneName)
    {
        return true;
    }

    public async Task<bool> UpdateStockItem(int ItemId,int newQuantity)
    {
        return true;
    }

    public async Task<bool> UpdateZoneCapacity(int Itemid, int quantity)
    {
        return true;
    }

}
