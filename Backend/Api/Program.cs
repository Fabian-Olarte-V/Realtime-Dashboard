using Api.Filters;
using Api.Middlewares;
using Application.DependencyInjection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Infraestructure.BackgroundJobs;
using Infraestructure.Persistance;
using Infrastructure.DependencyInjection;
using Infrastructure.Persistance.Seed;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new InfraestructureModule()));
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new ApplicationModule()));

builder.Services.AddApplication();
builder.Services.AddInfraestructure(builder.Configuration);

builder.Services.AddScoped<SuccessResponseResultFilter>();
builder.Services.AddScoped<ExceptionHandlingMiddleware>();

builder.Services.AddHostedService<DeadlineFailWorker>();
builder.Services.AddMemoryCache();
builder.Services.AddSwaggerGen();


builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<SuccessResponseResultFilter>();
})
.AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});


builder.Services.AddCors(opt =>
    {
        opt.AddPolicy("AllowedFrontendOrigins", policy =>
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
app.UseCors("AllowedFrontendOrigins");

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();
app.Run();
