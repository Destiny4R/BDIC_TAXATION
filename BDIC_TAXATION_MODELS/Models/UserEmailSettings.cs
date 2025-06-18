using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.Models
{
    public class UserEmailSettings
    {
        [Key]
        public int Id { get; set; }
        [StringLength(450)]
        public string ApplicationuserId { get; set; }
        [ForeignKey(nameof(ApplicationuserId))]
        public ApplicationUser Applicationuser { get; set; }
        public string? PreferredLanguage { get; set; }
        public bool EnableEmailNotifications { get; set; }
        public bool EnableSMSNotifications { get; set; }
        public bool EnableInAppNotifications { get; set; }
    }
}
