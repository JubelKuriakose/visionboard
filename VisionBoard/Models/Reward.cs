#nullable disable

using System.ComponentModel.DataAnnotations;
using VisionBoard.Utils;

namespace VisionBoard.Models
{
    public partial class Reward
    {
        public int Id { get; set; }

        [Required(ErrorMessage = AppConstants.NameRequiredMessage)]
        [MaxLength(50, ErrorMessage = AppConstants.NameLengthMessage)]
        public string Name { get; set; }

        [MaxLength(600, ErrorMessage = AppConstants.DescriptionLengthMessage)]
        public string Description { get; set; }

        public string PictureUrl { get; set; }
        public bool Status { get; set; }

        public virtual Goal Goal { get; set; }

    }
}
