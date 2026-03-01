var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SQL Connection 
builder.Services.AddSingleton<IDBConnectionFactory>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");

    return new SqlConnectionFactory(connectionString!);
});

// Register Services 
builder.Services.AddScoped<RepoRepository>();
builder.Services.AddScoped<IRepositoryService,RepositoryService>();

// 3rd party services
var githubBaseUrl = builder.Configuration.GetSection("Github");
builder.Services.AddHttpClient<IGitHubService, GitHubService>(client =>
{
    client.BaseAddress = new Uri (githubBaseUrl["BaseUrl"]!);
    client.DefaultRequestHeaders.Add("User-Agent", "LiquidLabsHarith");
    client.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

