using BorrowingService;
using BorrowingService.CatalogClient;
using BorrowingService.Features.Borrows.Commands.Create;
using BorrowingService.InventoryClient;
using BorrowingService.UserService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using BorrowingService.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContextPool<BorrowContext>(opt =>
		opt.UseNpgsql(builder.Configuration.GetConnectionString("BorrowContext") ?? throw new InvalidOperationException("Connection string 'BorrowContext' not found.")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.AddScoped<CatalogClient>(serviceProvider =>
{
	var httpClient = serviceProvider.GetRequiredService<System.Net.Http.HttpClient>();
	var baseUrl = "https://catalog-container-app.happyrock-19bd815d.northeurope.azurecontainerapps.io/";
	return new CatalogClient(baseUrl, httpClient);
});
builder.Services.AddScoped<InventoryClient>(serviceProvider =>
{
	var httpClient = serviceProvider.GetRequiredService<System.Net.Http.HttpClient>();
	var baseUrl = "https://inventorymgmt-c-app.happyrock-19bd815d.northeurope.azurecontainerapps.io/";
	return new InventoryClient(baseUrl, httpClient);
});

builder.Services.AddGrpcClient<UserManager.UserManagerClient>(options =>
{
	options.Address = new Uri("https://usermgmt-container-app.happyrock-19bd815d.northeurope.azurecontainerapps.io:443");
});

builder.Services.AddValidatorsFromAssemblyContaining<CreateBorrowCommandValidator>();



var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

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
