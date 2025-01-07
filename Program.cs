using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using DotNetEnv;
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

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddTransient<EmailService>();

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
app.UseAuthorization();
app.MapControllers();

app.Run();
