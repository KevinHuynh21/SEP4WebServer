using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Data
{
    public class ApiCurrentDataPackage
    {
        public List<DataContainer> data { get; set; }
        
        public TimestampAttribute lastDataPoint { get; set; }

        public ApiCurrentDataPackage(List<DataContainer> data, TimestampAttribute lastDataPoint)
        {
            this.data = data;
            this.lastDataPoint = lastDataPoint;
        }
    }
}