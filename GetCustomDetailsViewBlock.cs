using Custom.Plugin.Customer.CustomerAttributes.Components;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Framework.Pipelines;
using Sitecore.Commerce.Plugin.Customers;
using System.Threading.Tasks;
using LX.Plugin.Customer.CustomerAttributes;

namespace Custom.Plugin.Customer.CustomerAttributes.Pipelines.Blocks
{
    [PipelineDisplayName(CustomerAttributesConstants.Pipelines.Blocks.GetCustomDetailsViewBlock)]
    public class GetCustomerDetailsViewBlock : Sitecore.Commerce.Plugin.Customers.GetCustomerDetailsViewBlock
    {
        public GetCustomerDetailsViewBlock(IGetLocalizedCustomerStatusPipeline getLocalizedCustomerStatusPipeline) : base(getLocalizedCustomerStatusPipeline)
        {
        }

        protected override async Task PopulateDetails(EntityView view, Sitecore.Commerce.Plugin.Customers.Customer customer, bool isAddAction, bool isEditAction, CommercePipelineExecutionContext context)
        {
            await base.PopulateDetails(view, customer, isAddAction, isEditAction, context);

            if (customer == null)
            {
                return;
            }
            var details = customer.GetComponent<CustomerExtended>();
            view.Properties.Add(new ViewProperty
            {
                Name = nameof(CustomerExtended.Company),
                IsRequired = false,
                RawValue = details?.Company,
                IsReadOnly = !isEditAction && !isAddAction
            });

            view.Properties.Add(new ViewProperty
            {
                Name = nameof(CustomerExtended.ReceiveEmailUpdates),
                DisplayName = "Subscribe?",
                IsRequired = false,
                RawValue = details?.ReceiveEmailUpdates,
                IsReadOnly = !isEditAction && !isAddAction
            });
        }
    }
}
