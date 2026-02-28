var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SQL Connection 
builder.Services.AddSingleton<IDBConnectionFactory, SqlConnectionFactory>();

// 3rd party services
builder.Services.AddHttpClient<IGitHubService, GitHubService>(client =>
{
    client.BaseAddress = new Uri ("https://api.github.com/");
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

app.Run();

