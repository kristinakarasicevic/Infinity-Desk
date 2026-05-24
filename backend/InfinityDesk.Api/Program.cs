using InfinityDesk.Api.Data;
using InfinityDesk.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi(); 
builder.Services.AddControllers(); //dodaje potrebne servise za rad sa kontrolerima
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<TokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseStaticFiles(); //dozvoli direktan pristup fajlovima iz wwwroot foldera preko URL-a
app.UseHttpsRedirection();
app.MapControllers();
app.Run();

