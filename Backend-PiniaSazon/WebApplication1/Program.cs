using PiniaSazon.Application.Interfaces;
using PiniaSazon.Application.Services;
using PiniaSazon.Domain.Interfaces;
using PiniaSazon.Infrastructure.Data;
using PiniaSazon.Infrastructure.Repositories;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// servicios al contenedor
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// bd 
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddSingleton(new DatabaseContext(connectionString));

// repositorio
builder.Services.AddScoped<IRecetaRepository, RecetaRepository>();
builder.Services.AddScoped<IIngredienteRepository, IngredienteRepository>();
builder.Services.AddScoped<IComentarioRepository, ComentarioRepository>();
builder.Services.AddScoped<IUnidadMedidaRepository, UnidadMedidaRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();

// servicio
builder.Services.AddScoped<IRecetaService, RecetaService>();
builder.Services.AddScoped<IIngredienteService, IngredienteService>();
builder.Services.AddScoped<IComentarioService, ComentarioService>();
builder.Services.AddScoped<IUnidadMedidaService, UnidadMedidaService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();

//configuación JSON serialixer
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

//cors pa permitir el front a vue
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:8080") //front
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PiniSazon API V1");
    c.RoutePrefix = string.Empty;
});


//activacion cors
app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
