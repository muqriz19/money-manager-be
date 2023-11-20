using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using moneyManagerBE.Data;
using moneyManagerBE.Services.Accounts;
using moneyManagerBE.Services.Authorization;
using moneyManagerBE.Services.Categories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Description = "Bearer Authentication with JWT Token",
            Type = SecuritySchemeType.Http
        }
    );

    config.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

string myOrigin = "myOrigin";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myOrigin, policy =>
    {
        policy.WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// register my Service/IServices
builder.Services.AddScoped<IAccountsService, AccountsService>();
builder.Services.AddScoped<ICategoriesServices, CategoriesService>();
builder.Services.AddScoped<IAuthorization, AuthorizationService>();
// add auto mapper
// but how does it get the AutoMapperProfile.cs to run and map
builder.Services.AddAutoMapper(typeof(Program).Assembly);
// builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

string jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
string jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

Console.WriteLine(jwtIssuer);
Console.WriteLine(jwtKey);


// add jwt token authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});



// register db context
// connect to services
// ** older implementation **
// builder.Services.AddEntityFrameworkNpgsql().AddDbContext<AppDbContext>(options => {
//     options.UseNpgsql(builder.Configuration.GetConnectionString("moneyManagerDB"));
// });

// ** newer implementation **
//https://stackoverflow.com/questions/62917136/addentityframework-was-called-on-the-service-provider-but-useinternalservic
//https://www.npgsql.org/efcore/api/Microsoft.Extensions.DependencyInjection.NpgsqlServiceCollectionExtensions.html#Microsoft_Extensions_DependencyInjection_NpgsqlServiceCollectionExtensions_AddEntityFrameworkNpgsql_IServiceCollection_
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("moneyManagerDB"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(myOrigin);

app.UseHttpsRedirection();

// apply for jwt token
app.UseAuthentication();


// apply cors

app.UseAuthorization();

app.MapControllers();

app.Run();
