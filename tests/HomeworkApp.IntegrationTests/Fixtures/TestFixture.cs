using FluentMigrator.Runner;
using HomeworkApp.Dal.Extensions;
using HomeworkApp.Dal.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HomeworkApp.IntegrationTests.Fixtures
{
    public class TestFixture
    {
        public IUserRepository UserRepository { get; }
        
        public ITaskRepository TaskRepository { get; }
        
        public ITaskLogRepository TaskLogRepository { get; }
        
        public ITaskCommentRepository TaskCommentRepository { get; }
        
        public ITakenTaskRepository TakenTaskRepository { get; }
        
        public IRecentTaskCommentRepository RecentTaskCommentRepository { get; }
        
        public IUserScheduleRepository UserScheduleRepository { get; }
        
        public IUserRateLimitRepository UserRateLimitRepository { get; }

        public TestFixture()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddDalInfrastructure(config)
                        .AddDalRepositories();
                })
                .Build();
            
            ClearDatabase(host);
            host.MigrateUp();

            var serviceProvider = host.Services;
            UserRepository = serviceProvider.GetRequiredService<IUserRepository>();
            TaskRepository = serviceProvider.GetRequiredService<ITaskRepository>();
            TaskLogRepository = serviceProvider.GetRequiredService<ITaskLogRepository>();
            TaskCommentRepository = serviceProvider.GetRequiredService<ITaskCommentRepository>();
            TakenTaskRepository = serviceProvider.GetRequiredService<ITakenTaskRepository>();
            RecentTaskCommentRepository = serviceProvider.GetRequiredService<IRecentTaskCommentRepository>();
            UserScheduleRepository = serviceProvider.GetRequiredService<IUserScheduleRepository>();
            UserRateLimitRepository = serviceProvider.GetRequiredService<IUserRateLimitRepository>();
            
            FluentAssertionOptions.UseDefaultPrecision();
        }

        private static void ClearDatabase(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateDown(0);
        }
    }
}