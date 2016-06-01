using System;

namespace BoC.CmsHelpers.Abstraction.Models
{
    public class ImageField
    {
        public string MediaPath { get; set; }
        public string Alt { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string HSpace { get; set; }
        public string VSpace { get; set; }
        public string Src { get; set; }
        public Guid MediaId { get; set; }
    }
}