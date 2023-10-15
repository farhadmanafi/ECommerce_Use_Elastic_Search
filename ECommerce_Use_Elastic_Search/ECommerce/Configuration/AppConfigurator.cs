using Nest;

namespace ECommerce.Configuration
{
    public static class AppConfigurator
    {
        public static void ConfigElastic(IServiceCollection serviceCollection, HostBuilderContext context)
        {
            var config = context.Configuration.GetSection("ElasticDbConfiguration").Get<ElasticConfiguration>();

            var elasticSettings = new ConnectionSettings(new Uri(config.Server))
                .BasicAuthentication(config.UserName, config.Password)
                .DefaultIndex(config.DefaultIndexName); // Elasticsearch index name
            
            var elasticClient = new ElasticClient(elasticSettings);

            serviceCollection.AddSingleton<IElasticClient>(elasticClient);
        }
    }
}
