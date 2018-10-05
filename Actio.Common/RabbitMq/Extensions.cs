using System.Reflection;
using System.Threading.Tasks;
using Actio.Common.Commands;
using Actio.Common.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit.Instantiation;
using RawRabbit;

namespace Actio.Common.RabbitMq
{
    public static class Extensions
    {

        //Code from Igo
        public static Task WithCommandHandlerAsync<TCommand>(this IBusClient bus,
            ICommandHandler<TCommand> handler) where TCommand : ICommand
            => bus.SubscribeAsync<TCommand>(async msg =>
                {
                    await handler.HandleAsync(msg);
                },
                ctx => ctx.UseSubscribeConfiguration(cfg
                    => cfg.FromDeclaredQueue(q
                        => q.WithName(GetQueueName<TCommand>()))));

        public static Task WithEventHandlerAsync<TEvent>(this IBusClient bus, IEventHandler<TEvent> handler) where TEvent : IEvent
            => bus.SubscribeAsync<TEvent>(msg
                    => handler.HandleAsync(msg),
                ctx => ctx.UseSubscribeConfiguration(cfg
                    => cfg.FromDeclaredQueue(q
                        => q.WithName(GetQueueName<TEvent>()))));

        private static string GetQueueName<T>()
            => $"{Assembly.GetEntryAssembly().GetName()}/{typeof(T).Name}";

        
        public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            var opt = new RabbitMqOptions();
            var section = configuration.GetSection("rabbitmq");
            section.Bind(opt);
            var client = RawRabbitFactory.CreateSingleton(new RawRabbitOptions
            {
                ClientConfiguration = opt
            });
            services.AddSingleton<IBusClient>(s => client);
        }


        //public static Task WithEventHandlerAsync<TEvent>(this IBusClient bus,
        //    IEventHandler<TEvent> handler) where TEvent : IEvent
        //    => bus.SubscribeAsync<TEvent>(msg => handler.HandleAsync(msg),
        //        ctx => ctx.UseConsumeConfiguration(cfg =>
        //            cfg.FromQueue(GetQueueName<TEvent>())));

        //public static Task WithCommandHandlerAsync<TCommand>(this IBusClient bus,
        //    ICommandHandler<TCommand> handler) where TCommand : ICommand
        //    => bus.SubscribeAsync<TCommand>(msg => handler.HandleAsync(msg),
        //        ctx => ctx.UseConsumeConfiguration(cfg =>
        //            cfg.FromQueue(GetQueueName<TCommand>())));


        //private static string GetQueueName<T>()
        //    => $"{Assembly.GetEntryAssembly().GetName()}/{typeof(T).Name}";

        //public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
        //{
        //    var options = new RabbitMqOptions();
        //    var section = configuration.GetSection("rabbitmq");
        //    section.Bind(options);
        //    var client = RawRabbitFactory.CreateSingleton(new RawRabbitOptions
        //    {
        //        ClientConfiguration = options
        //    });
        //    services.AddSingleton<IBusClient>(_ => client);
        //}
    }
}
