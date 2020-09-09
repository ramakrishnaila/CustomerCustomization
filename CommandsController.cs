using Custom.Plugin.Customer.CustomerAttributes.Commands;
using Microsoft.AspNetCore.Mvc;
using Sitecore.Commerce.Core;
using System;
using System.Threading.Tasks;
using System.Web.Http.OData;


namespace Custom.Plugin.Customer.CustomerAttributes.Controllers
{
  
    /// <inheritdoc />
    /// <summary>
    /// Defines a controller
    /// </summary>
    /// <seealso cref="T:Sitecore.Commerce.Core.CommerceController" />
    public class CommandsController : CommerceController
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sitecore.Commerce.Plugin.Sample.CommandsController" /> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="globalEnvironment">The global environment.</param>
        public CommandsController(IServiceProvider serviceProvider, CommerceEnvironment globalEnvironment)
            : base(serviceProvider, globalEnvironment)
        {
        }

        [HttpPut]
        [Route("UpdateCustomer()")]
        public async Task<IActionResult> UpdateCustomer([FromBody] ODataActionParameters value)
        {
            var customerId = Convert.ToString(value["customerID"]);
            var subscribeEmail = Convert.ToString(value["subscribeEmail"]);
            var company = Convert.ToString(value["company"]);
            var setGenericCustomerAttributeCommand = this.Command<UpdateCustomerCommand>();
            await setGenericCustomerAttributeCommand.Process(this.CurrentContext, customerId,Convert.ToBoolean(subscribeEmail) , company);
            return new ObjectResult(setGenericCustomerAttributeCommand);
        }
    }
}
