using Vendor.Api;
using Vendor.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("VendorLoaderSettings.json", optional: false, reloadOnChange: true);

builder.Services.AddControllers()
    .AddApplicationPart(typeof(VendorController).Assembly);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterServices(builder.Configuration);

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