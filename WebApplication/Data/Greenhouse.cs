using System.Collections;
using System.Dynamic;

namespace WebApplication.Data
{
    public class Greenhouse
    {
        public ArrayList Plants;
        public int Id { get; set; }
        
        public string Name { get; set; }
        public float Temperature { get; set; }
        public float CO2 { get; set; }
        public float Humidity { get; set; }
        public float Sunlight { get; set; }

        public Greenhouse(int id, string name, float temperature, float co2, float humidity, float sunlight)
        {
            Plants = new ArrayList();
            Id = id;
            Name = name;
            Temperature = temperature;
            CO2 = co2;
            Humidity = humidity;
            Sunlight = sunlight;
        }

        public void AddPlant(Plant plant)
        {
            Plants.Add(plant);
        }

        public void RemovePlant(Plant plant)
        {
            Plants.Remove(plant);
        }

        public ArrayList GetPlants()
        {
            return Plants;
        }
    }
}