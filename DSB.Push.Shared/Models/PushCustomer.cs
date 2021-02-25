using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DSB.Push.Shared.Models
{
    /// <summary>
    /// A user who can receive push notifications
    /// </summary>
    public struct PushCustomer
    {
        /// <summary>
        /// The unique identifier for this customer
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// The registered device tokens for this customer, all of which can receive push notifications
        /// </summary>
        public List<DeviceToken>DeviceTokens { get; set;  }
    }
}