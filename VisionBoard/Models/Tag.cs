using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VisionBoard.Utils;

#nullable disable

namespace VisionBoard.Models
{
    public partial class Tag
    {
        public Tag()
        {
            GoalTags = new HashSet<GoalTags>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = AppConstants.NameRequiredMessage)]
        [MaxLength(50, ErrorMessage = AppConstants.NameLengthMessage)]
        public string Name { get; set; }

        [Required(ErrorMessage = AppConstants.ColourRequiredMessage)]
        public string Colour { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<GoalTags> GoalTags { get; set; }
    }
}
