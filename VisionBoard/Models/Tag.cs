using System.Collections.Generic;

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
        public string Name { get; set; }
        public string Colour { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<GoalTags> GoalTags { get; set; }
    }
}
