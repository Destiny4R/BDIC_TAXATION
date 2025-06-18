using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.ViewModels
{
    public class FileReturnVM
    {
        [Display(Name = "Income Year")]
        [Required]
        public double IncomeAmount { get; set; }
        [StringLength(250)]
        [Display(Name = "Asset Name")]
        [Required]
        public string AssetName { get; set; }
        [StringLength(20)]
        [Display(Name = "Return Year")]
        [Required]
        public string SessionYear { get; set; }
        [Display(Name = " Select Taxable Asset")]
        [Required]
        public string AssetTaxable { get; set; }
    }
}
