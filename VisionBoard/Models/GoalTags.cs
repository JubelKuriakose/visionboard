using System.Collections.Generic;

#nullable disable

namespace VisionBoard.Models
{
    public partial class GoalTags
    {
        public int GoalId { get; set; }
        public int TagId { get; set; }

        public virtual Goal Goal { get; set; }
        public virtual Tag Tag { get; set; }

    }
}
