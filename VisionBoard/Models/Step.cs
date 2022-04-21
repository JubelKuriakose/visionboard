using System;
using System.ComponentModel.DataAnnotations;
using VisionBoard.Utils;

#nullable disable

namespace VisionBoard.Models
{
    public partial class Step
    {
        public int Id { get; set; }

        [Display(Name = "Step Name")]
        [Required(ErrorMessage = AppConstants.NameRequiredMessage)]
        [MaxLength(50, ErrorMessage = AppConstants.NameLengthMessage)]
        public string Name { get; set; }

        [MaxLength(600, ErrorMessage = AppConstants.DescriptionLengthMessage)]
        public string Description { get; set; }

        [Required(ErrorMessage = AppConstants.WeightRequiredMessage)]
        [Range(1, 40000000, ErrorMessage = AppConstants.WeightLengthMessage)]
        public int Weight { get; set; }

        public DateTime? DueDate { get; set; }
        public bool Status { get; set; }

        [Display(Name = "Goal Name")]
        public int GoalId { get; set; }

        public virtual Goal Goal { get; set; }
    }
}
