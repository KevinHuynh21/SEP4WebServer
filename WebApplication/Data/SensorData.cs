namespace WebApplication.Data
{
    public class SensorData
    {
        public string type { get; set; }
        public double value { get; set; }


        public SensorData()
        {
            
        }

        public SensorData(string type, double value)
        {
            this.type = type;
            this.value = value;
        }
    }
}