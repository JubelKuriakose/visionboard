
namespace VisionBoard.Models
{
    public partial class Measurement
    {

        public int Id { get; set; }
        public string Type { get; set; }
        public int CurrentValue { get; set; }
        public int TotalValue { get; set; }
        public string Unit { get; set; }

        public virtual Goal Goal { get; set; }
    }
}
