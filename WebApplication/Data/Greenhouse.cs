using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

namespace WebApplication.Data
{
    public class Greenhouse
    {
        public List<Plant> Plants { get; set; }
        public int greenHouseID { get; set; }
        
        public bool WindowIsOpen { get; set; }
        
        public int userID { get; set; }
        public string Name { get; set; }
        
        public int waterFrequency { get; set; }
        
        public double waterVolume { get; set; }
        
        public string waterTimeOfDay { get; set; }
        
        public string lastWaterDate { get; set; }
        
        public string lastMeasurement { get; set; }

        public ArrayList sensorData { get; set; }
        
        public ArrayList sharedWith { get; set; }
        
        public ArrayList temperatureThreshold { get; set; }
        
        public ArrayList humidityThreshold { get; set; }

        public ArrayList co2Threshold { get; set; }
   

        public Greenhouse(string name, int greenHouseId, int userId,List<Plant> plants,int waterFrequency,double waterVolume,string waterTimeOfDay, string lastWaterDate,ArrayList sensorData, ArrayList SharedWith)
        {
            Plants = plants;
            greenHouseID = greenHouseId;
            userID = userId;
            Name = name;
            this.waterFrequency = waterFrequency;
            this.waterVolume = waterVolume;
            this.waterTimeOfDay = waterTimeOfDay;
            this.lastWaterDate = lastWaterDate;
            this.sensorData = sensorData;
            this.sharedWith = SharedWith;

        }

        public Greenhouse()
        {
            sensorData = new ArrayList();
            Plants = new List<Plant>();
            sharedWith = new ArrayList();
            temperatureThreshold = new ArrayList();
            humidityThreshold = new ArrayList();
            co2Threshold = new ArrayList();
        }

        public void AddPlant(Plant plant)
        {
            Plants.Add(plant);
        }

        public void RemovePlant(Plant plant)
        {
            Plants.Remove(plant);
        }

    
    }
}