using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic.CompilerServices;
using WebApplication.Data;
using WebApplication.Network;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private HttpClient client = new HttpClient();
        private NetworkImpl networkimpl = new NetworkImpl();
        
        [HttpGet]
        public async Task<string> getUser([FromQuery] string username, [FromQuery] string password)
        {
            string message = networkimpl.getUser(username, password);
            return message;
        }

        [HttpGet]
        [Route("{UserId}/Greenhouse")]
        public async Task<string> getMyGreenhouses([FromRoute] int userId)
        {
            Console.Write(userId);
            string message = networkimpl.getGreenhouses(userId);
            return message;
        }
        
        [HttpGet]
        [Route("{UserId}/Greenhouse/{GreenhouseId}")]
        public async Task<string> getGreenhouseById([FromRoute] int userId,int greenHouseID)
        {
            string message = networkimpl.getGreenhouseByID(userId, greenHouseID);
           return message;
        }
        
        [HttpGet]
        [Route("{UserId}/Greenhouse/{GreenhouseId}/CurrentData")]
        public async Task<String> getCurrentData([FromRoute] int userId, int greenhouseId)
        {
            string message = networkimpl.GetCurrentData(userId, greenhouseId); 
            return message ;
        }

        [HttpGet]
        [Route("{UserId}/Greenhouse/{GreenhouseId}/averageDataHistory")]
        public async Task<string> getAverageData([FromRoute] int userId, int greenhouseId, [FromQuery] DateTime timeFrom, [FromQuery] DateTime timeTo)
        {
            string message = networkimpl.getAverageData(userId, greenhouseId,timeFrom,timeTo);
            return message;
        }
        

        [HttpPost]
        [Route("{UserId}/Greenhouse/{GreenhouseId}/waternow")]
        public async Task<ActionResult> waterNow([FromRoute] int userId, int greenhouseId)
        {
            await networkimpl.waterNow(userId, greenhouseId);
            return StatusCode(200);
        }

        [HttpGet]
        [Route("{UserId}/Greenhouse/{GreenhouseId}/openWindow/{openOrClose}")]
        public async Task<ActionResult> openWindow([FromRoute] int userId, int greenhouseId, int openOrClose)
        {
            await networkimpl.openWindow(userId, greenhouseId,openOrClose);
            return StatusCode(200);
        }

        [HttpPost]
        [Route("{UserId}/addGreenhouse/")]
        public async Task<ActionResult<String>> addGreenhouse([FromBody] Greenhouse greenhouse)
        {
            Message message = await networkimpl.addGreenHouse(greenhouse);
            return Ok(message.json);
        }

        [HttpPost]
        [Route("{UserId}/Greenhouse/{GreenhouseId}/addPlant")]
        public async Task<ActionResult<String>> addPlant([FromBody] Plant plant)
        {
            Message message = await networkimpl.addPlant(plant);
            return Ok(message.json);
        }


        [HttpPost]
        [Route("addUser")]
        public async Task<ActionResult<String>> addUser([FromBody] User user)
        {
            Message message = await networkimpl.addUser(user);
            return Ok(message.json);
        }

    }
}