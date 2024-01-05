using WebStore.Repository;
using WebStore.Repository.Contracts;
using WebStore.Repository.Repositories.ADO;
using WebStore.Repository.Repositories.Dapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Repositories
builder.Services.AddScoped<IRelationalDatabaseConnection, RelationalDatabaseConnection>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepositoryDapper>();

var app = builder.Build();

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
