using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Core.Entities;
using Store.Data.DAL;
using StoreApi.Admin.Dtos.CategoryDtos;
using StoreApi.Admin.Profiles;
using StoreApi.Client.Profiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddFluentValidation(x=>x.RegisterValidatorsFromAssemblyContaining<CategoryPostDto>());
builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequireNonAlphanumeric = false;
}).AddDefaultTokenProviders().AddEntityFrameworkStores<StoreDbContext>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<StoreDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings:Default").Value));

builder.Services.AddAutoMapper(opt => opt.AddProfile(new AdminMapper()));
builder.Services.AddAutoMapper(opt => opt.AddProfile(new ClientMapper()));




var app = builder.Build();

//dotnet ef --startup-project ../StoreApi migrations add
//dotnet ef --startup-project ../StoreApi database update

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

