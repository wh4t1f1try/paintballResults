using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Paintball.Abstractions.Converters;
using Paintball.Abstractions.Mappers;
using Paintball.Abstractions.Services;
using Paintball.Abstractions.Validators;
using Paintball.Converters;
using Paintball.Database.Abstractions.Repositories;
using Paintball.Database.Contexts;
using Paintball.Database.Repositories;
using Paintball.Mappers;
using Paintball.Services;
using Paintball.Validators;
using PaintballResults.Api.Middleware;


var builder = WebApplication.CreateBuilder(args);
var painballResultsSwaggerEndpoint = "/paintballresults-api/v1/paintballresults-api-documentation.json";
var paintballResultsRouteprefix = "paintballresults-api";
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<ICsvDataStringValidator, CsvDataStringValidator>();
builder.Services.AddScoped<IDuplicatesChecker, DuplicatesChecker>();
builder.Services.AddScoped<IStringToGameResultMapper, StringToGameResultMapper>();
builder.Services.AddScoped<GlobalExceptionHandlerMiddleware>();
builder.Services.AddScoped<IStreamToStringConverter, StreamToStringConverter>();
builder.Services.AddScoped<IGameResultService, GameResultService>();
builder.Services.AddScoped<IImportService, ImportService>();
builder.Services.AddScoped<IGameResultRepository, GameResultRepository>();
builder.Services.AddScoped<IGameResultValidator, GameResultValidator>();
builder.Services.AddScoped<IStringToDataRecordConverter, StringToDataRecordConverter>();
builder.Services.AddScoped<IDataRecordValidator, DataRecordValidator>();
builder.Services.AddDbContext<GameResultContext>(
    options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(
        //    options =>
        //{
        //    options.SwaggerEndpoint(painballResultsSwaggerEndpoint, "version from .json");
        //    options.RoutePrefix = paintballResultsRouteprefix;
        //}
    );
}


app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();