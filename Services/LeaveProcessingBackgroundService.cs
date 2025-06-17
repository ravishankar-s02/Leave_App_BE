using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

public class LeaveProcessingBackgroundService : BackgroundService
{
    private readonly ILogger<LeaveProcessingBackgroundService> _logger;
    private readonly IConfiguration _configuration;

    public LeaveProcessingBackgroundService(ILogger<LeaveProcessingBackgroundService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Running automatic leave processing at: {time}", DateTimeOffset.Now);

            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                using var cmd = new SqlCommand("sp_AutoMoveScheduledToTaken", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                await conn.OpenAsync(stoppingToken);
                await cmd.ExecuteNonQueryAsync(stoppingToken);

                _logger.LogInformation("Leave processing executed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing leaves.");
            }

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // Runs every 24 hours
        }
    }
}
