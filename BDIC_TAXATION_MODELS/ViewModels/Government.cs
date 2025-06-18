using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.ViewModels
{
    public class Government
    {
        public int? Id { get; set; }
        [MaxLength(50), Required]        
        public string Name { get; set; }
        public int? Code { get; set; }

    }
}
