using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;

namespace WebApplication.Data
{
    public class ApiReceipt
    {
        public TimestampAttribute timeOfExecution { get; set; }

        public ApiReceipt(TimestampAttribute timeOfExecution)
        {
            this.timeOfExecution = timeOfExecution;
        }
        
    }
}