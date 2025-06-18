using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.Models
{
    public class UsersAccount
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string OtherName { get; set; }
        [StringLength(50)]
        public string Surname { get; set; }
        [StringLength(50)]
        public string Position { get; set; }
        [StringLength(250)]
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
        [StringLength(450)]
        public string ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser Applicationuser { get; set; }
        public string? Longitude { get; set; }
        [StringLength(150)]
        public string? Latitude { get; set; }
        [StringLength(750)]
        public string? IpAddress { get; set; }
        [StringLength(750)]
        public string? Others { get; set; }
    }
}
