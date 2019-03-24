using System.Text;
using AuthServer.Models;
using AuthServer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AuthServer
{
    public class Startup
    {
        private IHostingEnvironment _env;
        private readonly string ContentRootPathToken = "%CONTENTROOTPATH%";
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var dataConn = Configuration["ConnectionStrings:DataConnection"];
            var identityConn = Configuration["ConnectionStrings:IdentityConnection"];


            dataConn = dataConn.Replace(ContentRootPathToken, _env.ContentRootPath);
            identityConn = identityConn.Replace(ContentRootPathToken, _env.ContentRootPath);

            services.AddDbContext<AuthIdentityDbContext>(options => options.UseSqlServer(identityConn));
            services.AddDbContext<DataDbContext>(options => options.UseSqlServer(dataConn));

            services.AddTransient<IUserService, UserService>();

            services.AddDefaultIdentity<AppUser>()
                .AddEntityFrameworkStores<AuthIdentityDbContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["JWT:Issuer"],
                        ValidAudience = Configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]))
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, AuthIdentityDbContext authDbContext, DataDbContext dataDbContext)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                authDbContext.Database.EnsureDeleted();
                authDbContext.Database.EnsureCreated();

                //basic db without identity server
                dataDbContext.Database.EnsureDeleted();
                dataDbContext.Database.EnsureCreated();
            }

            app.UseAuthentication();
            app.UseMvc();
           
        }
    }
}