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
               opt.UseSqlite("Data Source=UserReviewDb"));

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
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidateIssuerSigningKey = true,
                         RequireExpirationTime = false,
                         ValidIssuer = Configuration["JwtIssuer"],
                         ValidAudience = Configuration["JwtIssuer"],
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                         ClockSkew = TimeSpan.Zero

                     }; 
                 }); 

            
            services.AddCors(options => options.AddPolicy("EnableAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            }));

            services.AddSwaggerDocument( config => {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "UserReviewApi";
                    document.Info.Description = "a user centered review-aggregator"; 
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "Pierre Edimo",
                        Email = "pierreedimo@live.com"
                    };
                    document.Info.License = new NSwag.OpenApiLicense
                    {
                        Name = "MIT"
                    }; 
                }; 
            }); 

         


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DatabaseContext context )
        {
           
            context.Database.EnsureCreated();

           

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("EnableAll"); 

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseOpenApi();

            app.UseSwaggerUi3(); 

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
