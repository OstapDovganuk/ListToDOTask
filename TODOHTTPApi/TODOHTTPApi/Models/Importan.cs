using System.ComponentModel.DataAnnotations;

namespace TODOHTTPApi.Models
{
   public enum Importance
    {
        [Display(Name = "low")]
        low,
        [Display(Name = "normal")]
        normal,
        [Display(Name = "high")]
        high
    }
}