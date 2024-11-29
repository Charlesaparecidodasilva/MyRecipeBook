using MyRecipeBook.API.Filter;
using MyRecipeBook.API.Middleware;
using MyRacipeBook.Application;
using MyRecipeBook.Infrastruture;
using Microsoft.EntityFrameworkCore.Storage;
using MyRecipebook.Domain.Enum;
using MyRecipeBook.Infrastruture.Extensions;
using MyRecipeBook.Infrastruture.Migrations;
using MyRecipeBook.API.Converter;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MyRecipebook.Domain.Security.Tokens;
using MyRecipeBook.API.Token;
;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

                                          //Aqui é para configurar para limpar os espaços da requisição
builder.Services.AddControllers().AddJsonOptions(option => option.JsonSerializerOptions.Converters.Add(new StringConverter()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<IdsFilter>();
   options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
   {
        Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below. 
                      Example: 'Bearer 12345abcdef'",

        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
   });
   options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
           Name  = "Bearer",
           In = ParameterLocation.Header
        },
        new List<string>()
        }
    });


});


//builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastruture(builder.Configuration);

builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();


// isso aqui é para tornar o "/user" do swagger em ninusculo..
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CutureMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

MigrationDataBase();

app.Run();

void MigrationDataBase()
{
    //Se é teste, nao é para executar o que esta em baixo.
    if (builder.Configuration.IsUnitTestEnviroment())
        return;

    var dataBaseType = builder.Configuration.DataBaseType();
    var stringConexao = builder.Configuration.ConnectionString();

    var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

    DatabaseMigration.Migrate(dataBaseType, stringConexao, serviceScope.ServiceProvider);
}

public partial class Program
{

}
