
using System.ComponentModel.DataAnnotations;
using VisionBoard.Utils;

namespace VisionBoard.Models
{
    public partial class Measurement
    {
        public int Id { get; set; }
        public string Type { get; set; }

        [Required(ErrorMessage = AppConstants.CurrentValueRequiredMessage)]
        [Range(0, 40000000, ErrorMessage = AppConstants.CurrentValueLengthMessage)]
        public int CurrentValue { get; set; }

        [Required(ErrorMessage = AppConstants.TotalValueRequiredMessage)]
        [Range(1, 40000000, ErrorMessage = AppConstants.TotalValueLengthMessage)]
        public int TotalValue { get; set; }

        [Required(ErrorMessage = AppConstants.UnitRequiredMessage)]
        [MaxLength(50, ErrorMessage = AppConstants.UnitLengthMessage)]
        public string Unit { get; set; }

        public virtual Goal Goal { get; set; }
    }
}
