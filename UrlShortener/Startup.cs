using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Core.Implementations;
using StackExchange.Redis.Extensions.Newtonsoft;
using UrlShortener.Services;
using UrlShortener.Storage;
using static Newtonsoft.Json.NullValueHandling;
using static Newtonsoft.Json.TypeNameHandling;

namespace UrlShortener
{
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo { Title = "URLShortener API", Version = "v1" });
                config.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
            });

            AddServices(services);
        }

        /// <summary>
        /// Adds the services.
        /// </summary>
        /// <param name="services">The services.</param>
        private void AddServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            
            services.AddSingleton<ISerializer>(service => new NewtonsoftSerializer(new JsonSerializerSettings
            {
                TypeNameHandling = All,
                NullValueHandling = Include
            }));

            services.AddSingleton<IRedisCacheClient>(service =>
            {
                RedisConfiguration redisConfiguration = new RedisConfiguration()
                {
                    AbortOnConnectFail = true,
                    Hosts = new RedisHost[]
                    {
                        new RedisHost
                                {
                                    Host = Configuration["KeyValueStore:host"],
                                    Port = Convert.ToInt32(Configuration["KeyValueStore:port"])
                                }
                    },
                    AllowAdmin = true,
                    Database = 0,
                    ServerEnumerationStrategy = new ServerEnumerationStrategy()
                    {
                        Mode = ServerEnumerationStrategy.ModeOptions.All,
                        TargetRole = ServerEnumerationStrategy.TargetRoleOptions.Any,
                        UnreachableServerAction = ServerEnumerationStrategy.UnreachableServerActionOptions.Throw
                    },
                    PoolSize = 50
                };

                IRedisCacheConnectionPoolManager redisCacheConnectionPoolManager = new RedisCacheConnectionPoolManager(redisConfiguration);
                ISerializer serializer = service.GetRequiredService<ISerializer>();
                return new RedisCacheClient(redisCacheConnectionPoolManager, serializer, redisConfiguration);
            });

            services.AddSingleton<IUrlService>(service => new UrlService(service.GetRequiredService<IKeyValueStore>(), service.GetRequiredService<IHttpContextAccessor>()));
            services.AddSingleton<IKeyValueStore>(service => new KeyValueStore(service.GetRequiredService<IRedisCacheClient>()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "URLShortener API");
            });

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