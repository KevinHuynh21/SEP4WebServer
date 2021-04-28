

namespace WebApplication.Data
{
    public class DataContainer
    {
        public double data { get; set; }
        public DataType type { get; set; }

        public DataContainer(double data, DataType type)
        {
            this.data = data;
            this.type = type;
        }
    }
}