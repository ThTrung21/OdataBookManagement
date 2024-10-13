using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using ODataAPI.Models;
using ODataAPI;

using ODataAPI.Models;
using Microsoft.AspNetCore.OData.Routing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(option =>
{
    option.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Add services to the container.
builder.Services.AddControllers()
    .AddOData(opt => opt.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100)
    .AddRouteComponents("odata", GetEdmModel()));

// Register the in-memory database
builder.Services.AddDbContext<BookDbContext>(opt => opt.UseInMemoryDatabase("BookDb"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Ensure CORS is applied before routing
app.UseCors("AllowAll");

app.UseRouting();

app.Use(next => context =>
{
    var endpoint = context.GetEndpoint();
    if (endpoint == null)
        return next(context);
    IEnumerable<string> templates;
    IODataRoutingMetadata metadata = endpoint.Metadata.GetMetadata<IODataRoutingMetadata>();
    if (metadata != null)
    {
        templates = metadata.Template.GetTemplates();
    }
    return next(context);
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

static IEdmModel GetEdmModel()
{
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<EDM.Book>("Books");
    builder.EntitySet<EDM.Press>("Presses");
    return builder.GetEdmModel();
}

// Seed database
using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<BookDbContext>();
context.Database.EnsureCreated(); // This should trigger the seeding
