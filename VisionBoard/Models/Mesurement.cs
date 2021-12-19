using System;
using System.Collections.Generic;

#nullable disable

namespace VisionBoard.Models
{
    public partial class Mesurement
    {
        public Mesurement()
        {
            Goals = new HashSet<Goal>();
        }

        public int Id { get; set; }
        public int GoalId { get; set; }
        public string Type { get; set; }
        public int CurrentValue { get; set; }
        public int TotalValue { get; set; }
        public string Unit { get; set; }

        public virtual Goal Goal { get; set; }
        public virtual ICollection<Goal> Goals { get; set; }
    }
}
