using System.Text;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using UserRegisterDynamo;
using UserRegisterDynamo.Repository;
using UserRegisterDynamo.Services;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services.Configure<EmailSettings>(options =>
{
    options.SmtpServer = Environment.GetEnvironmentVariable("EMAIL_SMTP_SERVER");
    options.SmtpPort = int.Parse(Environment.GetEnvironmentVariable("EMAIL_SMTP_PORT"));
    options.Username = Environment.GetEnvironmentVariable("EMAIL_USERNAME");
    options.Password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");
});
var key = Environment.GetEnvironmentVariable("SECRET_KEY");
builder.Services.Configure<SecretSettings>(opts =>
{
    opts.Secret = key;
});

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddAuthentication(opts =>
{
    opts.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
    opts.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opts =>
{
    opts.RequireHttpsMetadata = false;
    opts.SaveToken=true;
    opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey= new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateAudience = false,
    };
}
);
builder.Services.AddTransient<EmailService>();
builder.Services.AddTransient<TokenService>();

builder.Services.AddSingleton<IAmazonDynamoDB, AmazonDynamoDBClient>();
builder.Services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
