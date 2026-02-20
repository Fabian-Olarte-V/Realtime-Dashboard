using Infraestructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfraestructure();
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(opt =>
    {
        opt.AddPolicy("Frontend", policy =>
        {
            policy
                .WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });

    }
);


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();

app.UseCors("Frontend");
app.MapControllers();
app.Run();
