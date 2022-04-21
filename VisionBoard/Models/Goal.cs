using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VisionBoard.Utils;

#nullable disable

namespace VisionBoard.Models
{
    public partial class Goal
    {
        public Goal()
        {
            Steps = new HashSet<Step>();
            GoalTags = new HashSet<GoalTags>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = AppConstants.NameRequiredMessage)]
        [MaxLength(50, ErrorMessage = AppConstants.NameLengthMessage)]
        public string Name { get; set; }

        [MaxLength(600, ErrorMessage = AppConstants.DescriptionLengthMessage)]
        public string Description { get; set; }

        public DateTime StartOn { get; set; }
        public DateTime? EndingOn { get; set; }
        public int? Magnitude { get; set; }
        public string PictureUrl { get; set; }
        public int? RewardId { get; set; }
        public bool Status { get; set; }
        public int? MeasurementId { get; set; }

        public virtual Measurement Measurement { get; set; }
        public virtual Reward Reward { get; set; }
        public virtual ICollection<GoalTags> GoalTags { get; set; }
        public virtual ICollection<Step> Steps { get; set; }
    }
}
