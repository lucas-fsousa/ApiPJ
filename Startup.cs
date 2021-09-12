using ApiPJ.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ApiPJ.Business.Repository.CustomerDefinition;
using ApiPJ.Configurations.Security;
using ApiPJ.Business.Repository.EmployeeDefinition;
using ApiPJ.Business.Repository.ApartmentDefinition;
using ApiPJ.Business.Repository.ReserveDefintion;

namespace ApiPJ {
  public class Startup {
    public Startup(IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {

      services.AddControllers().ConfigureApiBehaviorOptions(options => {
        options.SuppressConsumesConstraintForFormFileParameters = true;
      });
      services.AddSwaggerGen(c => {

        // definition of the name of the XML file that contains the detailed information
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        // Definition of the path where the file is located
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);

        c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiPJ", Version = "v1" });

        // ============= START AUTHENTICATION CONFIGURATION BEARER =========
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
          Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
          Name = "Authorization",
          In = ParameterLocation.Header,
          Type = SecuritySchemeType.ApiKey,
          Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement {{
          new OpenApiSecurityScheme {
            Reference = new OpenApiReference {
              Type = ReferenceType.SecurityScheme,
              Id = "Bearer"
            }
          },
           Array.Empty<string>()
          }
        });
      });

      var secret = Encoding.ASCII.GetBytes(Configuration.GetSection("JwtConfigurations:Secret").Value);
      services.AddAuthentication(x => {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(x => {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(secret),
          ValidateIssuer = false,
          ValidateAudience = false
        };
      });
      // ============= END AUTHENTICATION CONFIGURATION BEARER =========


      // Here is the connection string definition (appSettings.json) using SQLSERVER
      services.AddDbContext<Context>(options => {
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), x => x.MigrationsAssembly(typeof(Context).Assembly.FullName).EnableRetryOnFailure());
      });


      // configuration of the design pattern where the class that will work for the requested interface is informed to startup example: <Interface, Model class>
      services.AddScoped<ICustomerRepository, CustomerRepository>();
      services.AddScoped<IEmployeeRepository, EmployeeRepository>();
      services.AddScoped<IReserveRepository, ReserveRepository>();
      services.AddScoped<IApartmentRepository, ApartmentRepository>();
      services.AddScoped<IAuthenticationService, JwtTokenService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      if(env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiPJ v1"));
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();
      

      app.UseEndpoints(endpoints => {
        endpoints.MapControllers();
      });
    }
  }
}
