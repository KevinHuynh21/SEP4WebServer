using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

namespace WebApplication.Data
{
    public class Greenhouse
    {
        public ArrayList Plants;
        public int greenHouseID { get; set; }
        
        public int userID { get; set; }
        public string Name { get; set; }
        
        public int waterFrequency { get; set; }
        
        public double waterVolume { get; set; }
        
        public string waterTimeOfDay { get; set; }
        
        public string lastWaterDate { get; set; }
        
        public string lastMeasurement { get; set; }

        public ArrayList sensorData { get; set; }
        
        public ArrayList sharedWith { get; set; }
   

        public Greenhouse(string name, int greenHouseId, int userId,ArrayList plants,int waterFrequency,double waterVolume,string waterTimeOfDay, string lastWaterDate,ArrayList sensorData, ArrayList SharedWith)
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