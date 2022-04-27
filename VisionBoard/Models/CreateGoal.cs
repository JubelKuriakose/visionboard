using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VisionBoard.Utils;

#nullable disable

namespace VisionBoard.Models
{
    public partial class CreateGoal
    {
        public CreateGoal()
        {
            Measurement = new HashSet<Measurement>();
            Rewards = new HashSet<Reward>();
            Steps = new HashSet<Step>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = AppConstants.NameRequiredMessage)]
        [MaxLength(50, ErrorMessage = AppConstants.NameLengthMessage)]
        public string Name { get; set; }

        [MaxLength(600, ErrorMessage = AppConstants.DescriptionLengthMessage)]
        public string Description { get; set; }

        [DisplayName("Start On")]
        public DateTime StartOn { get; set; }

        [DisplayName("Ending On")]
        public DateTime? EndingOn { get; set; }

        [DisplayName("Importance")]
        public int? Magnitude { get; set; }
        public string PictureUrl { get; set; }

        [DisplayName("Reward")]
        public int? RewardId { get; set; }
        public bool Status { get; set; }

        [DisplayName("Tags")]
        public int[] TagIds { get; set; }

        public virtual Reward Reward { get; set; }
        public virtual ICollection<Measurement> Measurement { get; set; }
        public virtual ICollection<Reward> Rewards { get; set; }
        public virtual ICollection<Step> Steps { get; set; }
    }
}
