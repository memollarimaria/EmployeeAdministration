using EmployeeAdministration.Helpers;
using EmployeeAdministration.Interfaces;
using EmployeeAdministration.Services;
using Entities.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System;
using EmployeeAdministration.Middlewares;
using Serilog;
using Microsoft.AspNetCore.Identity;
using Entities.Enum;
using EmployeeAdministration.EventBus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<EmployeeAdministrationContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, UserRole>()
 .AddEntityFrameworkStores<EmployeeAdministrationContext>()
 .AddDefaultTokenProviders();


builder.Services.AddLogging(loggingBuilder =>
    loggingBuilder.AddSerilog(dispose: true)
);

//DI
builder.Services.AddScoped<IUser, UserService>();
builder.Services.AddScoped<IProject, ProjectService>();
builder.Services.AddScoped<ITask, TaskService>();
builder.Services.AddSingleton<IRabbitMQ, RabbitMQService>();
builder.Services.AddTransient<UserEvent>();       
builder.Services.AddTransient<ProjectEvent>();       
builder.Services.AddTransient<TaskEvent>();
builder.Services.AddHostedService<RabbitMQListener>(); 



builder.Services.AddHttpContextAccessor();

Log.Logger = new LoggerConfiguration()
	.MinimumLevel.Information()
	.Enrich.FromLogContext()
	.WriteTo.Console()    
	.WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day) 
	.CreateLogger();

builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration.WriteTo.Console();
    loggerConfiguration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddLogging();


builder.Services.AddScoped<Jwt>();

builder.Services.AddSingleton<RabbitMQ.Client.ConnectionFactory>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    return new RabbitMQ.Client.ConnectionFactory()
    {
        HostName = configuration["RabbitMQ:HostName"],
        Port = int.Parse(configuration["RabbitMQ:Port"]),
        UserName = configuration["RabbitMQ:UserName"],
        Password = configuration["RabbitMQ:Password"]
    };
});

builder.Services.AddAuthentication(opt =>
{
	opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(opt =>
{
	opt.RequireHttpsMetadata = false;
	opt.SaveToken = true;
	opt.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
				builder.Configuration.GetSection("AppSettings:Token").Value!)),
		ValidateIssuer = true,
		ValidIssuer = builder.Configuration["JWT:Issuer"],
		ValidateAudience = true,
		ValidAudience = builder.Configuration["JWT:Audience"],
	};
});

builder.Services.AddSwaggerGen(c => {
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "EmployeeAdministration",
		Version = "v1"
	});
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
	{
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer jhfdkj.jkdsakjdsa.jkdsajk\"",
	});
	c.AddSecurityRequirement(new OpenApiSecurityRequirement {
		{
			new OpenApiSecurityScheme {
				Reference = new OpenApiReference {
					Type = ReferenceType.SecurityScheme,
						Id = "Bearer"
				}
			},
			new string[] {}
		}
	});
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}




app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
Log.CloseAndFlush();

