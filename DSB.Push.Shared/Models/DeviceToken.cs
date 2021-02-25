using System.ComponentModel.DataAnnotations;

namespace DSB.Push.Shared.Models
{
    /// <summary>
    /// Represents a token identifying a destination device
    /// </summary>
    public struct DeviceToken
    {
        /// <summary>
        /// The token identifying this device
        /// </summary>
        [Required]
        public string Token { get; set; }
    }
}