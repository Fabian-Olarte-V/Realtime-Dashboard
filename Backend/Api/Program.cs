using Infraestructure;
using Infraestructure.Persistance;
using Infraestructure.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfraestructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(opt =>
    {
        opt.AddPolicy("Frontend", policy =>
        {
            policy
                .AllowAnyHeader()
                .AllowAnyMethod();
        });

    }
);


var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(db);
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
