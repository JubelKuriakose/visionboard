using System.Collections.Generic;

#nullable disable

namespace VisionBoard.Models
{
    public partial class Reward
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Descrption { get; set; }
        public string PictureUrl { get; set; }
        public bool Status { get; set; }

        public virtual Goal Goal { get; set; }

    }
}
