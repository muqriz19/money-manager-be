using Microsoft.EntityFrameworkCore;
using moneyManagerBE.Data;
using moneyManagerBE.Services.Accounts;
using moneyManagerBE.Services.Categories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// apply cors

app.UseAuthorization();

app.MapControllers();

app.Run();
