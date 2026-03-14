using Application.Common;
using Infraestructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Domain.AggregateModels.Tickets;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infraestructure.BackgroundJobs
{
    public class DeadlineFailWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<DeadlineFailWorker> _logger;

        public DeadlineFailWorker(IServiceScopeFactory serviceScopeFactory, ILogger<DeadlineFailWorker> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var interval = TimeSpan.FromSeconds(15);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var clock = scope.ServiceProvider.GetRequiredService<IClock>();

                    var now = clock.UtcNow;

                    var overdue = await db.Tickets
                        .Where(
                            t => t.DeadlineAt < now 
                            && (t.Status == TicketStatus.NEW || t.Status == TicketStatus.IN_PROGRESS)
                        ).ToListAsync(stoppingToken);

                    if (overdue.Count > 0)
                    {
                        foreach(var t in overdue)
                        {
                            t.Status = TicketStatus.FAILED;
                            t.Version += 1;
                            t.UpdatedAt = now;
                        }
                        
                        await db.SaveChangesAsync(stoppingToken);
                        _logger.LogInformation("Marked {Count} tickets as failed due to deadline", overdue.Count);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while processing deadline fail worker");
                }

                await Task.Delay(interval, stoppingToken);
            }
        }
    }
}
