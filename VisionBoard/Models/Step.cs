using System;

#nullable disable

namespace VisionBoard.Models
{
    public partial class Step
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Weight { get; set; }
        public DateTime? DueDate { get; set; }
        public bool Status { get; set; }
        public int GoalId { get; set; }

        public virtual Goal Goal { get; set; }
    }
}
