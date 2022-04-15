using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

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
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartOn { get; set; }
        public DateTime? EndingOn { get; set; }
        public int? Magnitude { get; set; }
        public IFormFile Picture { get; set; }
        public int? RewardId { get; set; }
        public bool Status { get; set; }
        public int[] TagIds { get; set; }

        public virtual Reward Reward { get; set; }
        public virtual ICollection<Measurement> Measurement { get; set; }
        public virtual ICollection<Reward> Rewards { get; set; }
        public virtual ICollection<Step> Steps { get; set; }
    }
}
