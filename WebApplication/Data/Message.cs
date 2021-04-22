namespace WebApplication.Data
{
    public class Message
    {
        public Enum command { get; set; }
        public string json { get; set; }
        
        public Message(Enum command, string json)
        {
            this.command = command;
            this.json = json;
        }
    }
}