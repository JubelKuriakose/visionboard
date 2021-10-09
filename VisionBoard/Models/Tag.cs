using System.Collections.Generic;

#nullable disable

namespace VisionBoard.Models
{
    public partial class Tag
    {
        public Tag()
        {
            Goals = new HashSet<Goal>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Colour { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<Goal> Goals { get; set; }
    }
}
