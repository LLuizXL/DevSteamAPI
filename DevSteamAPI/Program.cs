using DevSteamAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DevSteamAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));

//Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
);
});



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


//Adicionar o Swagger com o JWT Bearer
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
    {
        new OpenApiSecurityScheme
        {
        Reference = new OpenApiReference
            {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
            },
            Scheme = "oauth2",
            Name = "Bearer",
            In = ParameterLocation.Header,

        },
        new List<string>()
        }
    });
});

// Serviço de EndPoints do Identity Framework
builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
})
    .AddEntityFrameworkStores<DevSteamAPIContext>()
    .AddDefaultTokenProviders(); // Adiocionando o provedor de tokens padrão


//Adicionar Serviço de Autenticação e Autorização

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
//Swagger em ambiente de produção.
app.UseSwagger();
app.UseSwaggerUI();

//Mapear os EndPoints padrão do Identity Framework
app.MapGroup("/Users").MapIdentityApi<IdentityUser>();
app.MapGroup("/Roles").MapIdentityApi<IdentityRole>();

app.UseHttpsRedirection();
//Permitir a autnenticação e autorização de qualquer origem
app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowAll");

app.MapControllers();

app.Run();
