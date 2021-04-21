using System;
using Microsoft.AspNetCore.Mvc;

namespace WebServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServerController:ControllerBase
    {
        [HttpGet]
        public string getUser([FromQuery] string username, [FromQuery] string password)
        {
            Console.Write(username+password);
            return username + password;
        }
        
    }
}