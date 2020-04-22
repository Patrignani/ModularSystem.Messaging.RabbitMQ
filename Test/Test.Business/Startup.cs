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
            services.AddRabbitMQ(Configuration, options => {
                options.AddConnection("rabbitmq");
            });
            services.AddScoped<ICommandHandler<UserCommandGetAll, ICollection<User>>, UserHandle>();
            services.AddScoped<ICommandHandler<UserCommandGet, User>, UserHandle>();
            services.AddScoped<ICommandHandler<UserCommandDelete>, UserHandle>();
            services.AddScoped<ICommandHandler<UserCommandInsert, UserId>, UserHandle>();
            services.AddScoped<ICommandHandler<UserCommandUpdate>, UserHandle>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRabbitMq()
             .RequestReplyCommand<UserCommandGetAll, ICollection<User>>()
             .RequestReplyCommand<UserCommandGet, User>()
             .SubscribeToCommand<UserCommandDelete>()
             .RequestReplyCommand<UserCommandInsert, UserId>()
             .SubscribeToCommand<UserCommandUpdate>();

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
