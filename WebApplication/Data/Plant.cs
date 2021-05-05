namespace WebApplication.Data
{
    public class Plant
    {
        public int plantID { get; set; }
        public int greenHouseID { get; set; }
        public string Name { get; set; }
        public int PlantScore { get; set; }
        public float HumidityRequirement { get; set; }
        public float TemperatureRequirement { get; set; }
        public float CO2Requirement { get; set; }

        public Plant(int plantId, int greenHouseId, string name, int plantScore, float humidityRequirement, float temperatureRequirement, float co2Requirement)
        {
            plantID = plantId;
            greenHouseID = greenHouseId;
            Name = name;
            PlantScore = plantScore;
            HumidityRequirement = humidityRequirement;
            TemperatureRequirement = temperatureRequirement;
            CO2Requirement = co2Requirement;

        }
    }
}