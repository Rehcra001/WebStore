using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebStore.API.LoginData;
using WebStore.API.Services;
using WebStore.API.Services.Contracts;
using WebStore.Repository;
using WebStore.Repository.Contracts;
using WebStore.Repository.Repositories.ADO;
using WebStore.Repository.Repositories.Dapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddDbContext<LoginDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("WebStoreLoginDB")));

builder.Services.AddDefaultIdentity<IdentityUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<LoginDbContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Repositories
builder.Services.AddScoped<IRelationalDatabaseConnection, RelationalDatabaseConnection>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepositoryADO>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductRepository, ProductRepositoryADO>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepositoryDapper>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepositoryDapper>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IOrderRepository, OrderRepositoryDapper>();
builder.Services.AddScoped<IOrderServices, OrderServices>();

var app = builder.Build();

RoleManager<IdentityRole> roleManager = builder.Services.BuildServiceProvider().GetService<RoleManager<IdentityRole>>();
UserManager<IdentityUser> userManager = builder.Services.BuildServiceProvider().GetService<UserManager<IdentityUser>>();
RoleManager<IdentityRole> customerRoleManager = builder.Services.BuildServiceProvider().GetService<RoleManager<IdentityRole>>();
await SeedAdministratorRoleAndUser.Seed(roleManager, userManager, customerRoleManager);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
