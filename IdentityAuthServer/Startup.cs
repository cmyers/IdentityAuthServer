using System.Text;
using IdentityAuthServer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System;

namespace IdentityAuthServer
{
    public class Startup
    {
        private IWebHostEnvironment _env;
        private const string CONTENTROOTPATHTOKEN = "%CONTENTROOTPATH%";
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();
            services.AddControllers();

            // TODO create options classes for app settings
            var identityConn = Configuration.GetConnectionString("IdentityConnection");

            identityConn = identityConn.Replace(CONTENTROOTPATHTOKEN, _env.ContentRootPath);

            services.AddDbContext<IdentityAuthDbContext>(options => options.UseSqlServer(identityConn));

            services.AddDefaultIdentity<AppUser>()
                .AddEntityFrameworkStores<IdentityAuthDbContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        RequireSignedTokens = true,
                        RequireExpirationTime = true,
                        ValidateIssuer = true,
                        ValidIssuer = Configuration["JWT:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = Configuration["JWT:Audience"],
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(Configuration["JWT:Key"])),
                        ClockSkew = TimeSpan.Zero //default is 5 minutes
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IdentityAuthDbContext authDbContext)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                authDbContext.Database.EnsureDeleted();
                authDbContext.Database.EnsureCreated();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}