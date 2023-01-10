using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Store.Core.Entities;
using Store.Data.DAL;
using StoreApi.Admin.Dtos.CategoryDtos;
using StoreApi.Admin.Profiles;
using StoreApi.Client.Profiles;
using Swashbuckle.AspNetCore.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddFluentValidation(x=>x.RegisterValidatorsFromAssemblyContaining<CategoryPostDto>());
builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequireNonAlphanumeric = false;
}).AddDefaultTokenProviders().AddEntityFrameworkStores<StoreDbContext>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("admin", new OpenApiInfo
    {
        Title = "StoreApp Admin Api",
        Version = "V1",
    });
    opt.SwaggerDoc("users", new OpenApiInfo
    {
        Title = "StoreApp Users Api",
        Version = "v1",
    });

    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In=ParameterLocation.Header,
        Description="Please enter a valid token",
        Name="Authorization",
        Type=SecuritySchemeType.Http,
        BearerFormat="JWT",
        Scheme="Bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
    opt.AddFluentValidationRules();
});

builder.Services.AddDbContext<StoreDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings:Default").Value));
builder.Services.AddHttpContextAccessor();


builder.Services.AddSingleton(provider => new MapperConfiguration(config =>
{
    config.AddProfile(new AdminMapper(provider.GetService<IHttpContextAccessor>()));
}).CreateMapper());
builder.Services.AddAutoMapper(opt => opt.AddProfile(new ClientMapper()));

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidAudience = builder.Configuration.GetSection("JWT:audience").Value,
        ValidIssuer = builder.Configuration.GetSection("JWT:issuer").Value,
        IssuerSigningKey=new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT:secret").Value)),

    };
});


var app = builder.Build();

//dotnet ef --startup-project ../StoreApi migrations add
//dotnet ef --startup-project ../StoreApi database update

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/swagger/admin/swagger.json", "Admin Api");
        opt.SwaggerEndpoint("/swagger/users/swagger.json", "Client Api");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();

