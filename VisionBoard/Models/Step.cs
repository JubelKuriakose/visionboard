using System;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace VisionBoard.Models
{
    public partial class Step
    {
        public int Id { get; set; }

        [Display(Name = "Step Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public int Weight { get; set; }
        public DateTime? DueDate { get; set; }
        public bool Status { get; set; }

        [Display(Name = "Goal Name")]
        public int GoalId { get; set; }

        public virtual Goal Goal { get; set; }
    }
}
