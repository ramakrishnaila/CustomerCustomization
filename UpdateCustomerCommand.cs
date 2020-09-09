using Custom.Plugin.Customer.CustomerAttributes.Components;
using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.EntityViews.Commands;
using Sitecore.Commerce.Plugin.Customers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Custom.Plugin.Customer.CustomerAttributes.Commands
{
    public class UpdateCustomerCommand : CommerceCommand
    {
        private readonly GetCustomerCommand _getCustomerCommand;
        private readonly GetEntityViewCommand _getEntityViewCommand;
        private readonly DoActionCommand _doActionCommand;
        private readonly PersistEntityPipeline _persistEntityPipeline;
        private readonly FindEntityCommand _findEntityCommand;

        public UpdateCustomerCommand(
            PersistEntityPipeline persistEntityPipeline,
            FindEntityCommand findEntityCommand,
        IServiceProvider serviceProvider,
            GetCustomerCommand getCustomerCommand,
            GetEntityViewCommand getEntityViewCommand,
            DoActionCommand doActionCommand)
            : base(serviceProvider)
        {
            this._getCustomerCommand = getCustomerCommand;
            this._getEntityViewCommand = getEntityViewCommand;
            this._doActionCommand = doActionCommand;
            this._persistEntityPipeline = persistEntityPipeline;
            this._findEntityCommand = findEntityCommand;
        }

        public virtual async Task<Sitecore.Commerce.Plugin.Customers.Customer> Process(CommerceContext commerceContext,string customerID, bool subscribeEmail , string company)
        {
            try
            {
                var customerEntity = await _getCustomerCommand.Process(commerceContext, customerID);
                if (customerEntity != null)
                {
                    var customerEntityView = await _getEntityViewCommand.Process(commerceContext, customerEntity.Id, "Master", "", "");
                    var composerEditView = customerEntityView.ChildViews.Where(x => x.Name == "Details").FirstOrDefault() as EntityView;
                    if (composerEditView != null)
                    {
                        commerceContext.Logger.LogDebug($"Edit customer detail view loaded: {DateTime.Now}");
                        var updatedCustomerEntity = await _findEntityCommand.Process(commerceContext, typeof(Sitecore.Commerce.Plugin.Customers.Customer), composerEditView.EntityId) as Sitecore.Commerce.Plugin.Customers.Customer;
                        var customDetails = new CustomerExtended();
                        customDetails.ReceiveEmailUpdates = Convert.ToBoolean(subscribeEmail);
                        customDetails.Company = company;
                        updatedCustomerEntity.SetComponent(customDetails);
                        await this._persistEntityPipeline.Run(new PersistEntityArgument(updatedCustomerEntity), commerceContext.PipelineContext);
                    }
                    else
                    {
                        commerceContext.Logger.LogInformation($"Edit customer entityview is empty: {DateTime.Now}");
                    }
                }
               else
                {
                    commerceContext.Logger.LogInformation($"Customer entityview is null: {DateTime.Now}");
                }
                return customerEntity;
            }
            catch (Exception e)
            {
                commerceContext.Logger.LogInformation($"Exception occured in getting customer { e.StackTrace} and id is {e.Message}");
                return await Task.FromException<Sitecore.Commerce.Plugin.Customers.Customer>(e);
            }
        }
    }
}
