using System.Collections.Generic;

#nullable disable

namespace VisionBoard.Models
{
    public partial class Reward
    {
        public Reward()
        {
            Goals = new HashSet<Goal>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Descrption { get; set; }
        public int GoalId { get; set; }
        public string PictureUrl { get; set; }
        public bool Status { get; set; }

        public virtual Goal Goal { get; set; }
        public virtual ICollection<Goal> Goals { get; set; }
    }
}
