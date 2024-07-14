using DotnetHomework.Api.Formatters;
using DotnetHomework.Api.Repository;
using DotnetHomework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.RespectBrowserAcceptHeader = true;
    //options.FormatterMappings.SetMediaTypeMappingForFormat("txt", "text/plain");
    options.OutputFormatters.Add(new CsvOutputFormatter());
})
    .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        })
    .AddXmlSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Register the Swagger generator
builder.Services.AddSwaggerGen(c =>
 {
     c.SwaggerDoc("v1", new OpenApiInfo { Title = "Homework API", Version = "v1" });
 });

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

var databaseType = builder.Configuration.GetValue<string>("DatabaseType");

//Set connection string and repository based on configuration
switch (databaseType)
{
    case "SqlServer":
        builder.Services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped(typeof(IDocumentRepository<>), typeof(SqlDocumentRepository<>));
        break;
    case "SQLite":
        builder.Services.AddDbContext<DataContext>(options =>
            options.UseSqlite("Data Source=app.db"));
        builder.Services.AddScoped(typeof(IDocumentRepository<>), typeof(SqlDocumentRepository<>));
        break;
    case "InMemoryDb":
    default:
        builder.Services.AddDbContext<DataContext>(options =>
            options.UseInMemoryDatabase("InMemoryDb"));
        builder.Services.AddScoped(typeof(IDocumentRepository<>), typeof(SqlDocumentRepository<>));
        break;
}

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
