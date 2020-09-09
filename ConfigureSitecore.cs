namespace Custom.Plugin.Customer
{
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.EntityViews;
    using Sitecore.Commerce.Plugin.Customers;
    using Sitecore.Framework.Configuration;
    using Sitecore.Framework.Pipelines.Definitions.Extensions;

    /// <summary>
    /// The configure sitecore class.
    /// </summary>
    public class ConfigureSitecore : IConfigureSitecore
    {
        /// <summary>
        /// The configure services.
        /// </summary>
        /// <param name="services">
        /// The services.
        /// </param>
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);

            services.Sitecore().Pipelines(config =>
            config
                .ConfigurePipeline<IGetEntityViewPipeline>(c =>
                {
                    c.Replace<Sitecore.Commerce.Plugin.Customers.GetCustomerDetailsViewBlock, Custom.Plugin.Customer.CustomerAttributes.Pipelines.Blocks.GetCustomerDetailsViewBlock>();
                })
                .ConfigurePipeline<ITranslateEntityViewToCustomerPipeline>(c =>
                {
                    c.Add<Custom.Plugin.Customer.CustomerAttributes.Pipelines.Blocks.TranslateEntityViewToCustomerBlock>()
                        .After<Sitecore.Commerce.Plugin.Customers.TranslateEntityViewToCustomerBlock>();
                })
                .ConfigurePipeline<IUpdateCustomerDetailsPipeline>(c =>
                {
                    c.Replace<Sitecore.Commerce.Plugin.Customers.UpdateCustomerDetailsBlock,
                        CustomerAttributes.Pipelines.Blocks.UpdateCustomerDetailsBlock>();
                })
                .ConfigurePipeline<IConfigureServiceApiPipeline>(configure => configure.Add<Sitecore.Commerce.Core.ConfigureServiceApiBlock>()));

            services.RegisterAllCommands(assembly);
        }
    }
}