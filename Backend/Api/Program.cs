using Autofac;
using Autofac.Extensions.DependencyInjection;
using Infraestructure.BackgroundJobs;
using Infraestructure.Persistance;
using Infrastructure.DependencyInjection;
using Infrastructure.Persistance.Seed;
using Application.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new InfraestructureModule()));
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new ApplicationModule()));

builder.Services.AddApplication();
builder.Services.AddInfraestructure(builder.Configuration);

builder.Services.AddHostedService<DeadlineFailWorker>();
builder.Services.AddMemoryCache();
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

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
