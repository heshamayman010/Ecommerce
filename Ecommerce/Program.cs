using Ecommerce.Controllers.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Security.Principal;
using System.Text;

namespace Ecommerce
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // to configure the appdbcontext 
            builder.Services.AddDbContext<AppDbContext>(
                options =>
                {

                    options.UseSqlServer("Server = .\\SQLEXPRESS; Database = Ecommerce; Integrated Security = SSPI; TrustServerCertificate =True; ");

                }

                ); ;

            //____________________________________________________________________________________


            // to enable the m.ef.identity on the appuser and the dbcontext 
            //_______________________________________________________________________________________
            builder.Services.AddIdentity<Appuser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
            //____________________________________________________________________________________


            // to enable the authentication of the token using the jwt 
            // ___________________________________________________________________________

            builder.Services.AddAuthentication(options =>
            {

                // to check if the token is valid or not 
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                // to check if you are not valid to redirect you again to the login form 
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }


            // and here we will check for the specified token parameters to be able to use it 
            ).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {

                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["jwt:audiance"],
                    ValidIssuer = builder.Configuration["jwt:issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:Secret"]))
                };
            });
            //__________________________________________________________________________________________

            // to modify the cors options 
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("firstpolicy", x => { x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });


            });
            // _________________________________________________________________________________




            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            //________________________________________________________________________________________

            // Register Swagger for v1
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ecommerce v1", Version = "v1" });
            });

            // Register Swagger for v2
            builder.Services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v2", new OpenApiInfo
                {
                    Version = "v2",
                    Title = "Ecommerce v2",
                    Description = "Ecommerce"
                });

                // To Enable authorization using Swagger (JWT)
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\""
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });


            //___________________________________________________________________________


            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecommerce v1");
                    c.SwaggerEndpoint("/swagger/v2/swagger.json", "Ecommerce v2");
                });
                ;
            }

            app.UseStaticFiles();
            app.UseCors("firstpolicy");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}















