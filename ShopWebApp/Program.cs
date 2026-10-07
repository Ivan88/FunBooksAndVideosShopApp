using DomainInterfaces;
using Microsoft.AspNetCore.Diagnostics;
using ShopModels.Exceptions;
using ShopRepos;
using ShopReposInterfaces;
using ShopServices;
using ShopServices.Rules;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//Repositories should be scoped. AddSingleton -> to keep changes through requests for test purpose
builder.Services.AddSingleton<ICustomerRepo, CustomerRepo>();
builder.Services.AddSingleton<IOrderRepo, OrderRepo>();
builder.Services.AddSingleton<IProductRepo, ProductRepo>();
builder.Services.AddSingleton<IShippingSlipRepo, ShippingSlipRepo>();

builder.Services.AddScoped<IBusinessRule, MembershipBusinessRule>();
builder.Services.AddScoped<IBusinessRule, ShippingSlipBusinessRule>();
builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseExceptionHandler(exceptionHandlerApp =>
{
	exceptionHandlerApp.Run(async context =>
	{
		var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
		if (exception != null)
		{
			context.Response.StatusCode = exception switch
			{
				EntityNotFoundException => StatusCodes.Status404NotFound,
				OrderAlreadyCreatedException => StatusCodes.Status409Conflict,
				OrderAlreadyContainsProductException => StatusCodes.Status409Conflict,
				_ => context.Response.StatusCode
			};
			await context.Response.WriteAsJsonAsync(new { error = exception.Message });
		}
	});
});

app.UseHttpsRedirection();

app.MapPost("/customers/{customerId:int}/orders", (int customerId, IOrderService orderService) => orderService.CreateOrder(customerId));
app.MapGet("/orders/{orderId:int}", (int orderId, IOrderService orderService) => orderService.GetOrder(orderId));
app.MapPut("/orders/{orderId:int}/products/{productId}", (int orderId, int productId, IOrderService orderService) => orderService.AddProductToOrder(orderId, productId));
app.MapPut("/orders/{orderId:int}", (int orderId, IOrderService orderService) => orderService.ProcessOrder(orderId));

//endpoints to check repos (in real app I rather create dedicated service for each entity
app.MapGet("/customers", (ICustomerRepo repo) => repo.GetCustomers());
app.MapGet("/products", (IProductRepo repo) => repo.GetProducts());
app.MapGet("/shipping-slips", (IShippingSlipRepo repo) => repo.GetShippingSlips());

app.Run();