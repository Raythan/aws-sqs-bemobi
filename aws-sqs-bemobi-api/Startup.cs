using Amazon.SQS;
using aws_sqs_bemobi_api.DbContext;
using aws_sqs_bemobi_api.Helpers;
using aws_sqs_bemobi_api.Models;
using aws_sqs_bemobi_api.Repository;
using aws_sqs_bemobi_api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace aws_sqs_bemobi_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // Parâmetro para identificação no AWSSQS
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            // Injeção de dependências do AWSSQS
            services.AddAWSService<IAmazonSQS>();
            services.Configure<ServiceConfiguration>(Configuration.GetSection("ServiceConfiguration"));
            services.AddTransient<IAWSSQSService, AWSSQSService>();
            services.AddTransient<IAWSSQSHelper, AWSSQSHelper>();

            // Injeção de dependências da camada de repository
            services.AddTransient<IFileRepository, FileRepository>();
            services.AddTransient(_ => new DBContext(Configuration["ConnectionStrings:DefaultConnection"]));
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "aws_sqs_bemobi_api", Version = "v1" });
            });
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins, builder =>
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "aws_sqs_bemobi_api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // Parâmetro para identificação no AWSSQS
            app.UseCors(MyAllowSpecificOrigins);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
