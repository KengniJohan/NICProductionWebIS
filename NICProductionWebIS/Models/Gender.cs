using System.ComponentModel.DataAnnotations;

namespace NICProductionWebIS.Models
{
    public enum Gender
    {
        [Display(Name ="Masculin")]
        Male = 'M',
        [Display(Name = "Féminin")]
        Female = 'F'
    }
}
