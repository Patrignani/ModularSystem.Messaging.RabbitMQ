using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModularSystem.Messaging.RabbitMQ;
using ModularSystem.Messaging.RabbitMQ.Core.Command;
using ModularSystem.Messaging.RabbitMQ.ServiceHost;
using Test.Business.Handle;
using Test.Signature.Command.User;
using Test.Signature.DTOs;

namespace Test.Business
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRabbitMQ(Configuration, options =>
            {
                options.AddConnection("rabbitmq");
            })
              .RequestReplyCommand<UserCommandGetAll, ICollection<User>, UserHandle>(option => {
                  option.QueueName = "Teste";
              })
              .RequestReplyCommand<UserCommandGet, User, UserHandle>()
              .SubscribeToCommand<UserCommandDelete, UserHandle>(option => {
                  option.QueueName = "Deletar";
              })
              .RequestReplyCommand<UserCommandInsert, UserId, UserHandle>()
              .SubscribeToCommand<UserCommandUpdate, UserHandle>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
