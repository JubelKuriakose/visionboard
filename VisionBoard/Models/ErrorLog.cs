using System;

#nullable disable

namespace VisionBoard.Models
{
    public partial class ErrorLog
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime DateTime { get; set; }

    }
}
