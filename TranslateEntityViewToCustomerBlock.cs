using Custom.Plugin.Customer.CustomerAttributes.Components;
using Custom.Plugin.Customer.CustomerAttributes;
using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Customers;
using Sitecore.Framework.Pipelines;
using System;
using System.Threading.Tasks;

namespace Custom.Plugin.Customer.CustomerAttributes.Pipelines.Blocks
{
    [PipelineDisplayName(CustomerAttributesConstants.Pipelines.Blocks.TranslateEntityViewToCustomerBlock)]
    public class TranslateEntityViewToCustomerBlock : PipelineBlock<Sitecore.Commerce.Plugin.Customers.Customer, Sitecore.Commerce.Plugin.Customers.Customer, CommercePipelineExecutionContext>
    {
        public override Task<Sitecore.Commerce.Plugin.Customers.Customer> Run(Sitecore.Commerce.Plugin.Customers.Customer customer, CommercePipelineExecutionContext context)
        {
            if (customer == null || !customer.HasComponent<CustomerDetailsComponent>())
            {
                return Task.FromResult(customer);
            }
            var details = customer.GetComponent<CustomerDetailsComponent>();
            var customDetails = new CustomerExtended();
            foreach (EntityView view in details.View.ChildViews)
            {
                foreach (ViewProperty viewProperty in view.Properties)
                {
                    if (viewProperty.Name == nameof(CustomerExtended.ReceiveEmailUpdates))
                    {
                        customDetails.ReceiveEmailUpdates = Convert.ToBoolean(view.GetPropertyValue(viewProperty.Name));
                    }
                    if (viewProperty.Name == nameof(CustomerExtended.Company))
                    {
                        customDetails.Company = view.GetPropertyValue(viewProperty.Name)?.ToString();
                    }
                }
            }
            context.Logger.LogDebug($"Executing TranslateEntityViewToCustomerBlock for customer special attributes");
            customer.SetComponent(customDetails);
            return Task.FromResult(customer);
        }
    }
}
