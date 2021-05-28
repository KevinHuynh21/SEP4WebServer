namespace WebApplication.Data
{
    public class Plant
    {
        public int plantID { get; set; }
        public int greenHouseID { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
     

        public Plant(int plantId, int greenHouseId, string name,string url)
        {
            plantID = plantId;
            greenHouseID = greenHouseId;
            Name = name;
            Url = url;

        }
        
        public Plant()
        {
            
        }
    }
}