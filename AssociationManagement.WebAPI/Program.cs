using Elastic.Apm.NetCoreAll;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using AssociationManagement.Application;
using AssociationManagement.Core.Entities;
using AssociationManagement.DataAccess;
using AssociationManagement.DataAccess.Persistance;
using AssociationManagement.DataAccess.Persistance.Configurations;
using AssociationManagement.Tools.Logging;
using AssociationManagement.WebAPI.Middlewares.Filters;
using AssociationManagement.WebAPI.Middlewares.GlobalExceptionHandler;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers(options => {
    options.Filters.Add(typeof(CustomValidationActionFilter));
});
builder.Services.Configure<ApiBehaviorOptions>(options => {
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "backend API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
                { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            Array.Empty<string>()
        }
    });
    c.CustomSchemaIds(type => type.ToString());
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidIssuer = "back",

            ValidateAudience = true,
            ValidAudience = "back",

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("17f2507a9757e7a342da035f1f863126e0cfa9f57c0ac75917052a09efefc280")),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorizationBuilder();
builder.Services.AddDataAccess();
builder.Services.AddApplication();

builder.Services.AddIdentityCore<ApplicationUser>()
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddApiEndpoints();

builder.Services.AddCors(
    options => options.AddPolicy(
        "wasm",
        policy => policy
            .WithOrigins(builder.Configuration["WebAPIUrl"]!, builder.Configuration["ClientUrl"]!)
            .AllowAnyMethod()
            .SetIsOriginAllowed(pol => true)
            .AllowAnyHeader()
            .AllowCredentials()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();
var app = builder.Build();
app.MigrateDatabase();
app.MapIdentityApi<ApplicationUser>();
app.UseCors("wasm");
if(app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
var logger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(logger);
var x = app.Configuration.GetSection("ElasticApm");
app.UseAllElasticApm(app.Configuration.GetSection("ElasticApm"));
app.Run();