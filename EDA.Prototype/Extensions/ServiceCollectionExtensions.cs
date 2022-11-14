using System.Reflection;
using Azure.Messaging.EventHubs.Producer;
using EDA.Prototype.Infrastructure.CQS.Interfaces;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EDA.Prototype.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServiceCollections(this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddControllers();

        // custom functions
        services.AddAzureEventHubProducerClient(configuration);
        services.RegisterImplementations();
    }

    private static void RegisterImplementations(this IServiceCollection services)
    {
        // Question: Why didn't `scan.FromAssemblyDependencies(Assembly.GetExecutingAssembly()) work?`
        services.Scan(scan => scan
            .FromApplicationDependencies(assembly => assembly.FullName?.StartsWith("EDA.Prototype") ?? false)
                .AddClasses(classes => classes
                    .InNamespaces("EDA.Prototype")
                    .AssignableToAny(
                        typeof(IRequestHandler<,>),
                        typeof(ICommandHandler<>)))
                .AsImplementedInterfaces());
    }

    private static void AddAzureEventHubProducerClient(this IServiceCollection services, IConfiguration configuration)
    {
        // TODO:    Maybe a potential cool thing to do would be setting up multiple clients for different EventHub instances,
        //          and therefore allowing messages being sent to different EventHub instances based on some kinds of conditions.
        //          (Not sure if there would be any practice use case though.)
        //
        //          Problem:
        //          To do so, one problem that needs to be solved is to figure out a way to register all **different** "EventHubProducerClient" properly &
        //          a way to access the **correct** "EventHubProducerClient" when used.
        //
        //          Solution:
        //          One potential solution is to create a new data type called "XXXEventHubProducerClient".
        //          (new EventPrinterEventHubProducerClient(namespaceConnStr, "Azure:EdaPrototypeEventHubNamespace:EventPrinter:Name")
        var eventHubNamespaceName = configuration.GetValue<string>("Azure:EdaPrototypeEventHubNamespace:EventHubInstanceName");
        var eventHubNamespaceConnectionStringForSender = configuration.GetValue<string>("Azure:EdaPrototypeEventHubNamespace:ConnectionStringForSender");

        var eventHubProducerClient = new EventHubProducerClient(eventHubNamespaceConnectionStringForSender, eventHubNamespaceName);
        services.TryAddSingleton<EventHubProducerClient>(eventHubProducerClient);
    }
}