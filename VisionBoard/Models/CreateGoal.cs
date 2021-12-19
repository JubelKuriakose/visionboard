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
            Mesurements = new HashSet<Mesurement>();
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
        public int? TagId { get; set; }
        public int? RewardId { get; set; }
        public bool Status { get; set; }

        public virtual Reward Reward { get; set; }
        public virtual Tag Tag { get; set; }
        public virtual ICollection<Mesurement> Mesurements { get; set; }
        public virtual ICollection<Reward> Rewards { get; set; }
        public virtual ICollection<Step> Steps { get; set; }
    }
}
