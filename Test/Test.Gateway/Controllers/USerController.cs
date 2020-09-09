using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModularSystem.Messaging.RabbitMQ.Core.DTOs;
using ModularSystem.Messaging.RabbitMQ.Core.EventBus;
using Test.Signature.Command.User;
using Test.Signature.DTOs;

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

            var tasks = new List<Task<RabbitRPC<ICollection<User>>>>();
            for (var count = 0; count < 2; count++)
            {
                var userCommand = new UserCommandGetAll();
                tasks.Add(_bus.PublishWaitAsync(userCommand, "Teste"));
               
            }

            var values = await Task.WhenAll(tasks);


            return Ok(values.Select(x => x.Data));
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
