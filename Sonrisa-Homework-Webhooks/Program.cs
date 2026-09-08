using Sonrisa_Homework_Webhooks.Repositories;
using Sonrisa_Homework_Webhooks.Services;
using Sonrisa_Homework_Webhooks.Services.AlertEvaluators;
using Sonrisa_Homework_Webhooks.Services.NotificationDispatchers;

namespace Sonrisa_Homework_Webhooks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSingleton<IAlertRepository, InMemoryAlertRepository>();
            builder.Services.AddSingleton<IEventRepository, InMemoryEventRepository>();

            builder.Services.AddSingleton<IMessageQueue, MessageQueue>();

            builder.Services.AddSingleton<IAlertEngine, AlertEngine>();

            builder.Services.AddSingleton<IAlertEvaluator, MarketMovementAlertEvaluator>();

            builder.Services.AddSingleton<INotificationDispatcher, NotificationDispatcher>();

            builder.Services.AddHostedService<NotificationWorker>();

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
