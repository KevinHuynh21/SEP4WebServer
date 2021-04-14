namespace WebApplication.Data
{
    public class Plant
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public int PlantScore { get; set; }
        public float HumidityRequirement { get; set; }
        public float TemperatureRequirement { get; set; }
        public float CO2Requirement { get; set; }
        public float SunlightRequirement { get; set; }

        public Plant(int id, string name, int plantScore, float humidityRequirement, float temperatureRequirement, float co2Requirement, float sunlightRequirement)
        {
            Id = id;
            Name = name;
            PlantScore = plantScore;
            HumidityRequirement = humidityRequirement;
            TemperatureRequirement = temperatureRequirement;
            CO2Requirement = co2Requirement;
            SunlightRequirement = sunlightRequirement;
        }
    }
}