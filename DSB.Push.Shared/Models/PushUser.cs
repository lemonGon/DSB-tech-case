using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DSB.Push.Shared.Models
{
    /// <summary>
    /// A user who can receive push notifications
    /// </summary>
    public struct PushUser
    {
        /// <summary>
        /// The unique identifier for this user
        /// </summary>
        [Required]
        public string Id { get; set; }
        /// <summary>
        /// The registered device tokens for this user, all of which can receive push notifications
        /// </summary>
        public List<DeviceToken>DeviceTokens { get; set;  }
    }
}