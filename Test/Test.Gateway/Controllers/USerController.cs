using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModularSystem.Messaging.RabbitMQ.Core.DTOs;
using ModularSystem.Messaging.RabbitMQ.Core.EventBus;
using Test.Signature.Command.User;

namespace Test.Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class USerController : ControllerBase
    {
        private readonly IRabbitMQEventBus _bus;
        public USerController(IRabbitMQEventBus bus)
        {
            _bus = bus;
        }

        // GET: api/USer
        [HttpGet]
        public async Task<ActionResult> Get()
        {

            ICollection<Signature.DTOs.User> @return = null;

            for (var count = 0; count < 1; count++)
            {
                var userCommand = new UserCommandGetAll();
                var value = await _bus.PublishWaitAsync(userCommand);
                @return = value.Data;
            }
           
            return Ok(@return);
        }

        // GET: api/USer/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/USer
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/USer/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
