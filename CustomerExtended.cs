using Sitecore.Commerce.Core;
using System.ComponentModel.DataAnnotations;

namespace Custom.Plugin.Customer.CustomerAttributes.Components
{
    public class CustomerExtended : Component
    {
        [Display(Name = "Subscribe?")]
        public bool ReceiveEmailUpdates { get; set; }
        public string Company { get; set; }
    }
}
