using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication.Data;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public string getUser([FromQuery] string username, [FromQuery] string password)
        {
            return username + password;
        }

        [HttpGet]
        [Route("{UserId}/Greenhouse")]
        public string getMyGreenhouses([FromRoute] int userId)
        {
            return "Here are your greenhouses" + userId;
        }
        
        [HttpGet]
        [Route("{UserId}/Greenhouse/{GreenhouseId}")]
        public string getGreenhouseById([FromRoute] int userId)
        {
            return "GreenhouseById" + userId;
        }
        
        [HttpGet]
        [Route("{UserId}/Greenhouse/{GreenhouseId}/CurrentData")]
        public string getCurrentData([FromRoute] int userId, int greenhouseId)
        {
            return "Hello" + userId + greenhouseId;
        }

        [HttpGet]
        [Route("{UserId}/Greenhouse/{GreenhouseId}/averageData")]
        public string getAverageData([FromRoute] int userId, int greenhouseId)
        {
            return "Helloaverage" + userId + greenhouseId;
        }

        [HttpPost]
        [Route("{UserId}/Greenhouse/{GreenhouseId}/waternow")]
        public StatusCodeResult waterNow([FromRoute] int userId, int greenhouseId)
        {
            //WaterNow
            return StatusCode(200);
        }

        [HttpPost]
        [Route("{UserId}/Greenhouse/{GreenhouseId}/openWindow")]
        public StatusCodeResult openWindow([FromRoute] int userId, int greenhouseId)
        {
            //Opens window
            return StatusCode(200);
        }

        [HttpPost]
        [Route("{UserId}/addGreenhouse")]
        public string addGreenhouse([FromBody] Greenhouse greenhouse)
        {
            //Returns Id of the greenhouse
            if (greenhouse != null)
            {
                return "Greenhouse added" + greenhouse.Id;
            }
            return "Greenhouse is null";
        }

        [HttpPost]
        [Route("{UserId}/Greenhouse/{GreenhouseId}/addPlant")]
        public string addPlant([FromBody] Plant plant)
        {
            if (plant != null)
            {
                return "Plant added" + plant.Id; 
            }
            return "plant is null";
        }


        [HttpPost]
        [Route("addUser")]
        public string addUser([FromBody] User user)
        {
            
            if (user != null)
            {
                return "User added" + user.Id;
            }
            return "User is null";
        }

    }
}