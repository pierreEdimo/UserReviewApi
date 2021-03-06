using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using userVoice.DBContext;

using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using userVoice.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using userVoice.Services;
using Microsoft.OpenApi.Models; 



namespace userVoice
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
            services.AddControllers().AddNewtonsoftJson();

           

            services.AddDbContext<DatabaseContext>(opt =>
               opt.UseSqlite("Data Source=Userreview"));

            services.AddTransient<IFileStorageService, InAppStorageService>();

            services.AddHttpContextAccessor(); 

            services.AddAutoMapper(typeof(Startup));  

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddIdentity<UserEntity, IdentityRole>(config =>
            {
                config.Password.RequireDigit = true;
                config.Password.RequireLowercase = true; 
                config.Password.RequiredLength = 6;
                config.Password.RequireNonAlphanumeric = true;
                config.User.RequireUniqueEmail = true;
            })
             .AddEntityFrameworkStores<DatabaseContext>()
             .AddDefaultTokenProviders();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                 .AddJwtBearer(config =>
                 {
                     config.RequireHttpsMetadata = false;
                     config.SaveToken = true;
                     config.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = false,
                         ValidateAudience = false,
                         ValidateIssuerSigningKey = true,
                         RequireExpirationTime = false,
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["jwt:key"])),
                         ClockSkew = TimeSpan.Zero

                     }; 
                 });

            services.AddSwaggerGen( config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1", 
                    Title = "UserVoice", 
                    Description = " A user centered review Aggregator", 
                    License = new OpenApiLicense()
                    {
                        Name = "MIT"
                    }, 
                    Contact = new OpenApiContact()
                    {
                        Name = "Pierre Edimo",
                        Email = "pierredimo@live.com"
                    }
                }); 
            }
                ); 

            
            services.AddCors(options => options.AddPolicy("EnableAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            }));

      

         


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DatabaseContext context )
        {
           
            context.Database.EnsureCreated();

            app.UseSwagger();

            app.UseSwaggerUI( config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "userReview"); 
            }
                ); 

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("EnableAll"); 

            app.UseHttpsRedirection();

            app.UseStaticFiles();

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
